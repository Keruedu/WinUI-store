using Microsoft.UI.Xaml.Controls;
using ShoesShop.Core.Models;
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

    private void CategoryCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var category = (sender as ComboBox)?.SelectedItem as Category;

        if (category is not null)
        {
            ViewModel.NewShoes.CategoryID = category.ID;
        }
    }
}
