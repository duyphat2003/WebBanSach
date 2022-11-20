using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanSach.Models;

namespace WebBanSach.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        QLBANSACHEntities1 db = new QLBANSACHEntities1();
        // GET: Admin/Admin

        //Xử lý Sách
        public ActionResult DSTheloai()
        {
            if (Session["Manager"] == null)
            {
                return View("Login");
            }
            else
            {
                var listcate = db.CHUDEs.ToList();
                return View(listcate.ToList());
            }
        }

        public ActionResult CreateCD()
        {
            if (Session["Manager"] == null)
            {
                return View("Login");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCD(CHUDE chude)
        {
            if (Session["Manager"] == null)
            {
                return View("Login");
            }
            else
            {
                try
                {
                    db.CHUDEs.Add(chude);
                    db.SaveChanges();
                    return RedirectToAction("DSTheloai");
                }
                catch
                {
                    return Content("ERROR !!!");
                }
            }
        }

        // Login, Logout & Register 

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(QUANLY user)
        {
            if (string.IsNullOrEmpty(user.TenDN))
                ModelState.AddModelError(string.Empty, "Vui lòng nhập tài khoản của bạn");
            if (string.IsNullOrEmpty(user.Matkhau))
                ModelState.AddModelError(string.Empty, "Vui lòng nhập mật khẩu đã được cấp");
            if (ModelState.IsValid)
            {
                var manager = db.QUANLies.FirstOrDefault(k => k.TenDN == user.TenDN && k.Matkhau == user.Matkhau);
                if (manager != null)
                {
                    Session["Manager"] = manager;
                }
                else
                {
                    ViewBag.ThongBao = "Vui lòng nhập lại thông tin chính xác";
                }
            }
            return RedirectToAction("DSTheloai", "Admin");
        }


        public ActionResult Register()
        {
            QUANLY qUANLY = (QUANLY)Session["Manager"];
            string level = qUANLY.VaiTro.ToUpper();
            if(level == "CAO CẤP" || level == "CAO")
            {
                return View();
            }
            else
            {
                return RedirectToAction("DSTheloai", "Admin");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(QUANLY user)
        {
            if (string.IsNullOrEmpty(user.TenDN))
                ModelState.AddModelError(string.Empty, "Vui lòng nhập tài khoản của bạn");
            if (string.IsNullOrEmpty(user.Matkhau))
                ModelState.AddModelError(string.Empty, "Vui lòng nhập mật khẩu");
            if (string.IsNullOrEmpty(user.VaiTro))
                ModelState.AddModelError(string.Empty, "Vui lòng nhập vai trò");
            if (ModelState.IsValid)
            {
                var manager = db.QUANLies.FirstOrDefault(k => k.TenDN == user.TenDN);
                if (manager == null)
                {  
                    int id = 0;
                    while(true)
                    {
                        var managerID = db.QUANLies.FirstOrDefault(k => k.ID == id);
                        if (managerID == null)
                        {
                            break;
                        }
                        else id++;
                        
                    }
                    user.ID = id;
                    db.QUANLies.Add(user);
                    db.SaveChanges();
                   return View("Login");
                }

            }
            return View();
        }

        public ActionResult LogOut()
        {
            Session["Manager"] = null;
            return View("Login");
        }

    }
}