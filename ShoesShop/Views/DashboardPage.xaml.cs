using Microsoft.UI.Xaml.Controls;
using ShoesShop.ViewModels;
using Microsoft.UI.Xaml.Navigation;

namespace ShoesShop.Views;

public sealed partial class DashboardPage : Page
{
    public DashboardViewModel ViewModel
    {
        get;
    }

    public DashboardPage()
    {
        ViewModel = App.GetService<DashboardViewModel>();
        InitializeComponent();
        DataContext = ViewModel;
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        await ViewModel.LoadDashboardDataCommand.ExecuteAsync(null);
    }





}
