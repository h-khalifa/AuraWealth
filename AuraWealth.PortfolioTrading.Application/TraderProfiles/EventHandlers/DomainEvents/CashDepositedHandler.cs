using AuraWealth.PortfolioTrading.Application.Common.Interfaces;
using AuraWealth.PortfolioTrading.Application.TraderProfiles.ReadModels;
using AuraWealth.PortfolioTrading.Domain.TraderProfiles.Events;
using MediatR;

namespace AuraWealth.PortfolioTrading.Application.TraderProfiles.EventHandlers.DomainEvents
{
    public class CashDepositedHandler : INotificationHandler<CashDeposited>
    {
        private readonly ITransactionRecordRepository _transactionRecordRepository;
        public CashDepositedHandler(ITransactionRecordRepository transactionRecordRepository)
        {
            _transactionRecordRepository = transactionRecordRepository;
        }
        public async Task Handle(CashDeposited notification, CancellationToken cancellationToken)
        {
            var transactionRecord = new TransactionRecord()
            {
                Id = Guid.NewGuid(),
                TraderId = notification.TraderId,
                TotalAmount = notification.Amount.Amount,
                OccurredOn = DateTime.UtcNow,
                TransactionType = "CashDeposited"
            };

            await _transactionRecordRepository.AddAsync(transactionRecord, cancellationToken);
        }
    }
}
