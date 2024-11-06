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
    
    public Tuple<List<Shoes>, long> GetShoes(
        int page, int rowsPerPage,
        Dictionary<string, Tuple<decimal, decimal>> numberFieldsOptions,
        Dictionary<string, string> textFieldsOptions,
        Dictionary<string, IDao.SortType> sortOptions);

    public Tuple<List<Order>, long>  GetOrders(
        int page, int rowsPerPage,
        Dictionary<string,Tuple<string,string>> dateFieldsOptions,
        Dictionary<string, Tuple<decimal, decimal>> numberFieldsOptions,
        Dictionary<string, string> textFieldsOptions,
        Dictionary<string, IDao.SortType> sortOptions);

    public Tuple<List<Category>, long> GetCategories(
        int page, int rowsPerPage,
        Dictionary<string, Tuple<decimal, decimal>> numberFieldsOptions,
        Dictionary<string, string> textFieldsOptions,
        Dictionary<string, IDao.SortType> sortOptions);

    public Tuple<Category, string, int> AddCategory(Category newCategory);
    public Tuple<Category, string, int> UpdateCategory(Category newCategory);
    public Tuple<string, int> DeleteCategory(int categoryId);

    public Tuple<List<OrderDetail>, long> GetOrderDetailsByID(
        int orderID,
        int page, int rowsPerPage,
        Dictionary<string, string> whereOptions,
        Dictionary<string, SortType> sortOptions);

    public Tuple<List<User>, long> GetUsers(
        int page, int rowsPerPage,
        Dictionary<string, Tuple<decimal, decimal>> numberFieldsOptions,
        Dictionary<string, string> textFieldsOptions,
        Dictionary<string, SortType> sortOptions);

    public User GetUserByID(
        int userID);

    public Tuple<bool, string> DeleteShoesByID(int shoesID);
    public Tuple<bool, string, Shoes> AddShoes(Shoes newShoes);

    public Tuple<bool, string, Shoes> UpdateShoes(Shoes newShoes);
}
