namespace AuraWealth.MarketData.Application.Common.Interfaces;

public interface IMarketDataUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}