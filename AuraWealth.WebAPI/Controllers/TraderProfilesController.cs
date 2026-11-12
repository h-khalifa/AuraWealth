using AuraWealth.BuildingBlocks.IntegrationEvents;
using AuraWealth.PortfolioTrading.Application.TraderProfiles.Commands.BuyAsset;
using AuraWealth.PortfolioTrading.Application.TraderProfiles.Commands.DepositCash;
using AuraWealth.PortfolioTrading.Application.TraderProfiles.Commands.SellAsset;
using AuraWealth.PortfolioTrading.Application.TraderProfiles.Commands.WithdrawCash;
using AuraWealth.PortfolioTrading.Application.TraderProfiles.Queries.GetAvailableCash;
using AuraWealth.PortfolioTrading.Application.TraderProfiles.Queries.GetPortfolioSummary;
using AuraWealth.PortfolioTrading.Application.TraderProfiles.Queries.GetTransactionHistory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuraWealth.WebAPI.Controllers
{
    [Route("api/trader-profiles")]
    [ApiController]
    public class TraderProfilesController : ControllerBase
    {
        private readonly IMediator _mediator;

        // 🔥 The Simulated Identity: 
        // Until the Auth Module is built, we pretend this user is logged in.
        private readonly Guid _simulatedUserId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");

        public TraderProfilesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[HttpPost]
        //public async Task<IActionResult> OpenProfile(CancellationToken cancellationToken)
        //{
        //    // We pass the simulated user ID into the Application layer
        //    // The Application layer has NO IDEA this isn't a real authenticated user.
        //    var command = new OpenTraderProfileCommand(_simulatedUserId);

        //    var profileId = await _mediator.Send(command, cancellationToken);

        //    return Ok(new { TraderProfileId = profileId });
        //}

        //[HttpGet("my-history")]
        //public async Task<IActionResult> GetTransactionHistory(CancellationToken cancellationToken)
        //{
        //    // Execute the blazingly fast Dapper read model query
        //    var query = new GetTransactionHistoryQuery() { TraderId = _simulatedUserId };

        //    var history = await _mediator.Send(query, cancellationToken);

        //    return Ok(history);
        //}

        #region temp
        [HttpPost("simulate-registration")]
        public async Task<IActionResult> SimulateUserRegistration(CancellationToken cancellationToken)
        {
            // Simulate the Identity module publishing the integration event
            var integrationEvent = new UserRegisteredIntegrationEvent() { UserId = _simulatedUserId };

            // Publish triggers your UserRegisteredIntegrationEventHandler
            await _mediator.Publish(integrationEvent, cancellationToken);

            return Ok(new { Message = "Simulated user profile created." });
        }
        #endregion

        #region Commands
        [HttpPost("deposit")]
        public async Task<IActionResult> DepositCash([FromBody] decimal amount, CancellationToken cancellationToken)
        {
            var command = new DepositCashCommand() { TraderId = _simulatedUserId, Amount = amount };
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }
        [HttpPost("withdraw")]
        public async Task<IActionResult> WithdrawCash([FromBody] decimal amount, CancellationToken cancellationToken)
        {
            var command = new WithdrawCashCommand() { TraderId = _simulatedUserId, Amount = amount };
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }
        [HttpPost("buy")]
        public async Task<IActionResult> BuyAsset([FromBody] BuyAssetRequest request, CancellationToken cancellationToken)
        {
            var command = new BuyAssetCommand() { TraderId = _simulatedUserId, TickerSymbol = request.TickerSymbol, Quantity = request.Quantity, UnitPrice = request.Price };
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }
        [HttpPost("sell")]
        public async Task<IActionResult> SellAsset([FromBody] SellAssetRequest request, CancellationToken cancellationToken)
        {
            var command = new SellAssetCommand() { TraderId = _simulatedUserId, TickerSymbol = request.TickerSymbol, Quantity = request.Quantity, UnitPrice = request.Price };
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }
        #endregion
        #region Queries
        [HttpGet("summary")]
        public async Task<IActionResult> GetPortfolioSummary(CancellationToken cancellationToken)
        {
            var query = new GetPortfolioSummaryQuery() { TraderId = _simulatedUserId };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
        [HttpGet("history")]
        public async Task<IActionResult> GetTransactionHistory(CancellationToken cancellationToken)
        {
            var query = new GetTransactionHistoryQuery() { TraderId = _simulatedUserId };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
        [HttpGet("cash")]
        public async Task<IActionResult> GetAvailableCash(CancellationToken cancellationToken)
        {
            var query = new GetAvailableCashQuery() { TraderId = _simulatedUserId };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
        //remaining query for holding quantity can be added here in the future if needed
        #endregion
    }

    // Temporary DTOs for the API requests (these could also live in an API Contracts folder)
    public record BuyAssetRequest(string TickerSymbol, decimal Quantity, decimal Price);
    public record SellAssetRequest(string TickerSymbol, decimal Quantity, decimal Price);
}
