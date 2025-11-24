using Bib.Application.Common.Interfaces;
using Bib.Infrastructure.Data.Connections;
using Bib.Infrastructure.Data.Initialization;
using Bib.Infrastructure.Data.Repositories;
using Bib.Infrastructure.Data.UnitOfWork;
using Microsoft.Extensions.Configuration;


namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Registrar IDbConnectionFactory
            services.AddScoped<IDbConnectionFactory>(_ => new SqlConnectionFactory(connectionString));

            services.AddScoped<IDatabaseInitializer, SimpleDbInitializer>();

            // Registrar UnitOfWork
            services.AddScoped<IUnitOfWork, DapperUnitOfWork>();

            // Registrar Repositories
            //services.AddScoped<IPublisherRepository, PublisherRepository>();


            // 👇 Repositories (opcional - se quiser usar diretamente sem UoW)
            //services.AddScoped<IPublisherRepository, PublisherRepository>();
            //services.AddScoped<IOrderRepository, OrderRepository>();
            //services.AddScoped<IUserRepository, UserRepository>();

            // Services


            // Configura Dapper Mappings
            //DapperMappings.Configure();

            return services;
        }
    }
}
