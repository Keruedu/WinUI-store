namespace ShoesShop.Contracts.Services;
public interface IStoreLastOpenPageService
{
    public Task<string> GetLastOpenPageAsync();
    public Task<bool> GetOpenLastPageAsync();

    Task<bool> SaveOpenLastPageAsync(bool openLastPage);
    Task<bool> TryGetOpenLastPageAsync();

    Task<bool> SaveLastOpenPageAsync(string page);
    Task<string> TryGetLastOpenPageAsync();
}
