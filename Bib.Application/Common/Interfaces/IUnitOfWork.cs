using System;
using System.Threading.Tasks;

namespace Bib.Application.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPublisherRepository PublisherRepository { get; }
        IAuthorRepository AuthorRepository { get; }
        IBookRepository BookRepository { get; }
        IUserRepository UserRepository { get; }
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
