using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoesShop.Contracts.Services;
public partial interface ILocalSettingServiceUsingApplicationData
{
    public string ReadSettingSync(string key);

    public string SaveSettingSync(string key, string value);

    public bool DeleteSettingSync(string key);
}
