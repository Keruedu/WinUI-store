using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoesShop.Contracts.Services;

namespace ShoesShop.Services;
public class StoreServerOriginService : IStoreServerOriginService
{
    private const string SettingsKey = "ServerOrigin";

    private readonly ILocalSettingsService _localSettingsService;
    public string Host
    {
        get; set;
    } = "http://localhost";

    public int Port
    {
        get; set;
    } = 8080;

    public StoreServerOriginService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
        InitializeAsync();
    }

    public async void InitializeAsync()
    {
        await TryGetServerOriginAsync();
    }

    public async Task<bool> SaveServerOriginAsync(string host, int port)
    {
        Host = host;
        Port = port;
        await _localSettingsService.SaveSettingAsync(SettingsKey, (host, port));

        return true;
    }

    public async Task<(string Host, int Port)> TryGetServerOriginAsync()
    {
        var storedValue = await _localSettingsService.ReadSettingAsync<(string Host, int Port)>(SettingsKey);

        if (storedValue != default)
        {
            Host = storedValue.Host;
            Port = storedValue.Port;
        }

        return (Host, Port);
    }
}
