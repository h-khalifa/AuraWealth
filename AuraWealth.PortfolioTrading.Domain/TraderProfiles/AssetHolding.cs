using AuraWealth.BuildingBlocks.Domain;
using AuraWealth.PortfolioTrading.Domain.Common;

namespace AuraWealth.PortfolioTrading.Domain.TraderProfiles
{
    public class AssetHolding : Entity
    {
        public TickerSymbol Symbol { get; private set; }
        public decimal Quantity { get; private set; }

        public AssetHolding(TickerSymbol symbol, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount cannot be negative.");
            Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
            Quantity = amount;
        }

        public void AddShares(decimal quantity)
        {
            if (quantity < 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), "Amount to add cannot be negative.");
            Quantity += quantity;
        }

        public void RemoveShares(decimal quantity)
        {
            if (quantity < 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), "Amount to remove cannot be negative.");
            if (quantity > Quantity)
                throw new InvalidOperationException("Cannot remove more shares than currently held.");
            Quantity -= quantity;
        }
    }
}
