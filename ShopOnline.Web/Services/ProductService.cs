using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Net.Http.Json;

namespace ShopOnline.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient httpClient;

        public ProductService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ProductDto> GetItem(int id)
        {
            try
            {
                var response = await this.httpClient.GetAsync($"api/Product/{id}");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(ProductDto);
                    }
                    return await response.Content.ReadFromJsonAsync<ProductDto>();
                }
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetItems()
        {
            try
            {
                var response = await this.httpClient.GetAsync("api/Product");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(IEnumerable<ProductDto>);
                    }
                    var products = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
                    return products;
                }
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetItemsByCategory(int id)
        {
            try
            {
                var response = await this.httpClient.GetAsync($"api/Product/{id}/GetItemsByCategory");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ProductDto>();
                    }
                    var products = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
                    return products;
                }
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http Status Code - {response.StatusCode} Message - {message}");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ProductCategoryDto>> GetProductCategories()
        {
            try
            {
                var response = await this.httpClient.GetAsync("api/Product/GetProductCategories");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ProductCategoryDto>();
                    }
                    var productCategories = await response.Content.ReadFromJsonAsync<IEnumerable<ProductCategoryDto>>();
                    return productCategories;
                }
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http Status Code - {response.StatusCode} Message - {message}");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategory(int id)
        {
            try
            {
                var response = await this.httpClient.GetAsync($"api/Product/{id}/GetItemsByCategory");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ProductDto>();
                    }
                    var products = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
                    return products;
                }
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http Status Code - {response.StatusCode} Message - {message}");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
