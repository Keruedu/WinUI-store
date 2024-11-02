using Microsoft.UI.Xaml.Controls;

using ShoesShop.ViewModels;

namespace ShoesShop.Views;

public sealed partial class AddShoesPage : Page
{
    public AddShoesViewModel ViewModel
    {
        get;
    }

    public AddShoesPage()
    {
        ViewModel = App.GetService<AddShoesViewModel>();
        InitializeComponent();
    }
}
