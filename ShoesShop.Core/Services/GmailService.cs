using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ShoesShop.Core.Contracts.Services;

namespace ShoesShop.Core.Services;
public class GmailService : IEmailService
{
    private readonly string _email= "nqvinhdongthap322004@gmail.com";
    private readonly string passsword = "qmkx oxkz yaom cbdq";
    public GmailService()
    {

    }
    public void sendMail(string to,string[] cc,string subject, string payload, bool isHtml)
    {
        MailMessage mailMessage = new MailMessage
        {
            From = new MailAddress(_email),
            Subject = subject,
            Body = payload,
            IsBodyHtml = isHtml,
        };
        if (to!=null && to.Trim().Length!=0)
        {
            mailMessage.To.Add(to);
        }
        foreach (var item in cc)
        {
            mailMessage.CC.Add(item);
        }
        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587) 
        {
            Credentials = new NetworkCredential(_email, passsword), 
            EnableSsl = true 
        }; 
        smtpClient.Send(mailMessage);

    }
}
