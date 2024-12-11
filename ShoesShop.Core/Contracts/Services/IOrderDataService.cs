using ShoesShop.Core.Models;
using ShoesShop.Core.Services.DataAcess;
namespace ShoesShop.Core.Contracts.Services;
public interface IOrderDataService
{
    public string SearchParams
    {
        get; set;
    }

    public Tuple<int, int,
    Dictionary<string, Tuple<string, string>>,
    Dictionary<string, Tuple<decimal, decimal>>,
    Dictionary<string, string>,
    Dictionary<string, IDao.SortType>> SearchQuery
    {

        get; set;
    }

    public bool IsDirty
    {
        get; set;
    }
    public Task<(IEnumerable<Order>, int, string, int)> LoadDataAsync();
    public (IEnumerable<Order>, int, string, int) GetData();

    Task<(Order, string, int)> CreateAOrderAsync(Order addOrderDetail);

    Task<(Order, string, int)> UpdateOrderAsync(Order order);

    Task<(string, int)> DeleteOrderAsync(Order order);
}
