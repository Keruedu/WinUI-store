using ShoesShop.Core.Models;

namespace ShoesShop.Core.Contracts.Services;
public interface IReviewDataService
{
    public string ShoesId
    {
        set; get;
    }
    public Task<(IEnumerable<Review>, int, string, int)> LoadDataAsync();
    public (IEnumerable<Review>, int, string, int) GetData();
}
