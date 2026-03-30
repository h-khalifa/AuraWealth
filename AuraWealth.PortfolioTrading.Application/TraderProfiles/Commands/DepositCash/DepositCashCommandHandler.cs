using AuraWealth.BuildingBlocks.Domain;
using AuraWealth.PortfolioTrading.Application.Common.Interfaces;
using AuraWealth.PortfolioTrading.Domain.Common;
using AuraWealth.PortfolioTrading.Domain.TraderProfiles;
using MediatR;

namespace AuraWealth.PortfolioTrading.Application.TraderProfiles.Commands.DepositCash
{
    public class DepositCashCommandHandler : IRequestHandler<DepositCashCommand>
    {
        private readonly IDomainRepository<TraderProfile> _traderProfileRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DepositCashCommandHandler(IDomainRepository<TraderProfile> traderProfileRepository, IUnitOfWork unitOfWork)
        {
            _traderProfileRepository = traderProfileRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(DepositCashCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the trader profile
            var traderProfile = await _traderProfileRepository.GetByIdAsync(request.TraderId, cancellationToken);
            if (traderProfile == null)
                throw new KeyNotFoundException();

            // Deposit the cash
            traderProfile.DepositCash(new Money(request.Amount));

            // Update the trader profile in the repository
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
