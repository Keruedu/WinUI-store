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
            "inactive" => new SolidColorBrush(ColorUtils.HexToColor("#FFA52F")),
            "banned" => new SolidColorBrush(ColorUtils.HexToColor("#FFA52F")),
            "shipped" => new SolidColorBrush(ColorUtils.HexToColor("#FFA52F")),
            "delivered" => new SolidColorBrush(ColorUtils.HexToColor("#4A69E2")),
            "cancelled" => new SolidColorBrush(ColorUtils.HexToColor("D00000")),
            _ => new SolidColorBrush(Colors.Gray)
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
