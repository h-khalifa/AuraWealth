using AuraWealth.BuildingBlocks.Domain;
using AuraWealth.PortfolioTrading.Application.Common.Interfaces;
using AuraWealth.PortfolioTrading.Domain.Common;
using AuraWealth.PortfolioTrading.Domain.TraderProfiles;
using MediatR;

namespace AuraWealth.PortfolioTrading.Application.TraderProfiles.Commands.BuyAsset
{
    public class BuyAssetCommandHandler : IRequestHandler<BuyAssetCommand>
    {
        private readonly IDomainRepository<TraderProfile> _traderProfileRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BuyAssetCommandHandler(IDomainRepository<TraderProfile> traderProfileRepository, IUnitOfWork unitOfWork)
        {
            _traderProfileRepository = traderProfileRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(BuyAssetCommand request, CancellationToken cancellationToken)
        {
            //load the trader profile
            var traderProfile = await _traderProfileRepository.GetByIdAsync(request.TraderId, cancellationToken);
            if (traderProfile == null) 
                throw new KeyNotFoundException();

            //perform the buy asset operation
            traderProfile.BuyAsset(new TickerSymbol(request.TickerSymbol), request.Quantity, new Money(request.UnitPrice));

            //save changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
