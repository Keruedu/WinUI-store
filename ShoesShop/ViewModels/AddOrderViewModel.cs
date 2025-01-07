using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using ShoesShop.Contracts.Services;
using ShoesShop.Contracts.ViewModels;
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Http;
using ShoesShop.Core.Models;
using ShoesShop.Core.Services;
using ShoesShop.Services;
using WinUIEx.Messaging;

namespace ShoesShop.ViewModels;

public partial class AddOrderViewModel : ResourceLoadingViewModel, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly IShoesDataService _ShoesDataService;
    private readonly IOrderDataService _orderDataService;
    private readonly ICategoryDataService _categoryDataService;
    private readonly IAddressDataService _addressDataService;
    private readonly IUserDataService _userDataService;

    public event Action<string, string>? ShowDialogRequested;

    public ObservableCollection<Shoes> Source { get; } = new ObservableCollection<Shoes>();
    public ObservableCollection<Shoes> SelectedShoes { get; } = new ObservableCollection<Shoes>();
    public RelayCommand AddOrderCommand
    {
        get;
    }

    public RelayCommand<Shoes> ToggleShoesSelectionCommand
    {
        get;
    }

    public int UserId
    {
        get; set;
    } = 1;
    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

    public Address NewAddress { get; set; } = new Address();

    public ObservableCollection<string> Addresses { get; } = new ObservableCollection<string>();
    public string SelectedAddress
    {
        get; set;
    }
    public string SelectedStatusOrder
    {

        get; set;
    }

    public decimal TotalAmount => SelectedShoes.Sum(shoes =>
    {
        var quantity = newDetailQuantity.FirstOrDefault(dq => dq.Item1 == shoes.ID)?.Item2 ?? 1;
        return shoes.Price * quantity;
    });

    public RelayCommand<Shoes> UpdateQuantityCommand
    {
        get;
    }

    public ObservableCollection<Tuple<int,int>> newDetailQuantity 
    { 
        get; set;
    } = new ObservableCollection<Tuple<int, int>>();

    public AddOrderViewModel(INavigationService navigationService, IUserDataService userDataService, IShoesDataService ShoesDataService, IOrderDataService orderDataService, ICategoryDataService categoryDataService, IAddressDataService addressDataService, IStorePageSettingsService storePageSettingsService) : base(storePageSettingsService)
    {
        AddOrderCommand = new RelayCommand(OnAddOrder);

        _navigationService = navigationService;
        _ShoesDataService = ShoesDataService;
        _categoryDataService = categoryDataService;
        _orderDataService = orderDataService;
        _addressDataService = addressDataService;
        _userDataService = userDataService;

        SelectedStatusOrder = "Pending";

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

        NewAddress = new Address
        {
            Street = "123 Default St",
            City = "Default City",
            State = "Default State",
            ZipCode = "12345",
            Country = "Default Country"
        };

        SelectedSortOption = SortOptions[0];
        ToggleShoesSelectionCommand = new RelayCommand<Shoes>(ToggleShoesSelection);
        OnPropertyChanged(nameof(TotalAmount));
    }

    public int GetQuantityForShoes(int shoesId)
    {
        var quantityTuple = newDetailQuantity.FirstOrDefault(item => item.Item1 == shoesId);
        return quantityTuple != null ? quantityTuple.Item2 : 1;
    }

    public void SetQuantityForShoes(int shoesId, int quantity)
    {
        var detail = newDetailQuantity.FirstOrDefault(d => d.Item1 == shoesId);
        if (detail != null)
        {
            newDetailQuantity.Remove(detail);
        }
        newDetailQuantity.Add(new Tuple<int, int>(shoesId, quantity));
        OnPropertyChanged(nameof(newDetailQuantity));

        OnPropertyChanged(nameof(TotalAmount)); // Cập nhật lại tổng tiền
    }

    private void ToggleShoesSelection(Shoes shoes)
    {
        if (SelectedShoes.Contains(shoes))
        {
            SelectedShoes.Remove(shoes);
        }
        else
        {
            SelectedShoes.Add(shoes);
        }
    }

    public async void LoadCategories()
    {
        await _categoryDataService.LoadDataAsync();
        var (categories, _, _) = _categoryDataService.GetData();

        if (categories is not null)
        {
            foreach (var category in categories)
            {
                CategoryFilters.Add(category);
            }
        }

    }

    private async void OnAddOrder()
    {
        
        if (SelectedShoes.Count == 0)
        {
            ErrorMessage = "Please select at least one Shoes";
            ShowDialogRequested?.Invoke("Error", ErrorMessage);
            LoadData();
            NotfifyChanges();
            return;
        }

        IsLoading = true;

        try
        {
            // Create a new address
            var (newAddress, addressErrorMessage, addressErrorCode) = await _addressDataService.CreateAddressAsync(NewAddress);
            if (addressErrorCode == 0)
            {
                
                ErrorMessage = addressErrorMessage;
                ShowDialogRequested?.Invoke("Error", ErrorMessage);
                LoadData();
                NotfifyChanges();
                IsLoading = false;
                return;
            }
            NewAddress = newAddress;


            var orderDetails = new List<Detail>();
            foreach (var shoes in SelectedShoes)
            {
                var detailQuantity = newDetailQuantity.FirstOrDefault(dq => dq.Item1 == shoes.ID);
                var quantity = detailQuantity != null ? detailQuantity.Item2 : 1;

                if (shoes.Stock <= 0)
                {
                    
                    ErrorMessage = $"Shoes {shoes.Name} is out of stock.";
                    ShowDialogRequested?.Invoke("Error", ErrorMessage);
                    LoadData();
                    NotfifyChanges();
                    IsLoading = false;
                    return;
                }
                if (shoes.Stock < quantity)
                {
                    
                    ErrorMessage = $"The quantity for {shoes.Name} exceeds the available stock.";
                    ShowDialogRequested?.Invoke("Error", ErrorMessage);
                    LoadData();
                    NotfifyChanges();
                    IsLoading = false;
                    return;
                }

                orderDetails.Add(new Detail
                {
                    ShoesID = shoes.ID,
                    Quantity = quantity,
                    Price = shoes.Price * quantity,
                    Shoes = shoes
                });

                // Update the stock
                shoes.Stock -= quantity;
                var (_, errorMessageShoes, errorCodeShoes) = await Task.Run(async () => await _ShoesDataService.UpdateShoesAsync(shoes));
                if (errorCodeShoes == 0)
                {
                    IsLoading = false;
                    ErrorMessage = errorMessageShoes;
                    ShowDialogRequested?.Invoke("Error", ErrorMessage);
                    NotfifyChanges();
                    return;
                }
            }
            var user = await _userDataService.GetUserByIdAsync(UserId);
            var order = new Order
            {
                UserID = UserId,
                User = user,
                OrderDate = OrderDate.ToString(),
                Status = SelectedStatusOrder,
                Details = orderDetails,
                AddressID = NewAddress.ID,
            };

            var (neworder, errorMessage, errorCode) = await Task.Run(async () => await _orderDataService.CreateAOrderAsync(order));

            neworder.User = user;
            neworder.Details = orderDetails;

            if (errorCode == 1)
            {
                IsLoading = false;
                ShowDialogRequested?.Invoke("Success", "Order updated successfully.");
                GmailNotificationService gmailNotificationService = new GmailNotificationService(_userDataService);
                gmailNotificationService.NotifyMakingOrder(neworder);
                _navigationService.NavigateTo("ShoesShop.ViewModels.OrdersViewModel");
            }
            else
            {
                IsLoading = false;
                ErrorMessage = errorMessage;
                ShowDialogRequested?.Invoke("Error", ErrorMessage);
                NotfifyChanges();
                
            }
        }
        catch (Exception ex)
        {
            IsLoading = false;
            ErrorMessage = $"An error occurred: {ex.Message}";
            ShowDialogRequested?.Invoke("Error", ErrorMessage);
            NotfifyChanges();
        }

    }

    public async void LoadData()
    {
        IsDirty = false;
        IsLoading = true;
        InfoMessage = string.Empty;
        ErrorMessage = string.Empty;
        NotfifyChanges();

        _ShoesDataService.searchQuery = await BuildSearchQueryAsync();

        await _ShoesDataService.LoadDataAsync();

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

            IsLoading = false;
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
    private void OnApplyFiltersAndSearch()
    {
        LoadData();
    }


}
