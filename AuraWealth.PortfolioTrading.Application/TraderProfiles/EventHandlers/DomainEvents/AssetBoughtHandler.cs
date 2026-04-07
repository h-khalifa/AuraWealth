using AuraWealth.PortfolioTrading.Application.Common.Interfaces;
using AuraWealth.PortfolioTrading.Application.TraderProfiles.ReadModels;
using AuraWealth.PortfolioTrading.Domain.TraderProfiles.Events;
using MediatR;

namespace AuraWealth.PortfolioTrading.Application.TraderProfiles.EventHandlers.DomainEvents
{
    public class AssetBoughtHandler : INotificationHandler<AssetBought>
    {
        private readonly ITransactionRecordRepository _transactionRecordRepository;
        private readonly IUnitOfWork _unitOfWork;
        ////private readonly IEventBus _eventBus; in the future, we might want to publish an event after handling this domain event

        public AssetBoughtHandler(ITransactionRecordRepository transactionRecordRepository, IUnitOfWork unitOfWork)
        {
            _transactionRecordRepository = transactionRecordRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(AssetBought notification, CancellationToken cancellationToken)
        {
            var transactionRecord = new TransactionRecord
            {
                Id = Guid.NewGuid(),
                TraderId = notification.TraderId,
                Symbol = notification.Symbol.Symbol,
                Quantity = notification.Quantity,
                PricePerUnit = notification.ExecutionPrice.Amount,
                TransactionType = "AssetBought",
                OccurredOn = DateTime.UtcNow
            };

            await _transactionRecordRepository.AddAsync(transactionRecord, cancellationToken);
            //await _unitOfWork.SaveChangesAsync(); called by the command handler after all domain events are handled, so we don't call it here

            // In the future, we might want to publish an event after handling this domain event, for example:
        }
    }
}
