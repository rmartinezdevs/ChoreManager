using ChoreManager.Application.UseCases.ChoreUseCases.Implementations;
using ChoreManager.Application.UseCases.ChoreUseCases.Interfaces;
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
            services.AddScoped<ICreateChoreUseCase, CreateChoreUseCase>();
            services.AddScoped<IDeleteChoreUseCase, DeleteChoreUseCase>();
            services.AddScoped<ISelectAllChoresUseCase, SelectAllChoresUseCase>();
            services.AddScoped<ISelectChoreByIdUseCase, SelectChoreByIdUseCase>();
            services.AddScoped<IUpdateChoreUseCase, UpdateChoreUseCase>();

            return services;
        }
    }
}
