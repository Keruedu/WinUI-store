using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoesShop.Core.Contracts.Repository;
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Models;
using ShoesShop.Core.Repository;


namespace ShoesShop.Core.Services;
public class StatisticDataService : IStatisticDataService
{
    private readonly IStatisticRepository _statisticRepository;

    private (int, string, int) _countSellingShoes;
    private (int, string, int) _countShoes;
    private (int, string, int) _countNewOrders;

    public StatisticDataService(IStatisticRepository statisticRepository)
    {
        _statisticRepository = statisticRepository;
    }

    public async Task<(int, string, int)> LoadDataAsync(string query)
    {
        _countSellingShoes = await _statisticRepository.CountSellingShoesAsync(query);
        _countShoes = await _statisticRepository.CountShoesAsync();
        _countNewOrders = await _statisticRepository.CountNewOrdersAsync(query);

        return _countSellingShoes;
    }

    public (int, string, int) CountShoesAsync()
    {
        return _countShoes;
    }
    public (int, string, int) CountNewOrdersAsync(string query)
    {
        return _countNewOrders;

    }
    public (int, string, int) CountSellingShoesAsync(string query)
    {
        return _countSellingShoes;

    }

    public async Task<(IEnumerable<Revenue_Profit>, string, int)> GetRevenue_ProfitAsync(string query)
    {

        return await _statisticRepository.GetRevenue_ProfitAsync(query);

    }
        
    public async Task<(IEnumerable<ShoesSaleStat>, string, int)> GetShoesSaleStatAsync(string query)
    {
        return await _statisticRepository.GetShoesSaleStatAsync(query);
    }
}