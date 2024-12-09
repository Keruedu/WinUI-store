using CloudinaryDotNet.Actions;
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Models;
using ShoesShop.Core.Services.DataAcess;

namespace ShoesShop.Core.Services;
public class OrderDataService : IOrderDataService
{
    private readonly IDao _dao;
    private readonly IAuthenticationService _authenticationService;

    // (orders, totalItems, errorMessage, ErrorCode)
    private (IEnumerable<Order>, int, string, int) _orderDataTuple;

    public bool IsInitialized => _orderDataTuple.Item1 is not null;
    public bool IsDirty { get; set; } = true;

    private string _searchParams;
    public string SearchParams
    {

        get => _searchParams;
        set
        {
            _searchParams = value;
            IsDirty = true;
        }
    }

    private Tuple<int, int,
    Dictionary<string, Tuple<string, string>>,
    Dictionary<string, Tuple<decimal, decimal>>,
    Dictionary<string, string>,
    Dictionary<string, IDao.SortType>> _searchQuery;

    public Tuple<int, int,
    Dictionary<string, Tuple<string, string>>,
    Dictionary<string, Tuple<decimal, decimal>>,
    Dictionary<string, string>,
    Dictionary<string, IDao.SortType>> SearchQuery
    {
        get => _searchQuery;
        set
        {
            _searchQuery = value;
        }
    }

    public (IEnumerable<Order>, int, string, int) GetData() => _orderDataTuple;

    public OrderDataService(IDao dao, IAuthenticationService authenticationService)
    {
        _dao = dao;
        _authenticationService = authenticationService;
    }

    public async Task<(IEnumerable<Order>, int, string, int)> LoadDataAsync()
    {
        var (currentPage, itemsPerPage, dateFieldOptions, numberFieldsOptions, textFieldsOptions, sortOptions) = _searchQuery;

        // Get Orders
        var (success, message, orders, totalOrders) = _dao.GetOrders(currentPage, itemsPerPage, dateFieldOptions, numberFieldsOptions, textFieldsOptions, sortOptions);
        if (!success)
        {
            _orderDataTuple = (null, 0, message, 0);
            return _orderDataTuple;
        }

        _orderDataTuple = (orders, (int)totalOrders, "Success", 1);
        return _orderDataTuple;
    }

    public async Task<(Order, string, int)> CreateAOrderAsync(Order newOrder)
    {

        //// 1. Thêm Order
        //var (success, message, orderAfterAdd) = _dao.AddOrder(newOrder);
        //if (!success)
        //    return await Task.FromResult((new Order(), message, 0));

        //// 2. Thêm từng chi tiết vào Order
        //foreach (var detail in newOrder.Details)
        //{
        //    var (detailSuccess, detailMessage) = _dao.AddDetail(orderAfterAdd.ID, detail);
        //    if (!detailSuccess)
        //        return await Task.FromResult((new Order(), detailMessage, 0));
        //}

        //// 3. Trả về đối tượng Order đã hoàn thành
        //return await Task.FromResult((orderAfterAdd, "Order created successfully.", 1));
        var (errorCode, Message, order) = _dao.AddOrder(newOrder);
        return await Task.FromResult((order, Message, errorCode ? 1 : 0));
    }

    public async Task<(Order, string, int)> UpdateOrderAsync(Order order)
    {
        var (errorCode, Message, _) = _dao.UpdateOrder(order);
        return await Task.FromResult((order, Message, errorCode ? 1 : 0));
    }

    public async Task<(string, int)> DeleteOrderAsync(Order order)
    {
        var (errorCode, Message) = _dao.DeleteOrder(order);
        return await Task.FromResult((Message, errorCode ? 1 : 0));
    }
}
