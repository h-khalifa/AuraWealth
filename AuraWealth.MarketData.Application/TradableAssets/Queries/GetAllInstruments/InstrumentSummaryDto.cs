namespace AuraWealth.MarketData.Application.TradableAssets.Queries.GetAllInstruments
{
    public sealed record InstrumentSummaryDto(
    Guid Id,
    string Symbol,
    string Name,
    string AssetClass,
    string Status,
    decimal? Price,
    string? CurrencyCode,
    DateTime? ObservedAtUtc);
}
