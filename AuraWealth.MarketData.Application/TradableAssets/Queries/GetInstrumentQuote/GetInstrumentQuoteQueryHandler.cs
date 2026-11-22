using AuraWealth.BuildingBlocks.Application.Interfaces;
using AuraWealth.MarketData.Domain.Services;
using AuraWealth.MarketData.Domain.TradableAssets.Enums;
using MediatR;

namespace AuraWealth.MarketData.Application.TradableAssets.Queries.GetInstrumentQuote;


public sealed class GetInstrumentQuoteQueryHandler(
    IReadConnection _read,
    IMarketSessionService sessionService,
    TimeProvider timeProvider) : IRequestHandler<GetInstrumentQuoteQuery, InstrumentQuoteDto>
{
    public async Task<InstrumentQuoteDto> Handle(GetInstrumentQuoteQuery request, CancellationToken cancellationToken)
    {
        var normalizedSymbol = request.Symbol.Trim().ToUpperInvariant();

        const string sql = """
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
            WHERE "Symbol" = @Symbol;
            """;

        var row = await _read.QueryFirstOrDefaultAsync<InstrumentRow>(sql, new { Symbol = normalizedSymbol })
            ?? throw new KeyNotFoundException($"Instrument '{normalizedSymbol}' was not found.");

        var utcNow = timeProvider.GetUtcNow().UtcDateTime;
        var assetClass = Enum.Parse<AssetClass>(row.AssetClass);
        var session = sessionService.GetCurrentSession(utcNow, assetClass);
        var maxAge = sessionService.GetMaxQuoteAge(session);

        var isFresh = row.ObservedAtUtc.HasValue
            && (utcNow - DateTime.SpecifyKind(row.ObservedAtUtc.Value, DateTimeKind.Utc)) <= maxAge;

        var canTrade = row.Status == nameof(ListingStatus.Active)
            && row.Price.HasValue
            && isFresh;

        return new InstrumentQuoteDto(
            row.Id,
            row.Symbol,
            row.Name,
            row.AssetClass,
            row.Status,
            row.Price,
            row.CurrencyCode,
            row.ObservedAtUtc,
            session.ToString(),
            isFresh,
            canTrade);
    }

    private sealed record InstrumentRow(
        Guid Id,
        string Symbol,
        string Name,
        string AssetClass,
        string Status,
        decimal? Price,
        string? CurrencyCode,
        DateTime? ObservedAtUtc);
}