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
    private ObservableCollection<Order> _recentOrders = new();

    [ObservableProperty]
    private ObservableCollection<Shoes> _top5BestSellingShoes = new();

    [ObservableProperty]
    private int _totalShoesInStock;

    [ObservableProperty]
    private Dictionary<string, int> _orderStatistics = new();

    [ObservableProperty]
    private string _selectedGroupBy = "month";

    [ObservableProperty]
    private PlotModel _orderStatisticsPlotModel = new();

    public DashboardViewModel(IStatisticDataService statisticDataService)
    {
        _statisticDataService = statisticDataService;
        LoadDashboardDataCommand = new AsyncRelayCommand(LoadDashboardDataAsync);
        LoadOrderStatisticsCommand = new AsyncRelayCommand<string?>(LoadOrderStatisticsAsync);
    }

    public IAsyncRelayCommand LoadDashboardDataCommand
    {
        get;
    }
    public IAsyncRelayCommand<string?> LoadOrderStatisticsCommand
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

            await LoadOrderStatisticsAsync(SelectedGroupBy);
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            System.Diagnostics.Debug.WriteLine($"Error loading dashboard data: {ex.Message}");
        }
    }

    private async Task LoadOrderStatisticsAsync(string? groupBy)
    {
        try
        {
            OrderStatistics = await _statisticDataService.GetOrderStatisticsAsync(groupBy ?? "month");
            UpdateOrderStatisticsPlotModel();
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            System.Diagnostics.Debug.WriteLine($"Error loading order statistics: {ex.Message}");
        }
    }

    private void UpdateOrderStatisticsPlotModel()
    {
        var plotModel = new PlotModel { Title = "Order Statistics" };
        var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
        var valueAxis = new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0 };

        var barSeries = new BarSeries
        {
            ItemsSource = OrderStatistics.Select(stat => new BarItem { Value = stat.Value }).ToList(),
            LabelPlacement = LabelPlacement.Inside,
            LabelFormatString = "{0}"
        };

        categoryAxis.Labels.AddRange(OrderStatistics.Keys);

        plotModel.Axes.Add(categoryAxis);
        plotModel.Axes.Add(valueAxis);
        plotModel.Series.Add(barSeries);

        OrderStatisticsPlotModel = plotModel;
    }

}
