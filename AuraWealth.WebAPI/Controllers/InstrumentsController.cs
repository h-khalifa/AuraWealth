using AuraWealth.MarketData.Application.TradableAssets.Commands.ChangeInstrumentStatus;
using AuraWealth.MarketData.Application.TradableAssets.Commands.ListInstrument;
using AuraWealth.MarketData.Application.TradableAssets.Commands.UpdateInstrumentQuote;
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
}

public sealed record ListInstrumentRequest(string Symbol, string Name, AssetClass AssetClass);
public sealed record UpdateQuoteRequest(decimal Price, string? CurrencyCode = "USD", DateTime? ObservedAtUtc = null);
public sealed record ChangeInstrumentStatusRequest(ListingStatus NewStatus);