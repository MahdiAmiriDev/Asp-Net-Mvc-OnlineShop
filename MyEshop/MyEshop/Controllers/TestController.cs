using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEshop.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult Test1()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
        public ActionResult Test2()
        {
            return View();
        }
    }
}