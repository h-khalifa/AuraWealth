using MediatR;

namespace AuraWealth.MarketData.Application.TradableAssets.Queries.GetInstrumentQuote
{
    public sealed record GetInstrumentQuoteQuery(string Symbol) : IRequest<InstrumentQuoteDto>;
}
