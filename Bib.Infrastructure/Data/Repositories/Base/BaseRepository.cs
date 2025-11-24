using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Bib.Infrastructure.Data.Repositories.Base
{
    public abstract class BaseRepository
    {
        protected readonly IDbConnection _connection;
        protected readonly IDbTransaction? _transaction;

        protected BaseRepository(IDbConnection connection, IDbTransaction? transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        protected async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null)
        {
            return await _connection.QueryFirstOrDefaultAsync<T>(sql, parameters, _transaction);
        }

        protected async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null)
        {
            return await _connection.QueryAsync<T>(sql, parameters, _transaction);
        }

        protected async Task<int> ExecuteAsync(string sql, object? parameters = null)
        {
            return await _connection.ExecuteAsync(sql, parameters, _transaction);
        }

        protected async Task<T> ExecuteScalarAsync<T>(string sql, object? parameters = null)
        {
            return await _connection.ExecuteScalarAsync<T>(sql, parameters, _transaction);
        }
    }
}
