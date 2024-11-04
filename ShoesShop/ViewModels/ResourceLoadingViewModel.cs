using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShoesShop.Contracts.Services;
using ShoesShop.Core.Helpers;
using ShoesShop.Core.Models;


namespace ShoesShop.Services;

public partial class ResourceLoadingViewModel : ObservableRecipient
{
    [ObservableProperty]
    public bool isLoading;

    [ObservableProperty]
    public string? errorMessage = string.Empty;

    [ObservableProperty]
    public string? infoMessage = string.Empty;

    [ObservableProperty]
    public int currentPage = 1;

    [ObservableProperty]
    public int totalItems = 0;

    public int ItemsPerPage
    {
        get; set;
    } = 10;

    public List<SortObject> SortOptions
    {
        get; set;
    } = new List<SortObject>();
    public SortObject? SelectedSortOption
    {
        get; set;
    }
    public string SearchQuery
    {
        get; set;
    } = string.Empty;

    public int MinPrice
    {
        get; set;
    }
    public int MaxPrice
    {
        get; set;
    }

    [ObservableProperty]
    public List<Category> categoryFilters = new() { new() { ID = 0, Name = "All" } };
    public Category? SelectedCategory
    {
        get; set;
    }

    public DateTime? FromDate
    {
        get; set;
    }

    public DateTime? ToDate
    {
        get; set;
    }

    public int TotalPages => (int)Math.Ceiling((double)TotalItems / ItemsPerPage);
    public bool HasNextPage => CurrentPage < TotalPages;
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
    public bool HasInfo => !string.IsNullOrEmpty(InfoMessage);
    public bool ContentIsReady => !IsLoading && !HasError && !HasInfo;
    public int From => (CurrentPage - 1) * ItemsPerPage + 1;
    public int To => Math.Min(CurrentPage * ItemsPerPage, TotalItems);
    public bool ShowPagination => TotalPages > 1;

    [ObservableProperty]
    public bool isDirty = false;

    public Action FunctionOnCommand { get; set; } = () => { };

    public RelayCommand GoToNextPageCommand
    {
        get;
    }

    public RelayCommand GoToPreviousPageCommand
    {
        get;
    }

    private readonly IStorePageSettingsService _storePageSettingsService;

    public ResourceLoadingViewModel(IStorePageSettingsService storePageSettingsService)
    {
        _storePageSettingsService = storePageSettingsService;

        GoToNextPageCommand = new RelayCommand(GoToNextPage, () => HasNextPage);
        GoToPreviousPageCommand = new RelayCommand(GoToPreviousPage, () => HasPreviousPage);
    }

    protected async Task<string> BuildSearchParamsAsync()
    {
        ItemsPerPage = await _storePageSettingsService.GetItemsPerPageAsync();

        var paramBuilder = new SearchParamsBuilder();

        paramBuilder.Append("page", CurrentPage);
        paramBuilder.Append("limit", ItemsPerPage);

        if (SelectedSortOption is not null && SelectedSortOption.Value != "default")
        {
            paramBuilder.Append("sort", SelectedSortOption.SortString);
        }

        if (!string.IsNullOrEmpty(SearchQuery))
        {
            paramBuilder.Append("q", SearchQuery);
        }

        if (MinPrice > 0)
        {
            paramBuilder.Append("sellingPrice[gte]", MinPrice);
        }

        if (MaxPrice > 0)
        {
            paramBuilder.Append("sellingPrice[lte]", MaxPrice);
        }

        if (SelectedCategory is not null && SelectedCategory.ID != 0)
        {
            paramBuilder.Append("category", SelectedCategory.ID);
        }

        if (FromDate is not null)
        {
            paramBuilder.Append("orderDate[gte]", FromDate.Value.ToString("yyyy-MM-dd"));
        }

        if (ToDate is not null)
        {
            paramBuilder.Append("orderDate[lte]", ToDate.Value.ToString("yyyy-MM-dd"));
        }

        return paramBuilder.GetQueryString();
    }

    public void NotfifyChanges()
    {
        GoToPreviousPageCommand.NotifyCanExecuteChanged();
        GoToNextPageCommand.NotifyCanExecuteChanged();

        OnPropertyChanged(nameof(HasNextPage));
        OnPropertyChanged(nameof(HasPreviousPage));
        OnPropertyChanged(nameof(HasError));
        OnPropertyChanged(nameof(HasInfo));
        OnPropertyChanged(nameof(ContentIsReady));
        OnPropertyChanged(nameof(TotalPages));
        OnPropertyChanged(nameof(From));
        OnPropertyChanged(nameof(To));
        OnPropertyChanged(nameof(ShowPagination));
    }

    public virtual void GoToNextPage()
    {
        if (HasNextPage)
        {
            CurrentPage++;
            NotfifyChanges();
            FunctionOnCommand();
        }
    }

    public virtual void GoToPreviousPage()
    {
        if (HasPreviousPage)
        {
            CurrentPage--;
            NotfifyChanges();
            FunctionOnCommand();
        }
    }

    public virtual void SelectSortOption(SortObject sortOption)
    {
        if (sortOption.Value == "default")
        {
            if (SelectedSortOption?.Value != sortOption.Value)
            {
                IsDirty = true;
            }

            SelectedSortOption = null;
        }
        else
        {
            SelectedSortOption = sortOption;
            IsDirty = true;
        }
    }

    public virtual void SelectCategory(Category category)
    {
        if (category.ID == 0)
        {
            if (SelectedCategory?.Name != category.Name)
            {
                IsDirty = true;
            }

            SelectedCategory = null;
        }
        else
        {
            SelectedCategory = category;
            IsDirty = true;
        }
    }

    public virtual void Search(string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            if (!string.IsNullOrEmpty(SearchQuery))
            {
                IsDirty = true;
            }

            SearchQuery = string.Empty;
            return;
        }

        SearchQuery = query;
        IsDirty = true;
    }

    public virtual void ClearFilters()
    {
        if (SelectedSortOption is not null)
        {

            IsDirty = true;
        }

        SelectedSortOption = null;
        SearchQuery = string.Empty;
        MinPrice = 0;
        MaxPrice = 0;
    }

    public virtual void SetMinPrice(int price)
    {
        if (price == MinPrice)
        {
            return;
        }

        MinPrice = price;
        IsDirty = true;
    }

    public virtual void SetMaxPrice(int price)
    {
        if (price == MaxPrice)
        {
            return;
        }

        MaxPrice = price;
        IsDirty = true;
    }

    public virtual void SetFromDate(DateTime? date)
    {

        if (FromDate == date)
        {
            return;
        }

        FromDate = date;
        IsDirty = true;
    }

    public virtual void SetToDate(DateTime? date)
    {
        if (ToDate == date)
        {
            return;
        }

        ToDate = date;
        IsDirty = true;
    }
}

