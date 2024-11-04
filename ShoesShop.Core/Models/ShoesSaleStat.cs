using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShoesShop.Core.Models;
public class ShoesSaleStat
{
    [JsonPropertyName("id")]
    public string Id
    {
        get; set;
    }

    [JsonPropertyName("name")]
    public string Name
    {
        get; set;
    }

    [JsonPropertyName("image")]
    public string Image
    {

        get; set;
    }

    [JsonPropertyName("soldQuantity")]
    public int SoldQuantity
    {

        get; set;
    }
}
