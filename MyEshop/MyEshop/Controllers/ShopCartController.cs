using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DataLayer;
using DataLayer.ViewModels;
namespace MyEshop.Controllers
{
    public class ShopCartController : Controller
    {
        MyEshop_DataBaseEntities db = new MyEshop_DataBaseEntities();

        // GET: ShopCart
        public ActionResult ShowCart()
        {
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

        public ActionResult Index()
        {
            return View();
        }

        public List<ShowOrderViewModel> GetListOrder()
        {
            List<ShowOrderViewModel> list = new List<ShowOrderViewModel>();

            if (Session["ShopCart"] != null)
            {
                var listShop = (List<ShopCartItem>)Session["ShopCart"];

                foreach (var item in listShop)
                {
                    var product = db.Products.Where(x => x.ProductID == item.ProductID).Select(p => new
                    {
                        p.ImageName,
                        p.ProductTitle,
                        p.Price
                    }).Single();
                    list.Add(new ShowOrderViewModel()
                    {
                        Count = item.Count,
                        ProductID = item.ProductID,
                        Price = product.Price,
                        Image = product.ImageName,
                        Title = product.ProductTitle,
                        Sum = item.Count * product.Price
                    });
                }

            }
            return list;
        }

        public ActionResult Order()
        {
            return PartialView(GetListOrder());
        }

        public ActionResult OrderCommand(int id, int count)
        {
            List<ShopCartItem> shopList = (List<ShopCartItem>)Session["ShopCart"];

            int index = shopList.FindIndex(x => x.ProductID == id);

            if (count == 0)
            {
                shopList.RemoveAt(index);
            }
            else
            {
                shopList[index].Count = count;
            }
            Session["ShopCart"] = shopList;

            return PartialView("Order", GetListOrder());
        }

        [Authorize]
        public ActionResult Payment()
        {
            int userId = db.Users.Single(u => u.UserName == User.Identity.Name).userID;

            Orders order = new Orders()
            {
                UserID = userId,
                Date = DateTime.Now,
                IsFinaly = false,
            };
            db.Orders.Add(order);

            var listDetails = GetListOrder();
            foreach (var item in listDetails)
            {
                db.OrderDetails.Add(new OrderDetails()
                {
                    Count=item.Count,
                    OrderID = order.OrderID,
                    Price = item.Price,
                    ProductID = item.ProductID,
                });
            }
            db.SaveChanges();
            //TODO : Online Payment
            return null;
        }
    }
}