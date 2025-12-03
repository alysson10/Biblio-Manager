using Bib.Application.Common.Interfaces;
using Bib.Domain.Entities;
using Bib.Infrastructure.Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Bib.Infrastructure.Data.Repositories
{
    public class AuthorRepository : BaseRepository, IAuthorRepository
    {
        private const string BaseSelect = "SELECT Id, Name, Email, PhoneNumber, Status, CreatedAt, UpdatedAt, DeletedAt FROM Author ";

        public AuthorRepository(IDbConnection connection, IDbTransaction? transaction)
            : base(connection, transaction)
        {
        }

        public async Task<int> CreateAsync(Author author)
        {
            var sql = @"
                INSERT INTO Author (UserId, Name, Email, PhoneNumber, Status, CreatedAt)
                OUTPUT INSERTED.Id
                VALUES (@UserId, @Name, @Email, @PhoneNumber, @Status, @CreatedAt)";

            return await ExecuteScalarAsync<int>(sql, author);
        }

        public async Task<bool> DeleteHardAsync(int id)
        {
            var sql = "DELETE FROM Author WHERE Id = @Id";
            var affectedRows = await ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }

        public async Task<bool> DeleteSoftAsync(int id)
        {
            var sql = @"
                UPDATE Author
                SET Status = 0, 
                    UpdatedAt = GETUTCDATE()
                    DeletedAt = GETUTCDATE()
                WHERE Id = @Id";

            var affectedRows = await ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            var sql = $@"
                {BaseSelect}
                ORDER BY Name";

            return await QueryAsync<Author>(sql);
        }

        public async Task<Author> GetByIdAsync(int id)
        {
            var sql = $@"
                {BaseSelect}
                WHERE Id = @Id";

            return await QueryFirstOrDefaultAsync<Author>(sql, new { Id = id });
        }

        public async Task<bool> GetFirstByConditionAsync(string condition)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsInUseAsync(int id, string table, string column)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(Author author)
        {
            var sql = @"
                UPDATE Author
                SET Name = @Name, 
                    Email = @Email, 
                    PhoneNumber = @PhoneNumber,
                    Status = @Status,
                    CreatedAt = @CreatedAt,
                    UpdatedAt = GETUTCDATE()
                WHERE Id = @Id";

            var affectedRows = await ExecuteAsync(sql, author);
            return affectedRows > 0;
        }
    }
}
