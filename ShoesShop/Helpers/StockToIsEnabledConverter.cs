using Microsoft.UI.Xaml.Data;
using System;

namespace ShoesShop.Helpers
{
    public class StockToIsEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int stock)
            {
                return stock > 0;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
