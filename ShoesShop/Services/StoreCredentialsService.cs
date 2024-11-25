using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ShoesShop.Contracts.Services;

namespace ShoesShop.Services;
public class StoreCredentialsService:IStoreLoginCredentialsService
{
    private const string SettingsKey = "LoginCredentials";
    private const string RememberSettingsKey = "RememberLoginCredentials";

    private readonly ILocalSettingsService _localSettingsService;

    public string Email
    {
        get; private set;
    } = string.Empty;
    public string Password
    {
        get; private set;
    } = string.Empty;

    public StoreCredentialsService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }
    public async Task<bool> SaveCredentialsAsync(string email, string password)
    {
        Email = email;
        var (hashedPassword, entropy) = EncryptPassword(password);

        await _localSettingsService.SaveSettingAsync(SettingsKey, (Email, hashedPassword, entropy));

        return true;
    }

    public async Task<(string Email, string Password)> TryGetCredentialsAsync()
    {
        var storedCredentials = await _localSettingsService.ReadSettingAsync<(string Email, string Password, string Entropy)>(SettingsKey);

        if (storedCredentials != default)
        {
            Password = DecryptPassword(storedCredentials.Password, storedCredentials.Entropy);
            return (storedCredentials.Email, Password);
        }

        return (string.Empty, string.Empty);
    }

    private static (string, string) EncryptPassword(string password)
    {
        var passwordInBytes = Encoding.UTF8.GetBytes(password);
        var entropy = new byte[20];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(entropy);

        var cypherText = ProtectedData.Protect(passwordInBytes, entropy,
            DataProtectionScope.CurrentUser);
        var passwordIn64 = Convert.ToBase64String(cypherText);
        var entropyIn64 = Convert.ToBase64String(entropy);

        return (passwordIn64, entropyIn64);
    }

    private static string DecryptPassword(string passwordIn64, string entropyIn64)
    {
        var password = string.Empty;

        if (passwordIn64.Length != 0)
        {
            var cyperTextInBytes = Convert.FromBase64String(passwordIn64);
            var entropyInBytes = Convert.FromBase64String(entropyIn64);
            var passwordInBytes = ProtectedData.Unprotect(
                cyperTextInBytes,
                entropyInBytes,
                DataProtectionScope.CurrentUser
            );

            password = Encoding.UTF8.GetString(passwordInBytes);
        }

        return password;
    }

    public async Task<bool> SaveRememberCredentialsAsync(bool value)
    {
        await _localSettingsService.SaveSettingAsync(RememberSettingsKey, value);

        return value;
    }
    public async Task<bool> TryGetRememberCredentialsAsync()
    {
        var storedValue = await _localSettingsService.ReadSettingAsync<bool>(RememberSettingsKey);

        if (storedValue == default)
        {
            storedValue = false;
        }

        return storedValue;

    }

    public async Task<bool> ClearCredentailAsync()
    {
        Email = string.Empty;
        Password = string.Empty;

        await _localSettingsService.SaveSettingAsync(SettingsKey, (string.Empty, string.Empty, string.Empty));
        return true;
    }
    public async Task<bool> ClearRememberCredentialsAsync()
    {
        await _localSettingsService.SaveSettingAsync(RememberSettingsKey, false);
        return true;
    }
}
