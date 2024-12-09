using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;

namespace ShoesShop.Helpers;

public class StatusToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        string status = value as string;
        return status.ToLower() switch
        {
            "active" => new SolidColorBrush(ColorUtils.HexToColor("#4A69E2")),
            "banned" => new SolidColorBrush(ColorUtils.HexToColor("#FFA52F")),
            _ => new SolidColorBrush(Colors.Gray)
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
