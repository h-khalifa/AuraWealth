using FluentValidation;

namespace AuraWealth.MarketData.Application.TradableAssets.Commands.UpdateInstrumentQuote;

public sealed class UpdateInstrumentQuoteCommandValidator : AbstractValidator<UpdateInstrumentQuoteCommand>
{
    public UpdateInstrumentQuoteCommandValidator()
    {
        RuleFor(x => x.Symbol)
            .NotEmpty().WithMessage("Ticker symbol is required.")
            .Matches("^[a-zA-Z]{1,5}$").WithMessage("Ticker symbol must contain 1 to 5 letters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Quote price must be greater than zero.");

        RuleFor(x => x.CurrencyCode)
            .NotEmpty()
            .Length(3).WithMessage("Currency code must be a 3-letter ISO code.");
    }
}