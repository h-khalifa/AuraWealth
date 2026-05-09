namespace AuraWealth.PortfolioTrading.Application.TraderProfiles.Queries.GetAvailableCash
{
    public record GetAvailableCashDto(Guid TraderId, decimal CashBalance);
}
