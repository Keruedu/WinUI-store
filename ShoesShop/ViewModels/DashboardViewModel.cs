using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Models;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace ShoesShop.ViewModels;

public partial class DashboardViewModel : ObservableRecipient
{
    private readonly IStatisticDataService _statisticDataService;

    [ObservableProperty]
    private bool isLoading = true;

    [ObservableProperty]
    private bool isContentReady = false;

    [ObservableProperty]
    private int countSellingShoes = 0;

    [ObservableProperty]
    private int countNewOrders = 0;

    [ObservableProperty]
    private int countShoes = 0;

    [ObservableProperty]
    private string selectedType = "day";

    [ObservableProperty]
    private DateTime startDate;

    [ObservableProperty]
    private DateTime lastDate;

    public RelayCommand LoadDataRangeDateCommand;

    [ObservableProperty]
    private string errorMessage = "Please select date range from 1 to 30 days";

    private PlotModel _revenue_ProfitGraph;

    [ObservableProperty]
    public bool isDirty = false;

    public PlotModel Revenue_ProfitGraph
    {

        get => _revenue_ProfitGraph;
        set
        {
            if (_revenue_ProfitGraph != value)
            {
                _revenue_ProfitGraph = value;
                OnPropertyChanged(nameof(Revenue_ProfitGraph));
            }
        }

    }

    [ObservableProperty]
    public Visibility pickRangeDate = Visibility.Visible;

    [ObservableProperty]
    public Visibility grapVisibility = Visibility.Collapsed;

    [ObservableProperty]
    public IEnumerable<Revenue_Profit> revenue_Profits;

    [ObservableProperty]
    public IEnumerable<ShoesSaleStat> shoesSaleStats;



    public DashboardViewModel(IStatisticDataService statisticDataService)
    {
        _statisticDataService = statisticDataService;
        LoadDataAsync();
        LoadDataRangeDateCommand = new RelayCommand(ExecuteLoadDataRangeDateCommand);
    }

    partial void OnRevenue_ProfitsChanged(IEnumerable<Revenue_Profit> value)
    {
        var Graph = new PlotModel { Title = "Revenue & Profit Graph" };

        var categoryAxis = new CategoryAxis { Position = AxisPosition.Left, Maximum = value.Count() };

        foreach (var item in value)
        {
            categoryAxis.Labels.Add(item.Date);
        }


        Graph.Axes.Add(categoryAxis);

        double maxValue = 0;

        foreach (var item in value)
        {
            double minRevenue = value.Max(item => item.Revenue) / 100000;
            double minProfit = value.Max(item => item.Profit) / 100000;
            maxValue = Math.Max(minRevenue, minProfit);
        }

        var valueAxis = new LinearAxis { Position = AxisPosition.Bottom, MinimumPadding = 0, AbsoluteMinimum = 0, Maximum = maxValue * 1.1 };
        Graph.Axes.Add(valueAxis);

        // Tạo BarSeries cho doanh thu và lợi nhuận
        var barSeries1 = new BarSeries { Title = "Revenue", FillColor = OxyColors.Blue };
        foreach (var item in value)
        {
            barSeries1.Items.Add(new BarItem { Value = item.Revenue / 100000 });
        }


        var barSeries2 = new BarSeries { Title = "Profit", FillColor = OxyColors.Green };
        foreach (var item in value)
        {
            barSeries2.Items.Add(new BarItem { Value = item.Profit / 100000 });
        }

        Graph.Series.Add(barSeries1);
        Graph.Series.Add(barSeries2);

        Graph.IsLegendVisible = true;


        Revenue_ProfitGraph = Graph;
    }

    partial void OnSelectedTypeChanged(string value)
    {
        LoadDataAsync();

        if (value == "day")
        {
            PickRangeDate = Visibility.Visible;
            GrapVisibility = Visibility.Collapsed;
        }
        else
        {

            PickRangeDate = Visibility.Collapsed;

        }
    }

    public async void LoadShoesSaleStat()
    {
        var (returnList2, _, _) = await _statisticDataService.GetShoesSaleStatAsync(SelectedType);

        if (returnList2 != null)
        {
            ShoesSaleStats = returnList2;
        }
    }

    public async Task LoadDataAsync()
    {
        IsLoading = true;
        IsContentReady = false;

        await Task.Run(async () => await _statisticDataService.LoadDataAsync(SelectedType));

        var (countSellingShoes, _, _) = _statisticDataService.CountSellingShoesAsync(SelectedType);
        var (countNewOrders, _, _) = _statisticDataService.CountNewOrdersAsync(SelectedType);
        var (countShoes, _, _) = _statisticDataService.CountShoesAsync();

        if (countSellingShoes != null)
        {
            CountSellingShoes = countSellingShoes;
        }

        if (countNewOrders != null)
        {
            CountNewOrders = countNewOrders;
        }

        if (countShoes != null)
        {

            CountShoes = countShoes;
        }

        UpdateDataGraph();

        IsLoading = false;
        IsContentReady = true;
    }

    public async void UpdateDataGraph()
    {
        if (SelectedType != "day")
        {
            var (returnList, message, error) = await Task.Run(async () => await _statisticDataService.GetRevenue_ProfitAsync(SelectedType));

            if (returnList != null)
            {
                Revenue_Profits = returnList;
                GrapVisibility = Visibility.Visible;
            }

            LoadShoesSaleStat();
        }
    }

    public bool CanExecuteLoadDataRangeDateCommand()
    {
        ErrorMessage = string.Empty;

        TimeSpan timeSpan = LastDate - StartDate;

        if (timeSpan.Days > 0 && timeSpan.Days <= 30)
        {
            return true;
        }
        ErrorMessage = "Please select date range from 1 to 30 days";
        return false;
    }

    public async void ExecuteLoadDataRangeDateCommand()
    {
        IsDirty = false;

        string date1 = StartDate.ToString("yyyy-MM-dd");
        string date2 = LastDate.ToString("yyyy-MM-dd");

        string query = $"dateRange&startDate={date1}&endDate={date2}";

        var (returnList, message, error) = await _statisticDataService.GetRevenue_ProfitAsync(query);

        if (returnList != null)
        {
            Revenue_Profits = returnList;
        }

        var (returnList2, message2, error2) = await _statisticDataService.GetShoesSaleStatAsync(query);
        if (returnList2 != null)
        {
            ShoesSaleStats = returnList2;
        }

        GrapVisibility = Visibility.Visible;
    }

    partial void OnStartDateChanged(DateTime value)
    {
        LoadDataRangeDateCommand.NotifyCanExecuteChanged();
    }

    partial void OnLastDateChanged(DateTime value)
    {
        LoadDataRangeDateCommand.NotifyCanExecuteChanged();
    }
}
