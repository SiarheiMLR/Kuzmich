using Kuzmich.Domain.Entities;
using Kuzmich.Domain.Models;

namespace Kuzmich.UI.Services
{
    public interface ILaptopService
    {
        /// <summary>
        /// Получение списка всех ноутбуков
        /// </summary>
        /// <param name="categoryNormalizedName">нормализованное имя категории для фильтрации</param>
        /// <param name="pageNo">номер страницы списка</param>
        Task<ResponseData<ListModel<Laptop>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1);

        /// <summary>
        /// Поиск ноутбука по Id
        /// </summary>
        Task<ResponseData<Laptop>> GetProductByIdAsync(int id);

        /// <summary>
        /// Обновление ноутбука
        /// </summary>
        Task UpdateProductAsync(int id, Laptop product, IFormFile? formFile);

        /// <summary>
        /// Удаление ноутбука
        /// </summary>
        Task DeleteProductAsync(int id);

        /// <summary>
        /// Создание ноутбука
        /// </summary>
        Task<ResponseData<Laptop>> CreateProductAsync(Laptop product, IFormFile? formFile);
    }
}
