using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanSach.Models;

namespace WebBanSach.Controllers
{
    public class CartController : Controller
    {
        QLBANSACHEntities1 db = new QLBANSACHEntities1();
        public List<CartItem> GetCart()
        {
            List<CartItem> myCart = Session["GioHang"] as List<CartItem>;
            //Nếu giỏ hàng chưa tồn tại thì tạo mới và đưa vào Session
            if (myCart == null)
            {
                myCart = new List<CartItem>();
                Session["GioHang"] = myCart;
            }
            return myCart;
        }

        public ActionResult AddToCart(int id)
        {
            //Lấy giỏ hàng hiện tại
            List<CartItem> myCart = GetCart();
            CartItem currentProduct = myCart.FirstOrDefault(p => p.MaSach == id);
            if (currentProduct == null)
            {
                currentProduct = new CartItem(id);
                myCart.Add(currentProduct);
            }
            else
            {
                currentProduct.Number++; 
            }
            
            return RedirectToAction("GetCartInfo", "Cart", new
            {
                id = id
            });
        }

        private int GetTotalNumber()
        {
            int totalNumber = 0;
            List<CartItem> myCart = GetCart();
            if (myCart != null)
                totalNumber = myCart.Sum(sp => sp.Number);
            return totalNumber;
        }

        private decimal GetTotalPrice()
        {
            decimal totalPrice = 0;
            List<CartItem> myCart = GetCart();
            if (myCart != null)
                totalPrice = myCart.Sum(sp => sp.Total());
            return totalPrice;
        }

        public ActionResult GetCartInfo()
        {
            List<CartItem> myCart = GetCart();
            //Nếu giỏ hàng trống thì trả về trang ban đầu
            if (myCart != null || myCart.Count > 0)
            {
                ViewBag.TotalNumber = GetTotalNumber();
                ViewBag.TotalPrice = GetTotalPrice();
            }
            else
            {
                ViewBag.TotalNumber = 0;
                ViewBag.TotalPrice = 0;
            }

            return View(myCart); 
        }

        public ActionResult CartEmpty()
        {
            return PartialView();
        }

        public ActionResult CartNotEmpty()
        {
            List<CartItem> myCart = GetCart();
            if (myCart != null || myCart.Count > 0)
            {
                ViewBag.TotalNumber = GetTotalNumber();
                ViewBag.TotalPrice = GetTotalPrice();
            }
            return PartialView(myCart);
        }


        public ActionResult CartPartial()
        {
            ViewBag.TotalNumber = GetTotalNumber();
            ViewBag.TotalPrice = GetTotalPrice();

            return PartialView();
        }


        public ActionResult DelProductChosen(int id)
        {
            List<CartItem> myCart = GetCart();
            foreach (CartItem item in myCart)
            {
                if (item.MaSach == id)
                {
                    if (item.Number > 1)
                    {
                        item.Number--;
                        break;
                    }
                    if (item.Number == 1)
                    {
                        myCart.Remove(item);
                        break;
                    }
                }
            }
            ViewBag.TotalNumber = GetTotalNumber();
            ViewBag.TotalPrice = GetTotalPrice();
            return View("GetCartInfo", myCart);
        }


        public ActionResult PaySection(string HTGiaohang, string HTThanhtoan)
        {
            if (Session["Taikhoan"] != null)
            {
                List<CartItem> myCart = GetCart();
                if (myCart == null || myCart.Count == 0)
                {
                    return RedirectToAction("GetCartInfo", "Cart");
                }
                else
                {
                    KHACHHANG newUser = (KHACHHANG)Session["Taikhoan"];
                    int index = 0;
                    while (true)
                    {
                        var soDH = db.CTDATHANGs.FirstOrDefault(p => p.SoDH == index);
                        if (soDH != null)
                        {
                            index++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    foreach (CartItem item in myCart)
                    {
                        CTDATHANG cTDATHANG = new CTDATHANG();
                        cTDATHANG.SoDH = index;
                        cTDATHANG.Masach = item.MaSach;
                        cTDATHANG.Soluong = item.Number;
                        cTDATHANG.Dongia = item.Price;
                        cTDATHANG.Thanhtien = item.Total();
                        db.CTDATHANGs.Add(cTDATHANG);
                    }
                    DONDATHANG dONDATHANG = new DONDATHANG();
                    dONDATHANG.SoDH = index;
                    dONDATHANG.MaKH = newUser.MaKH;
                    dONDATHANG.NgayDH = DateTime.Now;
                    dONDATHANG.Trigia = GetTotalPrice();
                    dONDATHANG.Dagiao = false;
                    DateTime dateTime = DateTime.Now;
                    TimeSpan _newTime = new System.TimeSpan(15, 0, 0, 0);
                    DateTime newTime = dateTime.Add(_newTime);
                    dONDATHANG.Ngaygiaohang = newTime;
                    KHACHHANG cus = db.KHACHHANGs.Find(newUser.MaKH);
                    dONDATHANG.Tennguoinhan = cus.HoTenKH;
                    dONDATHANG.Diachinhan = cus.DiachiKH;
                    dONDATHANG.Dienthoainhan = cus.DienthoaiKH;
                    if (HTGiaohang == "Internet")
                        dONDATHANG.HTGiaohang = true;
                    else
                        dONDATHANG.HTGiaohang = false;

                    if (HTThanhtoan == "Online")
                        dONDATHANG.HTThanhtoan = true;
                    else
                        dONDATHANG.HTThanhtoan = false;
                    db.DONDATHANGs.Add(dONDATHANG);
                    db.SaveChanges();
                    Session["GioHang"] = null;
                    return RedirectToAction("UserCart", "User", new { id = cus.MaKH });
                }
            }
            Session["GioHang"] = null;
            return RedirectToAction("LoginRegister", "User", new { topic = "AcountInfo" });
        }
    }
}