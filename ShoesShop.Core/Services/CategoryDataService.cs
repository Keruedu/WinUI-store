
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Models;


namespace ShoesShop.Core.Services;

public class CategoryDataService : ICategoryDataService
{
    //private readonly ICategoryRepository _categoryRepository;
    private (List<Category>, string, int) _categoryTuple;

    public List<Category> Categories => _categoryTuple.Item1;
    private bool IsAlreadyFetched = false;
    public bool IsDirty = false;

    public CategoryDataService(/*ICategoryRepository categoryRepository*/)
    {
        //_categoryRepository = categoryRepository;
    }

    public (IEnumerable<Category>, string, int) GetData() => _categoryTuple;

    public async Task<(IEnumerable<Category>, string, int)> LoadDataAsync()
    {
        if (!IsAlreadyFetched || IsDirty)
        {
            //_categoryTuple = ((List<Category>, string, int))await _categoryRepository.GetCategoriesAsync();
            IsAlreadyFetched = true;
        }

        return _categoryTuple;
    }

    public Task<(Category, string, int)> AddCategoryAsync(Category category)
    {
        IsDirty = true;
        //return Task.Run(async () => await _categoryRepository.CreateCategoryAsync(category));
        return Task.FromResult((category, "Success", 1));
    }

    public Task<(Category, string, int)> UpdateCategoryAsync(Category category)
    {
        IsDirty = true;
        //return Task.Run(async () => await _categoryRepository.UpdateCategoryAsync(category));
        return Task.FromResult((category, "Success", 1));
    }

    public Task<(string, int)> DeleteCategoryAsync(Category category)
    {
        IsDirty = true;
        //return Task.Run(async () => await _categoryRepository.DeleteCategoryAsync(category));
        return Task.FromResult(("Success", 1));
    }

    public Task<(string, int)> ImportDataAsync(IEnumerable<Category> categories)
    {
        IsDirty = true;
        //return Task.Run(async () => await _categoryRepository.ImportDataAsync(categories));
        return Task.FromResult(("Success", 1));
    }
}
