using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoesShop.Core.Contracts.Services;
public interface IEmailService
{
    void sendMail(string to, IEnumerable<String> cc,string subject,string payload,bool isHtml);
}
