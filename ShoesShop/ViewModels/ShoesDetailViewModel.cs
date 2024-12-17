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
using WinUIEx.Messaging;

namespace ShoesShop.ViewModels;

public partial class ShoesDetailViewModel : ResourceLoadingViewModel, INavigationAware
{
    private readonly IShoesDataService _ShoesDataService;
    private readonly INavigationService _navigationService;
    private readonly ICategoryDataService _categoryDataService;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IMediator _mediator;

    [ObservableProperty]
    public bool isEditSession = false;

    public event Action<string, string>? ShowDialogRequested;

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
    [ObservableProperty]
    public Category selectedCategory = new Category();
    public bool IsImageSelected => !string.IsNullOrEmpty(SelectedImageName);
    public bool HasEditError => !string.IsNullOrEmpty(EditErrorMessage);


    [ObservableProperty]
    public Shoes? item;

    [ObservableProperty]
    public Shoes? editShoes;

    public RelayCommand SetEditItemSessionButtonCommand
    {
        get; set;
    }

    public RelayCommand SelectImageButtonCommand
    {
        get; set;
    }
    public RelayCommand UpdateButtonCommand
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

    public ShoesDetailViewModel(IShoesDataService ShoesDataService,IMediator mediator , INavigationService navigationService, ICategoryDataService categoryDataService, IStorePageSettingsService storePageSettingsService, ICloudinaryService cloudinaryService) : base(storePageSettingsService)
    {
        _ShoesDataService = ShoesDataService;
        _mediator = mediator;
        _navigationService = navigationService;
        _categoryDataService = categoryDataService;
        _cloudinaryService = cloudinaryService;

        SetEditItemSessionButtonCommand = new RelayCommand(() =>
        {
            IsEditSession = true;
            NotifyThisChanges();
        });

        
        //SelectImageButtonCommand = new RelayCommand(SelectImage, () => !IsImageSelected);
        //RemoveImageButtonCommand = new RelayCommand(RemoveImage);
        //CancelButtonCommand = new RelayCommand(CancelEdit, () => !IsEditLoading);
        //EditShoesButtonCommand = new RelayCommand(UpdateShoes, () => !IsEditLoading);

        SelectImageButtonCommand = new RelayCommand(SelectImage);
        RemoveImageButtonCommand = new RelayCommand(() =>
        {
            SelectedImageName = string.Empty;
            SelectedImagePath = string.Empty;
            if (EditShoes is not null)
            {
                EditShoes.Image = string.Empty;
            }
            NotifyThisChanges();
        }, () => IsImageSelected);
        UpdateButtonCommand = new RelayCommand(UpdateShoes, () => EditShoes is not null);
        CancelButtonCommand = new RelayCommand(() => _navigationService.NavigateTo(typeof(ShoesViewModel).FullName!));
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
        picker.FileTypeFilter.Add(".png");

        var hwnd = WindowNative.GetWindowHandle(App.MainWindow);
        InitializeWithWindow.Initialize(picker, hwnd);

        var file = await picker.PickSingleFileAsync();
        if (file != null)
        {
            SelectedImageName = file.Name;
            SelectedImagePath = file.Path;
            EditShoes.Image = SelectedImagePath;
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
            EditShoes.CategoryID = SelectedCategory.ID;
            // Check if there is an image to upload
            if (!string.IsNullOrEmpty(EditShoes?.Image) && EditShoes.Image == SelectedImagePath)
            {
                var imageUrl = await _cloudinaryService.UploadImageAsync(EditShoes.Image, "shoes");
                EditShoes.Image = imageUrl;
            }
            var (returnedShoes, message, ERROR_CODE) = await _ShoesDataService.UpdateShoesAsync(EditShoes);

            if (ERROR_CODE == 1)
            {
                Item = returnedShoes;
                CancelEdit();
                ShowDialogRequested?.Invoke("Success", "Shoes updated successfully.");
                _navigationService.NavigateTo(typeof(ShoesViewModel).FullName!);
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
            LoadCategories();
            for (int i = 0; i < CategoryOptions.Count; i++)
            {

                if (CategoryOptions[i].ID == Item.CategoryID)
                {

                    SelectedCategory = CategoryOptions[i];
                    break;
                }
            }
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
