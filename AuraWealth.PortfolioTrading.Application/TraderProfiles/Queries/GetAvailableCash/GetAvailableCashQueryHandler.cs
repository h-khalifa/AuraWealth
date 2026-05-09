using AuraWealth.PortfolioTrading.Application.Common.Interfaces;
using MediatR;

namespace AuraWealth.PortfolioTrading.Application.TraderProfiles.Queries.GetAvailableCash
{
    public class GetAvailableCashQueryHandler : IRequestHandler<GetAvailableCashQuery, GetAvailableCashDto>
    {
        private readonly IReadConnection _read;
        public GetAvailableCashQueryHandler(IReadConnection read)
        {
            _read = read;
        }
        public async Task<GetAvailableCashDto> Handle(GetAvailableCashQuery request, CancellationToken cancellationToken)
        {
            var cashBalance = await _read.QueryFirstOrDefaultAsync<decimal>("SELECT CashBalance FROM TraderProfiles WHERE Id = @TraderId", new { request.TraderId });
            return new GetAvailableCashDto(request.TraderId, cashBalance);
        }
    }
}
