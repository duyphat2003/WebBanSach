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
        private QLBANSACHEntities db = new QLBANSACHEntities();

        public ActionResult LoginRegister()
        {
            return View();
        }


        // GET: Users
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(KHACHHANG khachhang)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(khachhang.HoTenKH))
                    ModelState.AddModelError(string.Empty, "Họ tên không được để trống");

                if (string.IsNullOrEmpty(khachhang.DiachiKH))
                    ModelState.AddModelError(string.Empty, "Địa chỉ không được để trống");

                if (string.IsNullOrEmpty(khachhang.DienthoaiKH))
                    ModelState.AddModelError(string.Empty, "Điện thoại không được để trống");

                if (string.IsNullOrEmpty(khachhang.Email))
                    ModelState.AddModelError(string.Empty, "Email không được để trống");

                if (string.IsNullOrEmpty(khachhang.TenDN))
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống");
                
                if (string.IsNullOrEmpty(khachhang.Matkhau))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");

                
                var _khachhang = db.KHACHHANGs.FirstOrDefault(k => k.TenDN == khachhang.TenDN);

                if (_khachhang != null)
                    ModelState.AddModelError(string.Empty, "Đã có người đăng kí tên này");

                var mail = db.KHACHHANGs.FirstOrDefault(k => k.Email == khachhang.Email);
                if (mail != null)
                    ModelState.AddModelError(string.Empty, "Đã có người đăng kí mail này");

                int MaKH = 1;
                while (true)
                {
                    var id = db.KHACHHANGs.FirstOrDefault(k => k.MaKH == MaKH);
                    if (id != null)
                    {
                        MaKH += 1;
                    }
                    else
                        break;
                }
                khachhang.MaKH = MaKH;

                if (ModelState.IsValid)
                {
                    db.KHACHHANGs.Add(khachhang);
                    db.SaveChanges();
                }
                else
                {
                    return View("LoginRegister");
                }
            }
            return View("LoginRegister");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(KHACHHANG khachhang)
        {

            if (ModelState.IsValid)
            {

                if (string.IsNullOrEmpty(khachhang.TenDN))
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống");

                if (string.IsNullOrEmpty(khachhang.Matkhau))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                if (ModelState.IsValid)
                {
                    //Tim khách hàng có tên đăng nhập và password hợp lệ trong CSDL
                    var _khachhang = db.KHACHHANGs.FirstOrDefault(k => k.TenDN == khachhang.TenDN && k.Matkhau == khachhang.Matkhau);

                    if (_khachhang != null)
                    {
                        ViewBag.ThongBao = "Chúc mừng đăng nhập thành công"; //Lưu vào session
                        Session["Taikhoan"] = _khachhang;
                        return RedirectToAction("Home", "TrangChu");
                    }
                    else
                    {
                        ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                        return View("LoginRegister");
                    }
                       
 
                }
            }
            return View("LoginRegister");
        }
    }
}