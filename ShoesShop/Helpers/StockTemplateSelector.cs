using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace ShoesShop.Helpers
{
    public class StockTemplateSelector : DataTemplateSelector
    {
        public DataTemplate InStockTemplate
        {
            get; set;
        }
        public DataTemplate OutOfStockTemplate
        {
            get; set;
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var shoes = item as ShoesShop.Core.Models.Shoes;
            if (shoes != null && shoes.Stock == 0)
            {
                return OutOfStockTemplate;
            }
            return InStockTemplate;
        }
    }
}
