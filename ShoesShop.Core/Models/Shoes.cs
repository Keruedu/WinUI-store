using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoesShop;
public class Shoes : INotifyPropertyChanged
{
    public int ID
    {
        get; set;
    }
    public string CategoryID
    {
        get; set;
    }
    public string Name
    {
        get; set;
    }
    public string Size
    {
        get; set;
    }
    public string Color
    {
        get; set;
    }
    public float Price
    {
        get; set;
    }

    public int Stock
    {
        get; set;
    }

    public event PropertyChangedEventHandler PropertyChanged;
}
