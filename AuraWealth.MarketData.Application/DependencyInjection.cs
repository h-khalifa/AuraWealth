using System.Reflection;
using AuraWealth.MarketData.Domain.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AuraWealth.MarketData.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddMarketDataApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
        });

        services.AddValidatorsFromAssembly(assembly);

        services.AddSingleton<IMarketSessionService, ExchangeClockService>();

        return services;
    }
}