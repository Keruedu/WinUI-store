using System;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
namespace ShoesShop.Helpers;
public static class ColorUtils
{
    public static Windows.UI.Color HexToColor(string hex)
    {
        hex = hex.Replace("#", ""); // Loại bỏ ký tự #

        byte a = 255; // Mặc định alpha là 255
        int startIndex = 0;

        if (hex.Length == 8) // AARRGGBB
        {
            a = Convert.ToByte(hex.Substring(0, 2), 16);
            startIndex = 2;
        }

        byte r = Convert.ToByte(hex.Substring(startIndex, 2), 16);
        byte g = Convert.ToByte(hex.Substring(startIndex + 2, 2), 16);
        byte b = Convert.ToByte(hex.Substring(startIndex + 4, 2), 16);

        return Windows.UI.Color.FromArgb(a, r, g, b);
    }
}
