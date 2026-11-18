using AuraWealth.MarketData.Application.Common.Interfaces;
using AuraWealth.MarketData.Domain.Common;
using AuraWealth.MarketData.Domain.TradableAssets.ValueObjects;
using MediatR;

namespace AuraWealth.MarketData.Application.TradableAssets.Commands.UpdateInstrumentQuote;

public sealed class UpdateInstrumentQuoteCommandHandler(
    ITradableAssetRepository repository,
    IMarketDataUnitOfWork unitOfWork,
    TimeProvider timeProvider) : IRequestHandler<UpdateInstrumentQuoteCommand>
{
    public async Task Handle(UpdateInstrumentQuoteCommand request, CancellationToken cancellationToken)
    {
        var ticker = new TickerSymbol(request.Symbol);
        var asset = await repository.GetBySymbolAsync(ticker, cancellationToken)
            ?? throw new KeyNotFoundException($"Instrument '{ticker.Value}' was not found.");

        var observedAt = request.ObservedAtUtc ?? timeProvider.GetUtcNow().UtcDateTime;
        var quote = new MarketQuote(new Money(request.Price, request.CurrencyCode), observedAt);

        asset.UpdateQuote(quote);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}