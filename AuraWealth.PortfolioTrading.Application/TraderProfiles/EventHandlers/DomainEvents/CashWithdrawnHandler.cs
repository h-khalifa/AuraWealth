using AuraWealth.PortfolioTrading.Application.Common.Interfaces;
using AuraWealth.PortfolioTrading.Application.TraderProfiles.ReadModels;
using AuraWealth.PortfolioTrading.Domain.TraderProfiles.Events;
using MediatR;

namespace AuraWealth.PortfolioTrading.Application.TraderProfiles.EventHandlers.DomainEvents
{
    public class CashWithdrawnHandler : INotificationHandler<CashWithdrawn>
    {
        private readonly ITransactionRecordRepository _transactionRecordRepository;
        public CashWithdrawnHandler(ITransactionRecordRepository transactionRecordRepository)
        {
            _transactionRecordRepository = transactionRecordRepository;
        }
        public async Task Handle(CashWithdrawn notification, CancellationToken cancellationToken)
        {
            var transactionRecord = new TransactionRecord()
            {
                Id = Guid.NewGuid(),
                TraderId = notification.TraderId,
                TotalAmount = notification.Amount.Amount,
                OccurredOn = DateTime.UtcNow,
                TransactionType = "CashWithdrawn"
            };

            await _transactionRecordRepository.AddAsync(transactionRecord, cancellationToken);
        }
    }
}
