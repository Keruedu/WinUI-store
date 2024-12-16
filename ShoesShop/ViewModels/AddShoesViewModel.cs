using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShoesShop.Contracts.ViewModels;
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Models;
using ShoesShop;
using Windows.Storage.Pickers;
using WinRT.Interop;
using ShoesShop.Contracts.Services;
using ShoesShop.Services;

namespace ShoesShop.ViewModels;

public partial class AddShoesViewModel : ObservableRecipient, INavigationAware
{
    private readonly IShoesDataService _ShoesDataService;
    private readonly ICategoryDataService _categoryDataService;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly INavigationService _navigationService;
    private readonly IMediator _mediator;

    [ObservableProperty]
    public Shoes? newShoes;

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
    [ObservableProperty]
    public Category selectedCategory = new Category();

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

    public RelayCommand CreateButtonCommand
    {
        get; set;
    }

    public RelayCommand CancelButtonCommand
    {
        get; set;
    }

    public RelayCommand ResetButtonCommand
    {
        get; set;
    }

    public AddShoesViewModel(IShoesDataService ShoesDataService, INavigationService navigationService, ICategoryDataService categoryDataService, ICloudinaryService cloudinaryService, IMediator mediator)
    {
        _ShoesDataService = ShoesDataService;
        _navigationService = navigationService;
        _categoryDataService = categoryDataService;
        _cloudinaryService = cloudinaryService;
        _mediator = mediator;

        newShoes = new Shoes
        {
            CategoryID = 1,
            Name = "Your Shoes Name",
            Brand = "Your Shoes Brand",
            Stock = 1,
            Cost = 500,
            Price = 1000,
            Color = "Black",
            Size = "40",
            Image = "",
            Status = "Active",
        };

        SelectImageButtonCommand = new RelayCommand(SelectImage);
        RemoveImageButtonCommand = new RelayCommand(RemoveImage, () => IsImageSelected);
        //AddShoesButtonCommand = new RelayCommand(AddShoes, () => !IsLoading);
        //ResetButtonCommand = new RelayCommand(Reset, () => !IsLoading);

        CreateButtonCommand = new RelayCommand(AddShoes, () => NewShoes is not null);
        CancelButtonCommand = new RelayCommand(() => _navigationService.NavigateTo(typeof(ShoesViewModel).FullName!));
    }

    public async void SelectImage()
    {
        if (NewShoes is null)
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
            NewShoes.Image = SelectedImagePath;
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
            _navigationService.NavigateTo(typeof(ShoesViewModel).FullName!);
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
        //Todo: Task run must be done when Navigate call
        //await Task.Run(async () => await _categoryDataService.LoadDataAsync());
        await _categoryDataService.LoadDataAsync();
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
        SelectedCategory = CategoryOptions.FirstOrDefault();
    }

    public void OnNavigatedFrom()
    {
    }
}
