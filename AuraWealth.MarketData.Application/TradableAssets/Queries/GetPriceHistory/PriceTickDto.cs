namespace AuraWealth.MarketData.Application.TradableAssets.Queries.GetPriceHistory
{
    public sealed record PriceTickDto(
      Guid Id,
      decimal Price,
      string CurrencyCode,
      DateTime ObservedAtUtc);
}
