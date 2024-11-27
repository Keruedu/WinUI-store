using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml.Controls;
using ShoesShop.Contracts.Services;
using ShoesShop.Contracts.ViewModels;
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Models;
using ShoesShop.Services;

using ShoesShop;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace ShoesShop.ViewModels;

public partial class ShoesDetailViewModel : ResourceLoadingViewModel, INavigationAware
{
    private readonly IReviewDataService _reviewDataService;
    private readonly IShoesDataService _ShoesDataService;
    private readonly INavigationService _navigationService;
    private readonly ICategoryDataService _categoryDataService;
    private readonly ICloudinaryService _cloudinaryService;

    [ObservableProperty]
    public bool isEditSession = false;

    public bool IsEditButtonVisible => !IsEditSession;

    [ObservableProperty]
    public string editErrorMessage = string.Empty;
    [ObservableProperty]
    public bool isEditLoading = false;
    [ObservableProperty]
    public List<Category> categoryOptions = new();


    [ObservableProperty]
    public string selectedImageName = string.Empty;
    [ObservableProperty]
    public string selectedImagePath = string.Empty;
    public bool IsImageSelected => !string.IsNullOrEmpty(SelectedImageName);
    public bool HasEditError => !string.IsNullOrEmpty(EditErrorMessage);


    [ObservableProperty]
    public Shoes? item;

    [ObservableProperty]
    public Shoes? editShoes;

    [ObservableProperty]
    public Category? category;

    public ObservableCollection<Review> Source { get; } = new ObservableCollection<Review>();

    public RelayCommand SetEditItemSessionButtonCommand
    {
        get; set;
    }

    public RelayCommand SelectImageButtonCommand
    {
        get; set;
    }

    public RelayCommand RemoveImageButtonCommand
    {
        get; set;
    }

    public RelayCommand CancelButtonCommand
    {
        get; set;
    }

    public RelayCommand EditShoesButtonCommand
    {
        get; set;
    }

    public ShoesDetailViewModel(IReviewDataService reviewDataService, IShoesDataService ShoesDataService, INavigationService navigationService, ICategoryDataService categoryDataService, IStorePageSettingsService storePageSettingsService, ICloudinaryService cloudinaryService) : base(storePageSettingsService)
    {
        _reviewDataService = reviewDataService;
        _ShoesDataService = ShoesDataService;
        _navigationService = navigationService;
        _categoryDataService = categoryDataService;
        _cloudinaryService = cloudinaryService;

        SetEditItemSessionButtonCommand = new RelayCommand(() =>
        {
            IsEditSession = true;
            NotifyThisChanges();
        });

        SelectImageButtonCommand = new RelayCommand(SelectImage, () => !IsImageSelected);
        RemoveImageButtonCommand = new RelayCommand(RemoveImage);
        CancelButtonCommand = new RelayCommand(CancelEdit, () => !IsEditLoading);
        EditShoesButtonCommand = new RelayCommand(UpdateShoes, () => !IsEditLoading);
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

    public async void SelectImage()
    {
        if (EditShoes is null)
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

        var hwnd = WindowNative.GetWindowHandle(App.MainWindow);
        InitializeWithWindow.Initialize(picker, hwnd);

        var file = await picker.PickSingleFileAsync();
        if (file != null)
        {
            SelectedImageName = file.Name;
            SelectedImagePath = file.Path;
            EditShoes.Image = SelectedImagePath;
            OnPropertyChanged(nameof(EditShoes));
        }

        NotifyThisChanges();
    }

    public void RemoveImage()
    {
        EditShoes.Image = null;
        SelectedImageName = string.Empty;

        NotifyThisChanges();
    }

    public void NotifyThisChanges()
    {
        SetEditItemSessionButtonCommand.NotifyCanExecuteChanged();
        CancelButtonCommand.NotifyCanExecuteChanged();
        RemoveImageButtonCommand.NotifyCanExecuteChanged();
        SelectImageButtonCommand.NotifyCanExecuteChanged();

        OnPropertyChanged(nameof(IsEditButtonVisible));
        OnPropertyChanged(nameof(IsImageSelected));
        OnPropertyChanged(nameof(HasEditError));
    }

    public async void LoadReviewsAsync()
    {
        IsLoading = true;
        InfoMessage = string.Empty;
        ErrorMessage = string.Empty;
        NotfifyChanges();

        await Task.Run(async () => await _reviewDataService.LoadDataAsync());

        var (data, totalItems, message, ERROR_CODE) = _reviewDataService.GetData();

        if (data is not null)
        {
            foreach (var item in data)
            {
                if (string.IsNullOrEmpty(item.Content))
                {
                    item.Content = "No review content";
                }
                Source.Add(item);
            }

            IsLoading = false;
            TotalItems = totalItems;

            if (TotalItems == 0)
            {
                InfoMessage = "This Shoes hasn't had any reviews";
            }
        }
        else
        {
            if (ERROR_CODE != 1)
            {
                ErrorMessage = message;
            }
        }

        NotfifyChanges();
    }

    public async void DeleteShoes()
    {
        var (_, ERROR_CODE) = await _ShoesDataService.DeleteShoesAsync(Item);

        if (ERROR_CODE == 1)
        {
            _navigationService.GoBack();
        }
    }

    public async void UpdateShoes()
    {
        IsEditLoading = true;

        NotifyThisChanges();

        try
        {
            // Check if there is an image to upload
            if (!string.IsNullOrEmpty(EditShoes?.Image) && EditShoes.Image == SelectedImagePath)
            {
                var imageUrl = await _cloudinaryService.UploadImageAsync(EditShoes.Image);
                EditShoes.Image = imageUrl;
            }
            var (returnedShoes, message, ERROR_CODE) = await _ShoesDataService.UpdateShoesAsync(EditShoes);

            if (ERROR_CODE == 1)
            {
                Item = returnedShoes;
                CancelEdit();
            }
            else
            {
                EditErrorMessage = message;
            }
        }
        catch (Exception ex)
        {
            EditErrorMessage = $"An error occurred: {ex.Message}";
        }
        finally
        {
            IsEditLoading = false;
            NotifyThisChanges();
        }
    }

    public void CancelEdit()
    {
        EditShoes = Item;
        IsEditSession = false;
        SelectedImageName = string.Empty;
        NotifyThisChanges();
    }

    public void OnNavigatedTo(object parameter)
    {
        if (parameter is Shoes Shoes)
        {
            Item = Shoes;
            EditShoes = Item;
            _reviewDataService.ShoesId = Item?.ID.ToString() ?? string.Empty;
            LoadReviewsAsync();
            LoadCategories();
            foreach (var cate in CategoryOptions)
            {
                if(cate.ID == Item.CategoryID)
                {

                    Category = cate;
                    break;
                }
            }
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
