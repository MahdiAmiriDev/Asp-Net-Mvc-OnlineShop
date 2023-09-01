using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.ViewModels;
using DataLayer;
using System.Web.Security;

namespace MyEshop.Controllers
{
    public class AccountController : Controller
    {
        MyEshop_DataBaseEntities db = new MyEshop_DataBaseEntities();

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register (RegisterViewModel register)
        {
            if (ModelState.IsValid)
            {
                var ExistsEmail = db.Users.Any(u => u.Emain == register.Email.Trim().ToLower());
                var ExistsUserName = db.Users.Any(u => u.UserName == register.UserName.Trim().ToLower());

                if (ExistsEmail || ExistsUserName)
                {
                    if (ExistsEmail)
                    {
                        ShowRegisterErrorMessage("Email","ایمیل");
                    }
                    if (ExistsUserName)
                    {
                        ShowRegisterErrorMessage("UserName", "نام کاربری");
                    }
                }
                else
                {
                    Users user = new Users()
                    {
                        UserName = register.UserName,
                        Emain = register.Email,
                        IsActive = false,
                        RoleID = 1,
                        RegisterDate = DateTime.Now,
                        Password = FormsAuthentication.HashPasswordForStoringInConfigFile(register.Password, "MD5"),
                        ActiveCode = Guid.NewGuid().ToString()
                    };

                    db.Users.Add(user);
                    db.SaveChanges();
                    return View("SuccessRegister", user);
                }
            }

            return View(register);
        }

        public ActionResult Login()
        {
            return View();
        }

        public void ShowRegisterErrorMessage(string properyName , string propertyNameString)
        {
            ModelState.AddModelError(properyName , $"کاربر گرامی {propertyNameString} وارد شده قبلا در سایت ثبت نام شده است");
        }

    }
}