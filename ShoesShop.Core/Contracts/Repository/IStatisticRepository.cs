using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoesShop.Core.Models;

namespace ShoesShop.Core.Contracts.Repository;
public interface IStatisticRepository
{
    public Task<(int, string, int)> CountSellingShoesAsync(string query);
    public Task<(int, string, int)> CountShoesAsync();

    public Task<(int, string, int)> CountNewOrdersAsync(string query);

    public Task<(IEnumerable<Revenue_Profit>, string, int)> GetRevenue_ProfitAsync(string query);
    public Task<(IEnumerable<ShoesSaleStat>, string, int)> GetShoesSaleStatAsync(string query);
}