using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoesShop.Core.Models;
using ShoesShop.Core.Services.DataAcess;

namespace ShoesShop.Core.Contracts.Services;

public interface IAddressDataService
{
    Task<(Address, string, int)> CreateAddressAsync(Address address);
}
