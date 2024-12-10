using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Controls;
using ShoesShop.Contracts.Services;
using ShoesShop.Contracts.ViewModels;
using ShoesShop.Core.Models;
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Services;



namespace ShoesShop.ViewModels;
public partial class OrdersViewModel : ResourceLoadingViewModel, INavigationAware
{
    private readonly IOrderDataService _orderDataService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private Order? selected;

    public List<string> StatusFilters
    {
        get; set;
    } = new List<string> { "Pending", "Shipped", "Delivered", "Cancelled" };

    public ObservableCollection<Order> Source { get; private set; } = new ObservableCollection<Order>();


    private ObservableCollection<Order> _SelectedOrders = new ObservableCollection<Order>();
    public ObservableCollection<Order> SelectedOrders
    {
        get => _SelectedOrders;
        set
        {
            if (SetProperty(ref _SelectedOrders, value))
            {
                
            }
        }
    }

    public RelayCommand AddOrderCommand
    {
        get;
    }

    public RelayCommand ApplyFilterCommand
    {
        get;
    }

    public RelayCommand NavigateToAddOrderPageCommand
    {
        get;
    }

    public OrdersViewModel(INavigationService navigationService, IOrderDataService OrderDataService, IStorePageSettingsService storePageSettingsService) : base(storePageSettingsService)
    {
        _orderDataService = OrderDataService;
        _navigationService = navigationService;

        currentPage = 1;

        AddOrderCommand = new RelayCommand(AddOrder);
        ApplyFilterCommand = new RelayCommand(LoadDataAsync);
        NavigateToAddOrderPageCommand = new RelayCommand(() => _navigationService.NavigateTo(typeof(AddOrderViewModel).FullName!));

        FunctionOnCommand = LoadDataAsync;
    }

    private void UpdateCommands()
    {
        //DeleteOrderCommand.NotifyCanExecuteChanged();
        //EditOrderCommand.NotifyCanExecuteChanged();
    }

    private void AddOrder()
    {

    }

    private void DeleteOrder()
    {

    }

    private void EditOrder()
    {

    }

    private async void LoadDataAsync()
    {
        IsLoading = true;
        NotfifyChanges();

        _orderDataService.SearchQuery = await BuildSearchQueryOrderAsync();

        await _orderDataService.LoadDataAsync();

        var (data, totalItems, message, ERROR_CODE) = _orderDataService.GetData();

        if (data is not null)
        {
            Source.Clear();

            foreach (var item in data)
            {
                Source.Add(item);
            }

            TotalItems = totalItems;

            if (TotalItems == 0)
            {
                InfoMessage = "No orders found";
            }
        }
        else
        {
            if (ERROR_CODE != 1)
            {
                ErrorMessage = message;
            }
        }

        IsLoading = false;
        NotfifyChanges();
    }

    public void OnNavigatedTo(object parameter)
    {
        LoadDataAsync();
    }

    [RelayCommand]
    private void OnItemClick(User? clickedItem)
    {
        if (clickedItem != null)
        {
            _navigationService.SetListDataItemForNextConnectedAnimation(clickedItem);
            _navigationService.NavigateTo(typeof(UserDetailViewModel).FullName!, clickedItem);
        }
    }

    public void OnNavigatedFrom()
    {
    }

}
