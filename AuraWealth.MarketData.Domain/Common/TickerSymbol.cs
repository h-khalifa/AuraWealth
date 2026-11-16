using System.Text.RegularExpressions;
using AuraWealth.BuildingBlocks.Domain;

namespace AuraWealth.MarketData.Domain.Common;

public sealed partial class TickerSymbol : ValueObject
{
    public string Value { get; private set; } = default!;

    private TickerSymbol() { }

    public TickerSymbol(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Ticker symbol cannot be empty.", nameof(value));

        var normalized = value.Trim().ToUpperInvariant();

        if (!TickerRegex().IsMatch(normalized))
            throw new ArgumentException("Ticker symbol must be 1 to 5 uppercase letters.", nameof(value));

        Value = normalized;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    [GeneratedRegex("^[A-Z]{1,5}$")]
    private static partial Regex TickerRegex();
}