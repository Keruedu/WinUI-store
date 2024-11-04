using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShoesShop.Core.Models;
public class AddOrderDetail
{
    public string ShoesId
    {

        get; set;
    }
    public int Quantity
    {

        get; set;
    }

    public double Price
    {

        get; set;
    }
}
