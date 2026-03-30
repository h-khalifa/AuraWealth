using AuraWealth.BuildingBlocks.Domain;
using AuraWealth.PortfolioTrading.Application.Common.Interfaces;
using AuraWealth.PortfolioTrading.Domain.Common;
using AuraWealth.PortfolioTrading.Domain.TraderProfiles;
using MediatR;

namespace AuraWealth.PortfolioTrading.Application.TraderProfiles.Commands.WithdrawCash
{
    public class WithdrawCashCommandHandler : IRequestHandler<WithdrawCashCommand>
    {
        private readonly IDomainRepository<TraderProfile> _traderProfileRepository;
        private readonly IUnitOfWork _unitOfWork;
        public WithdrawCashCommandHandler(IDomainRepository<TraderProfile> traderProfileRepository, IUnitOfWork unitOfWork)
        {
            _traderProfileRepository = traderProfileRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(WithdrawCashCommand request, CancellationToken cancellationToken)
        {
            // retrieve the trader profile
            var traderProfile = await _traderProfileRepository.GetByIdAsync(request.TraderId, cancellationToken);
            if (traderProfile == null)
                throw new KeyNotFoundException();

            // perform the cash withdrawal
            traderProfile.WithdrawCash(new Money(request.Amount));

            // update the trader profile in the repository
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
