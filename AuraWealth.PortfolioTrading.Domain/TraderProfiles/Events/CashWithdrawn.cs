using AuraWealth.BuildingBlocks.Domain;
using AuraWealth.PortfolioTrading.Domain.Common;

namespace AuraWealth.PortfolioTrading.Domain.TraderProfiles.Events
{
    public record CashWithdrawn(Guid TraderId, Money Amount) : DomainEvent;
}
