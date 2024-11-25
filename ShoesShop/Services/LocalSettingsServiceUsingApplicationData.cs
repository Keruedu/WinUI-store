using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoesShop.Contracts.Services;
using Windows.Storage;

namespace ShoesShop.Services;
public partial class LocalSettingsServiceUsingApplicationData:ILocalSettingServiceUsingApplicationData
{
    ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;



    public string ReadSettingSync(string key)
    {
        if (localSettings.Values.ContainsKey(key))
        {
            return localSettings.Values[key].ToString();
        }

        return null;
    }

    public string SaveSettingSync(string key, string value)
    {
        if (localSettings.Values.ContainsKey(key)==true)
        {
            localSettings.Values[key] = value;
        }
        else
        {
            localSettings.Values.Add(key, value);
        }
        return key;
    }
}
