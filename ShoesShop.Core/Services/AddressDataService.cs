using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Models;
using ShoesShop.Core.Services.DataAcess;
namespace ShoesShop.Core.Services;

public class AddressDataService : IAddressDataService
{
    private readonly IDao _dao;
    private (int, string, int) _AddressDataTuple;

    public AddressDataService(IDao dao)
    {

        _dao = dao;
    }

    public async Task<(Address, string, int)> CreateAddressAsync(Address address)
    {
        var (errorCode, Message, newAddress) = _dao.AddAddress(address);

        return await Task.FromResult((newAddress, Message, errorCode ? 1 : 0));
    }
}
