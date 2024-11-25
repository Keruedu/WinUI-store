using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoesShop.Contracts.Services;
public interface IStoreLoginCredentialsService
{
    public string Email
    {
        get;
    }
    public string Password
    {

        get;
    }


    Task<bool> SaveCredentialsAsync(string email, string password);
    Task<(string Email, string Password)> TryGetCredentialsAsync();

    Task<bool> SaveRememberCredentialsAsync(bool value);
    Task<bool> TryGetRememberCredentialsAsync();

    Task<bool> ClearCredentailAsync();
    Task<bool> ClearRememberCredentialsAsync();
}
