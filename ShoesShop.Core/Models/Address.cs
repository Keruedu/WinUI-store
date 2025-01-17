﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoesShop.Core.Models;
public class Address : INotifyPropertyChanged
{
    public int ID
    {
        get; set;
    }
    public string Street
    {
        get; set;
    }
    public string City
    {
        get; set;
    }
    public string State
    {
        get; set;
    }
    public string ZipCode
    {
        get; set;
    }
    public string Country
    {
        get; set;
    }
    public event PropertyChangedEventHandler PropertyChanged;
}
