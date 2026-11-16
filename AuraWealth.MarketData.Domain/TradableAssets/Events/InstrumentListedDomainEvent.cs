using AuraWealth.BuildingBlocks.Domain;
using AuraWealth.MarketData.Domain.TradableAssets.Enums;

namespace AuraWealth.MarketData.Domain.TradableAssets.Events;

public sealed record InstrumentListedDomainEvent(
    Guid InstrumentId,
    string Symbol,
    string Name,
    AssetClass AssetClass,
    DateTime OccurredOnUtc) : DomainEvent;