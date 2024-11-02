using CommunityToolkit.WinUI.UI.Controls;

using Microsoft.UI.Xaml.Controls;

using ShoesShop.ViewModels;

namespace ShoesShop.Views;

public sealed partial class ShoesPage : Page
{
    public ShoesViewModel ViewModel
    {
        get;
    }

    public ShoesPage()
    {
        ViewModel = App.GetService<ShoesViewModel>();
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
