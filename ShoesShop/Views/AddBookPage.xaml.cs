using Microsoft.UI.Xaml.Controls;

using ShoesShop.ViewModels;

namespace ShoesShop.Views;

public sealed partial class AddBookPage : Page
{
    public AddBookViewModel ViewModel
    {
        get;
    }

    public AddBookPage()
    {
        ViewModel = App.GetService<AddBookViewModel>();
        InitializeComponent();
    }
}
