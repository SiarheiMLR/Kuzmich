using Kuzmich.API.Data;
using Kuzmich.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kuzmich.API
{
    public static class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            // Uri проекта
            var uri = "https://localhost:7003/";
            // Получение контекста БД
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            // Выполнение миграций
            await context.Database.MigrateAsync();
            // Заполнение данными
            if (!context.Categories.Any() && !context.Laptops.Any())
            {
                var categories = new Category[]
                {
                    new Category { Name = "Игровые", NormalizedName = "gaming"},
                    new Category { Name = "Бизнес", NormalizedName = "business"},
                    new Category { Name = "Для учёбы", NormalizedName = "study"},
                    new Category { Name = "Мультимедийные", NormalizedName = "media"},
                    new Category { Name = "Ультрабуки", NormalizedName = "ultrabook"},
                    new Category { Name = "Бюджетные", NormalizedName = "budget"}
                };
                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();

                var laptops = new List<Laptop>
                {
                    new Laptop { Name = "MSI Katana 17 B12UDXK-1224XBY", Description = "Игровой ноутбук с RTX 3050 и экраном 17.3\"", Price = 3699, CategoryId = categories.First(c => c.NormalizedName == "gaming").Id, Image = uri+"images/msi.png" },
                    new Laptop { Name = "Dell XPS 15 7590-6565", Description = "Бизнес-ноутбук премиум-класса с 4K экраном", Price = 4499, CategoryId = categories.First(c => c.NormalizedName == "business").Id, Image = uri+"images/dell.png" },
                    new Laptop { Name = "Acer Aspire Lite AL15-41 UN.31ZSI.014", Description = "Лёгкий ноутбук для учёбы и работы", Price = 1299, CategoryId = categories.First(c => c.NormalizedName == "study").Id, Image = uri+"images/acer.png" },
                    new Laptop { Name = "HP Pavilion Gaming 15-ec1008ur 13C90EA", Description = "Игровой ноутбук с AMD Ryzen 5 и GTX 1650", Price = 2999, CategoryId = categories.First(c => c.NormalizedName == "gaming").Id, Image = uri+"images/hp.png" },
                    new Laptop { Name = "Lenovo ThinkPad X1 Extreme Gen 3 20TK001GUS", Description = "Мощный бизнес-ноутбук с NVIDIA GTX 1650 Ti", Price = 5199, CategoryId = categories.First(c => c.NormalizedName == "business").Id, Image = uri+"images/thinkpad.png" },
                    new Laptop { Name = "ASUS VivoBook 15 X513EA-BQ686", Description = "Мультимедийный ноутбук с Intel Core i5", Price = 1799, CategoryId = categories.First(c => c.NormalizedName == "media").Id, Image = uri+"images/vivobook.png" },
                    new Laptop { Name = "Apple MacBook Air 13\" M1 2020 MGN63", Description = "Ультрабук на базе чипа Apple M1", Price = 4223, CategoryId = categories.First(c => c.NormalizedName == "ultrabook").Id, Image = uri+"images/macbook.png" },
                    new Laptop { Name = "HONOR MagicBook X15 BBR-WAI9 53011UGC-001", Description = "Бюджетный ноутбук с алюминиевым корпусом", Price = 1099, CategoryId = categories.First(c => c.NormalizedName == "budget").Id, Image = uri+"images/honor.png" }
                };
                await context.Laptops.AddRangeAsync(laptops);
                await context.SaveChangesAsync();
            }
        }
    }
}
