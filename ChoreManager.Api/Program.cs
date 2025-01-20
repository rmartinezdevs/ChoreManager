using ChoreManager.Api.Extensions;
using ChoreManager.Api.Middleware;
using ChoreManager.Application.AutoMapper;
using ChoreManager.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorTareas.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddControllers();

            builder.Services.AddInfrastructureServices(builder.Configuration.GetConnectionString("DefaultConnection"));
            builder.Services.AddApplicationServices();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

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
