using AuraWealth.PortfolioTrading.Application.Common.Interfaces;
using AuraWealth.PortfolioTrading.Application.TraderProfiles.ReadModels;
using AuraWealth.PortfolioTrading.Domain.TraderProfiles.Events;
using MediatR;

namespace AuraWealth.PortfolioTrading.Application.TraderProfiles.EventHandlers.DomainEvents
{
    public class AssetSoldHandler : INotificationHandler<AssetSold>
    {
        private readonly ITransactionRecordRepository _transactionRecordRepository;
        public AssetSoldHandler(ITransactionRecordRepository transactionRecordRepository)
        {
            _transactionRecordRepository = transactionRecordRepository;
        }
        public async Task Handle(AssetSold notification, CancellationToken cancellationToken)
        {
            var transactionRecord = new TransactionRecord()
            {
                Id = Guid.NewGuid(),
                TraderId = notification.TraderId,
                Symbol = notification.Symbol.Symbol,
                Quantity = notification.Quantity,
                PricePerUnit = notification.ExecutionPrice.Amount,
                TransactionType = "AssetSold",
                OccurredOn = DateTime.UtcNow
            };

            await _transactionRecordRepository.AddAsync(transactionRecord, cancellationToken);

        }
    }
}
