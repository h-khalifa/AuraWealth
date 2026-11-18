using AuraWealth.BuildingBlocks.Infrastructure.Interceptors;
using AuraWealth.MarketData.Application.Common.Interfaces;
using AuraWealth.MarketData.Infrastructure.Data;
using AuraWealth.MarketData.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuraWealth.MarketData.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddMarketDataInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("AuraWealthDb")
            ?? throw new InvalidOperationException("Connection string 'AuraWealthDb' not found.");

        services.AddDbContext<MarketDataDbContext>((sp, options) =>
        {
            var interceptor = sp.GetRequiredService<DispatchDomainEventsInterceptor>();

            options.UseNpgsql(connectionString, npgsql =>
                    npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "market_data"))
                .AddInterceptors(interceptor);
        });

        services.AddScoped<IMarketDataUnitOfWork>(sp => sp.GetRequiredService<MarketDataDbContext>());
        services.AddScoped<ITradableAssetRepository, TradableAssetRepository>();

        return services;
    }
}