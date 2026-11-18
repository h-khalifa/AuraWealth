using AuraWealth.MarketData.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AuraWealth.DbMigrator.Factories;

public sealed class MarketDataDbContextFactory : IDesignTimeDbContextFactory<MarketDataDbContext>
{
    public MarketDataDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var connectionString = configuration.GetConnectionString("AuraWealthDb")
            ?? throw new InvalidOperationException("Connection string 'AuraWealthDb' not found.");

        var optionsBuilder = new DbContextOptionsBuilder<MarketDataDbContext>();
        optionsBuilder.UseNpgsql(connectionString, npgsql =>
        {
            npgsql.MigrationsAssembly(typeof(MarketDataDbContextFactory).Assembly.FullName);
            npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "market_data");
        });

        return new MarketDataDbContext(optionsBuilder.Options);
    }
}