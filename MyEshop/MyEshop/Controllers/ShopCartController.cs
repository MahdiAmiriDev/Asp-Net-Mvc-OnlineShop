using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DataLayer.ViewModels;
using DataLayer;
namespace MyEshop.Controllers
{
    public class ShopCartController : Controller
    {

        // GET: ShopCart
        public ActionResult ShowCart()
        {
            MyEshop_DataBaseEntities db = new MyEshop_DataBaseEntities();

            List<ShopCartItemViewModel> list = new List<ShopCartItemViewModel>();
            if (Session["ShopCart"] != null)
            {
                List<ShopCartItem> shopList = (List<ShopCartItem>)Session["ShopCart"];
                
                foreach (var item in shopList)
                {
                    var product = db.Products.Where(x => x.ProductID == item.ProductID).Select
                        (p => new
                        {
                            p.ImageName,
                            p.ProductTitle
                        }).Single();
                    list.Add(new ShopCartItemViewModel
                    {
                        ProductID = item.ProductID,
                        Count = item.Count,
                        Image = product.ImageName,
                        Title = product.ProductTitle
                    });
                }
            }

            return PartialView(list);
        }
    }
}