using FluentValidation;

namespace AuraWealth.MarketData.Application.TradableAssets.Commands.ListInstrument;

public sealed class ListInstrumentCommandValidator : AbstractValidator<ListInstrumentCommand>
{
    public ListInstrumentCommandValidator()
    {
        RuleFor(x => x.Symbol)
            .NotEmpty().WithMessage("Ticker symbol is required.")
            .MaximumLength(5).WithMessage("Ticker symbol cannot exceed 5 characters.")
            .Matches("^[a-zA-Z]{1,5}$").WithMessage("Ticker symbol must contain 1 to 5 letters.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Instrument name is required.")
            .MaximumLength(200).WithMessage("Instrument name cannot exceed 200 characters.");

        RuleFor(x => x.AssetClass)
            .IsInEnum().WithMessage("Invalid asset class specified.");
    }
}