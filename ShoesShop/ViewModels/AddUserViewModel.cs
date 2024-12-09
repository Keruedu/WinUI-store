using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml.Controls;
using ShoesShop.Contracts.Services;
using ShoesShop.Contracts.ViewModels;
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Models;
using ShoesShop.Core.Services;
using ShoesShop.Services;
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

    public ObservableCollection<Review> Source { get; } = new ObservableCollection<Review>();

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
            Email = "User Email",
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
        CreateButtonCommand = new RelayCommand(AddUser, () => NewUser is not null);
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
            OnPropertyChanged(nameof(NewUser.Image));
        }

        NotifyThisChanges();
    }

    public async void AddUser()
    {
        IsEditLoading = true;

        NotifyThisChanges();

        try
        {
            // Check if there is an image to upload
            if (!string.IsNullOrEmpty(NewUser?.Image) && NewUser.Image == SelectedImagePath)
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
                EditErrorMessage = message;
                ShowDialogRequested?.Invoke("Error", message);
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
