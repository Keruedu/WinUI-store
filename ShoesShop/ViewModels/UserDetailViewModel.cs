using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml.Controls;
using ShoesShop.Contracts.Services;
using ShoesShop.Contracts.ViewModels;
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Models;
using ShoesShop.Core.Utils;
using Windows.Storage.Pickers;
using WinRT.Interop;
using WinUIEx.Messaging;

namespace ShoesShop.ViewModels;
public partial class UserDetailViewModel : ResourceLoadingViewModel, INavigationAware
{
    private readonly IUserDataService _UserDataService;
    private readonly INavigationService _navigationService;
    private readonly ICloudinaryService _cloudinaryService;


    public event Action<string, string>? ShowDialogRequested;
    public event Action<string>? ShowPasswordDialogRequested;


[ObservableProperty]
    public string editErrorMessage = string.Empty;
    [ObservableProperty]
    public bool isEditLoading = false;

    [ObservableProperty]
    public User? item;

    [ObservableProperty]
    public User? editUser;


    [ObservableProperty]
    public string selectedImageName = string.Empty;
    [ObservableProperty]
    public string selectedImagePath = string.Empty;

    public bool IsImageSelected => !string.IsNullOrEmpty(SelectedImageName);

    public RelayCommand SelectImageButtonCommand
    {
        get; set;
    }

    public RelayCommand RemoveImageButtonCommand
    {
        get; set;
    }

    public RelayCommand UpdateButtonCommand
    {
        get; set;
    }

    public RelayCommand CancelButtonCommand
    {
        get; set;
    }
    public RelayCommand DeleteButtonCommand
    {
        get; set;
    }

    public RelayCommand EditUserButtonCommand
    {
        get; set;
    }
    public RelayCommand ChangePasswordCommand
    {
        get; set;
    }


    public UserDetailViewModel(IUserDataService UserDataService, INavigationService navigationService, IStorePageSettingsService storePageSettingsService, ICloudinaryService cloudinaryService) : base(storePageSettingsService)
    {
        _UserDataService = UserDataService;
        _navigationService = navigationService;
        _cloudinaryService = cloudinaryService;

        SelectImageButtonCommand = new RelayCommand(SelectImage);
        RemoveImageButtonCommand = new RelayCommand(() =>
        {
            SelectedImageName = string.Empty;
            SelectedImagePath = string.Empty;
            if (EditUser is not null)
            {
                EditUser.Image = string.Empty;
            }
            NotifyThisChanges();
        }, () => IsImageSelected);
        UpdateButtonCommand = new RelayCommand(UpdateUser, () => EditUser is not null);
        CancelButtonCommand = new RelayCommand(() => _navigationService.NavigateTo(typeof(ShoesViewModel).FullName!));
        ChangePasswordCommand = new RelayCommand(OnChangePassword);
    }

    private void OnChangePassword()
    {
        ShowPasswordDialogRequested?.Invoke("Please enter your new password.");
    }
    public async void SelectImage()
    {
        if (EditUser is null)
        {
            return;
        }

        var picker = new FileOpenPicker
        {
            ViewMode = PickerViewMode.Thumbnail,
            SuggestedStartLocation = PickerLocationId.PicturesLibrary
        };
        picker.FileTypeFilter.Add(".jpg");
        picker.FileTypeFilter.Add(".jpeg");
        picker.FileTypeFilter.Add(".png");

        var hwnd = WindowNative.GetWindowHandle(App.MainWindow);
        InitializeWithWindow.Initialize(picker, hwnd);

        var file = await picker.PickSingleFileAsync();
        //Binding img make string to BitMapImage
        //Use StringToBitmapImageConverter to convertback
        if (file != null)
        {
            SelectedImageName = file.Name;
            SelectedImagePath = file.Path;
            EditUser.Image = SelectedImagePath;
            OnPropertyChanged(nameof(EditUser.Image));
        }

        NotifyThisChanges();
    }

    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;
        // tínnhan.vn -> xn--tnnhan-pta.vn
        try
        {
            // Use IdnMapping class to convert Unicode domain names.
            email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                  RegexOptions.None, TimeSpan.FromMilliseconds(200));

            // Examines the domain part of the email and normalizes it.
            string DomainMapper(Match match)
            {
                var idn = new IdnMapping();

                // Use IdnMapping class to convert Unicode domain names.
                string domainName = idn.GetAscii(match.Groups[2].Value);

                return match.Groups[1].Value + domainName;
            }
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
        catch (ArgumentException)
        {
            return false;
        }

        try
        {
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

    public async void UpdateUser()
    {
        IsEditLoading = true;

        NotifyThisChanges();

        try
        {
            if (!IsValidEmail(EditUser.Email))
            {
                EditErrorMessage = "Invalid email address.";
                ShowDialogRequested?.Invoke("Error", EditErrorMessage);
                return;
            }
            // Check if there is an image to upload
            if (!string.IsNullOrEmpty(EditUser?.Image) && EditUser.Image == SelectedImagePath)
            {
                var imageUrl = await _cloudinaryService.UploadImageAsync(EditUser.Image, "user");
                EditUser.Image = imageUrl;
            }
            var (returnedUser, message, ERROR_CODE) = await _UserDataService.UpdateUserAsync(EditUser);

            if (ERROR_CODE == 1)
            {
                Item = returnedUser;
                ShowDialogRequested?.Invoke("Success", "User updated successfully.");
                _navigationService.NavigateTo(typeof(UsersViewModel).FullName!);
            }
            else
            {
                if (message.Contains("23505: duplicate key value violates unique constraint \"user_email_unique\"")) // check if email is already in use
                {
                    EditErrorMessage = "The email address is already in use. Please use a different email.";
                }
                else
                {

                    EditErrorMessage = message;
                }
                ShowDialogRequested?.Invoke("Error", EditErrorMessage);
            }
        }
        catch (Exception ex)
        {
            EditErrorMessage = $"An error occurred: {ex.Message}";
            ShowDialogRequested?.Invoke("Error", EditErrorMessage);
        }
        finally
        {
            IsEditLoading = false;
            NotifyThisChanges();
        }
    }
    public async void UpdateUserPassword(string newPassword)
    {
        IsEditLoading = true;
        NotifyThisChanges();

        try
        {
            EditUser.Password = BcryptUtil.HashPassword(newPassword);
            var (returnedUser, message, ERROR_CODE) = await _UserDataService.UpdateUserAsync(EditUser);

            if (ERROR_CODE == 1)
            {
                Item = returnedUser;
                ShowDialogRequested?.Invoke("Success", "Password updated successfully.");
            }
            else
            {
                EditErrorMessage = message;
                ShowDialogRequested?.Invoke("Error", EditErrorMessage);
            }
        }
        catch (Exception ex)
        {
            EditErrorMessage = $"An error occurred: {ex.Message}";
            ShowDialogRequested?.Invoke("Error", EditErrorMessage);
        }
        finally
        {
            IsEditLoading = false;
            NotifyThisChanges();
        }
    }



    public void NotifyThisChanges()
    {
        UpdateButtonCommand.NotifyCanExecuteChanged();
        CancelButtonCommand.NotifyCanExecuteChanged();
        RemoveImageButtonCommand.NotifyCanExecuteChanged();
        SelectImageButtonCommand.NotifyCanExecuteChanged();

        OnPropertyChanged(nameof(IsImageSelected));
    }

    public void OnNavigatedTo(object parameter)
    {
        if (parameter is User User)
        {
            Item = User;
            EditUser = Item;
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
