using AuraWealth.MarketData.Application.Common.Interfaces;
using AuraWealth.MarketData.Domain.Common;
using AuraWealth.MarketData.Domain.TradableAssets.Enums;
using MediatR;

namespace AuraWealth.MarketData.Application.TradableAssets.Commands.ChangeInstrumentStatus;

public sealed class ChangeInstrumentStatusCommandHandler(
    ITradableAssetRepository repository,
    IMarketDataUnitOfWork unitOfWork,
    TimeProvider timeProvider) : IRequestHandler<ChangeInstrumentStatusCommand>
{
    public async Task Handle(ChangeInstrumentStatusCommand request, CancellationToken cancellationToken)
    {
        var ticker = new TickerSymbol(request.Symbol);
        var asset = await repository.GetBySymbolAsync(ticker, cancellationToken)
            ?? throw new KeyNotFoundException($"Instrument '{ticker.Value}' was not found.");

        var now = timeProvider.GetUtcNow().UtcDateTime;

        switch (request.NewStatus)
        {
            case ListingStatus.Active:
                asset.Activate(now);
                break;
            case ListingStatus.Suspended:
                asset.Suspend(now);
                break;
            case ListingStatus.Delisted:
                asset.Delist(now);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(request.NewStatus), "Unsupported listing status.");
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}