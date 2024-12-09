using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoesShop.Core.Models;
public class User : INotifyPropertyChanged
{
    public int ID
    {
        get; set;
    }
    public int AddressID
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
    public string Role
    {
        get; set;
    }
    public string Password
    {
        get; set; 
    }
    public string PhoneNumber
    {
        get; set;
    }
    public string Status
    {
        get; set;
    }
    public string Image
    {
        get; set;
    }
    public Address Address
    {
        get; set;
    }

    public event PropertyChangedEventHandler PropertyChanged;
}