using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEshop.Controllers
{
    public class ManageEmailController : Controller
    {
        
        public ActionResult SendActivationEmail()
        {
            return PartialView();
        }
    }
}