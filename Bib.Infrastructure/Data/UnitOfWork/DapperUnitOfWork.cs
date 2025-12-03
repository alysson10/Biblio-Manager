using Bib.Application.Common.Interfaces;
using Bib.Infrastructure.Data.Connections;
using Bib.Infrastructure.Data.Repositories;
using System.Data;
using System.Threading.Tasks;

namespace Bib.Infrastructure.Data.UnitOfWork
{
    public class DapperUnitOfWork : IUnitOfWork
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private IDbConnection? _connection;
        private IDbTransaction? _transaction;

        private IPublisherRepository? _publisherRepository;
        private IAuthorRepository? _authorRepository;
        private IBookRepository _bookRepository;
        private IUserRepository _userRepository;

        public DapperUnitOfWork(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IPublisherRepository PublisherRepository =>
            _publisherRepository ??= new PublisherRepository(GetConnection(), _transaction);

        public IAuthorRepository AuthorRepository =>
            _authorRepository ??= new AuthorRepository(GetConnection(), _transaction);

        public IBookRepository BookRepository =>
            _bookRepository ??= new BookRepository(GetConnection(), _transaction);

        public IUserRepository UserRepository =>
            _userRepository ??= new UserRepository(GetConnection(), _transaction);

        public async Task BeginTransactionAsync()
        {
            _transaction = GetConnection().BeginTransaction();
            await Task.CompletedTask;
        }

        public async Task CommitAsync()
        {
            _transaction?.Commit();
            await Task.CompletedTask;
        }

        public async Task RollbackAsync()
        {
            _transaction?.Rollback();
            await Task.CompletedTask;
        }

        private IDbConnection GetConnection()
        {
            return _connection ??= _connectionFactory.CreateConnection();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Dispose();
        }
    }
}
