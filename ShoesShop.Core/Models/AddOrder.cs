using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShoesShop.Core.Models;
public class AddOrder
{
    public string UserId
    {
         get; set;
    }

    public ICollection<Detail> OrderDetails
    {
         get; set;
    }
}
