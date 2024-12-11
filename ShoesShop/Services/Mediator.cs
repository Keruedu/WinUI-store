using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoesShop.Contracts.Services;

namespace ShoesShop.Services;
public class Mediator : IMediator
{
    private readonly List<Action> _subscribers = new List<Action>();

    public void Subscribe(Action action)
    {
        _subscribers.Add(action);
    }

    public void Notify()
    {
        foreach (var subscriber in _subscribers)
        {
            subscriber();
        }
    }
}
