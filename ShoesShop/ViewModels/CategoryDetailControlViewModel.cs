using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShoesShop.Contracts.Services;
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Models;

namespace ShoesShop.ViewModels;
public partial class CategoryDetailControlViewModel : ObservableRecipient
{
    private readonly ICategoryDataService _categoryDataservice;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    public Category editCategory = new();

    [ObservableProperty]
    public Category item = new();

    [ObservableProperty]
    public bool isEditSession = false;

    [ObservableProperty]
    public string editErrorMessage = string.Empty;

    [ObservableProperty]
    public bool isEditLoading = false;

    public bool IsEditButtonVisible => !IsEditSession;
    public bool HasEditError => !string.IsNullOrEmpty(EditErrorMessage);


    public RelayCommand CancelButtonCommand
    {
        get; set;
    }

    public RelayCommand EditCategoryButtonCommand
    {
        get; set;
    }

    public RelayCommand SetEditItemSessionButtonCommand
    {
        get; set;
    }


    public CategoryDetailControlViewModel(ICategoryDataService categoryDataService, INavigationService navigationService)
    {
        _categoryDataservice = categoryDataService;
        _navigationService = navigationService;

        CancelButtonCommand = new RelayCommand(OnCancelEdit, () => !IsEditLoading);
        EditCategoryButtonCommand = new RelayCommand(OnUpdateCategory, () => !IsEditLoading);
        SetEditItemSessionButtonCommand = new RelayCommand(OnSetEditItemSession);
    }

    public void OnSetEditItemSession()
    {
        EditCategory = Item;
        IsEditSession = true;
        NotifyThisChanges();
    }

    public async void OnUpdateCategory()
    {
        IsEditLoading = true;

        var (returnedBook, message, ERROR_CODE) = await _categoryDataservice.UpdateCategoryAsync(EditCategory);

        if (ERROR_CODE == 1)
        {
            Item = returnedBook;
            OnCancelEdit();
        }
        else
        {
            EditErrorMessage = message;
        }

        IsEditLoading = false;
        NotifyThisChanges();
    }

    public async void OnDeletCategory()
    {
        var (_, ERROR_CODE) = await _categoryDataservice.DeleteCategoryAsync(Item);

        if (ERROR_CODE == 1)
        {
            _navigationService.Refresh();
        }
    }

    public void OnCancelEdit()
    {
        EditCategory = Item;
        IsEditSession = false;
        NotifyThisChanges();
    }

    private void NotifyThisChanges()
    {
        CancelButtonCommand.NotifyCanExecuteChanged();
        EditCategoryButtonCommand.NotifyCanExecuteChanged();
        SetEditItemSessionButtonCommand.NotifyCanExecuteChanged();

        OnPropertyChanged(nameof(IsEditButtonVisible));
        OnPropertyChanged(nameof(HasEditError));
    }
}

