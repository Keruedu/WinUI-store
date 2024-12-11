
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Models;
using ShoesShop.Core.Services.DataAcess;

namespace ShoesShop.Core.Services;

public class CategoryDataService : ICategoryDataService
{
    //private readonly ICategoryRepository _categoryRepository;
    private readonly IDao _dao;
    private (List<Category>, string, int) _categoryTuple;

    public List<Category> Categories => _categoryTuple.Item1;
    private bool IsAlreadyFetched = false;
    public bool IsDirty = false;

    public CategoryDataService(/*ICategoryRepository categoryRepository*/ IDao dao)
    {
        //_categoryRepository = categoryRepository;
        _dao = dao;
    }

    public (IEnumerable<Category>, string, int) GetData() => _categoryTuple;

    public async Task<(IEnumerable<Category>, string, int)> LoadDataAsync()
    {
        if (!IsAlreadyFetched || IsDirty)
        {
            //_categoryTuple = ((List<Category>, string, int))await _categoryRepository.GetCategoriesAsync();
            var numberFieldsOptions = new Dictionary<string, Tuple<decimal, decimal>>(); // Không có điều kiện số
            var textFieldsOptions = new Dictionary<string, string>(); // Không có điều kiện văn bản
            var sortOptions = new Dictionary<string, IDao.SortType>(); // Không có điều kiện sắp xếp
            var result =  _dao.GetCategories(1, int.MaxValue, numberFieldsOptions, textFieldsOptions, sortOptions);
            _categoryTuple = (result.Item1, "success", 1);
            IsAlreadyFetched = true;
        }

        return _categoryTuple;
    }

    public Task<(Category, string, int)> AddCategoryAsync(Category category)
    {
        IsDirty = true;
        var result = _dao.AddCategory(category);
        return Task.FromResult((result.Item1, result.Item2, result.Item3));
    }

    public Task<(Category, string, int)> UpdateCategoryAsync(Category category)
    {
        IsDirty = true;
        //return Task.Run(async () => await _categoryRepository.UpdateCategoryAsync(category));
        var result = _dao.UpdateCategory(category);
        return Task.FromResult((result.Item1, result.Item2, result.Item3));
    }

    public Task<(string, int)> DeleteCategoryAsync(Category category)
    {
        IsDirty = true;
        //return Task.Run(async () => await _categoryRepository.DeleteCategoryAsync(category));
        var result = _dao.DeleteCategory(category.ID);
        return Task.FromResult((result.Item1, result.Item2));
    }

    public Task<(string, int)> ImportDataAsync(IEnumerable<Category> categories)
    {
        IsDirty = true;
        //return Task.Run(async () => await _categoryRepository.ImportDataAsync(categories));
        return Task.FromResult(("Success", 1));
    }
}
