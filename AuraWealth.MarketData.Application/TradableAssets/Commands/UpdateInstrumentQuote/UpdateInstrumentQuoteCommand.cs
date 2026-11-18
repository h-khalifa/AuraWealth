using MediatR;

namespace AuraWealth.MarketData.Application.TradableAssets.Commands.UpdateInstrumentQuote;

public sealed record UpdateInstrumentQuoteCommand(
    string Symbol,
    decimal Price,
    string CurrencyCode = "USD",
    DateTime? ObservedAtUtc = null) : IRequest;