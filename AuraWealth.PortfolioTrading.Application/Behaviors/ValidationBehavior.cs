using FluentValidation;
using MediatR;

namespace Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request == null || !_validators.Any())
            {
                return await next();
            }

            var ctx = new ValidationContext<TRequest>(request);

            // Run all the validators in parallel
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(ctx, cancellationToken)));

            // Gather any failures
            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

            if (failures.Count != 0)
            {
                // Throws FluentValidation's built-in ValidationException
                throw new ValidationException(failures);
            }
            // If validation passes, proceed to the next behavior or the handler
            //note: the next() delegate will call the next behavior in the pipeline or the actual request handler if this is the last behavior
            //this is how to configure a pipeline behavior to be preprocess
            return await next();
        }
    }
}