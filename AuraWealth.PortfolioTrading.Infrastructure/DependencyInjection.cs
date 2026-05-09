using AuraWealth.BuildingBlocks.Domain;
using AuraWealth.PortfolioTrading.Application.Common.Interfaces;
using AuraWealth.PortfolioTrading.Domain.TraderProfiles;
using AuraWealth.PortfolioTrading.Infrastructure.Data;
using AuraWealth.PortfolioTrading.Infrastructure.Interceptors;
using AuraWealth.PortfolioTrading.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace AuraWealth.PortfolioTrading.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Register the EF Core Interceptor
            services.AddScoped<DispatchDomainEventsInterceptor>();

            // 2. Register the DbContext with Postgres and wire up the Interceptor
            services.AddDbContext<PortfolioDbContext>((sp, options) =>
            {
                // Assuming your connection string in appsettings.json is named "AuraWealthDb"
                options.UseNpgsql(configuration.GetConnectionString("AuraWealthDb"));

                // We resolve the interceptor from the service provider (sp) so it can use MediatR
                options.AddInterceptors(sp.GetRequiredService<DispatchDomainEventsInterceptor>());
            });

            // 3. Bind the IUnitOfWork directly to the PortfolioDbContext instance
            // This guarantees that when a Command Handler asks for IUnitOfWork, 
            // it gets the EXACT same database transaction as the Repositories.
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<PortfolioDbContext>());

            // 4. Register the blazingly fast Dapper Read Connection
            services.AddScoped<IReadConnection, PostgresReadConnection>();

            // 5. Register the Repositories
            services.AddScoped<IDomainRepository<TraderProfile>, TraderProfileRepository>();
            services.AddScoped<ITransactionRecordRepository, TransactionRecordRepository>();
            return services;
        }
    }
}
