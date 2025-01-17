﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using ShoesShop.Core.Models;
using ShoesShop.ViewModels;

namespace ShoesShop.Views;
public sealed partial class OrderDetailPage : Page
{
    public OrderDetailViewModel ViewModel
    {
        get;
    }

    public OrderDetailPage()
    {
        InitializeComponent();
        ViewModel = App.GetService<OrderDetailViewModel>();
        DataContext = ViewModel;
        ViewModel.ShowDialogRequested += OnShowDialogRequested;
    }

    private async void OnShowDialogRequested(string title, string message)
    {
        var dialog = new ContentDialog
        {
            Title = title,
            Content = message,
            CloseButtonText = "OK",
            XamlRoot = this.Content.XamlRoot
        };
        
        await dialog.ShowAsync();
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

        var header = DetailsListView.Header as DependencyObject;
        if (header != null)
        {
            return FindControlInHeader<CheckBox>(header);
        }
        return null;
    }

    private void OnSelectAllChecked(object sender, RoutedEventArgs e)
    {
        foreach (var item in DetailsListView.Items)
        {
            var listViewItem = (ListViewItem)DetailsListView.ContainerFromItem(item);
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
        foreach (var item in DetailsListView.Items)
        {
            var listViewItem = (ListViewItem)DetailsListView.ContainerFromItem(item);
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

        foreach (var item in DetailsListView.Items)
        {
            var listViewItem = (ListViewItem)DetailsListView.ContainerFromItem(item);
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
            var selectedItem = checkBox.DataContext as Detail;
            if (selectedItem != null)
            {
                if (!ViewModel.SelectedDetails.Contains(selectedItem))
                {
                    ViewModel.SelectedDetails.Add(selectedItem);
                    //ViewModel.UpdateButtonStates();
                }
            }
        }
    }


    private void OnItemUnchecked(object sender, RoutedEventArgs e)
    {
        CheckBox checkBox = sender as CheckBox;
        if (checkBox != null && checkBox.DataContext != null)
        {
            var deselectedItem = checkBox.DataContext as Detail;
            if (deselectedItem != null)
            {
                if (ViewModel.SelectedDetails.Contains(deselectedItem))
                {
                    ViewModel.SelectedDetails.Remove(deselectedItem);
                    //ViewModel.UpdateButtonStates();
                }
            }
        }
    }

}
