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

        public ActionResult ShowComments(int id)
        {
            var comments = db.Product_Comments.Where(x => x.ProductID == id);

            return PartialView(comments);
        }

        public ActionResult CreateComment(int id)
        {
            return PartialView(new Product_Comments()
            {

                ProductID = id
            });
        }

        [HttpPost]
        public ActionResult CreateComment(Product_Comments productComment)
        {
            if (ModelState.IsValid)
            {
                productComment.CreateDate = DateTime.Now;
                db.Product_Comments.Add(productComment);
                db.SaveChanges();

                return PartialView("ShowComments", db.Product_Comments.Where(x => x.ProductID == productComment.ProductID));
            }

            return PartialView(productComment);
        }

        [Route("Archive")]
        public ActionResult ArchiveProduct(List<int> selectedGroups, int pageId = 1, string title = "", int minPrice = 0, int maxPrice = 0)
        {
            ViewBag.Groups = db.Product_Groups.ToList();
            ViewBag.ProducTitle = title;
            ViewBag.minPrice = minPrice;
            ViewBag.maxPrice = maxPrice;
            ViewBag.selectGroup = selectedGroups;
            ViewBag.pageId = pageId;

            List<Products> productsList = new List<Products>();

            if (selectedGroups != null && selectedGroups.Any())
            {
                foreach (var groupId in selectedGroups)
                {
                    productsList.AddRange(db.Product_Selected_Groups.Where(g => g.GroupID == groupId).Select(x => x.Products).ToList());
                }
                productsList = productsList.Distinct().ToList();
            }
            else
            {
                productsList.AddRange(db.Products.ToList());
            }

            if (!string.IsNullOrEmpty(title))
            {
                productsList = productsList.Where(x => x.ProductTitle.Contains(title)).ToList();
            }
            if (minPrice > 0)
            {
                productsList = productsList.Where(p => p.Price >= minPrice).ToList();
            }
            if (maxPrice > 0)
            {
                productsList = productsList.Where(p => p.Price <= maxPrice).ToList();
            }

            int take = 9;
            int skip = (pageId - 1) * take;
            ViewBag.PageCount = productsList.Count() / take;

            return View(productsList.OrderByDescending(p => p.CreateDate).Skip(skip).Take(take).ToList());
        }
    }
    
}
        
