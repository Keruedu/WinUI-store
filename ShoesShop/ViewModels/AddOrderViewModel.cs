using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.Extensions.Hosting;
using ShoesShop.Contracts.Services;
using ShoesShop.Contracts.ViewModels;
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Http;
using ShoesShop.Core.Models;
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

    public AddOrderViewModel(INavigationService navigationService, IShoesDataService ShoesDataService, IOrderDataService orderDataService, ICategoryDataService categoryDataService, IAddressDataService addressDataService, IStorePageSettingsService storePageSettingsService) : base(storePageSettingsService)
    {
        AddOrderCommand = new RelayCommand(OnAddOrder);

        _navigationService = navigationService;
        _ShoesDataService = ShoesDataService;
        _categoryDataService = categoryDataService;
        _orderDataService = orderDataService;
        _addressDataService = addressDataService;

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
        for (int i = 0; i < newDetailQuantity.Count; i++)
        {
            if (newDetailQuantity[i].Item1 == shoesId)
            {
                newDetailQuantity[i] = new Tuple<int, int>(shoesId, quantity);
                break;
            }
        }

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
            NotfifyChanges();
            await Task.Delay(2000); // Wait for 2 seconds
            ErrorMessage = string.Empty;
            NotfifyChanges();
            return;
        }

        try
        {
            // Create a new address
            var (newAddress, addressErrorMessage, addressErrorCode) = await _addressDataService.CreateAddressAsync(NewAddress);
            if (addressErrorCode == 0)
            {
                ErrorMessage = addressErrorMessage;
                NotfifyChanges();
                return;
            }
            NewAddress = newAddress;

            var orderDetails = SelectedShoes.Select(shoes =>
            {
                var detailQuantity = newDetailQuantity.FirstOrDefault(dq => dq.Item1 == shoes.ID);
                return new Detail
                {
                    ShoesID = shoes.ID,
                    Quantity = detailQuantity != null ? detailQuantity.Item2 : shoes.Stock,
                    Price = shoes.Price
                };
            }).ToList();

            var order = new Order
            {
                UserID = UserId,
                OrderDate = OrderDate.ToString(),
                Status = SelectedStatusOrder,
                Details = orderDetails,
                AddressID = NewAddress.ID,
            };

            var (_, errorMessage, errorCode) = await Task.Run(async () => await _orderDataService.CreateAOrderAsync(order));

            if (errorCode == 1)
            {
                ShowDialogRequested?.Invoke("Success", "Order updated successfully.");
                _navigationService.NavigateTo("ShoesShop.ViewModels.OrdersViewModel");
            }
            else
            {
                ErrorMessage = errorMessage;
                ShowDialogRequested?.Invoke("Error", ErrorMessage);
                NotfifyChanges();
            }
        }
        catch (Exception ex)
        {
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
            newDetailQuantity.Clear();

            foreach (var item in data)
            {
                Source.Add(item);
                newDetailQuantity.Add(new Tuple<int, int>(item.ID, 1));
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

    public void UpdateDetailQuantities()
    {
        for (int i = 0; i < newDetailQuantity.Count; i++)
        {
            var item = newDetailQuantity[i];
            newDetailQuantity[i] = new Tuple<int, int>(item.Item1, 1);
        }
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
