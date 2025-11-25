using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Transacciones.Core.Interfaces;
using Transacciones.Core.SharedKernel.Interfaces;
using Transacciones.Infrastructure.Data;
using Transacciones.Infrastructure.Services;

namespace Transacciones.Infrastructure
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
                    .AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));

            services.AddScoped<IUnitOfWork, UnitOfWork>();


            // services
            services.AddScoped<ICuentaService, CuentaService>();
            services.AddScoped<ITransaccionService, TransaccionService>();


            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found.");

            // Add DbContext from PostgreSQL
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

            return services;
        }
    }
}
