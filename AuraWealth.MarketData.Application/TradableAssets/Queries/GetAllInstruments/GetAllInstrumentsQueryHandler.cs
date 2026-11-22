using AuraWealth.BuildingBlocks.Application.Interfaces;
using AuraWealth.BuildingBlocks.Application.Models;
using MediatR;

namespace AuraWealth.MarketData.Application.TradableAssets.Queries.GetAllInstruments;

public sealed class GetAllInstrumentsQueryHandler(
    IReadConnection _read) : IRequestHandler<GetAllInstrumentsQuery, PagedResponse<InstrumentSummaryDto>>
{
    public async Task<PagedResponse<InstrumentSummaryDto>> Handle(
        GetAllInstrumentsQuery request,
        CancellationToken cancellationToken)
    {
        var pageNumber = Math.Max(1, request.PageNumber);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);
        var offset = (pageNumber - 1) * pageSize;

        const string countSql = """
            SELECT COUNT(*)
            FROM market_data."TradableAssets";
            """;

        const string itemsSql = """
            SELECT
                "Id",
                "Symbol",
                "Name",
                "AssetClass",
                "Status",
                "Quote_Price"         AS "Price",
                "Quote_Currency"      AS "CurrencyCode",
                "Quote_ObservedAtUtc" AS "ObservedAtUtc"
            FROM market_data."TradableAssets"
            ORDER BY "Symbol" ASC
            LIMIT @PageSize OFFSET @Offset;
            """;

        var totalCount = await _read.QueryFirstOrDefaultAsync<int>(countSql);
        var items = await _read.QueryAsync<InstrumentSummaryDto>(itemsSql, new { PageSize = pageSize, Offset = offset });

        return new PagedResponse<InstrumentSummaryDto>(
            items,
            totalCount,
            pageNumber,
            pageSize);
    }
}