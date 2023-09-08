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

        [Route("Register")]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Register")]
        [ValidateAntiForgeryToken]
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
                        ShowModelStateErrorMessage("Email","ایمیل شما قبلا در سایت ثبت نام شده است");
                    }
                    if (ExistsUserName)
                    {
                        ShowModelStateErrorMessage("UserName", "نام کاربری شما قبلا در سایت ثبت نام شده است");
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
                    string body = PartialToStringClass.RenderPartialView("ManageEmail", "SendActivationEmail", user);
                    SendEmail.Send(user.Emain, "ایمیل فعالسازی", body);
                    return View("SuccessRegister", user);
                }
            }

            return View(register);
        }

        [Route("Login")]
        //[Route("/Account/Login")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public ActionResult Login( LogInViewModel login , string ReturnUrl="/")
        {
            if (ModelState.IsValid)
            {
                string hashPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(login.Password,"MD5");
                Users user = db.Users.SingleOrDefault(u => u.Emain == login.Email && u.Password == hashPassword);
                if (user != null)
                {
                    if (!user.IsActive)
                    {
                        ShowModelStateErrorMessage("Email", "حساب کاربری شما فعال نشده است");
                        return View();
                    }

                    FormsAuthentication.SetAuthCookie(user.UserName, login.RememberMe);
                    return Redirect(ReturnUrl);

                }
                else
                {
                    ShowModelStateErrorMessage("Email", "کاربری با اطلاعات وارد شده یافت نشد");
                }
            }

            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        public void ShowModelStateErrorMessage(string properyName , string MessageText)
        {
            ModelState.AddModelError(properyName ,MessageText);
        }

        public ActionResult ActiveUser(string id)
        {
            //کاربری که اکتیو کدش برابر  کدی که از سمت ایمیل اومده را پیدا کن اون کاربر رو میاره که یوزر ماست از جنس اینتیتی یوزر
            var user = db.Users.SingleOrDefault(u => u.ActiveCode == id);

            if(user == null)
            {
                return HttpNotFound();
            }

            user.IsActive = true;
            user.ActiveCode = Guid.NewGuid().ToString();
            db.SaveChanges();
            ViewBag.UserName = user.UserName;
            return View();
        }
    }
}