using AuraWealth.BuildingBlocks.Application.Models;
using MediatR;

namespace AuraWealth.MarketData.Application.TradableAssets.Queries.GetPriceHistory
{
    public sealed record GetPriceHistoryQuery(
        string Symbol,
        int PageNumber = 1,
        int PageSize = 20) : IRequest<PagedResponse<PriceTickDto>>;
}
