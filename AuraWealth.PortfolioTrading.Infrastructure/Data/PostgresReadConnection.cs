using AuraWealth.PortfolioTrading.Application.Common.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace AuraWealth.PortfolioTrading.Infrastructure.Data
{
    public class PostgresReadConnection : IReadConnection
    {
        private readonly NpgsqlConnection _connection;

        public PostgresReadConnection(IConfiguration configuration)
        {
            // Assuming your connection string is named "AuraWealthDb"
            var connectionString = configuration.GetConnectionString("AuraWealthDb")
                                ?? throw new InvalidOperationException("Connection string missing.");
            _connection = new NpgsqlConnection(connectionString);
        }
        public async Task<int> ExecuteAsync(string sql, object? param = null)
        {
            await EnsureOpenAsync();
            return await _connection.ExecuteAsync(sql, param);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null)
        {
            await EnsureOpenAsync();
            return await _connection.QueryAsync<T>(sql, param);
        }

        public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null)
        {
            await EnsureOpenAsync();
            return await _connection.QueryFirstOrDefaultAsync<T>(sql, param);
        }

        private async Task EnsureOpenAsync()
        {
            if (_connection.State != ConnectionState.Open)
                await _connection.OpenAsync();
        }

        public void Dispose() => _connection.Dispose();
    }
}
