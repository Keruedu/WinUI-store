using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Models;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.ObjectModel;

namespace ShoesShop.ViewModels;

public partial class DashboardViewModel : ObservableRecipient
{
    private readonly IStatisticDataService _statisticDataService;

    [ObservableProperty]
    private int _totalOrders;

    [ObservableProperty]
    private ObservableCollection<Order> _recentOrders;

    [ObservableProperty]
    private ObservableCollection<Shoes> _top5BestSellingShoes;

    [ObservableProperty]
    private int _totalShoesInStock;

    public DashboardViewModel(IStatisticDataService statisticDataService)
    {
        _statisticDataService = statisticDataService;
        LoadDashboardDataCommand = new AsyncRelayCommand(LoadDashboardDataAsync);
        RecentOrders = new ObservableCollection<Order>();
        Top5BestSellingShoes = new ObservableCollection<Shoes>();
    }

    public IAsyncRelayCommand LoadDashboardDataCommand
    {
        get;
    }

    private async Task LoadDashboardDataAsync()
    {
        try
        {
            TotalOrders = await _statisticDataService.GetTotalOrdersAsync();


            TotalShoesInStock = await _statisticDataService.GetTotalShoesInStockAsync();
        

            var recentOrders = await _statisticDataService.GetRecentOrdersAsync();
            RecentOrders.Clear();
            foreach (var order in recentOrders)
            {
                RecentOrders.Add(order);
            }

            var topSellingShoes = await _statisticDataService.GetTop5BestSellingShoesAsync();
            Top5BestSellingShoes.Clear();
            foreach (var shoes in topSellingShoes)
            {
                Top5BestSellingShoes.Add(shoes);
            }
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            System.Diagnostics.Debug.WriteLine($"Error loading dashboard data: {ex.Message}");
        }
    }
}
