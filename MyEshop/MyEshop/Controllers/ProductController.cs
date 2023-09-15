using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;

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

    }
}