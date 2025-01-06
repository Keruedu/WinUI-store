using System;
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Models;
using ShoesShop.Core.Services.DataAcess;
namespace ShoesShop.Core.Services;
public class ShoesDataService : IShoesDataService
{
    private readonly IDao _dao;
    // (Shoes, totalItems, errorMessage, ErrorCode)
    private (IEnumerable<Shoes>, int, string, int) _ShoesDataTuple;



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

    public (IEnumerable<Shoes>, int, string, int) GetData() => _ShoesDataTuple;

    public ShoesDataService(IDao dao)
    {
        _dao = dao;
    }

    public async Task<(IEnumerable<Shoes>, int, string, int)> LoadDataAsync()
    {
        var (currentPage, itemsPerPage, numberFieldsOptions, textFieldsOptions, sortOptions) = _searchQuery;
        var (listShoes, totalShoes) = _dao.GetShoes(currentPage, itemsPerPage, numberFieldsOptions, textFieldsOptions, sortOptions);
        _ShoesDataTuple = (listShoes, (int)totalShoes, "Success", 1);
        //return _ShoesDataTuple;
        return _ShoesDataTuple;

    }

    public async Task<(Shoes, string, int)> CreateShoesAsync(Shoes Shoes)
    {
        //return await Task.Run(async () => await _ShoesRepository.CreateAShoesAsync(Shoes));
        var (errorCode, Message, _) = _dao.AddShoes(Shoes);
        return await Task.FromResult((Shoes, Message, errorCode ? 1 : 0));
    }

    public async Task<(string, int)> DeleteShoesAsync(Shoes Shoes)
    {
        //return await Task.Run(async () => await _ShoesRepository.DeleteShoesAsync(Shoes));
        var (errorCode, Message) = _dao.DeleteShoesByID(Shoes.ID);
        return await Task.FromResult((Message, errorCode ? 1 : 0));
    }

    public async Task<(Shoes, string, int)> UpdateShoesAsync(Shoes Shoes)
    {
        //return await Task.Run(async () => await _ShoesRepository.UpdateShoesAsync(Shoes));
        var (errorCode, Message, _) = _dao.UpdateShoes(Shoes);
        return await Task.FromResult((Shoes, Message, errorCode ? 1 : 0));
    }

    public async Task<(IEnumerable<Shoes>, int, string, int)> GetInactiveShoesAsync()
    {
        var (_, _, numberFieldsOptions, textFieldsOptions, sortOptions) = _searchQuery;
        int currentPage = 1;
        int itemsPerPage = int.MaxValue;
        if (textFieldsOptions.ContainsKey("Status") && textFieldsOptions["Status"] != "Inactive")
        {
            return await Task.FromResult((Enumerable.Empty<Shoes>(), 0, "Status filter already exists", 0));
        }

        textFieldsOptions["Status"] = "Inactive";
        var (listShoes, totalShoes) = _dao.GetShoes(currentPage, itemsPerPage, numberFieldsOptions, textFieldsOptions, sortOptions);
        return await Task.FromResult((listShoes, (int)totalShoes, "Success", 1));
    }


    public async Task<(int, string, int)> GetShoesCountByCategoryIdAsync(int categoryId)
    {
        try
        {
            var numberFieldsOptions = new Dictionary<string, Tuple<decimal, decimal>>();
            var textFieldsOptions = new Dictionary<string, string>();
            var sortOptions = new Dictionary<string, IDao.SortType>();

            if (categoryId != 0)
            {
                numberFieldsOptions["CategoryID"] = new Tuple<decimal, decimal>(categoryId, categoryId);
            }

            var (listShoes, totalShoes) = _dao.GetShoes(
                page: 1,
                rowsPerPage: int.MaxValue,
                numberFieldsOptions: numberFieldsOptions,
                textFieldsOptions: textFieldsOptions,
                sortOptions: sortOptions
            );

            // Return the count of shoes
            return await Task.FromResult(((int)totalShoes, "Success", 1));
        }
        catch (Exception ex)
        {
            // Handle any errors that occur
            return await Task.FromResult((0, ex.Message, 0));
        }
    }

    public async Task<(string, int)> ImportDataAsync(IEnumerable<Shoes> Shoess)
    {
        //return await Task.Run(async () => await _ShoesRepository.ImportDataAsync(Shoess));
        return await Task.FromResult(("Success", 1));
    }
}
