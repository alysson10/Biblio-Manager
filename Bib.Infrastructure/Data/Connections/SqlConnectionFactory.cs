using Microsoft.Data.SqlClient;
using System.Data;

namespace Bib.Infrastructure.Data.Connections
{
    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        public IDbTransaction CreateTransaction(IDbConnection connection)
        {
            return connection.BeginTransaction();
        }

        public string GetConnectionString() => _connectionString;
    }
}
