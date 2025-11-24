using Bib.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bib.Application.Common.Interfaces
{
    public interface IPublisherRepository
    {
        Task<Publisher> GetByIdAsync(int id);
        Task<bool> IsInUseAsync(int id, string table, string column);
        Task<bool> GetFirstByConditionAsync(string condition);
        Task<IEnumerable<Publisher>> GetAllAsync();
        Task<int> CreateAsync(Publisher publisher);
        Task<bool> UpdateAsync(Publisher publisher);
        Task<bool> DeleteSoftAsync(int id);
        Task<bool> DeleteHardAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
