using ShoesShop.Core.Models;

namespace ShoesShop.Core.Contracts.Services;

public interface ICategoryDataService
{
    public List<Category> Categories
    {
        get;
    }

    public Task<(IEnumerable<Category>, string, int)> LoadDataAsync();
    public (IEnumerable<Category>, string, int) GetData();

    public Task<(Category, string, int)> AddCategoryAsync(Category category);

    public Task<(Category, string, int)> UpdateCategoryAsync(Category category);

    public Task<(string, int)> DeleteCategoryAsync(Category category);

    public Task<(string, int)> ImportDataAsync(IEnumerable<Category> categories);
}
