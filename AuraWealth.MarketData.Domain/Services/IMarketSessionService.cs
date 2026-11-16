using AuraWealth.MarketData.Domain.TradableAssets.Enums;

namespace AuraWealth.MarketData.Domain.Services;

public interface IMarketSessionService
{
    MarketSession GetCurrentSession(DateTime utcNow, AssetClass assetClass);
    TimeSpan GetMaxQuoteAge(MarketSession session);
}