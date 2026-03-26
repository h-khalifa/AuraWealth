using AuraWealth.BuildingBlocks.Domain;
using AuraWealth.PortfolioTrading.Domain.Common;

namespace AuraWealth.PortfolioTrading.Domain.TraderProfiles.Events
{
    public record CashDeposited(Guid TraderId, Money Amount) : DomainEvent;
}
