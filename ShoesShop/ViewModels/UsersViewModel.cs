using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using ShoesShop.Contracts.ViewModels;
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Contracts.Services;
using ShoesShop.Core.Models;
using ShoesShop.Core.Services;
using ShoesShop.Services;
using System.Windows.Input;
using System.ComponentModel;


namespace ShoesShop.ViewModels;

public partial class UsersViewModel : ResourceLoadingViewModel, INavigationAware, INotifyPropertyChanged
{
    private readonly INavigationService _navigationService;
    private readonly IUserDataService _userDataService;
    public int TotalUsers
    {
        get; set;
    }
    public int BannedUsers
    {
        get; set;
    }

    private ObservableCollection<User> _selectedUsers = new ObservableCollection<User>();
    public ObservableCollection<User> SelectedUsers
    {
        get => _selectedUsers;
        set
        {
            if (SetProperty(ref _selectedUsers, value))
            {
                UpdateButtonStates();
            }
        }
    }

    public ObservableCollection<User> Source { get; } = new ObservableCollection<User>();
    public List<string> StatusFilters
    {
        get; set;
    } = new List<string> { "All", "Active", "Banned" };

    public List<string> RoleFilters
    {
        get; set;
    } = new List<string> { "All", "Admin", "Manager", "User" };


    public bool CanBanUsers => SelectedUsers.Any();
    public bool CanUnbanUsers => SelectedUsers.Any(user => user.Status == "Banned");



    public RelayCommand BanUsersCommand
    {
        get;
    }

    public RelayCommand UnbanUsersCommand
    {
        get;
    }

    public RelayCommand NavigateToAddUserPageCommand
    {
        get;
    }

    public UsersViewModel(INavigationService navigationService, IUserDataService userDataService, IStorePageSettingsService storePageSettingsService) : base(storePageSettingsService)
    {
        _navigationService = navigationService;
        _userDataService = userDataService;
        FunctionOnCommand = LoadData;


        SortOptions = new List<SortObject>
        {
            new() { Name = "Default", Value = "default", IsAscending = true },
            new() { Name = "Name (A-Z)", Value = "Name", IsAscending = true },
            new() { Name = "Name (Z-A)", Value="Name", IsAscending = false },
            new() { Name = "Email (A-Z)", Value="Email", IsAscending = true },
            new() { Name = "Email (Z-A)", Value="Email", IsAscending = false },
        };

        NavigateToAddUserPageCommand = new RelayCommand(() => _navigationService.NavigateTo(typeof(AddUserViewModel).FullName!));
        BanUsersCommand = new RelayCommand(BanSelectedUsers);
        UnbanUsersCommand = new RelayCommand(UnbanSelectedUsers);

        SelectedSortOption = SortOptions[0];
    }



    public async void LoadData()
    {
        IsDirty = false;
        IsLoading = true;
        InfoMessage = string.Empty;
        ErrorMessage = string.Empty;
        NotfifyChanges();

        _userDataService.searchQuery = await BuildSearchQueryUserAsync();

        await _userDataService.LoadDataAsync();

        var (data, totalItems, message, ERROR_CODE) = _userDataService.GetData();

        if (data is not null)
        {
            Source.Clear();
            TotalUsers = totalItems;
            var (bannedUsers, _, bannedMessage, bannedErrorCode) = await _userDataService.GetBannedUsersAsync();
            if (bannedUsers is not null)
            {
                BannedUsers = bannedUsers.Count();
            }
            else
            {
                if (bannedErrorCode == 0)
                {
                    ErrorMessage = bannedMessage;
                }
            }

            foreach (var item in data)
            {
                Source.Add(item);
            }

            TotalItems = totalItems;

            if (TotalItems == 0)
            {
                InfoMessage = "No Users found";
            }
        }
        else
        {
            if (ERROR_CODE == 0)
            {
                ErrorMessage = message;
            }
        }

        IsLoading = false;
        NotfifyChanges();
    }

    public void OnNavigatedTo(object parameter)
    {
        if (Source.Count <= 0)
        {
            LoadData();
        }
    }

    public void OnNavigatedFrom()
    {
    }

    [RelayCommand]
    private void OnItemClick(User? clickedItem)
    {
        if (clickedItem != null)
        {
            _navigationService.SetListDataItemForNextConnectedAnimation(clickedItem);
            _navigationService.NavigateTo(typeof(UserDetailViewModel).FullName!, clickedItem);
        }
    }

    [RelayCommand]
    private void OnApplyFiltersAndSearch()
    {
        currentPage = 1;
        LoadData();
    }



    private async void BanSelectedUsers()
    {
        foreach (var user in SelectedUsers)
        {
            user.Status = "Banned";
            var (message, errorCode) = await _userDataService.BanAndUnbanUser(user);
            if (errorCode != 1)
            {
                ErrorMessage = message;
                return;
            }
        }
        LoadData();
        SelectedUsers.Clear();
        OnPropertyChanged(nameof(SelectedUsers));
        BanUsersCommand.NotifyCanExecuteChanged();
        UnbanUsersCommand.NotifyCanExecuteChanged();
        UpdateButtonStates();
    }

    private async void UnbanSelectedUsers()
    {
        foreach (var user in SelectedUsers)
        {
            user.Status = "Active";
            var (message, errorCode) = await _userDataService.BanAndUnbanUser(user);
            if (errorCode != 1)
            {
                ErrorMessage = message;
                return;
            }
        }
        LoadData();
        SelectedUsers.Clear();
        OnPropertyChanged(nameof(SelectedUsers));
        BanUsersCommand.NotifyCanExecuteChanged();
        UnbanUsersCommand.NotifyCanExecuteChanged();
        UpdateButtonStates();
    }


    public void UpdateButtonStates()
    {
        OnPropertyChanged(nameof(CanBanUsers));
        OnPropertyChanged(nameof(CanUnbanUsers));
    }



}
