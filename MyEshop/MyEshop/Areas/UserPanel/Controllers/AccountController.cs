using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.ViewModels;
using DataLayer;
using System.Web.Security;

namespace MyEshop.Areas.UserPanel.Controllers
{
    public class AccountController : Controller
    {
        MyEshop_DataBaseEntities db = new MyEshop_DataBaseEntities();
            
        // GET: UserPanel/Account
        public ActionResult ChangePassword()
        {
            return View(); 
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel changePassword)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Single(u => u.UserName == User.Identity.Name);

                string oldPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(changePassword.OldPassword,"MD5");

                if(user.Password == oldPassword)
                {
                    string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(changePassword.Password, "MD5");
                    user.Password = hashedPassword;
                    db.SaveChanges();
                    ViewBag.Success = true;
                }
                else
                {
                    ModelState.AddModelError("OldPassword", "کلمه عبور درست نمی باشد");
                }

            }
            return View();
        }
    }
}