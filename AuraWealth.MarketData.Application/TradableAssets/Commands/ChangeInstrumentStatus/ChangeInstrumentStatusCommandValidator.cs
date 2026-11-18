using FluentValidation;

namespace AuraWealth.MarketData.Application.TradableAssets.Commands.ChangeInstrumentStatus;

public sealed class ChangeInstrumentStatusCommandValidator : AbstractValidator<ChangeInstrumentStatusCommand>
{
    public ChangeInstrumentStatusCommandValidator()
    {
        RuleFor(x => x.Symbol)
            .NotEmpty().WithMessage("Ticker symbol is required.")
            .Matches("^[a-zA-Z]{1,5}$").WithMessage("Ticker symbol must contain 1 to 5 letters.");

        RuleFor(x => x.NewStatus)
            .IsInEnum().WithMessage("Invalid listing status specified.");
    }
}