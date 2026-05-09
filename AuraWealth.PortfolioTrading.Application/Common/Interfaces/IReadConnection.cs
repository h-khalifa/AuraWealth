namespace AuraWealth.PortfolioTrading.Application.Common.Interfaces
{
    public interface IReadConnection
    {
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null);
        Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null);
        Task<int> ExecuteAsync(string sql, object? param = null);
    }
}
