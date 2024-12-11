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

public sealed partial class OrdersPage : Page
{
    public OrdersViewModel ViewModel
    {
        get;
    }

    public OrdersPage()
    {
        ViewModel = App.GetService<OrdersViewModel>();
        InitializeComponent();
        Loaded += UpdateVisualState;
        SizeChanged += UpdateVisualState;
        // Đăng ký lắng nghe sự kiện
        ViewModel.PropertyChanged += OnViewModelPropertyChanged;
        //ViewModel.BanOrdersCommand.CanExecuteChanged += (s, e) => UncheckAllCheckboxes();
        //ViewModel.UnbanOrdersCommand.CanExecuteChanged += (s, e) => UncheckAllCheckboxes();
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
                //ViewModel.LoadData();
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

    private void MinPriceTextBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
    {
        if (sender.Value > MaxPriceTextBox?.Value)
        {
            sender.Value = MaxPriceTextBox.Value;
            ViewModel.SetMinPrice((int)sender.Value);
        }
        else
        {
            ViewModel.SetMinPrice((int)sender.Value);
        }
    }

    private void FromDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
    {
        if (args.NewDate.HasValue)
        {
            ViewModel.SetFromDate(args.NewDate.Value.DateTime);
        }
    }

    private void ToDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
    {
        if (args.NewDate.HasValue)
        {
            ViewModel.SetToDate(args.NewDate.Value.DateTime);
        }
    }

    private void MaxPriceTextBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
    {
        if (sender.Value < MinPriceTextBox?.Value)
        {
            sender.Value = MinPriceTextBox.Value;
            ViewModel.SetMaxPrice((int)sender.Value);
        }
        else
        {
            ViewModel.SetMaxPrice((int)sender.Value);
        }
    }

    private void OnItemClick(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is Order clickedOrder)
        {
            ViewModel.ItemClickCommand.Execute(clickedOrder);
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

    private T FindControlInHeader<T>(DependencyObject parent) where T : DependencyObject
    {
        if (parent == null) return null;

        int childCount = VisualTreeHelper.GetChildrenCount(parent);
        for (int i = 0; i < childCount; i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);

            if (child is T control)
            {
                return control;
            }

            var result = FindControlInHeader<T>(child);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    private CheckBox FindCheckBoxInHeader()
    {

        var header = OrdersListView.Header as DependencyObject;
        if (header != null)
        {
            return FindControlInHeader<CheckBox>(header);
        }
        return null;
    }

    private void OnSelectAllChecked(object sender, RoutedEventArgs e)
    {
        foreach (var item in OrdersListView.Items)
        {
            var listViewItem = (ListViewItem)OrdersListView.ContainerFromItem(item);
            if (listViewItem != null)
            {
                var checkBox = FindCheckBoxInTemplate(listViewItem);
                if (checkBox != null)
                {
                    checkBox.IsChecked = true;
                }
            }
        }
    }

    private void OnSelectAllUnchecked(object sender, RoutedEventArgs e)
    {
        foreach (var item in OrdersListView.Items)
        {
            var listViewItem = (ListViewItem)OrdersListView.ContainerFromItem(item);
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

        foreach (var item in OrdersListView.Items)
        {
            var listViewItem = (ListViewItem)OrdersListView.ContainerFromItem(item);
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
            var selectedItem = checkBox.DataContext as Order;
            if (selectedItem != null)
            {
                if (!ViewModel.SelectedOrders.Contains(selectedItem))
                {
                    ViewModel.SelectedOrders.Add(selectedItem);
                }
            }
        }
    }


    private void OnItemUnchecked(object sender, RoutedEventArgs e)
    {
        CheckBox checkBox = sender as CheckBox;
        if (checkBox != null && checkBox.DataContext != null)
        {
            var deselectedItem = checkBox.DataContext as Order;
            if (deselectedItem != null)
            {
                if (ViewModel.SelectedOrders.Contains(deselectedItem))
                {
                    ViewModel.SelectedOrders.Remove(deselectedItem);
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
