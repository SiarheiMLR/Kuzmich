using Kuzmich.Domain.Entities;
using Kuzmich.Domain.Models;

namespace Kuzmich.UI.Services.CategoryService
{
    public interface ICategoryService // Интерфейс, описывающий метод получения списка всех категорий
    {
        /// <summary>
        /// Получение списка всех категорий
        /// </summary>
        /// <returns></returns>
        public Task<ResponseData<List<Category>>> GetCategoryListAsync();
    }
}
