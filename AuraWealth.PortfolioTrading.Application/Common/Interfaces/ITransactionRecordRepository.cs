using AuraWealth.PortfolioTrading.Application.TraderProfiles.ReadModels;

namespace AuraWealth.PortfolioTrading.Application.Common.Interfaces
{
    public interface ITransactionRecordRepository
    {
        // Used by the Event Handler to save the receipt
        Task AddAsync(TransactionRecord record, CancellationToken cancellationToken);

        // Used by the Queries to fetch the UI ledger
        Task<IEnumerable<TransactionRecord>> GetByTraderIdAsync(Guid traderId, int page, int pageSize, CancellationToken cancellationToken);
    }
}
