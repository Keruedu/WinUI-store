using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using ShoesShop.Core.Models;
using ShoesShop.ViewModels;

namespace ShoesShop.Views;

public sealed partial class OrdersDetailControl : UserControl
{

    public OrderDetailControlViewModel ViewModel
    {
        get;
    }


    public Order? ListDetailsMenuItem
    {
        get => GetValue(ListDetailsMenuItemProperty) as Order;
        set => SetValue(ListDetailsMenuItemProperty, value);
    }

    public static readonly DependencyProperty ListDetailsMenuItemProperty = DependencyProperty.Register("ListDetailsMenuItem", typeof(Order), typeof(OrdersDetailControl), new PropertyMetadata(null, OnListDetailsMenuItemPropertyChanged));

    public OrdersDetailControl()
    {
        InitializeComponent();
        ViewModel = App.GetService<OrderDetailControlViewModel>();
        UpdateItem();
    }

    private void UpdateItem()
    {
        ViewModel.Item = ListDetailsMenuItem ?? new();
        ViewModel.newStatus = ViewModel.Item.Status;
        ViewModel.newAddress = ViewModel.Item.Address;
        ViewModel.IsEditSession = false;
        ViewModel.NotifyThisChanges();
    }

    private static void OnListDetailsMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is OrdersDetailControl control)
        {
            control.ForegroundElement.ChangeView(0, 0, 1);
            control.UpdateItem();
        }
    }

    private async void DeleteItemButton_Click(object sender, RoutedEventArgs e)
    {
        var deleteFileDialog = new ContentDialog
        {
            Title = "Delete this order permanently?",
            Content = "If you delete this order, you won't be able to recover it. Do you want to delete it?",
            PrimaryButtonText = "Delete",
            CloseButtonText = "Cancel",
            XamlRoot = ForegroundElement.XamlRoot
        };
        var result = await deleteFileDialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            var button = sender as Button;
            if (button != null)
            {
                button.IsEnabled = false;   // Disable the button
                button.Opacity = 0.5;      // Make the button faded, adjust value as needed
            }

            ViewModel.OnDeleteOrder();

        }
    }
}
