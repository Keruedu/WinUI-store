using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using ShoesShop.Contracts.Services;
using ShoesShop.Services;
using ShoesShop.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ShoesShop.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class LoginControl : Page
{
    private readonly ILocalSettingServiceUsingApplicationData _localSettingServiceUsingApplicationData;
    public LoginControllViewModel ViewModel { get; }

    public LoginControl()
    {
        this.InitializeComponent();
        _localSettingServiceUsingApplicationData = App.GetService<ILocalSettingServiceUsingApplicationData>();
        ViewModel = App.GetService<LoginControllViewModel>();
        this.DataContext = ViewModel;
    }

    private void Login_Loaded(object sender, RoutedEventArgs e)
    {
        
    }

    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.Password = LoginPasswordBox.Password;
        if (RememberMeCheckBox.IsChecked == true)
        {
            _localSettingServiceUsingApplicationData.SaveSettingSync("email", ViewModel.Email);
            _localSettingServiceUsingApplicationData.SaveSettingSync("password", ViewModel.Password);
            _localSettingServiceUsingApplicationData.SaveSettingSync("isSavingSettings", "true");
        }
        else
        {
            _localSettingServiceUsingApplicationData.DeleteSettingSync("email");
            _localSettingServiceUsingApplicationData.DeleteSettingSync("password");
            _localSettingServiceUsingApplicationData.SaveSettingSync("isSavingSettings", "false");
        }
        ViewModel.LoginAsync();
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        var isRemember = _localSettingServiceUsingApplicationData.ReadSettingSync("isSavingSettings");
        ViewModel.IsSavingSettings = (string)isRemember == "true" ? true : false;
        var email = _localSettingServiceUsingApplicationData.ReadSettingSync("email");
        if (email != null)
        {
            ViewModel.Email = email;
            ViewModel.Password = _localSettingServiceUsingApplicationData.ReadSettingSync("password");
            LoginEmailBox.Text = email;
            LoginPasswordBox.Password = ViewModel.Password;
        }
    }
}
