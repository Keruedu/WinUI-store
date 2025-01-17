﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoesShop.Core.Models;
public class Shoes : INotifyPropertyChanged
{
    public int ID
    {
        get; set;
    }
    public int CategoryID
    {
        get; set;
    }
    public string Name
    {
        get; set;
    }
    public string Brand
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
    public decimal Cost
    {
        get; set;
    }
    public decimal Price
    {
        get; set;
    }
    public int Stock
    {
        get; set;
    }
    public string Image
    {
        get; set;
    }
    public string Description
    {
        get; set;
    }
    public string Status
    {
        get; set;
    }
    public event PropertyChangedEventHandler PropertyChanged;
}