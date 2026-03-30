using MediatR;

namespace AuraWealth.PortfolioTrading.Application.TraderProfiles.Commands.BuyAsset
{
    public class BuyAssetCommand : IRequest
    {
        public Guid TraderId { get; set; }
        public required string TickerSymbol { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
