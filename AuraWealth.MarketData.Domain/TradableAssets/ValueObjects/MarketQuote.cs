using AuraWealth.BuildingBlocks.Domain;
using AuraWealth.MarketData.Domain.Common;

namespace AuraWealth.MarketData.Domain.TradableAssets.ValueObjects;

public sealed class MarketQuote : ValueObject
{
    public Money Price { get; private set; } = default!;
    public DateTime ObservedAtUtc { get; private set; }

    private MarketQuote() { }

    public MarketQuote(Money price, DateTime observedAtUtc)
    {
        ArgumentNullException.ThrowIfNull(price);

        if (price.Amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Market quote price must be greater than zero.");

        if (observedAtUtc == default)
            throw new ArgumentException("Observed timestamp must be a valid UTC date.", nameof(observedAtUtc));

        Price = price;
        ObservedAtUtc = DateTime.SpecifyKind(observedAtUtc, DateTimeKind.Utc);
    }

    public bool IsFreshAt(DateTime currentUtc, TimeSpan maxAge)
    {
        var age = currentUtc - ObservedAtUtc;
        return age >= TimeSpan.Zero && age <= maxAge;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Price;
        yield return ObservedAtUtc;
    }
}