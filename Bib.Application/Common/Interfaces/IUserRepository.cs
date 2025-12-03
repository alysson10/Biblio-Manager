using Bib.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bib.Application.Common.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByConditionAsync(string condition);
        Task<bool> IsInUseAsync(int id, string table, string column);
        Task<bool> GetFirstByConditionAsync(string condition);
        Task<IEnumerable<User>> GetAllAsync();
        Task<int> CreateAsync(User user);
        Task<bool> UpdateAsync(User user);
        Task<bool> DeleteSoftAsync(int id);
        Task<bool> DeleteHardAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
