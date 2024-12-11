using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.IO;

namespace ShoesShop.Helpers;

public class StringToBitmapImageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is string path && !string.IsNullOrWhiteSpace(path))
        {
            try
            {
                // Kiểm tra nếu đường dẫn là URL hoặc đường dẫn cục bộ
                if (Uri.IsWellFormedUriString(path, UriKind.Absolute))
                {
                    return new BitmapImage(new Uri(path, UriKind.Absolute));
                }
                else if (File.Exists(path)) // Nếu là đường dẫn cục bộ
                {
                    // Tạo URI từ đường dẫn cục bộ
                    return new BitmapImage(new Uri($"file:///{path.Replace('\\', '/')}", UriKind.Absolute));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Convert Exception: {ex.Message}");
                return null; // Trả về null nếu không hợp lệ
            }
        }
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is BitmapImage bitmapImage)
        {
            var uri = bitmapImage.UriSource?.ToString();
            if (!string.IsNullOrWhiteSpace(uri))
            {
                if (uri.StartsWith("file:///"))
                {
                    // Xử lý để trả về đường dẫn cục bộ
                    return uri.Substring(8).Replace('/', '\\'); // Bỏ "file:///" và thay đổi ký tự
                }
                return uri; // Trả về URL
            }
        }
        return "";
    }
}