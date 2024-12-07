using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI.Controls;
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Services;

namespace ShoesShop.ViewModels;
public partial class LoginControllViewModel: ObservableRecipient
{
    private IAuthenticationService _authenticationService=new AuthenticationService();
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
    public bool IsSavingSettings
    {
        get; set;
    } = false;

    public RelayCommand SaveSettingsCommand
    {
        get; set;
    }

    public LoginControllViewModel()
    {

    }
    public async Task<(string Email, string Password)> GetStoredCredentialsAsync()
    {

        //var isRemembered = await _storeLoginCredentialsService.TryGetRememberCredentialsAsync();
        //IsRemembered = isRemembered;

        //if (isRemembered)
        //{
        //    var (email, password) = await _storeLoginCredentialsService.TryGetCredentialsAsync();
        //    Email = email;
        //    Password = password;
        //}
        //else
        //{
        //    Email = string.Empty;
        //    Password = string.Empty;
        //}

        return (Email, Password);
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
            //if (IsRemembered)
            //{
            //    await _storeLoginCredentialsService.SaveCredentialsAsync(Email, Password);
            //}
            //else
            //{
            //    await _storeLoginCredentialsService.ClearCredentailAsync();
            //}
            //await _storeLoginCredentialsService.SaveRememberCredentialsAsync(IsRemembered);
            //_navigationService.Refresh();
        }
        else
        {
            ErrorMessage = message;
            NotifyChanges();
        }
    }
    private void NotifyChanges()
    {
        //SaveSettingsCommand.NotifyCanExecuteChanged();

        OnPropertyChanged(nameof(IsAuthenticated));
        OnPropertyChanged(nameof(IsNotAuthenticated));
        OnPropertyChanged(nameof(HasError));
        OnPropertyChanged(nameof(IsNotLoading));
    }

}
