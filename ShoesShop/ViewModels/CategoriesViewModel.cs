using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using ShoesShop.Contracts.ViewModels;
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Models;
using ShoesShop.Core.Services;
using ShoesShop.Core.Services.DataAcess;

namespace ShoesShop.ViewModels;

public partial class CategoriesViewModel : ObservableRecipient, INavigationAware
{
    private readonly ICategoryDataService _categoryDataService;

    [ObservableProperty]
    public Category? selected;

    [ObservableProperty]
    public bool isLoading = true;

    [ObservableProperty]
    public bool isContentReady = false;

    public ObservableCollection<Category> CategoryList { get; private set; } = new();

    public CategoriesViewModel(ICategoryDataService categoryDataService)
    {
        _categoryDataService = categoryDataService;
    }


    public async void LoadCategories()
    {
        await Task.Run(async () => await _categoryDataService.LoadDataAsync());
        var (categories, _, _) = _categoryDataService.GetData();

        if (categories is not null)
        {
            foreach (var category in categories)
            {
                CategoryList.Add(category);
            }
        }

        IsLoading = false;
        IsContentReady = true;
    }

    public void OnNavigatedTo(object parameter)
    {
        LoadCategories();
        EnsureItemSelected();
    }

    public void OnNavigatedFrom()
    {
    }

    public void EnsureItemSelected()
    {
        if (CategoryList.Count > 0)
        {
            Selected ??= CategoryList[0];
        }
    }
}
