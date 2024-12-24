using ChoreManager.Application.UseCases.ChoreUseCases;
using ChoreManager.Domain.Interfaces;
using ChoreManager.Infrastructure.Context;
using ChoreManager.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GestorTareas.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // Configure Infrastructure Services
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("ChoreManager.Infrastructure")));

            builder.Services.AddScoped<IChoreRepository, ChoreRepository>();

            builder.Services.AddScoped<CreateChore>();
            builder.Services.AddScoped<DeleteChore>();
            builder.Services.AddScoped<SelectAllChores>();
            builder.Services.AddScoped<SelectChoreById>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.Migrate(); 
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                    options.RoutePrefix = string.Empty; 
                });
            }

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
