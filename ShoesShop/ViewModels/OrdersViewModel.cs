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

    [ObservableProperty]
    private OrderDetail? selected;


    public ObservableCollection<OrderDetail> Source { get; private set; } = new ObservableCollection<OrderDetail>();


    public RelayCommand AddOrderCommand
    {
        get;
    }

    public RelayCommand DeleteOrderCommand
    {
        get;
    }

    public RelayCommand EditOrderCommand
    {
        get;
    }

    public RelayCommand ApplyFilterCommand
    {
        get;
    }
    public OrdersViewModel(IOrderDataService OrderDataService, IStorePageSettingsService storePageSettingsService) : base(storePageSettingsService)
    {
        _orderDataService = OrderDataService;

        currentPage = 1;

        AddOrderCommand = new RelayCommand(AddOrder);
        DeleteOrderCommand = new RelayCommand(DeleteOrder, () => Selected != null);
        EditOrderCommand = new RelayCommand(EditOrder, () => Selected != null);
        ApplyFilterCommand = new RelayCommand(LoadDataAsync);

        FunctionOnCommand = LoadDataAsync;
    }

    private void UpdateCommands()
    {
        DeleteOrderCommand.NotifyCanExecuteChanged();
        EditOrderCommand.NotifyCanExecuteChanged();
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

        _orderDataService.SearchParams = await BuildSearchParamsAsync();

        await Task.Run(async () => await _orderDataService.LoadDataAsync());

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
            if (ERROR_CODE != 0)
            {
                ErrorMessage = message;
            }
        }

        IsLoading = false;
        NotfifyChanges();
        EnsureItemSelected();
    }

    public void OnNavigatedTo(object parameter)
    {
        LoadDataAsync();
    }

    public void OnNavigatedFrom()
    {
    }

    public void EnsureItemSelected()
    {
        Selected = Source.Any() ? Source.First() : null;
    }
}
