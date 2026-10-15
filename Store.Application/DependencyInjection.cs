using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Store.Application.Helpers;
using Store.Application.Services.Contract;
using Store.Application.Services.Implementation;
using Store.Infrastructure.Data;
using Store.Infrastructure.Repositories.Contract;
using Store.Infrastructure.Repositories.Implementation;

namespace Store.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? 
                throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<OperationHelper>();

            AddRepositories(services);
            AddServices(services);

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IOperationRepository, OperationRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IOperationService, OperationService>();
            services.AddScoped<IAuthService, AuthService>();
            return services;
        }
    }
}
