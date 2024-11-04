using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoesShop.Core.Models;
public class Order
{
    public int ID
    {
        get; set;
    }
    public int UserID
    {
        get; set;
    }
    public int AddressID
    {
        get; set;
    }
    public string OrderDate
    {
        get; set;
    }
    public string Status
    {
        get; set;
    }
    public decimal TotalAmount
    {
        get; set;
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> new-main
