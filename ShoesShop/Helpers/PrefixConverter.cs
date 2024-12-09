using Microsoft.UI.Xaml.Data;

namespace ShoesShop.Helpers;
public class PrefixConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is int id)
        {
            return $"#{id}";
        }
        return "#0";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
