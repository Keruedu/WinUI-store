using Microsoft.UI.Xaml.Controls;

using ShoesShop.ViewModels;

namespace ShoesShop.Views;

public sealed partial class AddOrderPage : Page
{
    public AddOrderViewModel ViewModel
    {
        get;
    }

    public AddOrderPage()
    {
        ViewModel = App.GetService<AddOrderViewModel>();
        InitializeComponent();
    }
}
