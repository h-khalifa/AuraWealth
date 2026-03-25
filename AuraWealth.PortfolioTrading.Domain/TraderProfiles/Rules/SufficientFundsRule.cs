using AuraWealth.BuildingBlocks.Domain;

namespace AuraWealth.PortfolioTrading.Domain.TraderProfiles.Rules
{
    public class SufficientFundsRule : IBusinessRule
    {
        private readonly decimal _availableQuantity;
        private readonly decimal _amount;
        public SufficientFundsRule(decimal availableQuantity, decimal amount)
        {
            _availableQuantity = availableQuantity;
            _amount = amount;
        }
        
        public string Message => "InSufficient Cash Balance.";

        public bool IsBroken()
        {
            return _availableQuantity < _amount;
        }
    }
}
