using AuraWealth.BuildingBlocks.Application.Interfaces;
using AuraWealth.BuildingBlocks.Application.Models;
using MediatR;

namespace AuraWealth.MarketData.Application.TradableAssets.Queries.GetPriceHistory;

public sealed class GetPriceHistoryQueryHandler(
    IReadConnection _read) : IRequestHandler<GetPriceHistoryQuery, PagedResponse<PriceTickDto>>
{
    public async Task<PagedResponse<PriceTickDto>> Handle(
        GetPriceHistoryQuery request,
        CancellationToken cancellationToken)
    {
        var normalizedSymbol = request.Symbol.Trim().ToUpperInvariant();
        var pageNumber = Math.Max(1, request.PageNumber);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);
        var offset = (pageNumber - 1) * pageSize;

        const string countSql = """
            SELECT COUNT(*)
            FROM market_data."PriceTicks"
            WHERE "Symbol" = @Symbol;
            """;

        const string itemsSql = """
            SELECT
                "Id",
                "Price",
                "CurrencyCode",
                "ObservedAtUtc"
            FROM market_data."PriceTicks"
            WHERE "Symbol" = @Symbol
            ORDER BY "ObservedAtUtc" DESC
            LIMIT @PageSize OFFSET @Offset;
            """;

        var totalCount = await _read.QueryFirstOrDefaultAsync<int>(
            countSql,
            new { Symbol = normalizedSymbol });

        var items = await _read.QueryAsync<PriceTickDto>(
            itemsSql,
            new { Symbol = normalizedSymbol, PageSize = pageSize, Offset = offset });

        return new PagedResponse<PriceTickDto>(
            items,
            totalCount,
            pageNumber,
            pageSize);
    }
}