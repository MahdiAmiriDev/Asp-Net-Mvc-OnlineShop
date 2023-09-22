using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using Utilities;

namespace MyEshop.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private MyEshop_DataBaseEntities db = new MyEshop_DataBaseEntities();

        // GET: Admin/Products
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.Groups = db.Product_Groups.ToList();
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductTitle,ShortDescription,Text,Price,ImageName,CreateDate")] Products products, List<int> selectedGroups, HttpPostedFileBase imageProduct, string tags)
        {
            if (ModelState.IsValid)
            {
                if (selectedGroups == null)
                {
                    ViewBag.ErrorSelectedGroups = true;
                    ViewBag.Groups = db.Product_Groups.ToList();
                    return View(products);
                }

                products.ImageName = "NoImage.png";

                if (imageProduct != null && imageProduct.IsImage())
                {
                    products.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imageProduct.FileName);
                    imageProduct.SaveAs(Server.MapPath("/Images/ProductImages/" + products.ImageName));
                    ImageResizer img = new ImageResizer();
                    img.Resize(Server.MapPath("/Images/ProductImages/" + products.ImageName),
                        Server.MapPath("/Images/ThumbNail/" + products.ImageName));
                }
                products.CreateDate = DateTime.Now;
                db.Products.Add(products);
                foreach (var selectedGroup in selectedGroups)
                {
                    db.Product_Selected_Groups.Add(new Product_Selected_Groups()
                    {
                        ProductID = products.ProductID,
                        GroupID = selectedGroup
                    });
                }
                if (!string.IsNullOrEmpty(tags))
                {
                    string[] tag = tags.Split(',');
                    foreach (var t in tag)
                    {
                        db.Product_Tags.Add(new Product_Tags()
                        {
                            ProductID = products.ProductID,
                            Tag = t.Trim()
                        });
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Groups = db.Product_Groups.ToList();
            return View(products);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            ViewBag.SelectedGroups = products.Product_Selected_Groups.ToList();
            ViewBag.Groups = db.Product_Groups.ToList();
            ViewBag.Tags = string.Join(",", products.Product_Tags.Select(x => x.Tag).ToList());
            return View(products);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductTitle,ShortDescription,Text,Price,ImageName,CreateDate")] Products products, List<int> selectedGroups, HttpPostedFileBase imageProduct, string tags)
        {
            if (ModelState.IsValid)
            {
                if (imageProduct != null && imageProduct.IsImage())
                {
                    if (products.ImageName != "NoImage.png")
                    {
                        System.IO.File.Delete(Server.MapPath("/Images/ProductImages/" + products.ImageName));
                        System.IO.File.Delete(Server.MapPath("/Images/ThumbNail/" + products.ImageName));
                    }

                    products.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imageProduct.FileName);
                    imageProduct.SaveAs(Server.MapPath("/Images/ProductImages/" + products.ImageName));
                    ImageResizer img = new ImageResizer();
                    img.Resize(Server.MapPath("/Images/ProductImages/" + products.ImageName),
                        Server.MapPath("/Images/ThumbNail/" + products.ImageName));
                }

                db.Entry(products).State = EntityState.Modified;

                db.Product_Tags.Where(x => x.ProductID == products.ProductID).ToList()
                    .ForEach(x => db.Product_Tags.Remove(x));

                if (!string.IsNullOrEmpty(tags))
                {
                    string[] tag = tags.Split(',');
                    foreach (var t in tag)
                    {
                        db.Product_Tags.Add(new Product_Tags()
                        {
                            ProductID = products.ProductID,
                            Tag = t.Trim()
                        });
                    }
                }

                db.Product_Selected_Groups.Where(x => x.ProductID == products.ProductID).ToList()
                    .ForEach(x => db.Product_Selected_Groups.Remove(x));

                foreach (var selectedGroup in selectedGroups)
                {
                    db.Product_Selected_Groups.Add(new Product_Selected_Groups()
                    {
                        ProductID = products.ProductID,
                        GroupID = selectedGroup
                    });
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SelectedGroups = selectedGroups;
            ViewBag.Groups = db.Product_Groups.ToList();
            ViewBag.Tags = tags;
            return View(products);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = db.Products.Find(id);
            db.Products.Remove(products);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Gallery(int id)
        {
            ViewBag.Galleries = db.Product_Galleries.Where(x => x.ProductID == id).ToList();

            var productGallery = new Product_Galleries
            {
                ProductID = id
            };

            return View(productGallery);
        }

        [HttpPost]
        public ActionResult Gallery(Product_Galleries galleries, HttpPostedFileBase uploadedImage)
        {
            if (ModelState.IsValid)
            {
                if(uploadedImage != null && uploadedImage.IsImage())
                {
                    galleries.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(uploadedImage.FileName);
                    uploadedImage.SaveAs(Server.MapPath("/Images/ProductImages/"+galleries.ImageName));
                    ImageResizer img = new ImageResizer();
                    img.Resize(Server.MapPath("/Images/ProductImages/" + galleries.ImageName), Server.MapPath("/Images/ThumbNail/" + galleries.ImageName));
                    db.Product_Galleries.Add(galleries);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Gallery", new {id = galleries.ProductID});
        }

        public ActionResult DeleteGallaery(int id)
        {
            var gallary = db.Product_Galleries.Find(id);

            System.IO.File.Delete(Server.MapPath("/Images/ProductImages" + gallary.ImageName));
            System.IO.File.Delete(Server.MapPath("/Images/ThumbNail" + gallary.ImageName));


            db.Product_Galleries.Remove(gallary);
            db.SaveChanges();
            return RedirectToAction("Gallery", "Products",new { id=gallary.ProductID});
        }

        public ActionResult ProductsFeaturs(int id)
        {
            ViewBag.Featurs = db.Product_Features.Where(x => x.ProductID == id).ToList();

            ViewBag.FeatureID = new SelectList(db.Features, "FeatureID", "FeatureTitle");

            var productId = new Product_Features
            {
                ProductID = id
            };

            return View(productId);
        }

        [HttpPost]
        public ActionResult ProductsFeaturs(Product_Features productFeatures)
        {
            if (ModelState.IsValid)
            {
                db.Product_Features.Add(productFeatures);
                db.SaveChanges();
            }

            return RedirectToAction("ProductsFeaturs", new {id = productFeatures.ProductID });
        }

        public void DeleteFeature(int id)
        {
            var feature = db.Product_Features.Find(id);
            db.Product_Features.Remove(feature);
            db.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
