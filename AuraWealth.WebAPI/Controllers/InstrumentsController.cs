using AuraWealth.BuildingBlocks.Application.Models;
using AuraWealth.MarketData.Application.TradableAssets.Commands.ChangeInstrumentStatus;
using AuraWealth.MarketData.Application.TradableAssets.Commands.ListInstrument;
using AuraWealth.MarketData.Application.TradableAssets.Commands.UpdateInstrumentQuote;
using AuraWealth.MarketData.Application.TradableAssets.Queries.GetAllInstruments;
using AuraWealth.MarketData.Application.TradableAssets.Queries.GetInstrumentQuote;
using AuraWealth.MarketData.Application.TradableAssets.Queries.GetPriceHistory;
using AuraWealth.MarketData.Domain.TradableAssets.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuraWealth.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class InstrumentsController(ISender sender) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ListInstrument(
        [FromBody] ListInstrumentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ListInstrumentCommand(
            request.Symbol,
            request.Name,
            request.AssetClass);

        var id = await sender.Send(command, cancellationToken);
        return Created($"/api/instruments/{request.Symbol.Trim().ToUpperInvariant()}", id);
    }

    [HttpPost("{symbol}/quote")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateQuote(
        string symbol,
        [FromBody] UpdateQuoteRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateInstrumentQuoteCommand(
            symbol,
            request.Price,
            request.CurrencyCode ?? "USD",
            request.ObservedAtUtc);

        await sender.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{symbol}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ChangeStatus(
        string symbol,
        [FromBody] ChangeInstrumentStatusRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ChangeInstrumentStatusCommand(symbol, request.NewStatus);
        await sender.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<InstrumentSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetAllInstrumentsQuery(pageNumber, pageSize), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{symbol}")]
    [ProducesResponseType(typeof(InstrumentQuoteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetQuote(
        string symbol,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetInstrumentQuoteQuery(symbol), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{symbol}/history")]
    [ProducesResponseType(typeof(PagedResponse<PriceTickDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHistory(
    string symbol,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 20,
    CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetPriceHistoryQuery(symbol, pageNumber, pageSize), cancellationToken);
        return Ok(result);
    }
}

public sealed record ListInstrumentRequest(string Symbol, string Name, AssetClass AssetClass);
public sealed record UpdateQuoteRequest(decimal Price, string? CurrencyCode = "USD", DateTime? ObservedAtUtc = null);
public sealed record ChangeInstrumentStatusRequest(ListingStatus NewStatus);