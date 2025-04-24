using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Kuzmich.Domain.Entities;
using Kuzmich.Domain.Models;
using Microsoft.AspNetCore.WebUtilities;

namespace Kuzmich.UI.Services.LaptopService
{
    public class ApiLaptopService(HttpClient httpClient) : ILaptopService
    {
        private readonly HttpClient _http = httpClient;

        private readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
        };

        public async Task<ResponseData<ListModel<Laptop>>> GetProductListAsync(string? category, int pageNo = 1)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["pageNo"] = pageNo.ToString()
            };
            if (!string.IsNullOrWhiteSpace(category))
                queryParams["category"] = category;

            var url = QueryHelpers.AddQueryString("", queryParams);

            try
            {
                var stream = await _http.GetStreamAsync(url);
                var result = await JsonSerializer.DeserializeAsync<ResponseData<ListModel<Laptop>>>(stream, _options);

                return result ?? new ResponseData<ListModel<Laptop>>
                {
                    Success = false,
                    ErrorMessage = "Ошибка десериализации данных"
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<ListModel<Laptop>>
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<ResponseData<Laptop>> GetProductByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<ResponseData<Laptop>>($"{id}")
                ?? new ResponseData<Laptop> { Success = false, ErrorMessage = "Ошибка получения данных" };
        }

        public async Task<ResponseData<Laptop>> CreateProductAsync(Laptop product, IFormFile? formFile)
        {
            var response = await _http.PostAsJsonAsync("", product);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ResponseData<Laptop>>(_options)
                       ?? new() { Success = false, ErrorMessage = "Ошибка чтения данных" };
            }
            return new ResponseData<Laptop> { Success = false, ErrorMessage = "Ошибка добавления" };
        }

        public async Task UpdateProductAsync(int id, Laptop product, IFormFile? formFile)
        {
            var response = await _http.PutAsJsonAsync($"{id}", product);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteProductAsync(int id)
        {
            var response = await _http.DeleteAsync($"{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
