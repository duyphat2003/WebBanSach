using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanSach.Models;

namespace WebBanSach.Controllers
{
    public class UserController : Controller
    {
        private QLBANSACHEntities database = new QLBANSACHEntities();
        // GET: Users
        [HttpGet]
        public ActionResult Register()
        {
            return View("Login");
        }
        [HttpPost]
        public ActionResult Register(KHACHHANG khachhang)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(khachhang.TenDN))
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống");
                if (string.IsNullOrEmpty(khachhang.Matkhau))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                if (string.IsNullOrEmpty(khachhang.Email))
                    ModelState.AddModelError(string.Empty, "Email không được để trống");
                if (string.IsNullOrEmpty(khachhang.DienthoaiKH))
                    ModelState.AddModelError(string.Empty, "Điện thoại không được để trống");

                //Kiểm tra xem có người nào đã đăng kí với tên đăng nhập này hay chưa
                var _khachhang = database.KHACHHANGs.FirstOrDefault(k => k.TenDN == khachhang.TenDN);
                if (_khachhang != null)
                    ModelState.AddModelError(string.Empty, "Đã có người đăng kí tên này");
                if (ModelState.IsValid)
                {
                    database.KHACHHANGs.Add(khachhang);
                    database.SaveChanges();

                }
                else
                {
                    return View();
                }
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(KHACHHANG khachhang)
        {

            if (ModelState.IsValid)
            {

                if (string.IsNullOrEmpty(khachhang.Matkhau))
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống");

                if (string.IsNullOrEmpty(khachhang.Matkhau))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                if (ModelState.IsValid)
                {
                    //Tim khách hàng có tên đăng nhập và password hợp lệ trong CSDL
                    var _khachhang = database.KHACHHANGs.FirstOrDefault(k => k.TenDN == khachhang.TenDN && k.Matkhau == khachhang.Matkhau);


                    if (_khachhang != null)
                    {
                        ViewBag.ThongBao = "Chúc mừng đăng nhập thành công"; //Lưu vào session


                        Session["Taikhoan"] = _khachhang;
                    }
                    else
                        ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }

            }
            return View();
        }
    }
}