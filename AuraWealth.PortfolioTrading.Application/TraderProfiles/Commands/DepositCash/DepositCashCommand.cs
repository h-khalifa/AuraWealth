using MediatR;

namespace AuraWealth.PortfolioTrading.Application.TraderProfiles.Commands.DepositCash
{
    public class DepositCashCommand : IRequest
    {
        public Guid TraderId { get; set; }
        public decimal Amount { get; set; }
    }
}
