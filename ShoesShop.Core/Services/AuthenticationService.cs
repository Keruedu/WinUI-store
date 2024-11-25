using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Models;
using ShoesShop.Core.Services.DataAcess;
using ShoesShop.Core.Utils;

namespace ShoesShop.Core.Services;
public class AuthenticationService : IAuthenticationService
{
    private IDao dao=new PostgreDao();
    public string AccessToken
    {
        get; set;
    }

    public string UserId
    {
        get; set;
    }
    public string GetAccessToken() => AccessToken;
    public string GetUserId() => UserId;
    public bool IsAuthenticated() => !string.IsNullOrEmpty(AccessToken);
    public async Task<(string, int)> LoginAsync(string email, string password)
    {
        var message = "Success";
        var ERROR_CODE = 0;
        try
        {
            User user = dao.GetUserByName(email);
            if (user == null) {
                message = "Not found!";
                ERROR_CODE = 404;
            }
            else if(BcryptUtil.ComparePlainAndHashed(password,user.Password)==false)
            {
                ERROR_CODE = 401;
                message = "Bad credentials";
            }
        }
        catch (Exception ex)
        {
            message = ex.Message;
            ERROR_CODE = 1;
        }
        return (message, ERROR_CODE);
    }
    public Task<bool> LogoutAsync()
    {
        AccessToken = string.Empty;

        return Task.FromResult(true);
    }
}
