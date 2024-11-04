using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Models;

namespace ShoesShop.Core.Services;
public class ShoesDataService : IShoesDataService
{
    //private readonly IShoesRepository _ShoesRepository;

    // (Shoess, totalItems, errorMessage, ErrorCode)
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

    public (IEnumerable<Shoes>, int, string, int) GetData() => _ShoesDataTuple;

    public ShoesDataService(/*IShoesRepository ShoesRepository*/)
    {
        //_ShoesRepository = ShoesRepository;
    }

    public async Task<(IEnumerable<Shoes>, int, string, int)> LoadDataAsync()
    {
        //_ShoesDataTuple = await _ShoesRepository.GetAllShoessAsync(SearchParams);

        //return _ShoesDataTuple;

        return (null, 0, "Error", 1);
    }

    public async Task<(Shoes, string, int)> CreateShoesAsync(Shoes Shoes)
    {
        //return await Task.Run(async () => await _ShoesRepository.CreateAShoesAsync(Shoes));
        return await Task.FromResult((Shoes, "Success", 1));
    }

    public async Task<(string, int)> DeleteShoesAsync(Shoes Shoes)
    {
        //return await Task.Run(async () => await _ShoesRepository.DeleteShoesAsync(Shoes));
        return await Task.FromResult(("Success", 1));
    }

    public async Task<(Shoes, string, int)> UpdateShoesAsync(Shoes Shoes)
    {
        //return await Task.Run(async () => await _ShoesRepository.UpdateShoesAsync(Shoes));
        return await Task.FromResult((Shoes, "Success", 1));
    }

    public async Task<(string, int)> ImportDataAsync(IEnumerable<Shoes> Shoess)
    {
        //return await Task.Run(async () => await _ShoesRepository.ImportDataAsync(Shoess));
        return await Task.FromResult(("Success", 1));
    }
}
