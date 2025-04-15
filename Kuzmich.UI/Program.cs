using Kuzmich.UI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Kuzmich.UI
{
    public class Program
    {
        public static async Task Main(string[] args) // ��������� async Task
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            //builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(connectionString));

            // ���������� SQLite
            var connectionString = builder.Configuration.GetConnectionString("SqLiteConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString));
            
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            //builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            // ���������� ���� ����� ApplicationUser, ������� ����������� �� ������ IdentityUser
            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
                {
                    // ��������� ����������� ������������� ������� �������
                    options.SignIn.RequireConfirmedAccount = true;
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;

                })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // �������� �������� ����������� � ��������, ��� ����������� �role� ����� �������� �admin�
            builder.Services.AddAuthorization(opt =>
            {
                opt.AddPolicy("admin", p =>
                p.RequireClaim(ClaimTypes.Role, "admin"));
            });

            // ��� �������� ��������� ������������� ������������ ������ ��������������� NoOpEmailSender � �������� ������� IEmailSender
            builder.Services.AddSingleton<IEmailSender, NoOpEmailSender>();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            // ����� ������������� ��������������
            await DbInit.SetupIdentityAdmin(app);

            app.Run();
        }
    }
}
