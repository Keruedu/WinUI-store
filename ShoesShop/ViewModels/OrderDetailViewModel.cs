using System.Collections.ObjectModel;
using CloudinaryDotNet.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using ShoesShop.Contracts.Services;
using ShoesShop.Contracts.ViewModels;
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Models;
using Windows.ApplicationModel.DataTransfer;
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


    public RelayCommand SaveButtonCommand
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
    public RelayCommand CopyToClipboardCommand
    {
        get; set;
    }

    private ObservableCollection<Detail> _selectedDetails = new ObservableCollection<Detail>();
    public ObservableCollection<Detail> SelectedDetails
    {
        get => _selectedDetails;
        set
        {
            if (SetProperty(ref _selectedDetails, value))
            {

            }
        }
    }

    public OrderDetailViewModel(IOrderDataService orderDataService, INavigationService navigationService) : base(null)
    {
        _orderDataService = orderDataService;
        _navigationService = navigationService;
        

        SaveButtonCommand = new RelayCommand(UpdateOrder, () => EditOrder is not null);
        NavigateToAddUserPageCommand = new RelayCommand(() =>
        {
            if (Item != null)
            {
                _navigationService.NavigateTo(typeof(UserDetailViewModel).FullName!, Item.User);
            }
        });
        CancelButtonCommand = new RelayCommand(() => _navigationService.NavigateTo(typeof(OrdersViewModel).FullName!));
        CopyToClipboardCommand = new RelayCommand(CopyToClipboard);
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

    private void CopyToClipboard()
    {
        if (Item != null && Item.User != null && Item.Address != null && Item.Details != null)
        {
            // Nội dung bạn muốn sao chép
            string contentToCopy =
            $@"Order ID: {Item.ID}
Order Date: {DateTime.Now:MM/dd/yyyy}
Total Amount: {Item.TotalAmount:C}

Customer Information:
Name: {Item.User.Name}
Address: {Item.Address.Street}, {Item.Address.City}, {Item.Address.State}, {Item.Address.ZipCode}, {Item.Address.Country}
Phone: {Item.User.PhoneNumber}

Order Details:";

            foreach (var shoes in Item.Details)
            {
                if (shoes != null && shoes.Shoes != null)
                {
                    contentToCopy += $"\n- {shoes.Shoes.Name} - Size {shoes.Shoes.Size} - Color {shoes.Shoes.Color} (Quantity: {shoes.Quantity}) - {shoes.Price:C}";
                }
            }

            contentToCopy += $@"

Order Status: {Item.Status}

Thank you for shopping at ShoesShop!";

            var dataPackage = new DataPackage();
            dataPackage.SetText(contentToCopy);
            Clipboard.SetContent(dataPackage);

            ShowDialogRequested?.Invoke("Copied", "Order details have been copied to clipboard.");
        }
        else
        {
            ShowDialogRequested?.Invoke("Error", "No order to copy or missing order details.");
        }
    }


    public void NotifyThisChanges()
    {
        SaveButtonCommand.NotifyCanExecuteChanged();
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
