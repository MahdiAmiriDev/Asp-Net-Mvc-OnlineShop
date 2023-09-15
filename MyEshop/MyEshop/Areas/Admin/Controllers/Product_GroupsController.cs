using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace MyEshop.Areas.Admin.Controllers
{
    public class Product_GroupsController : Controller
    {
        private MyEshop_DataBaseEntities db = new MyEshop_DataBaseEntities();

        // GET: Admin/Product_Groups
        public ActionResult Index()
        {
            var product_Groups = db.Product_Groups.Where(g => g.ParentID == null);
            return View(product_Groups.ToList());
        }

        // GET: Admin/Product_Groups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product_Groups product_Groups = db.Product_Groups.Find(id);
            if (product_Groups == null)
            {
                return HttpNotFound();
            }
            return View(product_Groups);
        }

        // GET: Admin/Product_Groups/Create
        public ActionResult Create()
        {
            ViewBag.ParentID = new SelectList(db.Product_Groups, "GroupID", "GroupTitle");
            return View();
        }

        // POST: Admin/Product_Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GroupID,GroupTitle,ParentID")] Product_Groups product_Groups)
        {
            if (ModelState.IsValid)
            {
                db.Product_Groups.Add(product_Groups);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ParentID = new SelectList(db.Product_Groups, "GroupID", "GroupTitle", product_Groups.ParentID);
            return View(product_Groups);
        }

        // GET: Admin/Product_Groups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product_Groups product_Groups = db.Product_Groups.Find(id);
            if (product_Groups == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentID = new SelectList(db.Product_Groups, "GroupID", "GroupTitle", product_Groups.ParentID);
            return View(product_Groups);
        }

        // POST: Admin/Product_Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GroupID,GroupTitle,ParentID")] Product_Groups product_Groups)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product_Groups).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParentID = new SelectList(db.Product_Groups, "GroupID", "GroupTitle", product_Groups.ParentID);
            return View(product_Groups);
        }

        // GET: Admin/Product_Groups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product_Groups product_Groups = db.Product_Groups.Find(id);
            if (product_Groups == null)
            {
                return HttpNotFound();
            }
            return View(product_Groups);
        }

        // POST: Admin/Product_Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product_Groups product_Groups = db.Product_Groups.Find(id);
            db.Product_Groups.Remove(product_Groups);
            db.SaveChanges();
            return RedirectToAction("Index");
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
