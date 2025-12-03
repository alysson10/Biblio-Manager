using Bib.Application.Common.Interfaces;
using Bib.Domain.Entities;
using Bib.Infrastructure.Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Bib.Infrastructure.Data.Repositories
{
    public class BookRepository : BaseRepository, IBookRepository
    {
        private const string BaseSelect = "SELECT Id, PublisherId, AuthorId, Title, ISBN, PublishedDate, Price, CreatedAt, UpdatedAt, DeletedAt FROM Books";

        public BookRepository(IDbConnection connection, IDbTransaction? transaction)
        : base(connection, transaction)
        {
        }

        public async Task<int> CreateAsync(Book publisher)
        {
            var sql = @"
                INSERT INTO Books (UserId, PublisherId, AuthorId, Title, ISBN, PublishedDate, Price, CreatedAt)
                OUTPUT INSERTED.Id
                VALUES (@UserId, @PublisherId, @AuthorId, @Title, @ISBN, @PublishedDate, @Price, @CreatedAt)";

            return await ExecuteScalarAsync<int>(sql, publisher);
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

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            var sql = $@"
                {BaseSelect}
                ORDER BY Name";

            return await QueryAsync<Book>(sql);
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            var sql = @"
                SELECT Id
                FROM Books 
                WHERE Id = @Id";

            return await QueryFirstOrDefaultAsync<Book>(sql, new { Id = id });
        }

        public Task<bool> IsInUseAsync(int id, string table, string column)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Book publisher)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> GetFirstByConditionAsync(string condition)
        {
            var sql = $"{BaseSelect} WHERE {condition}";
            return await QueryFirstOrDefaultAsync<bool>(sql, null);
        }
    }
}
