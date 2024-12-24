using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoesShop.Core.Models;

namespace ShoesShop.Core.Utils;
public class OrderNotificationUtil
{
    private readonly  string SHOP_NAME = "Super-Shop";
    private readonly string CURRENCY = "VNĐ";
    private string buildItemListsOfAnOrder(Order order)
    {
        const int WIDTH_IMAGE_PX = 100;
        const int HEIGHT_IMAGE_PX = 100;
        string items = "";
        foreach (var detail in order.Details)
        {
            items += $@"
                    <img src=""{detail.Shoes.Image}""
                    style=""width: {WIDTH_IMAGE_PX}px; height: {HEIGHT_IMAGE_PX}px; object-fit: cover;"" />
                    {detail.Quantity}x {detail.Shoes.Name} - {detail.Quantity * detail.Price} {CURRENCY}"   ;
        }
        return items;
    }
    public string createStringOfHtmlNotificationForCustomer(Order order)
    {
        string emailBody = $@"
                <h1>{SHOP_NAME}, Notification!!!</h1>
                <p>Thank you for your order, {order.User.Name}!</p>
                <p><strong>Order ID:</strong> {order.ID}</p>
                <p><strong>Order Date:</strong> {order.OrderDate}</p>
                <p><strong>Total Price:</strong>{order.TotalAmount} {CURRENCY}</p>
                <p><strong>Items:</strong></p>
                <pre>{buildItemListsOfAnOrder(order)}</pre>
                <p>We appreciate your business!</p>";
        return emailBody;
    }
}
