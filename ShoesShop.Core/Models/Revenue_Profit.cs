using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShoesShop.Core.Models;
public class Revenue_Profit
{
    [JsonPropertyName("date")]
    public string Date
    {
        get; set;
    }

    [JsonPropertyName("revenue")]
    public double Revenue
    {
        get; set;
    }

    [JsonPropertyName("profit")]
    public double Profit
    {
        get; set;
    }
}
