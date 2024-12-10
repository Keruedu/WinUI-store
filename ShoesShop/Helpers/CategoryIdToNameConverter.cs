using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Data;
using ShoesShop.Core.Services.DataAcess;

namespace ShoesShop.Helpers;

public class CategoryIdToNameConverter : IValueConverter
{
    private readonly Dictionary<int, string> categoryMapping;

    public CategoryIdToNameConverter()
    {
        // Initialize the PostgreDao and fetch the category data
        var postgreDao = new PostgreDao();
        categoryMapping = postgreDao.GetAllCategories();
    }

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is int categoryId && categoryMapping.ContainsKey(categoryId))
        {
            return categoryMapping[categoryId];
        }

        return "Unknown Category";  // Default value if not found
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();  // If you don't need to convert back, you can leave it unimplemented
    }
}
