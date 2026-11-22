namespace AuraWealth.MarketData.Application.TradableAssets.Queries.GetInstrumentQuote;

public sealed record InstrumentQuoteDto(
    Guid Id,
    string Symbol,
    string Name,
    string AssetClass,
    string Status,
    decimal? Price,
    string? CurrencyCode,
    DateTime? ObservedAtUtc,
    string CurrentSession,
    bool IsFresh,
    bool CanTrade);