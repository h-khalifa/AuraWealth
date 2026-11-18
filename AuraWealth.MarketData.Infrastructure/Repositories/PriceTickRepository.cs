using AuraWealth.MarketData.Application.Common.Interfaces;
using AuraWealth.MarketData.Application.Common.Models;
using AuraWealth.MarketData.Infrastructure.Data;

namespace AuraWealth.MarketData.Infrastructure.Repositories;

public sealed class PriceTickRepository(MarketDataDbContext dbContext) : IPriceTickRepository
{
    public async Task AddAsync(PriceTick tick, CancellationToken cancellationToken = default)
    {
        await dbContext.PriceTicks.AddAsync(tick, cancellationToken);
    }
}