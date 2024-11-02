using CommunityToolkit.WinUI.UI.Controls;

using Microsoft.UI.Xaml.Controls;

using ShoesShop.ViewModels;

namespace ShoesShop.Views;

public sealed partial class BooksPage : Page
{
    public BooksViewModel ViewModel
    {
        get;
    }

    public BooksPage()
    {
        ViewModel = App.GetService<BooksViewModel>();
        InitializeComponent();
    }

    private void OnViewStateChanged(object sender, ListDetailsViewState e)
    {
        if (e == ListDetailsViewState.Both)
        {
            ViewModel.EnsureItemSelected();
        }
    }
}
