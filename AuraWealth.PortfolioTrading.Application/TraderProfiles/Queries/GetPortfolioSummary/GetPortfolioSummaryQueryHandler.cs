using AuraWealth.PortfolioTrading.Application.Common.Interfaces;
using MediatR;

namespace AuraWealth.PortfolioTrading.Application.TraderProfiles.Queries.GetPortfolioSummary
{
    public class GetPortfolioSummaryQueryHandler : IRequestHandler<GetPortfolioSummaryQuery, PortfolioSummaryDto>
    {
        private readonly IReadConnection _read;
        public GetPortfolioSummaryQueryHandler(IReadConnection read)
        {
            _read = read;
        }
        public async Task<PortfolioSummaryDto> Handle(GetPortfolioSummaryQuery request, CancellationToken cancellationToken)
        {

            // 1. Get the Cash Balance from the TraderProfiles table
            var cashBalanceSql = "SELECT CashBalance_Amount FROM TraderProfiles WHERE Id = @TraderId";
            var cashBalance = await _read.QueryFirstOrDefaultAsync<decimal>(cashBalanceSql, new { request.TraderId });

            // 2. Get the Holdings from the AssetHoldings table
            var holdingsSql = "SELECT Symbol, Quantity FROM AssetHoldings WHERE TraderProfileId = @TraderId";
            var holdings = await _read.QueryAsync<AssetHoldingDto>(holdingsSql, new { request.TraderId });
            // 3. Return the assembled DTO
            return new PortfolioSummaryDto(request.TraderId, cashBalance, holdings.ToList());
        }
    }
}
