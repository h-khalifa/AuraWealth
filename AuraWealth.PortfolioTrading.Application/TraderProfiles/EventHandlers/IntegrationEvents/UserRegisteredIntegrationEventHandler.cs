using AuraWealth.BuildingBlocks.Domain;
using AuraWealth.BuildingBlocks.IntegrationEvents;
using AuraWealth.PortfolioTrading.Application.Common.Interfaces;
using AuraWealth.PortfolioTrading.Domain.TraderProfiles;
using MediatR;

namespace AuraWealth.PortfolioTrading.Application.TraderProfiles.EventHandlers.IntegrationEvents
{
    public class UserRegisteredIntegrationEventHandler : INotificationHandler<UserRegisteredIntegrationEvent>
    {
        private readonly IDomainRepository<TraderProfile> _traderProfileRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserRegisteredIntegrationEventHandler(IDomainRepository<TraderProfile> traderProfileRepository, IUnitOfWork unitOfWork)
        {
            _traderProfileRepository = traderProfileRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(UserRegisteredIntegrationEvent notification, CancellationToken cancellationToken)
        {
            // Check if a trader profile already exists for the user ID
            var traderProfile = await _traderProfileRepository.GetByIdAsync(notification.UserId, cancellationToken);
            if (traderProfile is not null)
                return;// If a trader profile already exists, do nothing (idempotent)

            // Create a new trader profile for the registered user
            traderProfile = new TraderProfile(notification.UserId);
            
            await _traderProfileRepository.AddAsync(traderProfile, cancellationToken);
            // Save changes to the database
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
