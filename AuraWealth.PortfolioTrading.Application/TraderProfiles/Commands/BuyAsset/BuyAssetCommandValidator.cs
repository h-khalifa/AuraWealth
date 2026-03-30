using FluentValidation;

namespace AuraWealth.PortfolioTrading.Application.TraderProfiles.Commands.BuyAsset
{
    public class BuyAssetCommandValidator : AbstractValidator<BuyAssetCommand>
    {
        public BuyAssetCommandValidator()
        {
            RuleFor(x => x.TraderId)
                .NotEmpty().WithMessage("Trader ID is required.");

            RuleFor(x => x.TickerSymbol)
                .NotEmpty().WithMessage("Ticker symbol is required.")
                .Length(1, 5).WithMessage("Ticker symbol must be between 1 and 5 characters.")
                .Must(symbol => symbol.All(char.IsLetter)).WithMessage("Ticker symbol must contain only letters.")
                .Must(symbol => symbol == symbol.ToUpper()).WithMessage("Ticker symbol must be uppercase.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than zero.");


        }
    }
}
