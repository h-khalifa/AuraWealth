using AuraWealth.BuildingBlocks.Domain;

namespace AuraWealth.MarketData.Domain.TradableAssets.Events;

public sealed record PriceUpdatedDomainEvent(
    Guid InstrumentId,
    string Symbol,
    decimal? PreviousPrice,
    decimal NewPrice,
    string CurrencyCode,
    DateTime ObservedAtUtc) : DomainEvent;