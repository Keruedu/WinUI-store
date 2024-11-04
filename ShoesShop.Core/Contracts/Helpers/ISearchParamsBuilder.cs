namespace ShoesShop.Core.Contracts.Helpers;
public interface ISearchParamsBuilder
{
    public void Append(string key, object value);
    public string GetQueryString();
}
