using System.Collections.ObjectModel;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ShoesShop.Contracts.Services;
using ShoesShop.Contracts.ViewModels;
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Models;
using ShoesShop.Services;

namespace ShoesShop.ViewModels;

public partial class AddCategoryViewModel : ObservableRecipient
{
    private readonly ICategoryDataService _categoryDataService;
    private readonly IMediator _mediator;

    [ObservableProperty]
    private Category newCategory = new();

    [ObservableProperty]
    private bool isLoading = false;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private string successMessage = string.Empty;

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
    public bool HasSuccess => !string.IsNullOrEmpty(SuccessMessage);

    public class CategoryAddedMessage
    {
        // This message can be empty, just serving as a notification.
    }

    public RelayCommand AddCategoryButtonCommand
    {
        get; set;
    }

    public RelayCommand CancelButtonCommand
    {
        get; set;
    }

    public AddCategoryViewModel(ICategoryDataService categoryDataService, IMediator mediator)
    {
        _categoryDataService = categoryDataService;
        _mediator = mediator;
        AddCategoryButtonCommand = new RelayCommand(OnAddCategoryButtonCommandAsync);
        CancelButtonCommand = new RelayCommand(OnCancelButtonCommand);
    }

    private async void OnAddCategoryButtonCommandAsync()
    {

        IsLoading = true;
        ErrorMessage = string.Empty;
        SuccessMessage = string.Empty;
        NotifyChanges();

        var (_, message, ERROR_CODE) = await _categoryDataService.AddCategoryAsync(NewCategory);

        if (ERROR_CODE == 1)
        {
            SuccessMessage = message;
            _mediator.Notify();
            OnCancelButtonCommand();
        }
        else
        {
            ErrorMessage = message;
        }

        IsLoading = false;
        NotifyChanges();
    }

    private void OnCancelButtonCommand()
    {
        NewCategory = new Category();
    }

    private void NotifyChanges()
    {
        AddCategoryButtonCommand.NotifyCanExecuteChanged();

        OnPropertyChanged(nameof(HasError));
        OnPropertyChanged(nameof(HasSuccess));
    }
}
