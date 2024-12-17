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
        ViewModel.ShowDialogRequested += OnShowDialogRequested;
    }

    private void CategoryCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var category = (sender as ComboBox)?.SelectedItem as Category;

        if (category is not null)
        {
            ViewModel.NewShoes.CategoryID = category.ID;
        }
    }


    private async void OnShowDialogRequested(string title, string message)
    {
        var dialog = new ContentDialog
        {
            Title = title,
            Content = message,
            CloseButtonText = "OK",
            XamlRoot = this.Content.XamlRoot
        };

        await dialog.ShowAsync();
    }
}
