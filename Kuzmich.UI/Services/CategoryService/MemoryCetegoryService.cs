using Kuzmich.Domain.Entities;
using Kuzmich.Domain.Models;

namespace Kuzmich.UI.Services.CategoryService
{
    public class MemoryCategoryService : ICategoryService
    {
        private readonly List<Category> _categories = new()
        {
            new() { Id = 1, Name = "Игровые", NormalizedName = "gaming" },
            new() { Id = 2, Name = "Бизнес", NormalizedName = "business" },
            new() { Id = 3, Name = "Для учёбы", NormalizedName = "study" },
            new() { Id = 4, Name = "Мультимедийные", NormalizedName = "media" },
            new() { Id = 5, Name = "Ультрабуки", NormalizedName = "ultrabook" },
            new() { Id = 6, Name = "Бюджетные", NormalizedName = "budget" }
        };

        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            return Task.FromResult(new ResponseData<List<Category>>
            {
                Data = _categories,
                Success = true
            });
        }
    }
}
