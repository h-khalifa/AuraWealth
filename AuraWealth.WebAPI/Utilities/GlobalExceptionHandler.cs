using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AuraWealth.WebAPI.Utilities
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

            var (statusCode, title) = exception switch
            {
                // Domain validation / invariant violations
                ArgumentOutOfRangeException => (StatusCodes.Status400BadRequest, "Validation Error"),
                ArgumentException => (StatusCodes.Status400BadRequest, "Invalid Argument"),

                // Business rule conflicts (e.g., Insufficient funds, selling more shares than owned)
                InvalidOperationException => (StatusCodes.Status409Conflict, "Business Rule Violation"),

                // Missing entities (if you throw KeyNotFoundException or a custom NotFoundException)
                KeyNotFoundException => (StatusCodes.Status404NotFound, "Resource Not Found"),

                // Fallback for genuine unexpected server crashes
                _ => (StatusCodes.Status500InternalServerError, "Server Error")
            };

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = statusCode == StatusCodes.Status500InternalServerError
                    ? "An unexpected error occurred on the server."
                    : exception.Message,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
            };

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true; // Signals that the exception is fully handled
        }
    }
}
