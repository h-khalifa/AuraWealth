using AuraWealth.BuildingBlocks.Domain;
using AuraWealth.MarketData.Domain.TradableAssets.Enums;

namespace AuraWealth.MarketData.Domain.TradableAssets.Events;

public sealed record InstrumentStatusChangedDomainEvent(
    Guid InstrumentId,
    string Symbol,
    ListingStatus PreviousStatus,
    ListingStatus NewStatus,
    DateTime OccurredOnUtc) : DomainEvent;