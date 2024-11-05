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
        CategoryList.Add(new Category
        {
            ID = 1,
            Name = "Sneakers",
            Description = "Comfortable and stylish sneakers for everyday wear."
        });

        CategoryList.Add(new Category
        {
            ID = 2,
            Name = "Boots",
            Description = "Durable boots suitable for hiking and rough terrains."
        });

        CategoryList.Add(new Category
        {
            ID = 3,
            Name = "Sandals",
            Description = "Lightweight sandals perfect for warm weather."
        });

        CategoryList.Add(new Category
        {
            ID = 4,
            Name = "Formal Shoes",
            Description = "Elegant formal shoes for office and special occasions."
        });

        CategoryList.Add(new Category
        {
            ID = 5,
            Name = "Slippers",
            Description = "Cozy slippers for indoor comfort."
        });

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
