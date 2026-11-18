using AuraWealth.MarketData.Application.Common.Models;

namespace AuraWealth.MarketData.Application.Common.Interfaces;

public interface IPriceTickRepository
{
    Task AddAsync(PriceTick tick, CancellationToken cancellationToken = default);
}