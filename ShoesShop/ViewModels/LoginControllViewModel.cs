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

public partial class LoginControllViewModel : ObservableRecipient
{
    private readonly INavigationService _navigationService;
    private readonly IAuthenticationService _authenticationService;

    [ObservableProperty]
    private string accessToken = string.Empty;

    [ObservableProperty]
    private bool isLoading = false;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public bool IsRemembered { get; set; } = false;

    [ObservableProperty]
    private string serverHost = "http://localhost";

    [ObservableProperty]
    private int serverPort = 8080;

    public bool IsAuthenticated => !string.IsNullOrEmpty(AccessToken);
    public bool IsNotAuthenticated => !IsAuthenticated;
    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
    public bool IsNotLoading => !IsLoading;

    [ObservableProperty]
    private bool isSavingSettings;

    public RelayCommand SaveSettingsCommand
    {
        get; set;
    }

    public LoginControllViewModel(INavigationService navigationService, IAuthenticationService authenticationService)
    {
        _navigationService = navigationService;
        _authenticationService = authenticationService;
        SaveSettingsCommand = new RelayCommand(SaveSettings);
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

    private void SaveSettings()
    {
        // Implement the logic to save settings
    }
}

