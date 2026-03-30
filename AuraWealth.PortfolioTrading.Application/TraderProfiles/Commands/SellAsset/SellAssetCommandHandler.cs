using AuraWealth.BuildingBlocks.Domain;
using AuraWealth.PortfolioTrading.Application.Common.Interfaces;
using AuraWealth.PortfolioTrading.Domain.Common;
using AuraWealth.PortfolioTrading.Domain.TraderProfiles;
using MediatR;

namespace AuraWealth.PortfolioTrading.Application.TraderProfiles.Commands.SellAsset
{
    public class SellAssetCommandHandler : IRequestHandler<SellAssetCommand>
    {
        private readonly IDomainRepository<TraderProfile> _traderProfileRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SellAssetCommandHandler(IDomainRepository<TraderProfile> traderProfileRepository, IUnitOfWork unitOfWork)
        {
            _traderProfileRepository = traderProfileRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(SellAssetCommand request, CancellationToken cancellationToken)
        {
            //load the trader profile
            var traderProfile = await _traderProfileRepository.GetByIdAsync(request.TraderId, cancellationToken);
            if (traderProfile == null)
                throw new KeyNotFoundException();

            //execute the sell asset operation
            traderProfile.SellAsset(new TickerSymbol(request.TickerSymbol), request.Quantity, new Money(request.UnitPrice));

            //persist the changes
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
