using ShoesShop.Core.Models;

namespace ShoesShop.Core.Contracts.Services;
public interface IOrderDataService
{
    public string SearchParams
    {
        get; set;
    }
    public bool IsDirty
    {
        get; set;
    }
    public Task<(IEnumerable<OrderDetail>, int, string, int)> LoadDataAsync();
    public (IEnumerable<OrderDetail>, int, string, int) GetData();

    Task<(OrderDetail, string, int)> CreateAOrderAsync(List<AddOrderDetail> addOrderDetail);

    Task<(OrderDetail, string, int)> UpdateOrderAsync(OrderDetail order);

    Task<(string, int)> DeleteOrderAsync(OrderDetail order);
}
