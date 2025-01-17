﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ShoesShop.Core.Models;
using ShoesShop.ViewModels;

namespace ShoesShop.Views;

public sealed partial class ShoesPage : Page
{
    public ShoesViewModel ViewModel
    {
        get;
    }

    public ShoesPage()
    {
        ViewModel = App.GetService<ShoesViewModel>();
        InitializeComponent();
        Loaded += UpdateVisualState;
        SizeChanged += UpdateVisualState;
    }

    private void UpdateVisualState(object sender, RoutedEventArgs e)
    {
        var windowWidth = App.MainWindow.Width;

        // VisualStateManager sucks

        if (windowWidth < 780)
        {
            FiltersAndSearchPanel.Visibility = Visibility.Collapsed;
            SmallFiltersAndSearchPanel.Visibility = Visibility.Visible;
            ToggleFiltersButton.Visibility = Visibility.Collapsed;
        }
        else
        {
            FiltersAndSearchPanel.Visibility = Visibility.Visible;
            SmallFiltersAndSearchPanel.Visibility = Visibility.Collapsed;
            ToggleFiltersButton.Visibility = Visibility.Visible;
        }
    }
    private void ToggleFiltersButton_Click(object sender, RoutedEventArgs e)
    {
        if (FiltersAndSearchPanel.Visibility == Visibility.Visible)
        {
            FiltersAndSearchPanel.Visibility = Visibility.Collapsed;
        }
        else
        {
            FiltersAndSearchPanel.Visibility = Visibility.Visible;
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

    private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = (sender as ComboBox)?.SelectedItem as ComboBoxItem;

        if (selectedItem is not null)
        {
            ViewModel.SelectStatus(selectedItem.Content.ToString());
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

    private void CategoryCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = (sender as ComboBox)?.SelectedItem as Category;

        if (selectedItem is not null)
        {
            ViewModel.SelectCategory(selectedItem);
        }
    }
}
