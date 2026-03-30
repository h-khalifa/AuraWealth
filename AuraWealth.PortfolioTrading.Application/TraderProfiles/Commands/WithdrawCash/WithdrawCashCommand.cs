using MediatR;

namespace AuraWealth.PortfolioTrading.Application.TraderProfiles.Commands.WithdrawCash
{
    public class WithdrawCashCommand : IRequest
    {
        public Guid TraderId { get; set; }
        public decimal Amount { get; set; }
    }
}
