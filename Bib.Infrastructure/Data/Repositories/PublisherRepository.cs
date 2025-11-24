using Bib.Application.Common.Interfaces;
using Bib.Domain.Entities;
using Bib.Infrastructure.Data.Repositories.Base;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Bib.Infrastructure.Data.Repositories
{
    public class PublisherRepository : BaseRepository, IPublisherRepository
    {
        private const string BaseSelect = "SELECT Id, Name, Description, PhoneNumber, Email, Site, Status, CreatedAt, UpdatedAt, DeleteddAt FROM Publishers ";

        public PublisherRepository(IDbConnection connection, IDbTransaction? transaction)
        : base(connection, transaction)
        {
        }

        public async Task<Publisher> GetByIdAsync(int id)
        {
            var sql = $@"
                {BaseSelect}
                WHERE Id = @Id";

            return await QueryFirstOrDefaultAsync<Publisher>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Publisher>> GetAllAsync()
        {
            var sql = @"
                SELECT Id, Name, Description, PhoneNumber, Email, Site, Status, CreatedAt, UpdatedAt, DeleteddAt 
                FROM Publishers 
                ORDER BY Name";

            return await QueryAsync<Publisher>(sql);
        }

        public async Task<int> CreateAsync(Publisher publisher)
        {
            var sql = @"
                INSERT INTO Publishers (Name, Description, PhoneNumber, Email, Site, Status, CreatedAt)
                OUTPUT INSERTED.Id
                VALUES (@Name, @Description, @PhoneNumber, @Email, @Site, @Status, GETUTCDATE())";

            return await ExecuteScalarAsync<int>(sql, publisher);
        }

        public async Task<bool> UpdateAsync(Publisher publisher)
        {
            var sql = @"
                UPDATE Publishers 
                SET Name = @Name, 
                    Description = @Description,
                    PhoneNumber = @PhoneNumber,
                    Email = @Email, 
                    Site = @Site, 
                    Status = @Status,
                    CreatedAt = @CreatedAt,
                    UpdatedAt = GETUTCDATE()
                WHERE Id = @Id";

            var affectedRows = await ExecuteAsync(sql, publisher);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteHardAsync(int id)
        {
            var sql = "DELETE FROM Publishers WHERE Id = @Id";
            var affectedRows = await ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }

        public async Task<bool> DeleteSoftAsync(int id)
        {
            var sql = @"
                UPDATE Publishers 
                SET Status = 0, 
                    UpdatedAt = GETUTCDATE()
                    DeletedAt = GETUTCDATE()
                WHERE Id = @Id";

            var affectedRows = await ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            var sql = "SELECT 1 FROM Publishers WHERE Id = @Id";
            var result = await QueryFirstOrDefaultAsync<int?>(sql, new { Id = id });
            return result.HasValue;
        }

        public async Task<bool> IsInUseAsync(int id, string table, string column)
        {
            var sql = $@"SELECT 1 FROM @Table t
                            INNER JOIN Books b
                                t.Id = t.@Column
                        WHERE t.Id = @Id";
            var result = await QueryFirstOrDefaultAsync<int?>(sql, new { Id = id, Table = table, Column = column });
            return result.HasValue;
        }

        //public async Task<Publisher> GetFirstByConditionAsync(string whereCondition, object parameters = null)
        //{
        //    var sql = $"{BaseSelect} WHERE {whereCondition}";
        //    return await QueryFirstOrDefaultAsync<Publisher>(sql, parameters);
        //}

        public async Task<bool> GetFirstByConditionAsync(string condition)
        {
            var sql = $"{BaseSelect} WHERE {condition}";
            return await QueryFirstOrDefaultAsync<bool>(sql, null);
        }
    }
}
