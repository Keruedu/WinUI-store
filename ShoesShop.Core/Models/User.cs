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
<<<<<<< HEAD
    public int AddressID 
    {
        get; set; 
=======
    public int AddressID
    {
        get; set;
>>>>>>> new-main
    }
    public string Name
    {
        get; set;
    }
    public string Email
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

    public event PropertyChangedEventHandler PropertyChanged;
}