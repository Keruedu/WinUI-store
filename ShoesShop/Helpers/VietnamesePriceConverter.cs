using System.Globalization;
using Microsoft.UI.Xaml.Data;

namespace ShoesShop.Helpers;

public class VietnamesePriceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is decimal price)
        {
            var cul = CultureInfo.GetCultureInfo("vi-VN");

            return price.ToString("#,### đ", cul.NumberFormat);
        }

        throw new ArgumentException("ExceptionVietnamesePriceConverterValueMustBeAnDecimal");
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
