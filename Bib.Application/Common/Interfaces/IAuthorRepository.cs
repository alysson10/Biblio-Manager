using Bib.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bib.Application.Common.Interfaces
{
    public interface IAuthorRepository
    {
        Task<Author> GetByIdAsync(int id);
        Task<bool> IsInUseAsync(int id, string table, string column);
        Task<bool> GetFirstByConditionAsync(string condition);
        Task<IEnumerable<Author>> GetAllAsync();
        Task<int> CreateAsync(Author publisher);
        Task<bool> UpdateAsync(Author publisher);
        Task<bool> DeleteSoftAsync(int id);
        Task<bool> DeleteHardAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
