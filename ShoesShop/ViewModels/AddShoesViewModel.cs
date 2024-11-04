using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShoesShop.Contracts.ViewModels;
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Models;
using ShoesShop;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace ShoesShop.ViewModels;

public partial class AddShoesViewModel : ObservableRecipient, INavigationAware
{
    private readonly IShoesDataService _ShoesDataService;
    private readonly ICategoryDataService _categoryDataService;

    [ObservableProperty]
    private Shoes newShoes = new()
    {
        Stock = 1,
        Price = 1000,
        Color = "White",
    };

    [ObservableProperty]
    public List<Category> categoryOptions = new();

    [ObservableProperty]
    private bool isLoading = false;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private string successMessage = string.Empty;

    [ObservableProperty]
    private string selectedImageName = string.Empty;

    public bool IsImageSelected => !string.IsNullOrEmpty(SelectedImageName);
    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
    public bool HasSuccess => !string.IsNullOrEmpty(SuccessMessage);


    public RelayCommand SelectImageButtonCommand
    {
        get; set;
    }

    public RelayCommand RemoveImageButtonCommand
    {
        get; set;
    }

    public RelayCommand AddShoesButtonCommand
    {
        get; set;
    }

    public RelayCommand ResetButtonCommand
    {
        get; set;
    }

    public AddShoesViewModel(IShoesDataService ShoesDataService, ICategoryDataService categoryDataService)
    {
        _ShoesDataService = ShoesDataService;
        _categoryDataService = categoryDataService;

        SelectImageButtonCommand = new RelayCommand(SelectImage, () => !IsImageSelected);
        RemoveImageButtonCommand = new RelayCommand(RemoveImage, () => IsImageSelected);
        AddShoesButtonCommand = new RelayCommand(AddShoes, () => !IsLoading);
        ResetButtonCommand = new RelayCommand(Reset, () => !IsLoading);
    }

    public async void SelectImage()
    {
        var picker = new FileOpenPicker
        {
            ViewMode = PickerViewMode.Thumbnail,
            SuggestedStartLocation = PickerLocationId.PicturesLibrary
        };
        picker.FileTypeFilter.Add(".jpg");
        picker.FileTypeFilter.Add(".jpeg");

        var hwnd = WindowNative.GetWindowHandle(App.MainWindow);
        InitializeWithWindow.Initialize(picker, hwnd);

        var file = await picker.PickSingleFileAsync();
        if (file != null)
        {
            SelectedImageName = file.Name;
            using var stream = await file.OpenStreamForReadAsync();
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            NewShoes.Image = "kk";
        }

        NotfifyChanges();
    }

    public void RemoveImage()
    {
        NewShoes.Image = "";
        SelectedImageName = string.Empty;

        NotfifyChanges();
    }

    public async void AddShoes()
    {
        IsLoading = true;
        ErrorMessage = string.Empty;
        SuccessMessage = string.Empty;
        NotfifyChanges();

        var (_, message, ERROR_CODE) = await _ShoesDataService.CreateShoesAsync(NewShoes);

        if (ERROR_CODE == 0)
        {
            SuccessMessage = message;
            Reset();
        }
        else
        {
            ErrorMessage = message;
        }

        IsLoading = false;
        NotfifyChanges();
    }

    public async void LoadCategories()
    {
        await Task.Run(async () => await _categoryDataService.LoadDataAsync());
        var (categories, _, _) = _categoryDataService.GetData();

        if (categories is not null)
        {
            CategoryOptions = (List<Category>)categories;
        }
    }

    public void Reset()
    {
        NewShoes = new()
        {
            Stock = 1,
            Price = 1000,
            Color = "White"
        };
        SelectedImageName = string.Empty;
        NotfifyChanges();
    }

    public void NotfifyChanges()
    {
        SelectImageButtonCommand.NotifyCanExecuteChanged();
        RemoveImageButtonCommand.NotifyCanExecuteChanged();

        OnPropertyChanged(nameof(IsImageSelected));
        OnPropertyChanged(nameof(HasError));
        OnPropertyChanged(nameof(HasSuccess));
    }

    public void OnNavigatedTo(object parameter)
    {
        LoadCategories();
    }

    public void OnNavigatedFrom()
    {
    }
}
