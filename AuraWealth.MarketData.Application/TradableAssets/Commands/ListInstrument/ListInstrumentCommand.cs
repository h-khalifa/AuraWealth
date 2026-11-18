using AuraWealth.MarketData.Domain.TradableAssets.Enums;
using MediatR;

namespace AuraWealth.MarketData.Application.TradableAssets.Commands.ListInstrument;

public sealed record ListInstrumentCommand(
    string Symbol,
    string Name,
    AssetClass AssetClass) : IRequest<Guid>;