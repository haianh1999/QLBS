using QLBS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace QLBS.Controllers
{
    public class AccountsController : Controller
    {
        // GET: Accounts
        Encrytion ecry = new Encrytion();
        LTQLDBContext db = new LTQLDBContext();


        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Register(Account acc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    acc.Password = ecry.PassWordEncrytion(acc.Password);
                    db.Accounts.Add(acc);
                    db.SaveChanges();
                    return RedirectToAction("Login", "Accounts");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Tài khoản đã tồn tại");
            }
            return View(acc);
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (CheckSession() == 1)
            {
                return RedirectToAction("Index", "HomeAdmin", new { Area = "Admin" });
            }
            else if (CheckSession() == 2)
            {
                return RedirectToAction("Index", "Products", new { Area = "" });
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(Account acc, string returnUrl)
        {
            try
            {
                if (!string.IsNullOrEmpty(acc.UserName) && !string.IsNullOrEmpty(acc.Password))
                {
                    using (var db = new LTQLDBContext())
                    {
                        var passToMD5 = ecry.PassWordEncrytion(acc.Password);
                        var account = db.Accounts.Where(m => m.UserName.Equals(acc.UserName) && m.Password.Equals(passToMD5)).Count();
                        if (account == 1)
                        {
                            FormsAuthentication.SetAuthCookie(acc.UserName, false);
                            Session["idUser"] = acc.UserName;
                            Session["roleUser"] = acc.RoleID;
                            return RedirectToLocal(returnUrl);
                        }
                        ModelState.AddModelError("", "Thông tin đăng nhập chưa chính xác");
                    }
                }

                ModelState.AddModelError("", "Username and password is required.");
            }
            catch
            {
                ModelState.AddModelError("", "Hệ thống đang được bảo trì, vui lòng liên hệ với quản trị viên");
            }
            return View(acc);
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {

            if (string.IsNullOrEmpty(returnUrl) || returnUrl == "/")
            {
                if (CheckSession() == 1)
                {
                    return RedirectToAction("Index", "HomeAdmin", new { Area = "Admin" });
                }
                else if (CheckSession() == 2)

                {
                    return RedirectToAction("Index", "Products", new { Area = "" });
                }

            }
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["iduser"] = null;
            return RedirectToAction("Login", "Accounts");
        }

        //Kiểm tra người dùng đăng nhập quyền gì
        private int CheckSession()
        {
            using (var db = new LTQLDBContext())
            {
                var user = HttpContext.Session["idUser"];

                if (user != null)
                {
                    var role = db.Accounts.Find(user.ToString()).RoleID;

                    if (role != null)
                    {
                        if (role.ToString() == "Admin")

                        {
                            return 1;
                        }
                        else if (role.ToString() == "client")
                        {
                            return 2;
                        }
                    }
                }
            }
            return 0;
        }

        public ActionResult Change_password(string id)
        {
            Account acc = db.Accounts.Find(id);
            return View(acc);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Change_password(Account acc, FormCollection form)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    acc.Password = ecry.PassWordEncrytion(form["Password"]);
                    db.Entry(acc).State = EntityState.Modified;
                    db.SaveChanges();
                    Response.Write("<script>alert('Data inserted successfully')</script>");
                    return RedirectToAction("Index", "Products");
                }
                catch
                {
                    ModelState.AddModelError("", "Xác nhận mật khẩu không chính xác!!");
                }
            }
            ModelState.AddModelError("", "Xác nhận mật khẩu không chính xác!!");
            return View(acc);
        }

        public ActionResult ShowInfoAcc(string id)
        {
            Account acc = db.Accounts.Find(id);
            return View(acc);
        }


    }
}