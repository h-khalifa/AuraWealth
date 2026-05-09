using AuraWealth.PortfolioTrading.Application.Common.Interfaces;
using AuraWealth.PortfolioTrading.Application.Common.Models;
using AuraWealth.PortfolioTrading.Application.TraderProfiles.ReadModels;
using MediatR;

namespace AuraWealth.PortfolioTrading.Application.TraderProfiles.Queries.GetTransactionHistory
{
    public class GetTransactionHistoryQueryHandler : IRequestHandler<GetTransactionHistoryQuery, PagedResponse<TransactionRecord>>
    {
        private readonly ITransactionRecordRepository _transactionRecordRepository;
        public GetTransactionHistoryQueryHandler(ITransactionRecordRepository transactionRecordRepository)
        {
            _transactionRecordRepository = transactionRecordRepository;
        }
        public async Task<PagedResponse<TransactionRecord>> Handle(GetTransactionHistoryQuery request, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _transactionRecordRepository.GetPagedByTraderIdAsync(request.TraderId, request.PageNumber, request.PageSize, cancellationToken);
            return new PagedResponse<TransactionRecord>(items, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
