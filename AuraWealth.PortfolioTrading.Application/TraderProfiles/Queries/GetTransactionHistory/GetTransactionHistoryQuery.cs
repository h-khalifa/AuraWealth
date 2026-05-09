using AuraWealth.PortfolioTrading.Application.Common.Models;
using AuraWealth.PortfolioTrading.Application.TraderProfiles.ReadModels;
using MediatR;

namespace AuraWealth.PortfolioTrading.Application.TraderProfiles.Queries.GetTransactionHistory
{
    public class GetTransactionHistoryQuery : IRequest<PagedResponse<TransactionRecord>>
    {
        public Guid TraderId { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
    }
}
