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
using WinUIEx.Messaging;

namespace ShoesShop.ViewModels;
public partial class UserDetailViewModel : ResourceLoadingViewModel, INavigationAware
{
    private readonly IUserDataService _UserDataService;
    private readonly INavigationService _navigationService;
    private readonly ICloudinaryService _cloudinaryService;


    public event Action<string, string>? ShowDialogRequested;


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

    public async void UpdateUser()
    {
        IsEditLoading = true;

        NotifyThisChanges();

        try
        {
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
