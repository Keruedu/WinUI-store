using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;
using ShoesShop.Core.Services;
using ShoesShop.Core.Services.DataAcess;

namespace ShoesShop.Helpers;
public class ShoesSoldConverter : IValueConverter
{
    private readonly PostgreDao _postgreDao;

    public ShoesSoldConverter()
    {
        _postgreDao = new PostgreDao(); // Ensure you have a way to initialize this properly
    }

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is int shoesId)
        {
            // Call the async method and wait for the result
            int shoesSold = GetTotalShoesSoldByIdAsync(shoesId).Result;
            return shoesSold.ToString();
        }
        return "0";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }

    private async Task<int> GetTotalShoesSoldByIdAsync(int shoesId)
    {
        return  _postgreDao.GetTotalShoesSoldByIdAsync(shoesId);
    }
}

