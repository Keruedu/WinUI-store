using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShoesShop.Contracts.ViewModels;
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Models;
using ShoesShop;
using Windows.Storage.Pickers;
using WinRT.Interop;
using ShoesShop.Contracts.Services;

namespace ShoesShop.ViewModels;

public partial class AddShoesViewModel : ObservableRecipient, INavigationAware
{
    private readonly IShoesDataService _ShoesDataService;
    private readonly ICategoryDataService _categoryDataService;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IMediator _mediator;

    [ObservableProperty]
    private Shoes newShoes = new()
    {
        CategoryID = 1,
        Name = "Your Shoes Name",
        Brand = "Your Shoes Brand",
        Stock = 1,
        Price = 1000,
        Color = "Black",
        Size = "40",
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

    [ObservableProperty]
    private string selectedImagePath = string.Empty;

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

    public AddShoesViewModel(IShoesDataService ShoesDataService, ICategoryDataService categoryDataService, ICloudinaryService cloudinaryService, IMediator mediator)
    {
        _ShoesDataService = ShoesDataService;
        _categoryDataService = categoryDataService;
        _cloudinaryService = cloudinaryService;
        _mediator = mediator;

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
            SelectedImagePath = file.Path;
            NewShoes.Image = SelectedImagePath;
            OnPropertyChanged(nameof(NewShoes));
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
        try {
            if (!string.IsNullOrEmpty(NewShoes?.Image) && NewShoes.Image == SelectedImagePath)
            {
                var imageUrl = await _cloudinaryService.UploadImageAsync(NewShoes.Image, "shoes");
                NewShoes.Image = imageUrl;
            }
            var (_, message, ERROR_CODE) = await _ShoesDataService.CreateShoesAsync(NewShoes);

            if (ERROR_CODE == 1)
            {
               
                SuccessMessage = message;
                _mediator.Notify();
                Reset();
            }
            else
            {
                ErrorMessage = message;
            }

        } catch (Exception ex) {
            ErrorMessage = $"An error occurred: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
            NotfifyChanges();
        }
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
            CategoryID = 1,
            Name = "Your Shoes Name",
            Stock = 1,
            Price = 1000,
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
