using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using ShoesShop.Core.Models;
using ShoesShop.ViewModels;

namespace ShoesShop.Views;

public sealed partial class UserDetailPage : Page
{
    public UserDetailViewModel ViewModel
    {
        get;
    }

    public UserDetailPage()
    {
        ViewModel = App.GetService<UserDetailViewModel>();
        InitializeComponent();
        if (ViewModel != null)
        {
            ViewModel.ShowDialogRequested += OnShowDialogRequested;
        }
        //Loaded += UpdateVisualState;
        //SizeChanged += UpdateVisualState;
    }

    //private static void OnListDetailsMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    //{
    //    //if (d is UserDetailPage control)
    //    //{
    //    //    control.ForegroundElement.ChangeView(0, 0, 1);
    //    //}
    //}
    private async void OnShowDialogRequested(string title, string message)
    {
        var dialog = new ContentDialog
        {
            Title = title,
            Content = message,
            CloseButtonText = "OK",
            XamlRoot = this.XamlRoot // Đảm bảo dialog hiển thị đúng
        };

        await dialog.ShowAsync();
    }
}
