using AuraWealth.BuildingBlocks.Application.Models;
using MediatR;

namespace AuraWealth.MarketData.Application.TradableAssets.Queries.GetAllInstruments
{
    public sealed record GetAllInstrumentsQuery(
    int PageNumber = 1,
    int PageSize = 20) : IRequest<PagedResponse<InstrumentSummaryDto>>;
}
