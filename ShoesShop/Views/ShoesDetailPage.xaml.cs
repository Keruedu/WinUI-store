using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using ShoesShop.Contracts.Services;
using ShoesShop.Core.Models;
using ShoesShop.ViewModels;
using ShoesShop;

namespace ShoesShop.Views;

public sealed partial class ShoesDetailPage : Page
{
    public ShoesDetailViewModel ViewModel
    {
        get;
    }

    public ShoesDetailPage()
    {
        ViewModel = App.GetService<ShoesDetailViewModel>();
        InitializeComponent();
        //Loaded += UpdateVisualState;
        //SizeChanged += UpdateVisualState;
        ViewModel.ShowDialogRequested += OnShowDialogRequested;
    }

    // Responsive 

    //private void UpdateVisualState(object sender, RoutedEventArgs e)
    //{
    //    var windowWidth = App.MainWindow.Width;

    //    if (windowWidth < 960)
    //    {
    //        DetailPanel.Orientation = Orientation.Vertical;
    //        EditShoesStackPanel.MinWidth = 300;
    //    }
    //    else
    //    {
    //        DetailPanel.Orientation = Orientation.Horizontal;
    //        EditShoesStackPanel.MinWidth = 600;
    //    }
    //}


    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        base.OnNavigatingFrom(e);
        if (e.NavigationMode == NavigationMode.Back)
        {
            var navigationService = App.GetService<INavigationService>();

            if (ViewModel.Item != null)
            {
                navigationService.SetListDataItemForNextConnectedAnimation(ViewModel.Item);
            }
        }
    }

    //private async void DeleteItemButton_Click(object sender, RoutedEventArgs e)
    //{
    //    var deleteFileDialog = new ContentDialog
    //    {
    //        Title = "Delete Shoes permanently?",
    //        Content = "If you delete this Shoes, you won't be able to recover it. Do you want to delete it?",
    //        PrimaryButtonText = "Delete",
    //        CloseButtonText = "Cancel",
    //        XamlRoot = DetailPanel.XamlRoot
    //    };
    //    var result = await deleteFileDialog.ShowAsync();

    //    if (result == ContentDialogResult.Primary)
    //    {
    //        ViewModel.DeleteShoes();
    //    }
    //}

    private void CategoryCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var category = (sender as ComboBox)?.SelectedItem as Category;

        if (category is not null)
        {
            ViewModel.EditShoes.CategoryID = category.ID;
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
