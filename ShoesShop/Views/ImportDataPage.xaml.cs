using Microsoft.UI.Xaml.Controls;

using ShoesShop.ViewModels;

namespace ShoesShop.Views
{
    public sealed partial class ImportDataPage : Page
    {
        public ImportDataViewModel ViewModel
        {
            get;
        }

        public ImportDataPage()
        {
            ViewModel = App.GetService<ImportDataViewModel>();
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
