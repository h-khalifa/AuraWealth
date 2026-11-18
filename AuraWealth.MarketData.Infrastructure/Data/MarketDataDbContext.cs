using System.Reflection;
using AuraWealth.MarketData.Application.Common.Interfaces;
using AuraWealth.MarketData.Domain.TradableAssets;
using Microsoft.EntityFrameworkCore;

namespace AuraWealth.MarketData.Infrastructure.Data;

public sealed class MarketDataDbContext(DbContextOptions<MarketDataDbContext> options)
    : DbContext(options), IMarketDataUnitOfWork
{
    public DbSet<TradableAsset> TradableAssets => Set<TradableAsset>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("market_data");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}