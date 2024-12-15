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
using ShoesShop.Core.Http;

namespace ShoesShop.Core.Services;

public class AuthenticationService : IAuthenticationService
{
    private IDao dao = new PostgreDao();
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
    public async Task<(string, int,User)> LoginAsync(string email, string password)
    {
        var message = "Success";
        var ERROR_CODE = 0;
        User user =null;
        try
        {
            // Retrieve the user from the database
            user = dao.GetUserByName(email);

            if (user == null || string.IsNullOrEmpty(user.Email))
            {
                message = "User not found. Did you sign up?";
                ERROR_CODE = 404;
            }
            else if (BcryptUtil.ComparePlainAndHashed(password, user.Password) == false)
            {
                message = "Invalid credentials.";
                ERROR_CODE = 401;
            }
            else if (user.Status == "Banned")
            {
                message = "Your account is banned.";
                ERROR_CODE = 403;
            }
            else if (user.Role == "User") 
            {
                message = "You do not have access to the admin page.";
                ERROR_CODE = 403;
            }
            else
            {
                AccessToken = GenerateAccessToken(user); 
                UserId = user.ID.ToString();
            }
        }
        catch (Exception ex)
        {
            message = $"An error occurred: {ex.Message}";
            ERROR_CODE = 1;
        }

        return (message, ERROR_CODE,user);
    }

    public void Logout()
    {

    }

    private string GenerateAccessToken(User user)
    {
        // Use JWT or any other token generation mechanism
        return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{user.Email}:{DateTime.UtcNow}"));
    }

    public Task<bool> LogoutAsync()
    {
        AccessToken = string.Empty;

        return Task.FromResult(true);
    }
}
