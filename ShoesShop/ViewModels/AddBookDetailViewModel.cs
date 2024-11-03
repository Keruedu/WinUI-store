﻿using CommunityToolkit.Mvvm.ComponentModel;

using ShoesShop.Contracts.ViewModels;
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Models;
using ShoesShop.Core.Services.DataAcess;

namespace ShoesShop.ViewModels;

public partial class AddBookDetailViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;
    

    [ObservableProperty]
    private SampleOrder? item;

    public AddBookDetailViewModel(ISampleDataService sampleDataService)
    {
        
        _sampleDataService = sampleDataService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is long orderID)
        {
            var data = await _sampleDataService.GetContentGridDataAsync();
            Item = data.First(i => i.OrderID == orderID);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}