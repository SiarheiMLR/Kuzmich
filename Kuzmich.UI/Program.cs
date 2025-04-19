using Kuzmich.UI.Data;
using Kuzmich.UI.Services.CategoryService;
using Kuzmich.UI.Services.LaptopService;
using Kuzmich.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Kuzmich.UI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Конфигурация подключения к базе данных (SQLite)
            var connectionString = builder.Configuration.GetConnectionString("SqLiteConnection")
                ?? throw new InvalidOperationException("Connection string 'SqLiteConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Настройка Identity с собственной моделью ApplicationUser
            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

            // Авторизация по роли "admin"
            builder.Services.AddAuthorization(opt =>
            {
                opt.AddPolicy("admin", p =>
                    p.RequireClaim(ClaimTypes.Role, "admin"));
            });

            // Подключение "пустого" email-отправителя
            builder.Services.AddSingleton<IEmailSender, NoOpEmailSender>();

            // Включаем сессии
            builder.Services.AddDistributedMemoryCache(); // Для хранения сессии в памяти
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Время жизни сессии
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddControllersWithViews();

            // Регистрация сервисов
            builder.Services.AddScoped<ILaptopService, MemoryLaptopService>();
            builder.Services.AddScoped<ICategoryService, MemoryCategoryService>();

            var app = builder.Build();

            // Конфигурация пайплайна
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // важно, если используешь авторизацию
            app.UseAuthorization();

            app.UseSession(); // <--- Важно! Добавлено для поддержки HttpContext.Session

            app.MapStaticAssets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.MapRazorPages().WithStaticAssets();

            // Инициализация администратора
            await DbInit.SetupIdentityAdmin(app);

            app.Run();
        }
    }
}
