using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoesShop.Core.Contracts.Services;
namespace ShoesShop.Core.Services;
public class ServiceFactory

{
    private static readonly Dictionary<string, Type> _choices = new Dictionary<string, Type>();

    public static void Register(Type parent, Type child)
    {
        _choices.Add(parent.Name, child);
    }
    public static object GetChildOf(Type parent)
    {
        Type type = _choices[parent.Name];
        object instance = Activator.CreateInstance(type);
        return instance;
    }
}
