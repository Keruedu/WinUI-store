using CommunityToolkit.WinUI.UI.Controls;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using ShoesShop.Core.Models;
using ShoesShop.ViewModels;
using Microsoft.UI.Xaml;
using System.Windows.Input;
using Microsoft.UI.Xaml.Media;
using System.ComponentModel;
using Microsoft.UI.Xaml.Navigation;

namespace ShoesShop.Views;

public sealed partial class UsersPage : Page
{
    public UsersViewModel ViewModel
    {
        get;
    }

    public UsersPage()
    {
        ViewModel = App.GetService<UsersViewModel>();
        InitializeComponent();
        Loaded += UpdateVisualState;
        SizeChanged += UpdateVisualState;
        // Đăng ký lắng nghe sự kiện
        ViewModel.PropertyChanged += OnViewModelPropertyChanged;
        ViewModel.BanUsersCommand.CanExecuteChanged += (s, e) => UncheckAllCheckboxes();
        ViewModel.UnbanUsersCommand.CanExecuteChanged += (s, e) => UncheckAllCheckboxes();
    }

    private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ViewModel.CurrentPage))
        {
            // Khi CurrentPage thay đổi, uncheck tất cả checkbox
            UncheckAllCheckboxes();
        }
    }


    //private void OnViewStateChanged(object sender, ListDetailsViewState e)
    //{
    //    if (e == ListDetailsViewState.Both)
    //    {
    //        ViewModel.EnsureItemSelected();
    //    }
    //}

    private void UpdateVisualState(object sender, RoutedEventArgs e)
    {
        var windowWidth = App.MainWindow.Width;

        // VisualStateManager sucks

        if (windowWidth < 780)
        {
            FiltersAndSearchPanel.Visibility = Visibility.Collapsed;
            SmallFiltersAndSearchPanel.Visibility = Visibility.Visible;
        }
        else
        {
            FiltersAndSearchPanel.Visibility = Visibility.Visible;
            SmallFiltersAndSearchPanel.Visibility = Visibility.Collapsed;
        }
    }

    private void SortByComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = (sender as ComboBox)?.SelectedItem as SortObject;

        if (selectedItem is not null)
        {
            ViewModel.SelectSortOption(selectedItem);
        }
    }


    private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        var text = (sender as TextBox)?.Text;

        if (text is not null)
        {
            ViewModel.Search(text);
        }
    }

    private void SearchTextBox_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter)
        {

            var text = (sender as TextBox)?.Text;

            if (text is not null)
            {
                ViewModel.Search(text);
                ViewModel.LoadData();
            }
        }
    }

    private void StatusCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = (sender as ComboBox)?.SelectedItem as string;

        if (selectedItem is not null)
        {
            ViewModel.SelectStatus(selectedItem);
        }
    }

    private void RoleCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = (sender as ComboBox)?.SelectedItem as string;

        if (selectedItem is not null)
        {
            ViewModel.SelectRole(selectedItem);
        }
    }

    private void OnItemClick(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is User clickedUser)
        {
            ViewModel.ItemClickCommand.Execute(clickedUser);
        }
    }

    private CheckBox FindCheckBoxInTemplate(DependencyObject container)
    {
        if (container == null) return null;

        int childCount = VisualTreeHelper.GetChildrenCount(container);
        for (int i = 0; i < childCount; i++)
        {
            var child = VisualTreeHelper.GetChild(container, i);

            if (child is CheckBox checkBox)
            {
                return checkBox; // Trả về CheckBox nếu tìm thấy
            }

            // Gọi đệ quy để tìm tiếp trong các cấp con
            var result = FindCheckBoxInTemplate(child);
            if (result != null)
            {
                return result;
            }
        }

        return null; // Không tìm thấy
    }

    private CheckBox FindCheckBoxInHeader()
    {
        // Tìm checkbox trong header của ListView
        var header = UsersListView.Header as StackPanel;
        if (header != null)
        {
            // Tìm Grid trong StackPanel
            var grid = header.Children.OfType<Grid>().FirstOrDefault();
            if (grid != null)
            {
                // Tìm checkbox trong Grid
                var checkBox = grid.Children.OfType<CheckBox>().FirstOrDefault();
                return checkBox;
            }
        }
        return null;
    }

    private void OnSelectAllChecked(object sender, RoutedEventArgs e)
    {
        foreach (var item in UsersListView.Items)
        {
            var listViewItem = (ListViewItem)UsersListView.ContainerFromItem(item);
            if (listViewItem != null)
            {
                var checkBox = FindCheckBoxInTemplate(listViewItem);
                if (checkBox != null)
                {
                    checkBox.IsChecked = true; // Chọn CheckBox
                }
            }
        }
    }

    private void OnSelectAllUnchecked(object sender, RoutedEventArgs e)
    {
        foreach (var item in UsersListView.Items)
        {
            var listViewItem = (ListViewItem)UsersListView.ContainerFromItem(item);
            if (listViewItem != null)
            {
                var checkBox = FindCheckBoxInTemplate(listViewItem);
                if (checkBox != null)
                {
                    checkBox.IsChecked = false; // Bỏ chọn CheckBox
                }
            }
        }
    }


    private void UncheckAllCheckboxes()
    {
        var headerCheckBox = FindCheckBoxInHeader();
        if (headerCheckBox != null)
        {
            headerCheckBox.IsChecked = false; // Uncheck checkbox header
        }

        foreach (var item in UsersListView.Items)
        {
            var listViewItem = (ListViewItem)UsersListView.ContainerFromItem(item);
            if (listViewItem != null)
            {
                var checkBox = FindCheckBoxInTemplate(listViewItem);
                if (checkBox != null)
                {
                    checkBox.IsChecked = false; // Uncheck checkbox
                }
            }
        }
    }

    private void OnItemChecked(object sender, RoutedEventArgs e)
    {
        CheckBox checkBox = sender as CheckBox;
        if (checkBox != null && checkBox.DataContext != null)
        {
            var selectedItem = checkBox.DataContext as User;
            if (selectedItem != null)
            {
                if (!ViewModel.SelectedUsers.Contains(selectedItem))
                {
                    ViewModel.SelectedUsers.Add(selectedItem);
                    ViewModel.UpdateButtonStates();
                }
            }
        }
    }


    private void OnItemUnchecked(object sender, RoutedEventArgs e)
    {
        CheckBox checkBox = sender as CheckBox;
        if (checkBox != null && checkBox.DataContext != null)
        {
            var deselectedItem = checkBox.DataContext as User;
            if (deselectedItem != null)
            {
                if (ViewModel.SelectedUsers.Contains(deselectedItem))
                {
                    ViewModel.SelectedUsers.Remove(deselectedItem);
                    ViewModel.UpdateButtonStates();
                }
            }
        }
    }
    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);

        // Hủy đăng ký sự kiện khi không còn cần thiết
        ViewModel.PropertyChanged -= OnViewModelPropertyChanged;
    }
}
