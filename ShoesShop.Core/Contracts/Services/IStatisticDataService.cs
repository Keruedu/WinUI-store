using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoesShop.Core.Models;

namespace ShoesShop.Core.Contracts.Services;
public interface IStatisticDataService
{
    Task<int> GetTotalOrdersAsync();
    Task<List<Order>> GetRecentOrdersAsync();

    Task<List<Shoes>> GetTop5BestSellingShoesAsync();

    Task<int> GetTotalShoesInStockAsync();

    Task<Dictionary<string, int>> GetOrderStatisticsAsync(string groupBy);

}


