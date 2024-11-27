using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoesShop.Core.Models;
public class Detail
{
    public int ID
    {
        get; set;
    }
    public int OrderID
    {
        get; set;
    }
    public int ShoesID
    {
        get; set;
    }
    public int Quantity
    {
        get; set;
    }
    public decimal Price
    {
        get; set;
    }
    public Shoes Shoes
    {
        get; set;
    }
    public event PropertyChangedEventHandler PropertyChanged;
}
