using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
namespace ShoesShop.Core.Utils;
public partial class BcryptUtil
{
    public static string HashPassword(string plainPassword)
    {
        if (plainPassword == null)
        {
            return null;
        }
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword, 10);
        return hashedPassword;
    }

    public static bool ComparePlainAndHashed(string plainPassword, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
    }
}
