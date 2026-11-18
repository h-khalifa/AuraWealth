using AuraWealth.MarketData.Domain.Common;
using AuraWealth.MarketData.Domain.TradableAssets;

namespace AuraWealth.MarketData.Application.Common.Interfaces;

public interface ITradableAssetRepository
{
    Task<TradableAsset?> GetBySymbolAsync(TickerSymbol symbol, CancellationToken cancellationToken = default);
    Task<bool> ExistsBySymbolAsync(TickerSymbol symbol, CancellationToken cancellationToken = default);
    Task AddAsync(TradableAsset asset, CancellationToken cancellationToken = default);
}