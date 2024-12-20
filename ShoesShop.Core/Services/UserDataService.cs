using OfficeOpenXml;
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Models;
using ShoesShop.Core.Services.DataAcess;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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


    public async Task<(string, int)> ImportUserFromExcelAsync(string filePath)
    {
        var users = new List<User>();
        try
        {
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    var user = new User
                    {
                        Name = worksheet.Cells[row, 1].Text,
                        Email = worksheet.Cells[row, 2].Text,
                        Role = worksheet.Cells[row, 3].Text,
                        Password = worksheet.Cells[row, 4].Text,
                        PhoneNumber = worksheet.Cells[row, 5].Text,
                        Status = worksheet.Cells[row, 6].Text,
                        Image = worksheet.Cells[row, 7].Text,
                        Address = new Address
                        {
                            Street = worksheet.Cells[row, 8].Text,
                            City = worksheet.Cells[row, 9].Text,
                            State = worksheet.Cells[row, 10].Text,
                            ZipCode = worksheet.Cells[row, 11].Text,
                            Country = worksheet.Cells[row, 12].Text
                        }
                    };
                    users.Add(user);
                }
            }

            foreach (var user in users)
            {
                var (errorCode, message, _) = _dao.AddUser(user);
                if (!errorCode)
                {
                    return (message, 0);
                }
            }

            return ("Import successful", 1);
        }
        catch (Exception ex)
        {
            return ($"Error: {ex.Message}", 0);
        }
    }

    public async Task<(string, int)> ImportShoesFromExcelAsync(string filePath)
    {
        var shoesList = new List<Shoes>();
        try
        {
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    var shoes = new Shoes
                    {
                        Name = worksheet.Cells[row, 1].Text,
                        Brand = worksheet.Cells[row, 2].Text,
                        Size = worksheet.Cells[row, 3].Text,
                        Color = worksheet.Cells[row, 4].Text,
                        Cost = decimal.Parse(worksheet.Cells[row, 5].Text),
                        Price = decimal.Parse(worksheet.Cells[row, 6].Text),
                        Stock = int.Parse(worksheet.Cells[row, 7].Text),
                        Image = worksheet.Cells[row, 8].Text,
                        Description = worksheet.Cells[row, 9].Text,
                        Status = worksheet.Cells[row, 10].Text,
                        CategoryID = int.Parse(worksheet.Cells[row, 11].Text)
                    };
                    shoesList.Add(shoes);
                }
            }

            foreach (var shoes in shoesList)
            {
                var (errorCode, message, _) = _dao.AddShoes(shoes);
                if (!errorCode)
                {
                    return (message, 0);
                }
            }

            return ("Import successful", 1);
        }
        catch (Exception ex)
        {
            return ($"Error: {ex.Message}", 0);
        }
    }





}
