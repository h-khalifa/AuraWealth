namespace AuraWealth.PortfolioTrading.Application.TraderProfiles.ReadModels
{
    public class TransactionRecord
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public Guid TraderId { get; init; }
        public string TransactionType { get; init; }
        public string Symbol { get; init; }
        public decimal Quantity { get; init; }
        public decimal PricePerUnit { get; init; }
        public decimal TotalAmount { get; init; }
        public DateTime OccurredOn { get; init; }
    }
}
