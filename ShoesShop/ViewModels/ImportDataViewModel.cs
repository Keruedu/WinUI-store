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

    public ImportDataViewModel(IUserDataService userDataService)
    {
        _userDataService = userDataService;
    }

    [RelayCommand]
    public async Task ImportUserDataAsync()
    {
        if (string.IsNullOrEmpty(FilePath))
        {
            StatusMessage = "Please select a file.";
            return;
        }

        IsImporting = true;
        var (message, _) = await _userDataService.ImportUserFromExcelAsync(FilePath);
        StatusMessage = message;
        IsImporting = false;
    }

    [RelayCommand]
    public async Task ImportShoesDataAsync()
    {
        if (string.IsNullOrEmpty(FilePath))
        {
            StatusMessage = "Please select a file.";
            return;
        }

        IsImporting = true;
        var (message, _) = await _userDataService.ImportShoesFromExcelAsync(FilePath);
        StatusMessage = message;
        IsImporting = false;
    }

    [RelayCommand]
    public async void SelectFile()
    {
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
}
