using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using ShoesShop.Contracts.ViewModels;
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Models;
using ShoesShop.Core.Services;
using ShoesShop.Core.Services.DataAcess;
using CommunityToolkit.Mvvm.Input;
using ShoesShop.Contracts.Services;

namespace ShoesShop.ViewModels;

public partial class CategoriesViewModel : ObservableRecipient, INavigationAware
{
    private readonly ICategoryDataService _categoryDataService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    public Category? selected;

    [ObservableProperty]
    public bool isLoading = true;

    [ObservableProperty]
    public bool isContentReady = false;

    public ObservableCollection<Category> CategoryList { get; private set; } = new();

    public RelayCommand NavigateToAddCategoryPageCommand
    {
        get;
    }

    public CategoriesViewModel(ICategoryDataService categoryDataService, INavigationService navigationService)
    {
        _categoryDataService = categoryDataService;
        _navigationService = navigationService;
        NavigateToAddCategoryPageCommand = new RelayCommand(() => _navigationService.NavigateTo(typeof(AddCategoryViewModel).FullName!));
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
