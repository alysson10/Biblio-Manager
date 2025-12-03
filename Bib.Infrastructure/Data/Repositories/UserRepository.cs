using Bib.Application.Common.Interfaces;
using Bib.Domain.Entities;
using Bib.Infrastructure.Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Bib.Infrastructure.Data.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private const string BaseSelect = "SELECT Id, Login, Password, Name, PhoneNumber, Email, Status, CreatedAt, UpdatedAt, DeletedAt FROM [User] ";

        public UserRepository(IDbConnection connection, IDbTransaction? transaction)
        : base(connection, transaction)
        {
        }

        public Task<int> CreateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteHardAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteSoftAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByConditionAsync(string condition)
        {
            var sql = $"{BaseSelect} WHERE {condition}";
            return await QueryFirstOrDefaultAsync<User>(sql, null);
        }

        public Task<Author> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetFirstByConditionAsync(string condition)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInUseAsync(int id, string table, string column)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }

        Task<User> IUserRepository.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
