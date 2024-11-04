using ShoesShop.Contracts.Services;

namespace ShoesShop.Services;
public class StorePageSettingsService : IStorePageSettingsService
{
    public int ItemsPerPage
    {
        get; set;
    } = 0;

    private const string ItemsPerPageKey = "ItemsPerPage";

    private readonly ILocalSettingsService _localSettingsService;

    public StorePageSettingsService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        ItemsPerPage = await TryGetItemsPerPageAsync();
    }

    public async Task<int> GetItemsPerPageAsync()
    {
        await TryGetItemsPerPageAsync();

        return ItemsPerPage;
    }

    public async Task<bool> SaveItemsPerPageAsync(int itemsPerPage)
    {
        await _localSettingsService.SaveSettingAsync(ItemsPerPageKey, itemsPerPage);

        return true;
    }

    public async Task<int> TryGetItemsPerPageAsync()
    {
        var itemsPerPage = await _localSettingsService.ReadSettingAsync<int>(ItemsPerPageKey);

        if (itemsPerPage == 0)
        {
            itemsPerPage = 10;
            await SaveItemsPerPageAsync(itemsPerPage);
        }

        ItemsPerPage = itemsPerPage;

        return ItemsPerPage;
    }
}
