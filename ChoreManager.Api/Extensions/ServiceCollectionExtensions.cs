using ChoreManager.Application.UseCases.ChoreUseCases.Implementations;
using ChoreManager.Application.UseCases.ChoreUseCases.Interfaces;
using ChoreManager.Application.Validators;
using ChoreManager.Domain.Interfaces;
using ChoreManager.Infrastructure.Context;
using ChoreManager.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

namespace ChoreManager.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string? connectionStringSQL)
        {
            services.AddDbContext<AppDbContext>(options =>
                            options.UseSqlServer(connectionStringSQL, b => b.MigrationsAssembly("ChoreManager.Infrastructure")));

            services.AddRepositoryServices();

            return services;
        }
        public static IServiceCollection AddApplicationServices (this IServiceCollection services)
        {
            services.AddUseCasesServices();
            services.AddValidationServices();

            return services;
        }
        private static IServiceCollection AddValidationServices(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<CreateChoreValidator>();
            services.AddValidatorsFromAssemblyContaining<ChoreValidator>();

            return services;
        }
        private static IServiceCollection AddRepositoryServices(this IServiceCollection services)
        {
            services.AddScoped<IChoreRepository, ChoreRepository>();

            return services;
        }
        private static IServiceCollection AddUseCasesServices(this IServiceCollection services)
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