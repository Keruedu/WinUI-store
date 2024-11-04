using CommunityToolkit.WinUI.UI.Controls;

using Microsoft.UI.Xaml.Controls;

using ShoesShop.ViewModels;

using ShoesShop;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ShoesShop.Views;

public sealed partial class OrdersPage : Page
{
    public OrdersViewModel ViewModel
    {
        get;
    }

    public OrdersPage()
    {
        ViewModel = App.GetService<OrdersViewModel>();
        InitializeComponent();
    }

    private void OnViewStateChanged(object sender, ListDetailsViewState e)
    {
        if (e == ListDetailsViewState.Both)
        {
            ViewModel.EnsureItemSelected();
        }
    }

    private void FromDate_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
    {
        var date = FromDate.Date;
        var convertedDate = date.HasValue ? date.Value.DateTime : DateTime.MinValue;
        ViewModel.SetFromDate(convertedDate);
    }

    private void ToDate_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
    {
        var date = ToDate.Date;
        date = date?.AddDays(1);
        var convertedDate = date.HasValue ? date.Value.DateTime : DateTime.MinValue;
        ViewModel.SetToDate(convertedDate);
    }
}
