using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Models;
using ShoesShop.Core.Services.DataAcess;
namespace ShoesShop.Core.Services;
public class UserDataService : IUserDataService
{

    private readonly IDao _dao;
    // (Users, totalItems, errorMessage, ErrorCode)
    private (IEnumerable<User>, int, string, int) _UsersDataTuple;

    private string _searchParams;
    public string SearchParams
    {

        get => _searchParams;
        set
        {
            _searchParams = value;
        }
    }

    private Tuple<int, int,
    Dictionary<string, Tuple<decimal, decimal>>,
    Dictionary<string, string>,
    Dictionary<string, IDao.SortType>> _searchQuery;


    public Tuple<int, int,
    Dictionary<string, Tuple<decimal, decimal>>,
    Dictionary<string, string>,
    Dictionary<string, IDao.SortType>> searchQuery
    {
        get => _searchQuery;
        set
        {
            _searchQuery = value;
        }
    }
    public (IEnumerable<User>, int, string, int) GetData() => _UsersDataTuple;

    public UserDataService(IDao dao)
    {
        _dao = dao;
    }

    public async Task<(IEnumerable<User>, int, string, int)> LoadDataAsync()
    {
        var (currentPage, itemsPerPage, numberFieldsOptions, textFieldsOptions, sortOptions) = _searchQuery;
        var (success, message, users, totalUsers) = _dao.GetUsers(currentPage, itemsPerPage, numberFieldsOptions, textFieldsOptions, sortOptions);
        if (!success)
        {
            _UsersDataTuple = (null, 0, message, 0);
            return _UsersDataTuple;
        }

        _UsersDataTuple = (users, (int)totalUsers, "Success", 1);
        return _UsersDataTuple;
    }

    public async Task<(User, string, int)> CreateUserAsync(User User)
    {
        var (errorCode, Message, _) = _dao.AddUser(User);
        return await Task.FromResult((User, Message, errorCode ? 1 : 0));
    }

    public async Task<(string, int)> BanAndUnbanUser(User User)
    {
        var (errorCode, Message) = _dao.BanAndUnbanUser(User);
        return await Task.FromResult((Message, errorCode ? 1 : 0));
    }


    public async Task<(User, string, int)> UpdateUserAsync(User User)
    {
        var (errorCode, Message, _) = _dao.UpdateUser(User);
        return await Task.FromResult((User, Message, errorCode ? 1 : 0));
    }

    public async Task<(IEnumerable<User>, int, string, int)> GetBannedUsersAsync()
    {
        var (_, _, numberFieldsOptions, textFieldsOptions, sortOptions) = _searchQuery;
        int currentPage = 1;
        int itemsPerPage = int.MaxValue;
        if (textFieldsOptions.ContainsKey("Status") && textFieldsOptions["Status"] != "Banned")
        {
            return await Task.FromResult((Enumerable.Empty<User>(), 0, "Status filter already exists", 0));
        }

        textFieldsOptions["Status"] = "Banned";
        var (errorCode, Message, users, totalUsers) = _dao.GetUsers(currentPage, itemsPerPage, numberFieldsOptions, textFieldsOptions, sortOptions);
        return await Task.FromResult((users, (int)totalUsers, Message, errorCode ? 1 : 0));
    }

    public async Task<(string, int)> ImportDataAsync(IEnumerable<User> User)
    {
        //return await Task.Run(async () => await _ShoesRepository.ImportDataAsync(Shoess));
        return await Task.FromResult(("Success", 1));
    }


}
