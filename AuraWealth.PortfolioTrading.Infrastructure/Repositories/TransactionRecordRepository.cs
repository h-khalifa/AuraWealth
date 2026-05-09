using AuraWealth.PortfolioTrading.Application.Common.Interfaces;
using AuraWealth.PortfolioTrading.Application.TraderProfiles.ReadModels;
using AuraWealth.PortfolioTrading.Infrastructure.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace AuraWealth.PortfolioTrading.Infrastructure.Repositories
{
    internal class TransactionRecordRepository : ITransactionRecordRepository
    {
        private readonly PortfolioDbContext _dbContext;

        public TransactionRecordRepository(PortfolioDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(TransactionRecord record, CancellationToken cancellationToken)
        {
            await _dbContext.TransactionRecords.AddAsync(record, cancellationToken);
        }

        public async Task<(IEnumerable<TransactionRecord> Items, int TotalCount)> GetPagedByTraderIdAsync(Guid traderId, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var connection = _dbContext.Database.GetDbConnection();
            var offset = (pageNumber - 1) * pageSize;

            // Notice the two SQL statements separated by a semicolon
            var sql = @"
                SELECT COUNT(*) FROM TransactionRecords WHERE TraderId = @TraderId;

                SELECT Id, TraderId, OccurredOn, TransactionType, Symbol, Quantity, PricePerUnit
                FROM TransactionRecords
                WHERE TraderId = @TraderId
                ORDER BY OccurredOn DESC
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

            // QueryMultipleAsync executes both statements in ONE database round-trip
            using var multi = await connection.QueryMultipleAsync(
                sql,
                new { TraderId = traderId, Offset = offset, PageSize = pageSize });

            // Read them in the exact order they were requested
            var totalCount = await multi.ReadFirstAsync<int>();
            var items = await multi.ReadAsync<TransactionRecord>();

            return (items, totalCount);
        }
    }
}
