using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebBanSach.Models;

namespace WebBanSach.Controllers
{
    public class UserController : Controller
    {
        private QLBANSACHEntities1 db = new QLBANSACHEntities1();
        public ActionResult LoginRegister(string topic)
        {
            if (Session["Taikhoan"] != null)
            {
                if (topic == "AcountInfo")
                {
                    KHACHHANG newUser = (KHACHHANG)Session["Taikhoan"];
                    return RedirectToAction("UserInfo", new { id = newUser.MaKH });
                }
                else if (topic == "CartInfo")
                {
                    KHACHHANG newUser = (KHACHHANG)Session["Taikhoan"];
                    return RedirectToAction("UserCart", new { id = newUser.MaKH });
                }
                else if(topic == "NofiContent")
                {
                    KHACHHANG newUser = (KHACHHANG)Session["Taikhoan"];
                    return RedirectToAction("UserNofi", new { id = newUser.MaKH });
                }
            }
            return View();
        }


        public ActionResult LoginRegisterPartial()
        {
            if (Session["Taikhoan"] != null)
            {
                KHACHHANG newUser = (KHACHHANG)Session["Taikhoan"];
                ViewBag.Account = newUser.HoTenKH;
            }
            else
                ViewBag.Account = "TÀI KHOẢN";

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditInfo(KHACHHANG khachhang, int id)
        {
            KHACHHANG myAccount = db.KHACHHANGs.Find(id);
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

                if (string.IsNullOrEmpty(khachhang.Matkhau))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");

                var mail = db.KHACHHANGs.FirstOrDefault(k => k.Email == myAccount.Email && k.MaKH != myAccount.MaKH);
                if (mail != null)
                    ModelState.AddModelError(string.Empty, "Đã có người đăng kí mail này");


                if (ModelState.IsValid)
                {
                    myAccount.HoTenKH = khachhang.HoTenKH;
                    myAccount.DiachiKH = khachhang.DiachiKH;  
                    myAccount.DienthoaiKH = khachhang.DienthoaiKH;  
                    myAccount.Email = khachhang.Email;  
                    myAccount.Matkhau = khachhang.Matkhau;
                    db.SaveChanges();
                }
                else
                {
                    return View("LoginRegister");
                }
            }

            return RedirectToAction("UserInfo", new { id = myAccount.MaKH });
        }

        public ActionResult UserInfo(int id)
        {
            KHACHHANG myAccount = db.KHACHHANGs.Find(id);
            ViewBag.Title = "THÔNG TIN TÀI KHOẢN";
            return View(myAccount);
        }

        public ActionResult UserCart(int id)
        {
            var myAccount = db.DONDATHANGs.Where(p => p.MaKH == id);
            ViewBag.Title = "ĐƠN ĐẶT HÀNG";
            return View(myAccount.ToList());
        }


        public ActionResult UserNofi()
        {
            var myAccount = db.THONGBAOs;
       
            ViewBag.Title = "THÔNG BÁO";
            return View(myAccount.ToList());
        }

        public ActionResult LogOut()
        {
            Session["Taikhoan"] = null;
            Session["GioHang"] = null;
            return RedirectToAction("Home", "TrangChu");
        }


        // GET: Users
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(KHACHHANG khachhang)
        {
            Session["GioHang"] = null;
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(khachhang.HoTenKH) )
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

                if (ModelState.IsValid)
                {
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
            Session["GioHang"] = null;
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
                        return RedirectToAction("UserInfo", new {id = _khachhang.MaKH});
                    }
                    else
                    {
                        ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                        return View("LoginRegister");
                    }
                       
 
                }
            }
            return RedirectToAction("Home", "TrangChu");
        }
    }
}