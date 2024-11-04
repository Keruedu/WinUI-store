using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoesShop.Core.Models;

namespace ShoesShop.Core.Contracts.Services;
public interface IStatisticDataService
{
    public Task<(int, string, int)> LoadDataAsync(string query);
    public (int, string, int) CountSellingShoesAsync(string query);
    public (int, string, int) CountShoesAsync();

    public (int, string, int) CountNewOrdersAsync(string query);

    public Task<(IEnumerable<Revenue_Profit>, string, int)> GetRevenue_ProfitAsync(string query);
    public Task<(IEnumerable<ShoesSaleStat>, string, int)> GetShoesSaleStatAsync(string query);
}
