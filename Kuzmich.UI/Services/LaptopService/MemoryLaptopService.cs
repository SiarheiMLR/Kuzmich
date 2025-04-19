using Kuzmich.Domain.Entities;
using Kuzmich.Domain.Models;
using Kuzmich.UI.Services.CategoryService;

namespace Kuzmich.UI.Services.LaptopService
{
    public class MemoryLaptopService : ILaptopService
    {
        private readonly List<Laptop> _laptops;
        private readonly List<Category> _categories;
        private int _nextId = 9;

        public MemoryLaptopService(ICategoryService categoryService)
        {
            _categories = categoryService.GetCategoryListAsync().Result.Data;

            _laptops = new List<Laptop>
            {
                new Laptop { Id = 1, Name = "MSI Katana 17 B12UDXK-1224XBY", Description = "Игровой ноутбук с RTX 3050 и экраном 17.3\"", Price = 3699, CategoryId = 1, Image = "msi.png" },
                new Laptop { Id = 2, Name = "Dell XPS 15 7590-6565", Description = "Бизнес-ноутбук премиум-класса с 4K экраном", Price = 4499, CategoryId = 2, Image = "dell.png" },
                new Laptop { Id = 3, Name = "Acer Aspire Lite AL15-41 UN.31ZSI.014", Description = "Лёгкий ноутбук для учёбы и работы", Price = 1299, CategoryId = 3, Image = "acer.png" },
                new Laptop { Id = 4, Name = "HP Pavilion Gaming 15-ec1008ur 13C90EA", Description = "Игровой ноутбук с AMD Ryzen 5 и GTX 1650", Price = 2999, CategoryId = 1, Image = "hp.png" },
                new Laptop { Id = 5, Name = "Lenovo ThinkPad X1 Extreme Gen 3 20TK001GUS", Description = "Мощный бизнес-ноутбук с NVIDIA GTX 1650 Ti", Price = 5199, CategoryId = 2, Image = "thinkpad.png" },
                new Laptop { Id = 6, Name = "ASUS VivoBook 15 X513EA-BQ686", Description = "Мультимедийный ноутбук с Intel Core i5", Price = 1799, CategoryId = 4, Image = "vivobook.png" },
                new Laptop { Id = 7, Name = "Apple MacBook Air 13\" M1 2020 MGN63", Description = "Ультрабук на базе чипа Apple M1", Price = 4223, CategoryId = 5, Image = "macbook.png" },
                new Laptop { Id = 8, Name = "HONOR MagicBook X15 BBR-WAI9 53011UGC-001", Description = "Бюджетный ноутбук с алюминиевым корпусом", Price = 1099, CategoryId = 6, Image = "honor.png" }
            };
        }

        public Task<ResponseData<ListModel<Laptop>>> GetProductListAsync(string? category, int pageNo = 1)
        {
            int? categoryId = null;

            if (!string.IsNullOrWhiteSpace(category))
            {
                var cat = _categories.FirstOrDefault(c => c.NormalizedName.Equals(category, StringComparison.OrdinalIgnoreCase));
                if (cat != null) categoryId = cat.Id;
            }

            var filtered = categoryId.HasValue
                ? _laptops.Where(l => l.CategoryId == categoryId.Value).ToList()
                : _laptops.ToList();

            var model = new ListModel<Laptop> { Items = filtered };

            return Task.FromResult(new ResponseData<ListModel<Laptop>> { Data = model, Success = true });
        }

        public Task<ResponseData<Laptop>> GetProductByIdAsync(int id)
        {
            var laptop = _laptops.FirstOrDefault(x => x.Id == id);
            if (laptop == null)
            {
                return Task.FromResult(new ResponseData<Laptop> { Success = false, ErrorMessage = "Ноутбук не найден" });
            }
            return Task.FromResult(new ResponseData<Laptop> { Data = laptop });
        }

        public Task UpdateProductAsync(int id, Laptop product, IFormFile? formFile)
        {
            var existing = _laptops.FirstOrDefault(x => x.Id == id);
            if (existing != null)
            {
                existing.Name = product.Name;
                existing.Description = product.Description;
                existing.Price = product.Price;
                existing.CategoryId = product.CategoryId;

                if (formFile != null)
                {
                    existing.Image = formFile.FileName;                    
                }
            }
            return Task.CompletedTask;
        }

        public Task DeleteProductAsync(int id)
        {
            var item = _laptops.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                _laptops.Remove(item);
            }
            return Task.CompletedTask;
        }

        public Task<ResponseData<Laptop>> CreateProductAsync(Laptop product, IFormFile? formFile)
        {
            product.Id = _nextId++;
            if (formFile != null)
            {
                product.Image = formFile.FileName;                
            }
            _laptops.Add(product);

            return Task.FromResult(new ResponseData<Laptop> { Data = product });
        }
    }
}