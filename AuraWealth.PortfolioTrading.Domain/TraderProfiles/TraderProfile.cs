using AuraWealth.BuildingBlocks.Domain;
using AuraWealth.PortfolioTrading.Domain.Common;
using AuraWealth.PortfolioTrading.Domain.TraderProfiles.Events;
using AuraWealth.PortfolioTrading.Domain.TraderProfiles.Rules;

namespace AuraWealth.PortfolioTrading.Domain.TraderProfiles
{
    public class TraderProfile : Entity, IAggregateRoot
    {
        public Money CashBalance { get; private set; }
        
        private readonly List<AssetHolding> _assetHoldings = new();
        public IReadOnlyCollection<AssetHolding> AssetHoldings => _assetHoldings.AsReadOnly();

        private TraderProfile()
        {
            
        }
        public TraderProfile(Guid traderId)
        {
            Id = traderId;
            CashBalance = new Money(0); // Default to 0 USD, can be modified later
        }

        public void DepositCash(Money amount)
        {
            if (CashBalance.CurrencyCode != amount.CurrencyCode)
                throw new InvalidOperationException("Currency mismatch between current balance and deposit amount.");
            //1. Update cash balance
            CashBalance = CashBalance.DepositMoney(amount);
            //2. Raise domain event
            AddDomainEvent(new CashDeposited(Id, amount));
        }

        public void WithdrawCash(Money amount)
        {
            if (CashBalance.CurrencyCode != amount.CurrencyCode)
                throw new InvalidOperationException("Currency mismatch between current balance and withdrawal amount.");
            
            CheckRule(new SufficientFundsRule(CashBalance.Amount, amount.Amount));

            //1. Update cash balance
            CashBalance = CashBalance.WithdrawMoney(amount);
            //2. Raise domain event
            AddDomainEvent(new CashWithdrawn(Id, amount));
        }

        public void BuyAsset(TickerSymbol symbol, decimal quantity, Money pricePerUnit)
        {
            if (pricePerUnit.CurrencyCode != CashBalance.CurrencyCode)
                throw new InvalidOperationException("Currency mismatch between current balance and price per unit.");

            var totalCost = new Money(pricePerUnit.Amount * quantity);

            CheckRule(new SufficientFundsRule(CashBalance.Amount, totalCost.Amount));

            //1. Update holdings
            var assetHolding = AssetHoldings.FirstOrDefault(h => h.Symbol == symbol);
            if (assetHolding != null)
            {
                assetHolding.AddShares(quantity);
            }
            else
            {
                _assetHoldings.Add(new AssetHolding ( symbol, quantity ));
            }
            
            //2. Update cash balance
            CashBalance = CashBalance.WithdrawMoney(totalCost);

            //3. Raise domain event
            AddDomainEvent(new AssetBought(Id, symbol, quantity, pricePerUnit));
        }

        public void SellAsset(TickerSymbol symbol, decimal quantity, Money pricePerUnit)
        {
            if (CashBalance.CurrencyCode != pricePerUnit.CurrencyCode)
                throw new InvalidOperationException("Currency mismatch between current balance and price per unit.");
            var assetHolding = AssetHoldings.FirstOrDefault(h => h.Symbol == symbol);
            if (assetHolding is null)
                throw new InvalidOperationException("Asset not found in holdings.");

            CheckRule(new SufficientAssetQuantityRule(assetHolding.Quantity, quantity));

            //1. Update holdings
            if (assetHolding.Quantity == quantity)
            {
                _assetHoldings.Remove(assetHolding);
            }
            else
            {
                assetHolding.RemoveShares(quantity);
            }
            //2. Update cash balance
            var totalProceeds = pricePerUnit.Amount * quantity;
            CashBalance = CashBalance.DepositMoney(new Money(totalProceeds));
            //3. Raise domain event
            AddDomainEvent(new AssetSold(Id, symbol, quantity, pricePerUnit));
        }

    }
}
