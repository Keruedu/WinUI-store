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

namespace ShoesShop.ViewModels;
public partial class AddUserViewModel : ResourceLoadingViewModel, INavigationAware
{
    private readonly IUserDataService _UserDataService;
    private readonly INavigationService _navigationService;
    private readonly ICloudinaryService _cloudinaryService;

    [ObservableProperty]
    public bool isEditSession = false;

    public event Action<string, string>? ShowDialogRequested;
    public event Action<string>? ShowPasswordDialogRequested;

    public bool IsEditButtonVisible => !IsEditSession;

    [ObservableProperty]
    public string editErrorMessage = string.Empty;

    [ObservableProperty]
    public bool isEditLoading = false;

    [ObservableProperty]
    public User? newUser;


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

    public RelayCommand CreateButtonCommand
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

    public RelayCommand NewUserButtonCommand
    {
        get; set;
    }

    public AddUserViewModel(IUserDataService UserDataService, INavigationService navigationService, IStorePageSettingsService storePageSettingsService, ICloudinaryService cloudinaryService) : base(storePageSettingsService)
    {
        _UserDataService = UserDataService;
        _navigationService = navigationService;
        _cloudinaryService = cloudinaryService;

        newUser = new User
        {
            Name = "User Name",
            Email = "newemail@gmail.com",
            PhoneNumber = "User Number",
            Password = "123",
            Role = "User",
            Status = "Active",
            Image = string.Empty,
            Address = new Address
            {
                Street = "User Street",
                City = "User City",
                State = "User State",
                ZipCode = "Code",
                Country = "User Country"
            }
        };

        SelectImageButtonCommand = new RelayCommand(SelectImage);
        RemoveImageButtonCommand = new RelayCommand(() =>
        {
            SelectedImageName = string.Empty;
            SelectedImagePath = string.Empty;
            if (NewUser is not null)
            {
                NewUser.Image = string.Empty;
            }
            NotifyThisChanges();
        }, () => IsImageSelected);
        CreateButtonCommand = new RelayCommand(ShowPasswordDialog, () => NewUser is not null);
        CancelButtonCommand = new RelayCommand(() => _navigationService.NavigateTo(typeof(UsersViewModel).FullName!));

    }




    public async void SelectImage()
    {
        if (NewUser is null)
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
            NewUser.Image = SelectedImagePath;
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


    private void ShowPasswordDialog()
    {
        ShowPasswordDialogRequested?.Invoke("Please enter a new password for the user.");
    }


    public async void AddUser(string password)
    {
        IsEditLoading = true;
        NotifyThisChanges();

        try
        {
            if (NewUser != null)
            {
                if (!IsValidEmail(NewUser.Email))
                {
                    EditErrorMessage = "Invalid email address.";
                    ShowDialogRequested?.Invoke("Error", EditErrorMessage);
                    return;
                }

                if (password.Length < 6)
                {
                    EditErrorMessage = "Password must be at least 6 characters long.";
                    ShowDialogRequested?.Invoke("Error", EditErrorMessage);
                    return;
                }

                NewUser.Password = BcryptUtil.HashPassword(password);

                if (!string.IsNullOrEmpty(NewUser.Image) && NewUser.Image == SelectedImagePath)
                {
                    var imageUrl = await _cloudinaryService.UploadImageAsync(NewUser.Image, "user");
                    NewUser.Image = imageUrl;
                }

                var (returnedUser, message, ERROR_CODE) = await _UserDataService.CreateUserAsync(NewUser);

                if (ERROR_CODE == 1)
                {
                    NewUser = returnedUser;
                    ShowDialogRequested?.Invoke("Success", "User added successfully.");
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
        CreateButtonCommand.NotifyCanExecuteChanged();
        CancelButtonCommand.NotifyCanExecuteChanged();
        RemoveImageButtonCommand.NotifyCanExecuteChanged();
        SelectImageButtonCommand.NotifyCanExecuteChanged();

        OnPropertyChanged(nameof(IsEditButtonVisible));
        OnPropertyChanged(nameof(IsImageSelected));
    }

    public void OnNavigatedTo(object parameter)
    {

    }

    public void OnNavigatedFrom()
    {
    }
}
