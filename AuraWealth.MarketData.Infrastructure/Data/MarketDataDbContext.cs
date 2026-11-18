using AuraWealth.MarketData.Application.Common.Interfaces;
using AuraWealth.MarketData.Application.Common.Models;
using AuraWealth.MarketData.Domain.TradableAssets;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AuraWealth.MarketData.Infrastructure.Data;

public sealed class MarketDataDbContext(DbContextOptions<MarketDataDbContext> options)
    : DbContext(options), IMarketDataUnitOfWork
{
    public DbSet<TradableAsset> TradableAssets => Set<TradableAsset>();
    public DbSet<PriceTick> PriceTicks => Set<PriceTick>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("market_data");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}