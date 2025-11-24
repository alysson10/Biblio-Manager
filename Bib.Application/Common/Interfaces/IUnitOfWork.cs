using System;
using System.Threading.Tasks;

namespace Bib.Application.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPublisherRepository PublisherRepository { get; }
        IBookRepository BookRepository { get; }
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
