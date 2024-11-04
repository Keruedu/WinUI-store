namespace ShoesShop.Contracts.Services;
public interface IStorePageSettingsService
{
    public Task<int> GetItemsPerPageAsync();

    Task<bool> SaveItemsPerPageAsync(int itemsPerPage);
    Task<int> TryGetItemsPerPageAsync();

}
