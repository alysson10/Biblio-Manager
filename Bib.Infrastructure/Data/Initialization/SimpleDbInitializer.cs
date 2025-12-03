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
                await CreateUserTableAsync();
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
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Publisher')
            BEGIN
                CREATE TABLE Publisher (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    UserId INT NOT NULL,
                    Name NVARCHAR(50) NOT NULL,
                    Description NVARCHAR(200) NULL,
                    PhoneNumber NVARCHAR(10) NULL,
                    Email NVARCHAR(50) NULL,
                    Site NVARCHAR(100) NULL,
                    Status BIT NOT NULL DEFAULT 1,
                    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
                    UpdatedAt DATETIME2 NULL,
                    DeletedAt DATETIME2 NULL

                    CONSTRAINT FK_Publisher_User
                    FOREIGN KEY (UserId) REFERENCES [User](Id)
                );
                
                -- Indexes
                CREATE INDEX IX_Publisher_Name ON Publisher (Name);
                CREATE INDEX IX_Publisher_Status ON Publisher (Status);                
                
                PRINT 'Tabela Publisher criada com sucesso';
            END";

            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(sql);
            _logger.LogInformation("Tabela Publishers verificada/criada");
        }

        private async Task CreateAuthorsTableAsync()
        {
            var sql = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Author')
            BEGIN
                CREATE TABLE Author (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    UserId INT NOT NULL,
                    Name NVARCHAR(50) NOT NULL,
                    Email NVARCHAR(50) NULL,
                    PhoneNumber NVARCHAR(10) NULL,
                    Status BIT NOT NULL DEFAULT 1,
                    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
                    UpdatedAt DATETIME2 NULL,
                    DeletedAt DATETIME2 NULL

                    CONSTRAINT FK_Author_User
                    FOREIGN KEY (UserId) REFERENCES [User](Id)
                );
                
                CREATE INDEX IX_Author_Name ON Author (Name);
                
                PRINT 'Tabela Author criada com sucesso';
            END";

            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(sql);
            _logger.LogInformation("Tabela Authors verificada/criada");
        }

        private async Task CreateBooksTableAsync()
        {
            var sql = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Book')
            BEGIN
                CREATE TABLE Book (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    UserId INT NOT NULL,
                    PublisherId INT NOT NULL,
                    AuthorId INT NOT NULL,
                    Title NVARCHAR(200) NOT NULL,
                    ISBN NVARCHAR(20) NULL,
                    PublishedDate DATE NULL,
                    Price DECIMAL(10,2) NULL,
                    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
                    UpdatedAt DATETIME2 NULL,
                    DeletedAt DATETIME2 NULL
                    
                    CONSTRAINT FK_Book_Publishers 
                    FOREIGN KEY (PublisherId) REFERENCES Publisher(Id),

                    CONSTRAINT FK_Book_Authors
                    FOREIGN KEY (AuthorId) REFERENCES Author(Id),

                    CONSTRAINT FK_Book_User
                    FOREIGN KEY (UserId) REFERENCES [User](Id)
                );
                
                CREATE INDEX IX_Book_Title ON Book (Title);
                CREATE INDEX IX_Book_PublisherId ON Book (PublisherId);
                CREATE INDEX IX_Book_AuthorId ON Book (AuthorId);
                CREATE INDEX IX_Book_ISBN ON Book (ISBN) WHERE ISBN IS NOT NULL;
                
                PRINT 'Tabela Books criada com sucesso';
            END";

            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(sql);
            _logger.LogInformation("Tabela Books verificada/criada");
        }

        private async Task CreateUserTableAsync()
        {
            var sql = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'User')
            BEGIN
                CREATE TABLE [User] (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Login NVARCHAR(20) NOT NULL,
                    Password NVARCHAR(150) NOT NULL,
                    Name NVARCHAR(50) NOT NULL,
                    PhoneNumber NVARCHAR(10) NULL,
                    Email NVARCHAR(50) NULL,
                    Status BIT NOT NULL DEFAULT 1,
                    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
                    UpdatedAt DATETIME2 NULL,
                    DeletedAt DATETIME2 NULL
                );
                
                PRINT 'Tabela User criada com sucesso';
            END";

            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(sql);
            _logger.LogInformation("Tabela User verificada/criada");
        }
    }
}
