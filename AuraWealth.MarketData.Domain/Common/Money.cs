using AuraWealth.BuildingBlocks.Domain;

namespace AuraWealth.MarketData.Domain.Common;

public sealed class Money : ValueObject
{
    public decimal Amount { get; private set; }
    public string CurrencyCode { get; private set; } = default!;

    private Money() { }

    public Money(decimal amount, string currencyCode = "USD")
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Money amount cannot be negative.");

        if (string.IsNullOrWhiteSpace(currencyCode) || currencyCode.Trim().Length != 3)
            throw new ArgumentException("Currency code must be a 3-letter ISO code.", nameof(currencyCode));

        Amount = amount;
        CurrencyCode = currencyCode.Trim().ToUpperInvariant();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return CurrencyCode;
    }
}