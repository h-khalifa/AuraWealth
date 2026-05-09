namespace AuraWealth.PortfolioTrading.Application.TraderProfiles.Queries.GetPortfolioSummary
{
    public record PortfolioSummaryDto(Guid TraderId, decimal cashBalance, List<AssetHoldingDto> Holdings);

    public record AssetHoldingDto(string Symbol, decimal Quantity);
}
