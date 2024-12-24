using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using ShoesShop.Core.Models;
using ShoesShop.Core.Utils;

namespace ShoesShop.Core.Services;
public class GmailNotificationService
{
    private readonly OrderNotificationUtil _orderNotificationUtil = new OrderNotificationUtil();
    private readonly ProductNotificationUtil _productNotificationUtil = new ProductNotificationUtil();
    private readonly GmailService _mailService = new GmailService();
    private readonly string ORDER_NOTIFICATION_SUBJECT = "Notification for making order successful!";
    private readonly string PRODUCT_NOTIFICATION_SUBJECt = "New product is coming out !!!";
    public void NotifyMakingOrder(Order order)
    {
        var orderBodyContent = _orderNotificationUtil.createStringOfHtmlNotificationForCustomer(order);
        _mailService.sendMail(order.User.Name, new string[0], ORDER_NOTIFICATION_SUBJECT, orderBodyContent, true);
    }
    public void NotifyNewProductForAllCustomers(Shoes shoes)
    {
        //tech-debt
        //list of cc 
        string[] cc=new string[2] {"vinh01515@gmail.com","vinh6473@gmail.com"};
        var productBodyContent = _productNotificationUtil.createStringOfHtmlNotificationForNewProduct(shoes);
        _mailService.sendMail(null, cc, PRODUCT_NOTIFICATION_SUBJECt, productBodyContent, true);
    }
}
