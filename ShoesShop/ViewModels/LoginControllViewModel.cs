using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ShoesShop.Contracts.Services;
using ShoesShop.Contracts.ViewModels;
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Services;
using ShoesShop.Views;

namespace ShoesShop.ViewModels;
public partial class LoginControllViewModel: ObservableRecipient
{
    private readonly INavigationService _navigationService = App.GetService<INavigationService>();

    private UIElement? _shell = null;
    private IAuthenticationService _authenticationService = new AuthenticationService();

    [ObservableProperty]
    public string accessToken = string.Empty;

    [ObservableProperty]
    public bool isLoading = false;
    [ObservableProperty]
    public string errorMessage = String.Empty;

    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsRemembered
    {
        get; set;
    } = false;
    [ObservableProperty]
    public string serverHost = "http://localhost";
    [ObservableProperty]
    public int serverPort = 8080;


    public bool IsAuthenticated => !string.IsNullOrEmpty(accessToken);
    public bool IsNotAuthenticated => !IsAuthenticated;
    public bool HasError => !string.IsNullOrEmpty(errorMessage);
    public bool IsNotLoading => !isLoading;

    [ObservableProperty]
    private bool isSavingSettings;


    public RelayCommand SaveSettingsCommand
    {
        get; set;
    }


    public LoginControllViewModel()
    {
    }

    public async void LoginAsync()
    {
        IsLoading = true;
        NotifyChanges();
        var (message, ErrorCode) = await Task.Run(async () => await _authenticationService.LoginAsync(Email, Password));

        IsLoading = false;

        if (ErrorCode == 0)
        {
            AccessToken = _authenticationService.GetAccessToken();
            NotifyChanges();
            App.MainWindow.Content = App.GetService<ShellPage>();
            _navigationService.NavigateTo(typeof(DashboardViewModel).FullName!);
        }
        else
        {
            ErrorMessage = message;
            NotifyChanges();
        }
    }
    private void NotifyChanges()
    {
        OnPropertyChanged(nameof(IsAuthenticated));
        OnPropertyChanged(nameof(IsNotAuthenticated));
        OnPropertyChanged(nameof(HasError));
        OnPropertyChanged(nameof(IsNotLoading));
    }

}
