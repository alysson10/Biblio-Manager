using Bib.Infrastructure.Data.Connections;
using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Bib.Infrastructure.Data.Initialization
{
    public class SimpleDbInitializer : IDatabaseInitializer
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<SimpleDbInitializer> _logger;

        public SimpleDbInitializer(
            IDbConnectionFactory connectionFactory,
            ILogger<SimpleDbInitializer> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            _logger.LogInformation("Iniciando inicialização do banco de dados...");

            try
            {
                await CreatePublishersTableAsync();
                await CreateAuthorsTableAsync();
                await CreateBooksTableAsync();

                _logger.LogInformation("Banco de dados inicializado com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inicializar banco de dados");
                throw;
            }
        }

        private async Task CreatePublishersTableAsync()
        {
            var sql = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Publishers')
            BEGIN
                CREATE TABLE Publishers (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Name NVARCHAR(50) NOT NULL,
                    Description NVARCHAR(200) NULL,
                    PhoneNumber NVARCHAR(10) NULL,
                    Email NVARCHAR(50) NULL,
                    Site NVARCHAR(100) NULL,
                    Status BIT NOT NULL DEFAULT 1,
                    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
                    UpdatedAt DATETIME2 NULL,
                    DeletedAt DATETIME2 NULL
                );
                
                -- Indexes
                CREATE INDEX IX_Publishers_Name ON Publishers (Name);
                CREATE INDEX IX_Publishers_Status ON Publishers (Status);                
                
                PRINT 'Tabela Publishers criada com sucesso';
            END";

            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(sql);
            _logger.LogInformation("Tabela Publishers verificada/criada");
        }

        private async Task CreateAuthorsTableAsync()
        {
            var sql = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Authors')
            BEGIN
                CREATE TABLE Authors (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Name NVARCHAR(100) NOT NULL,
                    Email NVARCHAR(255) NULL,
                    PhoneNumber NVARCHAR(10) NULL,
                    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
                    UpdatedAt DATETIME2 NULL,
                    DeletedAt DATETIME2 NULL
                );
                
                CREATE INDEX IX_Authors_Name ON Authors (Name);
                
                PRINT 'Tabela Authors criada com sucesso';
            END";

            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(sql);
            _logger.LogInformation("Tabela Authors verificada/criada");
        }

        private async Task CreateBooksTableAsync()
        {
            var sql = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Books')
            BEGIN
                CREATE TABLE Books (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    PublisherId INT NOT NULL,
                    AuthorId INT NOT NULL,
                    Title NVARCHAR(200) NOT NULL,
                    ISBN NVARCHAR(20) NULL,
                    PublishedDate DATE NULL,
                    Price DECIMAL(10,2) NULL,
                    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
                    UpdatedAt DATETIME2 NULL,
                    DeletedAt DATETIME2 NULL
                    
                    CONSTRAINT FK_Books_Publishers 
                    FOREIGN KEY (PublisherId) REFERENCES Publishers(Id),

                    CONSTRAINT FK_Books_Authors
                    FOREIGN KEY (AuthorId) REFERENCES Authors(Id)
                );
                
                CREATE INDEX IX_Books_Title ON Books (Title);
                CREATE INDEX IX_Books_PublisherId ON Books (PublisherId);
                CREATE INDEX IX_Books_AuthorId ON Books (AuthorId);
                CREATE INDEX IX_Books_ISBN ON Books (ISBN) WHERE ISBN IS NOT NULL;
                
                PRINT 'Tabela Books criada com sucesso';
            END";

            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(sql);
            _logger.LogInformation("Tabela Books verificada/criada");
        }        
    }
}
