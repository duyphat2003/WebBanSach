using System;
using System.Collections.Generic;
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
            // Đoạn này xử lý xem có manager login không nếu không có manager trùng
            // với trong database sẽ trả về lại login để đăng nhập lại.
            if (Session["Manager"] == null)
            {
                return View("Login");
            }
            else
            {
            var listcate = db.CHUDEs.ToList();
            return View(listcate);
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
    }
}