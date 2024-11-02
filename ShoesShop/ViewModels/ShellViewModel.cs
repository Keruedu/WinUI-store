using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Navigation;

using ShoesShop.Contracts.Services;
using ShoesShop.Views;
using static ShoesShop.IDao;

namespace ShoesShop.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    readonly IDao dao = new PostgreDao();
    [ObservableProperty]
    private bool isBackEnabled;

    [ObservableProperty]
    private object? selected;

    public INavigationService NavigationService
    {
        get;
    }

    public INavigationViewService NavigationViewService
    {
        get;
    }

    private Dictionary<string, SortType> _sortOptions = new();
    public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService)
    {
        dao.GetShoes(1,1, "p", _sortOptions);
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;
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
}
