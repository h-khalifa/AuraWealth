using MediatR;

namespace AuraWealth.PortfolioTrading.Application.TraderProfiles.Queries.GetAvailableCash
{
    public class GetAvailableCashQuery : IRequest<GetAvailableCashDto>
    {
        public Guid TraderId { get; set; }
    }
}
