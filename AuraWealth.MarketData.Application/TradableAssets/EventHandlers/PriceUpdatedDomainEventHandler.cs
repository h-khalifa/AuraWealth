using AuraWealth.MarketData.Application.Common.Interfaces;
using AuraWealth.MarketData.Application.Common.Models;
using AuraWealth.MarketData.Domain.TradableAssets.Events;
using MediatR;

namespace AuraWealth.MarketData.Application.TradableAssets.EventHandlers;

public sealed class PriceUpdatedDomainEventHandler(
    IPriceTickRepository priceTickRepository) : INotificationHandler<PriceUpdatedDomainEvent>
{
    public async Task Handle(PriceUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var tick = new PriceTick
        {
            Id = Guid.NewGuid(),
            InstrumentId = notification.InstrumentId,
            Symbol = notification.Symbol,
            Price = notification.NewPrice,
            CurrencyCode = notification.CurrencyCode,
            ObservedAtUtc = notification.ObservedAtUtc
        };

        await priceTickRepository.AddAsync(tick, cancellationToken);
    }
}