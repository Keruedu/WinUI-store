using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoesShop.Core.Models;
public class Category
{
    public int ID
    {
        get; set;
    }
    public string Name
    {
        get; set;
    }
    public string Description
    {
        get; set;
    }
    public event PropertyChangedEventHandler PropertyChanged;
}