using System.Text.Json;
using ShoesShop.Core.Contracts.Repository;
using ShoesShop.Core.Http;
using ShoesShop.Core.Models;

namespace ShoesShop.Core.Repository;

public class StatisticRepository : IStatisticRepository
{
    private readonly IHttpClientFactory _httpClientFactory;

    public StatisticRepository(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<(int, string, int)> CountSellingShoesAsync(string query)
    {
        var count = 0;
        var message = string.Empty;
        var ERROR_CODE = 0;

        try
        {
            using var client = _httpClientFactory.CreateClient("Backend");
            using var response = await client.GetAsync($"statistics/count-selling-books?type={query}");
            var content = response.Content.ReadAsStringAsync().Result;
            var httpResponse = JsonSerializer.Deserialize<HttpDataSchemaResponse<int>>(content);



            if (response.IsSuccessStatusCode)
            {
                count = httpResponse.Data;
            }
            else
            {
                message = "Internal Error";
                ERROR_CODE = (int)response.StatusCode;
            }
        }
        catch (Exception ex)
        {
            message = ex.Message;
            ERROR_CODE = -1;
        }

        return (count, message, ERROR_CODE);
    }
    public async Task<(int, string, int)> CountShoesAsync()
    {
        var count = 0;
        var message = string.Empty;
        var ERROR_CODE = 0;

        try
        {
            using var client = _httpClientFactory.CreateClient("Backend");
            using var response = await client.GetAsync($"statistics/count-books");
            var content = response.Content.ReadAsStringAsync().Result;
            var httpResponse = JsonSerializer.Deserialize<HttpDataSchemaResponse<int>>(content);


            if (response.IsSuccessStatusCode)
            {
                count = httpResponse.Data;
            }
            else
            {
                message = "Internal Error";
                ERROR_CODE = (int)response.StatusCode;
            }
        }
        catch (Exception ex)
        {
            message = ex.Message;
            ERROR_CODE = -1;
        }

        return (count, message, ERROR_CODE);
    }
    public async Task<(int, string, int)> CountNewOrdersAsync(string query)
    {
        var count = 0;
        var message = string.Empty;
        var ERROR_CODE = 0;

        try
        {
            using var client = _httpClientFactory.CreateClient("Backend");
            using var response = await client.GetAsync($"statistics/count-new-orders?type={query}");
            var content = response.Content.ReadAsStringAsync().Result;
            var httpResponse = JsonSerializer.Deserialize<HttpDataSchemaResponse<int>>(content);


            if (response.IsSuccessStatusCode)
            {
                count = httpResponse.Data;
            }
            else
            {
                message = "Internal Error";
                ERROR_CODE = (int)response.StatusCode;
            }
        }
        catch (Exception ex)
        {
            message = ex.Message;
            ERROR_CODE = -1;
        }

        return (count, message, ERROR_CODE);
    }

    public async Task<(IEnumerable<Revenue_Profit>, string, int)> GetRevenue_ProfitAsync(string query)
    {
        var categories = new List<Revenue_Profit>();
        var message = string.Empty;
        var ERROR_CODE = 0;

        try
        {
            using var client = _httpClientFactory.CreateClient("Backend");
            using var response = await client.GetAsync($"statistics/revenue-and-profit-stats?type={query}");
            var content = response.Content.ReadAsStringAsync().Result;
            var httpResponse = JsonSerializer.Deserialize<HttpDataSchemaResponse<IEnumerable<Revenue_Profit>>>(content);

            if (response.IsSuccessStatusCode)
            {
                categories = httpResponse.Data.ToList();
            }
            else
            {
                message = httpResponse.Error.Message;
                ERROR_CODE = (int)response.StatusCode;
            }
        }
        catch (Exception ex)
        {
            message = ex.Message;
            ERROR_CODE = -1;
        }

        return (categories, message, ERROR_CODE);
    }

    public async Task<(IEnumerable<ShoesSaleStat>, string, int)> GetShoesSaleStatAsync(string query)
    {

        var categories = new List<ShoesSaleStat>();
        var message = string.Empty;
        var ERROR_CODE = 0;

        try
        {
            using var client = _httpClientFactory.CreateClient("Backend");
            using var response = await client.GetAsync($"statistics/book-sale-stats?type={query}");
            var content = response.Content.ReadAsStringAsync().Result;
            var httpResponse = JsonSerializer.Deserialize<HttpDataSchemaResponse<IEnumerable<ShoesSaleStat>>>(content);

            if (response.IsSuccessStatusCode)
            {
                categories = httpResponse.Data.ToList();
            }
            else
            {
                message = httpResponse.Error?.Message;
                ERROR_CODE = (int)response.StatusCode;
            }
        }
        catch (Exception ex)
        {
            message = ex.Message;
            ERROR_CODE = -1;
        }
        return (categories, message, ERROR_CODE);

    }
}