namespace AuraWealth.PortfolioTrading.Application.TraderProfiles.Queries.GetHoldingQuantity
{
    public class GetHoldingQuantityQuery : MediatR.IRequest<AssetHoldingDto>
    {
        public Guid TraderId { get; set; }
        public string Symbol { get; set; }
    }
}
