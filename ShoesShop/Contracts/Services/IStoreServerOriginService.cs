using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoesShop.Contracts.Services;
public interface IStoreServerOriginService
{
    public string Host
    {
        get;
    }
    public int Port
    {
        get;
    }

    Task<bool> SaveServerOriginAsync(string host, int port);
    Task<(string Host, int Port)> TryGetServerOriginAsync();
}
