using Bib.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bib.Application.Common.Interfaces
{
    public interface IBookRepository
    {
        Task<Book> GetByIdAsync(int id);
        Task<bool> IsInUseAsync(int id, string table, string column);
        Task<IEnumerable<Book>> GetAllAsync();
        Task<int> CreateAsync(Book publisher);
        Task<bool> UpdateAsync(Book publisher);
        Task<bool> DeleteSoftAsync(int id);
        Task<bool> DeleteHardAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> GetFirstByConditionAsync(string condition);
    }
}
