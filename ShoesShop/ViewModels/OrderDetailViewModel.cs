using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using ShoesShop.Contracts.Services;
using ShoesShop.Contracts.ViewModels;
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Models;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace ShoesShop.ViewModels;
public partial class OrderDetailViewModel : ResourceLoadingViewModel, INavigationAware
{
    private readonly IOrderDataService _orderDataService;
    private readonly INavigationService _navigationService;

    public event Action<string, string>? ShowDialogRequested;

    [ObservableProperty]
    public string editErrorMessage = string.Empty;

    [ObservableProperty]
    public bool isEditLoading = false;

    [ObservableProperty]
    public Order? item;

    [ObservableProperty]
    public Order? editOrder;

    public ObservableCollection<Order> OrderItems { get; } = new ObservableCollection<Order>();


    public RelayCommand UpdateButtonCommand
    {
        get; set;
    }

    public RelayCommand CancelButtonCommand
    {
        get; set;
    }
    public RelayCommand NavigateToAddUserPageCommand
    {
        get;
    }

    public OrderDetailViewModel(IOrderDataService orderDataService, INavigationService navigationService) : base(null)
    {
        _orderDataService = orderDataService;
        _navigationService = navigationService;
        

        UpdateButtonCommand = new RelayCommand(UpdateOrder, () => EditOrder is not null);
        NavigateToAddUserPageCommand = new RelayCommand(() =>
        {
            if (Item != null)
            {
                _navigationService.NavigateTo(typeof(UserDetailViewModel).FullName!, Item.User);
            }
        });
        CancelButtonCommand = new RelayCommand(() => _navigationService.NavigateTo(typeof(OrdersViewModel).FullName!));
    }

    public async void UpdateOrder()
    {
        IsEditLoading = true;

        NotifyThisChanges();

        try
        {
            var (returnedOrder, message, errorCode) = await _orderDataService.UpdateOrderAsync(EditOrder);

            if (errorCode == 1)
            {
                Item = returnedOrder;
                ShowDialogRequested?.Invoke("Success", "Order updated successfully.");
                _navigationService.NavigateTo(typeof(OrdersViewModel).FullName!);
            }
            else
            {
                EditErrorMessage = message;
                ShowDialogRequested?.Invoke("Error", message);
            }
        }
        catch (Exception ex)
        {
            EditErrorMessage = $"An error occurred: {ex.Message}";
            ShowDialogRequested?.Invoke("Error", EditErrorMessage);
        }
        finally
        {
            IsEditLoading = false;
            NotifyThisChanges();
        }
    }

    public void NotifyThisChanges()
    {
        UpdateButtonCommand.NotifyCanExecuteChanged();
        CancelButtonCommand.NotifyCanExecuteChanged();
    }

    public void OnNavigatedTo(object parameter)
    {
        if (parameter is Order order)
        {
            Item = order;
            EditOrder = Item;
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
