using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using ShoesShop.Contracts.ViewModels;
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Contracts.Services;
using ShoesShop.Core.Models;
using ShoesShop.Core.Services;
using ShoesShop.Services;
namespace ShoesShop.ViewModels;

public partial class ShoesViewModel : ResourceLoadingViewModel, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly IShoesDataService _ShoesDataService;
    private readonly ICategoryDataService _categoryDataService;

    public ObservableCollection<Shoes> Source { get; } = new ObservableCollection<Shoes>();

    public RelayCommand NavigateToAddShoesPageCommand
    {
        get;
    }

    public ShoesViewModel(INavigationService navigationService, IShoesDataService ShoesDataService, ICategoryDataService categoryDataService, IStorePageSettingsService storePageSettingsService) : base(storePageSettingsService)
    {
        _navigationService = navigationService;
        _ShoesDataService = ShoesDataService;
        _categoryDataService = categoryDataService;
        FunctionOnCommand = LoadData;

        SortOptions = new List<SortObject>
        {
            new() { Name = "Default", Value = "default", IsAscending = true },
            new() { Name = "Name (A-Z)", Value = "Name", IsAscending = true },
            new() { Name = "Name (Z-A)", Value="Name", IsAscending = false },
            new() { Name = "Price (Low - High)", Value="Price", IsAscending = true },
            new() { Name = "Price (High - Low)", Value="Price", IsAscending = false },
            new() { Name = "Stock (Past)", Value="Stock", IsAscending = true },
            new() { Name = "Stock (Recent)", Value="Stock", IsAscending = false },
        };
        SelectedSortOption = SortOptions[0];
        NavigateToAddShoesPageCommand = new RelayCommand(() => _navigationService.NavigateTo(typeof(AddShoesViewModel).FullName!));
        LoadCategories();
    }

    public async void LoadCategories()
    {
        //await Task.Run(async () => await _categoryDataService.LoadDataAsync());
        await _categoryDataService.LoadDataAsync();
        var (categories, _, _) = _categoryDataService.GetData();

        if (categories is not null)
        {
            foreach (var category in categories)
            {
                if (!CategoryFilters.Any(c => c.ID == category.ID))
                {
                    CategoryFilters.Add(category);
                }
            }
        }

    }

    public async void LoadData()
    {
        IsDirty = false;
        IsLoading = true;
        InfoMessage = string.Empty;
        ErrorMessage = string.Empty;
        NotfifyChanges();

        //use rest api, not implemented yet
        //_ShoesDataService.SearchParams = await BuildSearchParamsAsync();

        _ShoesDataService.searchQuery = await BuildSearchQueryAsync();

        await Task.Run(async () => await _ShoesDataService.LoadDataAsync());


        var (data, totalItems, message, ERROR_CODE) = _ShoesDataService.GetData();

        if (data is not null)
        {
            Source.Clear();

            foreach (var item in data)
            {
                if (item.Image is null || item.Image == "")
                {
                    item.Image = "https://res.cloudinary.com/dyocg3k6j/image/upload/v1732185753/cld-sample-5.jpg";
                }
                Source.Add(item);

            }

            TotalItems = totalItems;

            if (TotalItems == 0)
            {
                InfoMessage = "No Shoes found";
            }
        }
        else
        {
            if (ERROR_CODE == 0)
            {
                ErrorMessage = message;
            }
        }

        IsLoading = false;
        NotfifyChanges();
    }

    public void OnNavigatedTo(object parameter)
    {
        if (Source.Count <= 0)
        {
            LoadData();
            LoadCategories();
        }
    }

    public void OnNavigatedFrom()
    {
    }

    [RelayCommand]
    private void OnItemClick(Shoes? clickedItem)
    {
        if (clickedItem != null)
        {
            _navigationService.SetListDataItemForNextConnectedAnimation(clickedItem);
            _navigationService.NavigateTo(typeof(ShoesDetailViewModel).FullName!, clickedItem);
        }
    }

    [RelayCommand]
    private void OnApplyFiltersAndSearch()
    {
        currentPage = 1;
        LoadData();
    }
}
