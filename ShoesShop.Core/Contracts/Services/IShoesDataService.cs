using ShoesShop.Core.Models;
using ShoesShop.Core.Services.DataAcess;

namespace ShoesShop.Core.Contracts.Services;
public interface IShoesDataService
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

    public Task<(IEnumerable<Shoes>, int, string, int)> LoadDataAsync();
    public (IEnumerable<Shoes>, int, string, int) GetData();

    public Task<(Shoes, string, int)> CreateShoesAsync(Shoes Shoes);

    public Task<(string, int)> DeleteShoesAsync(Shoes Shoes);

    public Task<(Shoes, string, int)> UpdateShoesAsync(Shoes Shoes);
    public Task<(int, string, int)> GetShoesCountByCategoryIdAsync(int categoryId);

    public Task<(string, int)> ImportDataAsync(IEnumerable<Shoes> Shoes);
}
