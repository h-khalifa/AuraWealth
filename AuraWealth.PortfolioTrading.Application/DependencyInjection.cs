using Application.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AuraWealth.PortfolioTrading.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //var assembly = typeof(DependencyInjection).Assembly; // Use your Application layer assembly here
            var assembly = Assembly.GetExecutingAssembly();

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(assembly);

                // Register the pipeline behavior
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            // 2. Register all FluentValidation validators automatically
            services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}
