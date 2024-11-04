using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using ShoesShop.Core.Models;

namespace ShoesShop;
public interface IDao
{
    public enum SortType
    {
        Ascending,
        Descending
    }
    Tuple<List<Shoes>, int> GetShoes(
        int page, int rowsPerPage,
        string keyword,
        Dictionary<string, SortType> sortOptions
    );

    bool DeleteShoes(int id);
    bool AddShoes(Shoes info);

    bool UpdateShoes(Shoes info);
}
