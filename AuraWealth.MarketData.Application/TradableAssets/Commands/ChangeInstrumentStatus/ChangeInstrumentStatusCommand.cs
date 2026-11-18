using AuraWealth.MarketData.Domain.TradableAssets.Enums;
using MediatR;

namespace AuraWealth.MarketData.Application.TradableAssets.Commands.ChangeInstrumentStatus;

public sealed record ChangeInstrumentStatusCommand(
    string Symbol,
    ListingStatus NewStatus) : IRequest;