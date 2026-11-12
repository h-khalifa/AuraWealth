using AuraWealth.PortfolioTrading.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AuraWealth.DbMigrator.Factories
{
    internal class PortfolioDbContextFactory
        : IDesignTimeDbContextFactory<PortfolioDbContext>
    {
        public PortfolioDbContext CreateDbContext(string[] args)
        {
            // Build config to read from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<PortfolioDbContext>();
            var connectionString = configuration.GetConnectionString("AuraWealthDb");

            // Tell EF to store the migrations IN THIS project, under a specific folder
            builder.UseNpgsql(connectionString, x =>
                x.MigrationsAssembly(typeof(PortfolioDbContextFactory).Assembly.FullName));

            return new PortfolioDbContext(builder.Options);
        }
    }
}
