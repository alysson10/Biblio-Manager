using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bib.Application.Common.Interfaces
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<int> InsertAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
    }
}
