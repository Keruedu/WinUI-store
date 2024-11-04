using ShoesShop.Contracts.Services;

namespace ShoesShop.Services;
public class StoreLastOpenPageService : IStoreLastOpenPageService
{
    private const string SettingsKey = "LastOpenPage";
    private const string SettingsKeyOpenLastPage = "OpenLastPage";
    private readonly ILocalSettingsService _localSettingsService;

    public StoreLastOpenPageService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

    public async Task<bool> SaveLastOpenPageAsync(string page)
    {
        await _localSettingsService.SaveSettingAsync(SettingsKey, page);

        return true;
    }
    public async Task<string> TryGetLastOpenPageAsync()
    {
        var page = await _localSettingsService.ReadSettingAsync<string>(SettingsKey);

        return page;
    }

    public async Task<bool> GetOpenLastPageAsync()
    {
        return await TryGetOpenLastPageAsync();
    }


    public async Task<bool> SaveOpenLastPageAsync(bool openLastPage)
    {
        await _localSettingsService.SaveSettingAsync(SettingsKeyOpenLastPage, openLastPage);

        return openLastPage;
    }

    public async Task<bool> TryGetOpenLastPageAsync()
    {
        var openLastPage = await _localSettingsService.ReadSettingAsync<bool>(SettingsKeyOpenLastPage);
        return openLastPage;
    }

    public async Task<string> GetLastOpenPageAsync()
    {
        return await TryGetLastOpenPageAsync();
    }
}
