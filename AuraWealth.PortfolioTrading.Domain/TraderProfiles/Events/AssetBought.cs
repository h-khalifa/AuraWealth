using AuraWealth.BuildingBlocks.Domain;
using AuraWealth.PortfolioTrading.Domain.Common;

namespace AuraWealth.PortfolioTrading.Domain.TraderProfiles.Events
{
    public record AssetBought(Guid TraderId, TickerSymbol Symbol, decimal Quantity, Money ExecutionPrice) : DomainEvent;
}
