using Microsoft.UI.Xaml.Controls;

using ShoesShop.ViewModels;

namespace ShoesShop.Views;

public sealed partial class DashboardPage : Page
{
    public DashboardViewModel ViewModel
    {
        get;
    }

    public DashboardPage()
    {
        ViewModel = App.GetService<DashboardViewModel>();
        InitializeComponent();
    }

    private void TimeFrame_Checked(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var radioButton = sender as RadioButton;
        if (radioButton != null)
        {
            ViewModel.SelectedType = radioButton.Content.ToString() switch
            {

                "Day" => "day",
                "Week" => "week",
                "Month" => "month",
                "Year" => "year",
                _ => "day"
            };
        }
    }

    private void StartDate_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
    {
        PickDateError.Text = "";

        var date = StartDate.Date;

        var today = DateTimeOffset.Now;
        var sevenDaysAgo = today.AddDays(-7);

        if (date > today)
        {
            sender.Date = sevenDaysAgo;
            PickDateError.Text = "The start day is not greater than today";
            return;
        }

        ViewModel.StartDate = date.Value.DateTime;
        ViewModel.IsDirty = true;
    }

    private void EndDate_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
    {
        PickDateError.Text = "";

        var date = EndDate.Date;
        var today = DateTimeOffset.Now;

        if (date > today)
        {
            sender.Date = today;
            PickDateError.Text = "The last date is not greater than today";
            return;
        }

        ViewModel.LastDate = date.Value.DateTime;
        ViewModel.IsDirty = true;
    }
}
