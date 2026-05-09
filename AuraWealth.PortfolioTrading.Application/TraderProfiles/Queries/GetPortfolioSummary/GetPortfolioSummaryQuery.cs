using MediatR;

namespace AuraWealth.PortfolioTrading.Application.TraderProfiles.Queries.GetPortfolioSummary
{
    public class GetPortfolioSummaryQuery : IRequest<PortfolioSummaryDto>
    {
        public Guid TraderId { get; set; }
    }
}
