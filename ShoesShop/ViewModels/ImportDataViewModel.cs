using CommunityToolkit.Mvvm.ComponentModel;
using ShoesShop.Core.Contracts.Services;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Windows.Storage.Pickers;
using Windows.Storage;
using System;
using System.Threading.Tasks;

namespace ShoesShop.ViewModels;
public partial class ImportDataViewModel : ObservableRecipient
{
    private readonly IUserDataService _userDataService;

    [ObservableProperty]
    private string _filePath = string.Empty;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private bool _isImporting;

    [ObservableProperty]
    private string _successMessage = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _isSuccessVisible;

    [ObservableProperty]
    private bool _isErrorVisible;

    public ImportDataViewModel(IUserDataService userDataService)
    {
        _userDataService = userDataService;
    }

    [RelayCommand]
    public async Task ImportUserDataAsync()
    {
        ClearMessages();

        if (string.IsNullOrEmpty(FilePath))
        {
            ErrorMessage = "Please select a file.";
            IsErrorVisible = true;
            return;
        }

        IsImporting = true;
        var (message, result) = await _userDataService.ImportUserFromExcelAsync(FilePath);
        if (result != 0)
        {
            SuccessMessage = message;
            IsSuccessVisible = true;
        }
        else
        {
            ErrorMessage = message;
            IsErrorVisible = true;
        }
        IsImporting = false;
    }

    [RelayCommand]
    public async Task ImportShoesDataAsync()
    {
        ClearMessages();

        if (string.IsNullOrEmpty(FilePath))
        {
            ErrorMessage = "Please select a file.";
            IsErrorVisible = true;
            return;
        }

        IsImporting = true;
        var (message, result) = await _userDataService.ImportShoesFromExcelAsync(FilePath);
        if (result != 0)
        {
            SuccessMessage = message;
            IsSuccessVisible = true;
        }
        else
        {
            ErrorMessage = message;
            IsErrorVisible = true;
        }
        IsImporting = false;
    }

    [RelayCommand]
    public async void SelectFile()
    {
        ClearMessages();

        var picker = new FileOpenPicker();
        picker.ViewMode = PickerViewMode.Thumbnail;
        picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        picker.FileTypeFilter.Add(".xls");
        picker.FileTypeFilter.Add(".xlsx");

        // Get the current window's HWND
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

        StorageFile file = await picker.PickSingleFileAsync();
        if (file != null)
        {
            FilePath = file.Path;
            StatusMessage = "File selected: " + FilePath;
        }
    }

    private void ClearMessages()
    {
        SuccessMessage = string.Empty;
        ErrorMessage = string.Empty;
        IsSuccessVisible = false;
        IsErrorVisible = false;
    }
}
