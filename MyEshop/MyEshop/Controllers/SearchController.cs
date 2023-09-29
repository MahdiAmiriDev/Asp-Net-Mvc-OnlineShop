using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace MyEshop.Controllers
{
    public class SearchController : Controller
    {

        MyEshop_DataBaseEntities db = new MyEshop_DataBaseEntities();

        // GET: Search

        public ActionResult Index(string key)
        {
            List<Products> productList = new List<Products>();

            productList.AddRange(db.Product_Tags.Where(x => x.Tag.Contains(key) )
                                .Select(x => x.Products).ToList());

            productList.AddRange(db.Products.Where(x => x.ProductTitle.Contains(key) ||
                                                        x.ShortDescription.Contains(key) ||
                                                        x.Text.Contains(key)));

            ViewBag.Search = key;

            return View(productList.Distinct());
        }
    }
}