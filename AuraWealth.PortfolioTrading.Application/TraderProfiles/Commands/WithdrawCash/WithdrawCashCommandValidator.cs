using FluentValidation;

namespace AuraWealth.PortfolioTrading.Application.TraderProfiles.Commands.WithdrawCash
{
    public class WithdrawCashCommandValidator : AbstractValidator<WithdrawCashCommand>
    {
        public WithdrawCashCommandValidator()
        {
            RuleFor(x => x.TraderId)
                .NotEmpty().WithMessage("TraderId is required.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");
        }
    }
}
