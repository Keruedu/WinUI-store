using Microsoft.UI.Xaml.Controls;

using ShoesShop.ViewModels;

namespace ShoesShop.Views;

public sealed partial class AddCategoryPage : Page
{
    public AddCategoryViewModel ViewModel
    {
        get;
    }

    public AddCategoryPage()
    {
        ViewModel = App.GetService<AddCategoryViewModel>();
        InitializeComponent();
    }
}
