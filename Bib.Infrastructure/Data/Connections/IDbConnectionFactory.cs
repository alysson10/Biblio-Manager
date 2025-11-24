using System.Data;

namespace Bib.Infrastructure.Data.Connections
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
        IDbTransaction CreateTransaction(IDbConnection connection);
        string GetConnectionString();
    }
}
