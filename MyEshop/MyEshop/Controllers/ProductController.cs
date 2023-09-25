using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using DataLayer.ViewModels;

namespace MyEshop.Controllers
{
    public class ProductController : Controller
    {
        MyEshop_DataBaseEntities db = new MyEshop_DataBaseEntities();

        public ActionResult ShowGroups()
        {
            var productGroups = db.Product_Groups.ToList();

            return PartialView(productGroups);
        }

        public ActionResult LastProduct()
        {
            var lastProducts = db.Products.OrderByDescending(x => x.CreateDate).Take(12);

            return PartialView(lastProducts);
        }
        [Route("ShowProduct/{id}")]
        public ActionResult ShowProduct(int id)
        {
            var product = db.Products.Find(id);
            ViewBag.ProductFeatures = product.Product_Features.DistinctBy(f => f.FeatureID).Select(f => new ShowProductFearureViewModel()
            {
                FeatureTitle = f.Features.FeatureTitle,
                Values = db.Product_Features.Where(fe => fe.FeatureID == f.FeatureID).Select(fe => fe.Value).ToList()
            }).ToList();
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }
    }
    
}
        
