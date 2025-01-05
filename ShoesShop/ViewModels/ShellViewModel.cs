using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Navigation;

using ShoesShop.Contracts.Services;
using ShoesShop.Core.Services.DataAcess;
using ShoesShop.Core.Services;
using ShoesShop.Views;
using static ShoesShop.Core.Services.DataAcess.IDao;
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Models;
using static ShoesShop.ViewModels.AddCategoryViewModel;
using ShoesShop.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using System.Text.Json;

namespace ShoesShop.ViewModels;

public partial class ShellViewModel : ObservableRecipient, INotifyPropertyChanged
{
    [ObservableProperty]
    private bool isBackEnabled;

    [ObservableProperty]
    private object? selected;

    [ObservableProperty]
    private bool isExpandedVisible = true;

    private readonly ICategoryDataService _categoryDataService;
    private readonly IShoesDataService _shoesDataService;
    private readonly IMediator _mediator;
    private readonly ILocalSettingServiceUsingApplicationData _localSettingServiceUsingApplicationData
        =App.GetService<ILocalSettingServiceUsingApplicationData>();

    public User user
    {
    get; set; 
    }

    public bool IsAdmin
    {
        get
        {
            return user.Role.ToLower() == "admin";
        }
    }

    public class CategoryFilter
    {
        public Category Category
        {
            get; set;
        }
        public int Count
        {
            get; set;
        }
    }

    public ObservableCollection<CategoryFilter> CategoryFilters { get; } = new ObservableCollection<CategoryFilter>();



    public Category? SelectedCategory
    {
        get; set;
    }

    public int NumberCategory
    {
        get; set;
    }

    public INavigationService NavigationService
    {
        get;
    }

    public INavigationViewService NavigationViewService
    {
        get;
    }


    private Dictionary<string, SortType> _sortOptions = new();
    public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService,
        ICategoryDataService categoryDataService, IShoesDataService shoesDataService, IMediator mediator)
    {
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;
        _categoryDataService = categoryDataService;
        _shoesDataService = shoesDataService;

        _mediator = mediator;
        _mediator.Subscribe(LoadCategories);
        LoadCategories();

        user = JsonSerializer.Deserialize<User>(_localSettingServiceUsingApplicationData.ReadSettingSync("userInfor"));
        if (string.IsNullOrEmpty(user.Image))
        {
            var userName = user.Name.Replace(" ", "+");
            user.Image = $"https://ui-avatars.com/api/?name={userName}&background=random";
        }
    }

    public async void LoadCategories()
    {
        await _categoryDataService.LoadDataAsync();
        var (categories, _, _) = _categoryDataService.GetData();

        if (categories is not null)
        {
            CategoryFilters.Clear();

            var (allShoesCount, _, _) = await _shoesDataService.GetShoesCountByCategoryIdAsync(0);
            CategoryFilters.Add(new CategoryFilter { Category = new Category { ID = 0, Name = "All" }, Count = allShoesCount });

            foreach (var category in categories)
            {
                if (!CategoryFilters.Any(c => c.Category.ID == category.ID))
                {
                    var (count, _, _) = await _shoesDataService.GetShoesCountByCategoryIdAsync(category.ID);
                    CategoryFilters.Add(new CategoryFilter { Category = category, Count = count });
                }
            }

            NumberCategory = CategoryFilters.Count;
            OnPropertyChanged(nameof(CategoryFilters));
        }
    }


    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        IsBackEnabled = NavigationService.CanGoBack;

        if (e.SourcePageType == typeof(SettingsPage))
        {
            Selected = NavigationViewService.SettingsItem;
            return;
        }

        var selectedItem = NavigationViewService.GetSelectedItem(e.SourcePageType);
        if (selectedItem != null)
        {
            Selected = selectedItem;
        }
    }

    public void UpdateVisibility(NavigationViewDisplayMode mode)
    {
        IsExpandedVisible = mode != NavigationViewDisplayMode.Compact;
    }

    public void Logout()
    {

        _localSettingServiceUsingApplicationData.DeleteSettingSync("userInfor");
    }
}
