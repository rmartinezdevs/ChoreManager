using ChoreManager.Application.UseCases.ChoreUseCases;
using ChoreManager.Domain.Interfaces;
using ChoreManager.Infrastructure.Context;
using ChoreManager.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChoreManager.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string? connectionStringSQL)
        {
            services.AddDbContext<AppDbContext>(options =>
                            options.UseSqlServer(connectionStringSQL, b => b.MigrationsAssembly("ChoreManager.Infrastructure")));

            services.AddScoped<IChoreRepository, ChoreRepository>();

            return services;
        }

        public static IServiceCollection AddApplicationServices (this IServiceCollection services)
        {
            services.AddScoped<CreateChoreUseCase>();
            services.AddScoped<DeleteChoreUseCase>();
            services.AddScoped<SelectAllChoresUseCase>();
            services.AddScoped<SelectChoreByIdUseCase>();

            return services;
        }
    }
}
