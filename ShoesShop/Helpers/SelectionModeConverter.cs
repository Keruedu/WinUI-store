using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;
namespace ShoesShop.Helpers;
public class SelectionModeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        // Hiển thị CheckBox khi chế độ là Multiple
        return value is bool isSelected && isSelected ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return value;
    }
}
