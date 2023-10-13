using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace MyEshop.Controllers
{
    public class HomeController : Controller
    {
        MyEshop_DataBaseEntities db = new MyEshop_DataBaseEntities();

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Slider()
        {
            DateTime dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            return PartialView(db.Slider.Where(s => s.IsActive && s.StartDate <= dateTime && s.EndDate >= dateTime));
        }

        public ActionResult AboutUs()
        {
            return View();
        }
    }
}