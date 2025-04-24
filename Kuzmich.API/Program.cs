
using Kuzmich.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Kuzmich.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Конфигурация подключения к базе данных (SQLite)
            var connString = builder.Configuration.GetConnectionString("Default");

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connString));

            //builder.Services.AddControllers();
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                    options.JsonSerializerOptions.WriteIndented = true;
                });

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi(); // Для Swagger

            var app = builder.Build();

            // Инициализация БД
            await DbInitializer.SeedData(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi(); // Swagger UI
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
            
        }
    }
}
