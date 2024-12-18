using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using ShoesShop.Core.Models;
using ShoesShop.ViewModels;

namespace ShoesShop.Views;

public sealed partial class AddUserPage : Page
{
    public AddUserViewModel ViewModel
    {
        get;
    }

    public AddUserPage()
    {
        ViewModel = App.GetService<AddUserViewModel>();
        InitializeComponent();
        if (ViewModel != null)
        {
            ViewModel.ShowDialogRequested += OnShowDialogRequested;
            ViewModel.ShowPasswordDialogRequested += OnShowPasswordDialogRequested;
        }
        //Loaded += UpdateVisualState;
        //SizeChanged += UpdateVisualState;
    }

    //private static void OnListDetailsMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    //{
    //    //if (d is AddUserPage control)
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



    private async void OnShowPasswordDialogRequested(string message)
    {
        var passwordBox1 = new PasswordBox();
        var passwordBox2 = new PasswordBox();
        var errorTextBlock = new TextBlock { Foreground = new SolidColorBrush(Colors.Red), Visibility = Visibility.Collapsed };

        var dialog = new ContentDialog
        {
            Title = "Enter New Password",
            Content = new StackPanel
            {
                Children =
                {
                    new TextBlock { Text = message },
                    new TextBlock { Text = "Enter Password:" },
                    passwordBox1,
                    new TextBlock { Text = "Confirm Password:" },
                    passwordBox2,
                    errorTextBlock
                }
            },
            PrimaryButtonText = "Confirm",
            CloseButtonText = "Cancel",
            XamlRoot = this.XamlRoot
        };

        while (true)
        {
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                if (passwordBox1.Password.Length < 6)
                {
                    errorTextBlock.Text = "Password must be at least 6 characters long.";
                    errorTextBlock.Visibility = Visibility.Visible;
                    passwordBox1.Password = string.Empty;
                    passwordBox2.Password = string.Empty;
                }
                else if (passwordBox1.Password != passwordBox2.Password)
                {
                    errorTextBlock.Text = "Passwords do not match. Please try again.";
                    errorTextBlock.Visibility = Visibility.Visible;
                    passwordBox1.Password = string.Empty;
                    passwordBox2.Password = string.Empty;
                }
                else
                {
                    ViewModel.AddUser(passwordBox1.Password);
                    break;
                }
            }
            else
            {
                break;
            }
        }
    }
}