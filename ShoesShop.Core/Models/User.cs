using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoesShop.Core.Models;
internal class User : INotifyPropertyChanged
{
    public int ID
    {
        get; set;
    }
    public string Name
    {
        get; set;
    }
    public string Email
    {
        get; set;
    }
    public string Phone
    {
        get; set;
    }
    public string Address
    {
        get; set;
    }

    public event PropertyChangedEventHandler PropertyChanged;
}
