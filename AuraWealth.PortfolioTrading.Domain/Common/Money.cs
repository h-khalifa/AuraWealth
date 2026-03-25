using AuraWealth.BuildingBlocks.Domain;

namespace AuraWealth.PortfolioTrading.Domain.Common
{
    public class Money : ValueObject
    {
        public string CurrencyCode { get { return "USD"; }  }
        public decimal Amount { get; }
        public Money(decimal amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            Amount = amount;
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return CurrencyCode;
            yield return Amount;
        }

        public Money DepositMoney(Money deposit)
        { 
            if (deposit == null)
                throw new ArgumentNullException(nameof(deposit));
            if (!CurrencyCode.Equals(deposit.CurrencyCode, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Currency codes do not match for deposit.");

            return new Money(Amount + deposit.Amount);
        }

        public Money WithdrawMoney(Money withdraw)
        {
            if (withdraw == null)
                throw new ArgumentNullException(nameof(withdraw));
            if (!CurrencyCode.Equals(withdraw.CurrencyCode, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Currency codes do not match for withdrawal.");
            if (withdraw.Amount > Amount)
                throw new InvalidOperationException("Insufficient funds for withdrawal.");

            return new Money(Amount - withdraw.Amount);

        }
    }
}
