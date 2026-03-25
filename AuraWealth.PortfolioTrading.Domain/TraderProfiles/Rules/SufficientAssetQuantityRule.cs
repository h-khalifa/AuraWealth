using AuraWealth.BuildingBlocks.Domain;

namespace AuraWealth.PortfolioTrading.Domain.TraderProfiles.Rules
{
    public class SufficientAssetQuantityRule : IBusinessRule
    {
        private readonly decimal _ownedQuantity;
        private readonly decimal _soldQuantity;
        public string Message => "Insufficient units in your credit";
        public SufficientAssetQuantityRule(decimal ownedQuantity, decimal soldQuantity)
        {
            _ownedQuantity = ownedQuantity;
            _soldQuantity = soldQuantity;
        }
        public bool IsBroken()
        {
            return _ownedQuantity < _soldQuantity;
        }
    }
}
