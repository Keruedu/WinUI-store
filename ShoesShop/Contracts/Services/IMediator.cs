using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml.Controls;

namespace ShoesShop.Contracts.Services;

public interface IMediator
{
    void Subscribe(Action action);
    void Notify();
}

