using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DataLayer.ViewModels;

namespace MyEshop.Controllers
{
    public class ShoppingCartController : ApiController
    {
        // GET: api/ShppingCart
        public int Get()
        {
            List<ShopCartItem> list = new List<ShopCartItem>();

            var sessions = HttpContext.Current.Session;

            if (sessions["ShopCart"] != null)
            {
                list = sessions["ShopCart"] as List<ShopCartItem>;
            }

            return list.Sum(l => l.Count);
        }

        // GET: api/ShppingCart/5
        public int Get(int id)
        {
            List<ShopCartItem> list = new List<ShopCartItem>();

            var sessions = HttpContext.Current.Session;

            if (sessions["ShopCart"] != null)
            {
                list = sessions["ShopCart"] as List<ShopCartItem>;
            }
            if (list.Any(x => x.ProductID == id))
            {
                int index = list.FindIndex(x => x.ProductID == id);
                list[index].Count += 1;
            }
            else
            {
                list.Add(new ShopCartItem
                {
                    ProductID = id,
                    Count = 1
                });
            }

            sessions["ShopCart"] = list;
            return Get();
        }
    }
}
