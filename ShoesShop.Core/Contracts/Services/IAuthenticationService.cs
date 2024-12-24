using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoesShop.Core.Models;

namespace ShoesShop.Core.Contracts.Services;
public interface IAuthenticationService
{
    public string GetAccessToken();
    public string GetUserId();
    public bool IsAuthenticated();
    public Task<(string, int,User)> LoginAsync(string email, string password);
    public Task<bool> LogoutAsync();
}
