namespace AuraWealth.MarketData.Application.Common.Models;

public sealed class PriceTick
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid InstrumentId { get; init; }
    public string Symbol { get; init; } = default!;
    public decimal Price { get; init; }
    public string CurrencyCode { get; init; } = default!;
    public DateTime ObservedAtUtc { get; init; }
}