using FluentValidation;

namespace AuraWealth.PortfolioTrading.Application.TraderProfiles.Commands.DepositCash
{
    public class DepositCashCommandValidator : AbstractValidator<DepositCashCommand>
    {
        public DepositCashCommandValidator()
        {
            RuleFor(x => x.TraderId)
                .NotEmpty().WithMessage("TraderId is required.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");
        }
    }
}
