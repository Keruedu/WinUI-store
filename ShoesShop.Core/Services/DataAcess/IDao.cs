using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using ShoesShop.Core.Models;

namespace ShoesShop.Core.Services.DataAcess;
public interface IDao
{
    public enum SortType
    {
        Ascending,
        Descending
    }
    //Shoes
    public Tuple<List<Shoes>, long> GetShoes(
        int page, int rowsPerPage,
        Dictionary<string, Tuple<decimal, decimal>> numberFieldsOptions,
        Dictionary<string, string> textFieldsOptions,
        Dictionary<string, IDao.SortType> sortOptions);
    public Tuple<bool, string, Dictionary<int, Shoes>> GetShoesByIds(List<int> shoesIds);
    public Tuple<bool, string> DeleteShoesByID(int shoesID);
    public Tuple<bool, string, Shoes> AddShoes(Shoes newShoes);

    public Tuple<bool, string, Shoes> UpdateShoes(Shoes newShoes);

    public int GetTotalShoesSoldByIdAsync(int shoesId);

    //Order
    public Tuple<bool, string, List<Order>, long> GetOrders(
        int page, int rowsPerPage,
        Dictionary<string,Tuple<string,string>> dateFieldsOptions,
        Dictionary<string, Tuple<decimal, decimal>> numberFieldsOptions,
        Dictionary<string, string> textFieldsOptions,
        Dictionary<string, IDao.SortType> sortOptions);

     public Tuple<bool, string, Order> AddOrder(Order newOrder);
    public Tuple<bool, string, Order> UpdateOrder(Order order);
    public Tuple<bool, string> DeleteOrder(Order order);
    //Category
    public Dictionary<int, string> GetAllCategories();
    public Tuple<List<Category>, long> GetCategories(
        int page, int rowsPerPage,
        Dictionary<string, Tuple<decimal, decimal>> numberFieldsOptions,
        Dictionary<string, string> textFieldsOptions,
        Dictionary<string, IDao.SortType> sortOptions);

    public Tuple<Category, string, int> AddCategory(Category newCategory);
    public Tuple<Category, string, int> UpdateCategory(Category newCategory);
    public Tuple<string, int> DeleteCategory(int categoryId);


    //User
    public Tuple<bool, string, List<User>, long> GetUsers(
        int page, int rowsPerPage,
        Dictionary<string, Tuple<decimal, decimal>> numberFieldsOptions,
        Dictionary<string, string> textFieldsOptions,
        Dictionary<string, SortType> sortOptions);
    public Tuple<bool, string, User> AddUser(User user);
    public Tuple<bool, string, User> UpdateUser(User user);
    public Tuple<bool, string> BanAndUnbanUser(User user);
    public User GetUserByID(
        int userID);
    public Tuple<bool, string, Dictionary<int, Address>> GetAddressesByIds(List<int> addressIds);
    public Tuple<bool, string, Dictionary<int, User>> GetUserByIds(List<int> userIds);
    public Tuple<bool, string, List<Detail>> GetDetailsByOrderIds(List<int> orderIds);
    public Tuple<bool, string> AddDetail(int orderId, Detail detail);
    public Tuple<bool, string> DeleteDetail(int orderId, int detailId);

    //dashboard

    public Task<List<Order>> GetRecentOrdersAsync();

    public Task<int> GetTotalOrdersAsync();

    public Task<int> GetTotalRevenueAsync();

    public Task<List<Shoes>> GetTop5BestSellingShoesAsync();

    public Task<int> GetTotalShoesInStockAsync();

    public User GetUserByName(string username);

    public Task<Dictionary<string, int>> GetOrderStatisticsAsync(string groupBy);

}
