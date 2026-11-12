using AuraWealth.PortfolioTrading.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AuraWealth.DbMigrator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                logger.LogInformation("Starting Database Migration Pipeline...");

                // 1. Resolve the Portfolio Module DbContext
                var portfolioContext = services.GetRequiredService<PortfolioDbContext>();

                // 2. Apply pending migrations
                logger.LogInformation("Applying migrations for PortfolioTrading module (schema: portfolio)...");
                await portfolioContext.Database.MigrateAsync();

                // Future modules will go here:
                // var identityContext = services.GetRequiredService<IdentityDbContext>();
                // await identityContext.Database.MigrateAsync();

                logger.LogInformation("All database migrations applied successfully.");
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "An error occurred while migrating the database.");
                Environment.ExitCode = 1; // Fails the CI/CD pipeline step
            }
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var connectionString = hostContext.Configuration.GetConnectionString("AuraWealthDb");

                    // Register DbContexts for the runtime migration execution
                    services.AddDbContext<PortfolioDbContext>(options =>
                        options.UseNpgsql(connectionString, x =>
                            x.MigrationsAssembly(typeof(Program).Assembly.FullName)));
                });
    }
}