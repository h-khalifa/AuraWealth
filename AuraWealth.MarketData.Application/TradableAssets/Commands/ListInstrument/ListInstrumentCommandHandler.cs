using AuraWealth.MarketData.Application.Common.Interfaces;
using AuraWealth.MarketData.Domain.Common;
using AuraWealth.MarketData.Domain.TradableAssets;
using MediatR;

namespace AuraWealth.MarketData.Application.TradableAssets.Commands.ListInstrument;

public sealed class ListInstrumentCommandHandler(
    ITradableAssetRepository repository,
    IMarketDataUnitOfWork unitOfWork,
    TimeProvider timeProvider) : IRequestHandler<ListInstrumentCommand, Guid>
{
    public async Task<Guid> Handle(ListInstrumentCommand request, CancellationToken cancellationToken)
    {
        var ticker = new TickerSymbol(request.Symbol);

        if (await repository.ExistsBySymbolAsync(ticker, cancellationToken))
            throw new InvalidOperationException($"An instrument with ticker '{ticker.Value}' is already listed.");

        var asset = TradableAsset.List(
            ticker,
            request.Name,
            request.AssetClass,
            timeProvider.GetUtcNow().UtcDateTime);

        await repository.AddAsync(asset, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return asset.Id;
    }
}