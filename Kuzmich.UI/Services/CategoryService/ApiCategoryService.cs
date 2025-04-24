using System.Net.Http.Json;
using System.Text.Json;
using Kuzmich.Domain.Entities;
using Kuzmich.Domain.Models;

namespace Kuzmich.UI.Services.CategoryService
{
    public class ApiCategoryService(HttpClient httpClient) : ICategoryService
    {
        private readonly HttpClient _http = httpClient;

        private readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
        };

        public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            try
            {
                var stream = await _http.GetStreamAsync("");
                var result = await JsonSerializer.DeserializeAsync<ResponseData<IEnumerable<Category>>>(stream, _options);

                if (result == null)
                {
                    return new ResponseData<List<Category>>
                    {
                        Success = false,
                        ErrorMessage = "Не удалось десериализовать данные"
                    };
                }

                return new ResponseData<List<Category>>
                {
                    Success = result.Success,
                    ErrorMessage = result.ErrorMessage,
                    Data = result.Data?.ToList() ?? new List<Category>()
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<List<Category>>
                {
                    Success = false,
                    ErrorMessage = $"Ошибка: {ex.Message}"
                };
            }
        }
    }
}


