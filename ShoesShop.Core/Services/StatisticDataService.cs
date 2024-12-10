using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Models;
using ShoesShop.Core.Services.DataAcess;

namespace ShoesShop.Core.Services
{
    public class StatisticDataService : IStatisticDataService
    {
        private readonly IDao _dao;
        private readonly IAuthenticationService _authenticationService;

        public StatisticDataService(IDao dao, IAuthenticationService authenticationService)
        {
            _dao = dao;
            _authenticationService = authenticationService;
        }

        public async Task<List<Order>> GetRecentOrdersAsync()
        {
            return await _dao.GetRecentOrdersAsync();
        }

        public async Task<int> GetTotalOrdersAsync()
        {
            return await _dao.GetTotalOrdersAsync();
        }

        public async Task<List<Shoes>> GetTop5BestSellingShoesAsync()
        {
            return await _dao.GetTop5BestSellingShoesAsync();
        }

        public async Task<int> GetTotalShoesInStockAsync()
        {
            return await _dao.GetTotalShoesInStockAsync();
        }

        public async Task<Dictionary<string, int>> GetOrderStatisticsAsync(string groupBy)
        {
            return await _dao.GetOrderStatisticsAsync(groupBy);
        }
    }
}
