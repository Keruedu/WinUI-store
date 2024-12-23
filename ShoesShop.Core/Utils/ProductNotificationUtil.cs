using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoesShop.Core.Models;

namespace ShoesShop.Core.Utils;
public class ProductNotificationUtil
{
    private readonly string SHOP_NAME = "Super-Shop";
    private readonly string CURRENCY = "VNĐ";
    private readonly int WIDTH_IMAGE_PX = 300;
    private readonly int HEIGHT_IMAGE_PX = 300;
    private string buildImage(Shoes shoes)
    {

        string img= $@"
                    <img src=""{shoes.Image}""
                    style=""width: {WIDTH_IMAGE_PX}px; height: {HEIGHT_IMAGE_PX}px; object-fit: cover;"" />";
        return img;
    }
    public string createStringOfHtmlNotificationForNewProduct(Shoes shoes)
    {
        string emailBody = $@"
                <h1>{SHOP_NAME}, Notification!!!</h1>
                <p>Come now and try on our new product, <strong>{shoes.Name}</strong>!</p>
                <p><strong>Descripton: </strong>{shoes.Description}</p>
                <p>Shopping now for the low price of, <strong>{shoes.Price} {CURRENCY}</strong>!</p>
                <p
                    style=""display: flex; justify-content: center; align-items: center;""
                    >{buildImage(shoes)}</p>
                <p>We hope to see you soon!</p>";
        return emailBody;

    }
}