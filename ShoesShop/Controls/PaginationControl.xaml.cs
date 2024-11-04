using System.Windows.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace ShoesShop.Controls;

public sealed partial class PaginationControl : UserControl
{
    public PaginationControl()
    {
        InitializeComponent();
        Loaded += UpdateVisualState;
        SizeChanged += UpdateVisualState;
    }

    // Responsive 

    private void UpdateVisualState(object sender, RoutedEventArgs e)
    {
        var windowWidth = App.MainWindow.Width;

        if (windowWidth < 640)
        {
            PagingTextFull.Visibility = Visibility.Collapsed;
            PagingTextCompact.Visibility = Visibility.Visible;
        }
        else
        {
            PagingTextFull.Visibility = Visibility.Visible;
            PagingTextCompact.Visibility = Visibility.Collapsed;
        }
    }

    public static readonly DependencyProperty CurrentPageProperty = DependencyProperty.Register(
      nameof(CurrentPage),
      typeof(int),
      typeof(PaginationControl),
      new PropertyMetadata(1, OnCurrentPagePropertyChanged)
    );

    // Custom properties

    public int CurrentPage
    {
        get => (int)GetValue(CurrentPageProperty);
        set => SetValue(CurrentPageProperty, value);
    }

    private static void OnCurrentPagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        // Handle property changed logic here if needed
    }

    public static readonly DependencyProperty TotalPagesProperty = DependencyProperty.Register(
      nameof(TotalPages),
      typeof(int),
      typeof(PaginationControl),
      new PropertyMetadata(1, OnTotalPagesPropertyChanged)
    );

    public int TotalPages
    {
        get => (int)GetValue(TotalPagesProperty);
        set => SetValue(TotalPagesProperty, value);
    }

    private static void OnTotalPagesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        // Handle property changed logic here if needed
    }

    public static readonly DependencyProperty ItemsPerPageProperty = DependencyProperty.Register(
      nameof(ItemsPerPage),
      typeof(int),
      typeof(PaginationControl),
      new PropertyMetadata(1, OnItemsPerPagePropertyChanged)
    );

    public int ItemsPerPage
    {
        get => (int)GetValue(ItemsPerPageProperty);
        set => SetValue(ItemsPerPageProperty, value);
    }

    private static void OnItemsPerPagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        // Handle property changed logic here if needed
    }

    public static readonly DependencyProperty TotalItemsProperty = DependencyProperty.Register(
      nameof(TotalItems),
      typeof(int),
      typeof(PaginationControl),
      new PropertyMetadata(1, OnTotalItemsPropertyChanged)
    );

    public int TotalItems
    {
        get => (int)GetValue(TotalItemsProperty);
        set => SetValue(TotalItemsProperty, value);
    }

    private static void OnTotalItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        // Handle property changed logic here if needed
    }

    public static readonly DependencyProperty FromProperty = DependencyProperty.Register(
      nameof(From),
      typeof(int),
      typeof(PaginationControl),
      new PropertyMetadata(1, OnFromPropertyChanged)
    );

    public int From
    {
        get => (int)GetValue(FromProperty);
        set => SetValue(FromProperty, value);
    }

    private static void OnFromPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        // Handle property changed logic here if needed
    }

    public static readonly DependencyProperty ToProperty = DependencyProperty.Register(
      nameof(To),
      typeof(int),
      typeof(PaginationControl),
      new PropertyMetadata(1, OnToPropertyChanged)
    );

    public int To
    {
        get => (int)GetValue(ToProperty);
        set => SetValue(ToProperty, value);
    }

    private static void OnToPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        // Handle property changed logic here if needed
    }

    public static readonly DependencyProperty GoToNextPageCommandProperty = DependencyProperty.Register(
      nameof(GoToNextPageCommand),
      typeof(ICommand),
      typeof(PaginationControl),
      new PropertyMetadata(null, OnGoToNextPageCommandPropertyChanged)
    );

    public ICommand GoToNextPageCommand
    {
        get => (ICommand)GetValue(GoToNextPageCommandProperty);
        set => SetValue(GoToNextPageCommandProperty, value);
    }

    private static void OnGoToNextPageCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {

        // Handle property changed logic here if needed
    }

    public static readonly DependencyProperty GoToPreviousPageCommandProperty = DependencyProperty.Register(
      nameof(GoToPreviousPageCommand),
      typeof(ICommand),
      typeof(PaginationControl),
      new PropertyMetadata(null, OnGoToPreviousPageCommandPropertyChanged)
    );

    public ICommand GoToPreviousPageCommand
    {
        get => (ICommand)GetValue(GoToPreviousPageCommandProperty);
        set => SetValue(GoToPreviousPageCommandProperty, value);
    }

    private static void OnGoToPreviousPageCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        // Handle property changed logic here if needed
    }

    public static readonly DependencyProperty HasNextPageProperty = DependencyProperty.Register(
      nameof(HasNextPage),
      typeof(bool),
      typeof(PaginationControl),
      new PropertyMetadata(false, OnHasNextPagePropertyChanged)
    );

    public bool HasNextPage
    {
        get => (bool)GetValue(HasNextPageProperty);
        set => SetValue(HasNextPageProperty, value);
    }

    private static void OnHasNextPagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        // Handle property changed logic here if needed
    }

    public static readonly DependencyProperty HasPreviousPageProperty = DependencyProperty.Register(
      nameof(HasPreviousPage),
      typeof(bool),
      typeof(PaginationControl),
      new PropertyMetadata(false, OnHasPreviousPagePropertyChanged)
    );

    public bool HasPreviousPage
    {

        get => (bool)GetValue(HasPreviousPageProperty);
        set => SetValue(HasPreviousPageProperty, value);
    }

    private static void OnHasPreviousPagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
    }
}