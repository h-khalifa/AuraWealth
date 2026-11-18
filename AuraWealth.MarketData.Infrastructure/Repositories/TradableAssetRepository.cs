using AuraWealth.MarketData.Application.Common.Interfaces;
using AuraWealth.MarketData.Domain.Common;
using AuraWealth.MarketData.Domain.TradableAssets;
using AuraWealth.MarketData.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AuraWealth.MarketData.Infrastructure.Repositories;

public sealed class TradableAssetRepository(MarketDataDbContext dbContext) : ITradableAssetRepository
{
    public async Task<TradableAsset?> GetBySymbolAsync(TickerSymbol symbol, CancellationToken cancellationToken = default)
    {
        return await dbContext.TradableAssets
            .FirstOrDefaultAsync(a => a.Symbol == symbol, cancellationToken);
    }

    public async Task<bool> ExistsBySymbolAsync(TickerSymbol symbol, CancellationToken cancellationToken = default)
    {
        return await dbContext.TradableAssets
            .AnyAsync(a => a.Symbol == symbol, cancellationToken);
    }

    public async Task AddAsync(TradableAsset asset, CancellationToken cancellationToken = default)
    {
        await dbContext.TradableAssets.AddAsync(asset, cancellationToken);
    }
}