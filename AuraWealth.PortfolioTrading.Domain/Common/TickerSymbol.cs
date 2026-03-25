using AuraWealth.BuildingBlocks.Domain;

namespace AuraWealth.PortfolioTrading.Domain.Common
{
    public class TickerSymbol : ValueObject
    {
        public string Symbol { get; private set; }
        public TickerSymbol(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException("Ticker symbol cannot be null or empty.", nameof(symbol));

            // 1. Auto-format to uppercase and trim spaces
            symbol = symbol.Trim().ToUpperInvariant();

            // 2. Enforce length (1 to 5 characters)
            if (symbol.Length < 1 || symbol.Length > 5)
                throw new ArgumentException("Ticker symbol must be between 1 and 5 characters.", nameof(symbol));

            // 3. Enforce letters only (All characters must be letters)
            if (!symbol.All(char.IsLetter))
                throw new ArgumentException("Ticker symbol can only contain letters.", nameof(symbol));

            Symbol = symbol;
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Symbol;
        }
    }
}
