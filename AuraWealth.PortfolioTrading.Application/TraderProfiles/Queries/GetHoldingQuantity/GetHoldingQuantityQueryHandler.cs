using AuraWealth.PortfolioTrading.Application.Common.Interfaces;

namespace AuraWealth.PortfolioTrading.Application.TraderProfiles.Queries.GetHoldingQuantity
{
    public class GetHoldingQuantityQueryHandler : MediatR.IRequestHandler<GetHoldingQuantityQuery, AssetHoldingDto>
    {
        private readonly IReadConnection _read;
        public GetHoldingQuantityQueryHandler(IReadConnection read) { _read = read; }
        public async Task<AssetHoldingDto> Handle(GetHoldingQuantityQuery request, CancellationToken cancellationToken)
        {
            var quantitySql = "SELECT Quantity FROM AssetHoldings WHERE TraderProfileId = @TraderId and Symbol = @Symbol";
            var quantity = await _read.QueryFirstOrDefaultAsync<decimal>(quantitySql, new { request.TraderId, request.Symbol });
            return new AssetHoldingDto(request.Symbol, quantity);
        }
    }
}
