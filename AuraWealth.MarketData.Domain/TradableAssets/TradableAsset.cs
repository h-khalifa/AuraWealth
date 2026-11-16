using AuraWealth.BuildingBlocks.Domain;
using AuraWealth.MarketData.Domain.Common;
using AuraWealth.MarketData.Domain.TradableAssets.Enums;
using AuraWealth.MarketData.Domain.TradableAssets.Events;
using AuraWealth.MarketData.Domain.TradableAssets.ValueObjects;

namespace AuraWealth.MarketData.Domain.TradableAssets;

public sealed class TradableAsset : Entity, IAggregateRoot
{
    public TickerSymbol Symbol { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public AssetClass AssetClass { get; private set; }
    public ListingStatus Status { get; private set; }
    public MarketQuote? CurrentQuote { get; private set; }

    private TradableAsset() { }

    private TradableAsset(
        Guid id,
        TickerSymbol symbol,
        string name,
        AssetClass assetClass,
        DateTime listedAtUtc)
    {
        Id = id;
        Symbol = symbol;
        Name = name;
        AssetClass = assetClass;
        Status = ListingStatus.Active;

        AddDomainEvent(new InstrumentListedDomainEvent(
            Id,
            Symbol.Value,
            Name,
            AssetClass,
            listedAtUtc));
    }

    public static TradableAsset List(
        TickerSymbol symbol,
        string name,
        AssetClass assetClass,
        DateTime listedAtUtc)
    {
        ArgumentNullException.ThrowIfNull(symbol);

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Instrument name cannot be empty.", nameof(name));

        return new TradableAsset(
            Guid.NewGuid(),
            symbol,
            name.Trim(),
            assetClass,
            DateTime.SpecifyKind(listedAtUtc, DateTimeKind.Utc));
    }

    public void UpdateQuote(MarketQuote newQuote)
    {
        ArgumentNullException.ThrowIfNull(newQuote);

        if (Status != ListingStatus.Active)
            throw new InvalidOperationException(
                $"Cannot update quote for {Symbol.Value} because its listing status is {Status}.");

        if (CurrentQuote is not null && newQuote.ObservedAtUtc < CurrentQuote.ObservedAtUtc)
            throw new InvalidOperationException(
                $"Out-of-order quote rejected for {Symbol.Value}. Incoming timestamp ({newQuote.ObservedAtUtc:O}) is older than current quote ({CurrentQuote.ObservedAtUtc:O}).");

        var previousPrice = CurrentQuote?.Price.Amount;
        CurrentQuote = newQuote;

        AddDomainEvent(new PriceUpdatedDomainEvent(
            Id,
            Symbol.Value,
            previousPrice,
            newQuote.Price.Amount,
            newQuote.Price.CurrencyCode,
            newQuote.ObservedAtUtc));
    }

    public void Suspend(DateTime occurredOnUtc)
    {
        if (Status == ListingStatus.Delisted)
            throw new InvalidOperationException($"Cannot suspend {Symbol.Value} because it is permanently delisted.");

        if (Status == ListingStatus.Suspended)
            return;

        var previous = Status;
        Status = ListingStatus.Suspended;

        AddDomainEvent(new InstrumentStatusChangedDomainEvent(
            Id, Symbol.Value, previous, Status, occurredOnUtc));
    }

    public void Activate(DateTime occurredOnUtc)
    {
        if (Status == ListingStatus.Delisted)
            throw new InvalidOperationException($"Cannot reactivate {Symbol.Value} because it is permanently delisted.");

        if (Status == ListingStatus.Active)
            return;

        var previous = Status;
        Status = ListingStatus.Active;

        AddDomainEvent(new InstrumentStatusChangedDomainEvent(
            Id, Symbol.Value, previous, Status, occurredOnUtc));
    }

    public void Delist(DateTime occurredOnUtc)
    {
        if (Status == ListingStatus.Delisted)
            return;

        var previous = Status;
        Status = ListingStatus.Delisted;

        AddDomainEvent(new InstrumentStatusChangedDomainEvent(
            Id, Symbol.Value, previous, Status, occurredOnUtc));
    }

    public bool CanTradeAt(DateTime currentUtc, TimeSpan maxQuoteAge)
    {
        return Status == ListingStatus.Active
            && CurrentQuote is not null
            && CurrentQuote.IsFreshAt(currentUtc, maxQuoteAge);
    }
}