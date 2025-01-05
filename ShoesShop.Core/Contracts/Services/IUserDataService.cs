using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoesShop.Core.Models;
using ShoesShop.Core.Services.DataAcess;

namespace ShoesShop.Core.Contracts.Services;
public interface IUserDataService
{
    public string SearchParams
    {
        get; set;
    }
    public Tuple<int, int,
        Dictionary<string, Tuple<decimal, decimal>>,
        Dictionary<string, string>,
        Dictionary<string, IDao.SortType>> searchQuery
    {
        get; set;
    }

    public Task<(IEnumerable<User>, int, string, int)> LoadDataAsync();
    public (IEnumerable<User>, int, string, int) GetData();

    public Task<(User, string, int)> CreateUserAsync(User User);
    public Task<(string, int)> BanAndUnbanUser(User User);
    public Task<(IEnumerable<User>, int, string, int)> GetBannedUsersAsync();

    public Task<(User, string, int)> UpdateUserAsync(User User);

    public  Task<(string, int)> ImportUserFromExcelAsync(string filePath);

    public Task<(string, int)> ImportShoesFromExcelAsync(string filePath);
    public Task<User> GetUserByIdAsync(int userId);
    public Task<(IEnumerable<User>, int, string, int)> GetCustomers();
}
