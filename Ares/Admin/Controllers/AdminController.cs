using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Parser.SyntaxTree;
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
                    int id = 0;
                    while (true)
                    {
                        var chudeID = db.CHUDEs.FirstOrDefault(k => k.MaCD == id);
                        if (chudeID == null)
                        {
                            break;
                        }
                        else id++;

                    }
                    chude.MaCD =  id;
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

        public ActionResult EditCD(int id)
        {
            if (Session["Manager"] == null)
            {
                return View("Login");
            }
            else
            {
                CHUDE cHUDE = db.CHUDEs.Find(id);
                return View(cHUDE);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCD(int id, string tenChuDe)
        {
            if (Session["Manager"] == null)
            {
                return View("Login");
            }
            else
            {
                CHUDE cHUDE = db.CHUDEs.Find(id);
                var topic = db.CHUDEs.FirstOrDefault(k => k.TenChuDe.ToUpper() == tenChuDe.ToUpper());
                if (topic != null)
                {
                    db.CHUDEs.Remove(cHUDE);
                }
                else
                    cHUDE.TenChuDe = tenChuDe;
                db.SaveChanges();
                return RedirectToAction("DSTheloai");
            }
        }

        public ActionResult DetailCD(int id)
        {
            CHUDE cHUDE = db.CHUDEs.Find(id);
            return View(cHUDE);
        }

        public ActionResult DeleteCD(int id)
        {
            CHUDE cHUDE = db.CHUDEs.Find(id);
            db.CHUDEs.Remove(cHUDE);
            db.SaveChanges();
            return RedirectToAction("DSTheloai");
        }

        // Xử lý sách
        public ActionResult DSSach()
        {
            if (Session["Manager"] == null)
            {
                return View("Login");
            }
            else
            {
                var listcate = db.SACHes.ToList();
                return View(listcate.ToList());
            }
        }
        public ActionResult CreateSach()
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
        public ActionResult CreateSach(SACH sACH, string tenChuDe, string tenTG, string tenNXB)
        {
            if (Session["Manager"] == null)
            {
                return View("Login");
            }
            else
            {
                try
                {
                    if (string.IsNullOrEmpty(sACH.Tensach))
                        ModelState.AddModelError(string.Empty, "Vui lòng nhập tên sách");

                    if (string.IsNullOrEmpty(sACH.Donvitinh))
                        ModelState.AddModelError(string.Empty, "Vui lòng nhập đơn vị tính");

                    if (string.IsNullOrEmpty(sACH.Dongia.ToString()) && sACH.Dongia > 0)
                        ModelState.AddModelError(string.Empty, "Vui lòng nhập giá sách");

                    if (string.IsNullOrEmpty(sACH.Mota))
                        ModelState.AddModelError(string.Empty, "Vui lòng nhập nội dung");

                    if (string.IsNullOrEmpty(sACH.Hinhminhhoa))
                        ModelState.AddModelError(string.Empty, "Vui lòng nhập tên ảnh của sách");

                    if (string.IsNullOrEmpty(tenChuDe))
                        ModelState.AddModelError(string.Empty, "Vui lòng nhập tên thể loại của sách");

                    if (string.IsNullOrEmpty(sACH.Soluongban.ToString()) && sACH.Soluongban > 0)
                        ModelState.AddModelError(string.Empty, "Vui lòng nhập giá sách");

                    if (string.IsNullOrEmpty(tenTG))
                        ModelState.AddModelError(string.Empty, "Vui lòng nhập tên tác giả");

                    if (string.IsNullOrEmpty(tenNXB))
                        ModelState.AddModelError(string.Empty, "Vui lòng nhập tên nhà xuất bản");

                    if (string.IsNullOrEmpty(sACH.Ngaycapnhat.ToString()))
                        ModelState.AddModelError(string.Empty, "Vui lòng nhập ngày cập nhật");

                    sACH.solanxem = 0;

                    int idBook = 0;
                    int idTopic = 0;
                    int idAuthor = 0;
                    int idNXB = 0;
                    while (true)
                    {
                        var sachID = db.SACHes.FirstOrDefault(k => k.Masach == idBook);
                        var topicID = db.CHUDEs.FirstOrDefault(k => k.MaCD == idTopic);
                        var tacgiaID = db.TACGIAs.FirstOrDefault(k => k.MaTG == idAuthor);
                        var nxbID = db.NHAXUATBANs.FirstOrDefault(k => k.MaNXB == idNXB);
                        if (sachID == null && tacgiaID == null && nxbID == null && topicID == null)
                        {
                            break;
                        }
                        if (topicID != null)
                        {
                            idTopic++;
                        }
                        if (sachID != null)
                        {
                            idBook++;
                        }
                        if (tacgiaID != null)
                        {
                            idAuthor++;
                        }
                        if (nxbID != null)
                        {
                            idNXB++;
                        }
                    }

                    CHUDE topic = new CHUDE();
                    topic.MaCD = idTopic;
                    var topics = db.CHUDEs.FirstOrDefault(k => k.TenChuDe.ToUpper() == tenChuDe.ToUpper());
                    if (topics == null)
                    {
                        topic.TenChuDe = tenChuDe;
                        sACH.MaTG = idTopic;
                        db.CHUDEs.Add(topic);
                        db.SaveChanges();
                    }
                    else
                    {
                        sACH.MaCD = topics.MaCD;
                    }

                    TACGIA author = new TACGIA();
                    author.MaTG = idAuthor;              
                    var tacgia = db.TACGIAs.FirstOrDefault(k => k.TenTG.ToUpper() == tenTG.ToUpper());
                    if (tacgia == null)
                    {
                        author.TenTG = tenTG;   
                        sACH.MaTG = idAuthor;
                        db.TACGIAs.Add(author);
                        db.SaveChanges();
                    }
                    else
                    {
                        sACH.MaTG = tacgia.MaTG;
                    }

                    NHAXUATBAN Provider = new NHAXUATBAN();
                    Provider.MaNXB = idNXB;
                    var NXB = db.NHAXUATBANs.FirstOrDefault(k => k.TenNXB.ToUpper() == tenNXB.ToUpper());
                    if (NXB == null)
                    {
                        Provider.TenNXB = tenNXB;
                        sACH.MaNXB = idNXB;
                        db.NHAXUATBANs.Add(Provider);
                        db.SaveChanges();
                    }
                    else
                    {
                        sACH.MaNXB = NXB.MaNXB;
                    }


                    sACH.Masach = idBook;
                    db.SACHes.Add(sACH);   

                    db.SaveChanges();
                    return RedirectToAction("DSSach");
                }
                catch
                {
                    return Content("ERROR !!!");
                }
            }
        }

        public ActionResult EditSach(int id)
        {
            if (Session["Manager"] == null)
            {
                return View("Login");
            }
            else
            {
                SACH sACH = db.SACHes.Find(id);
                return View(sACH);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSach(int id, string tenSach, string DonViTinh, decimal Gia, string NDSach, string hinhMinhHoa, string tenChuDe, string tenNXB, DateTime ngayCapNhap, int SLBan, int SLXem, string tenTG)
        {
            if (Session["Manager"] == null)
            {
                return View("Login");
            }
            else
            {
                SACH sACH = db.SACHes.Find(id);
                var nameSach = db.SACHes.FirstOrDefault(k => k.Tensach.ToUpper() == tenSach.ToUpper() && k.Masach != sACH.Masach);
                if (nameSach != null)
                {
                    db.SACHes.Remove(sACH);
                    db.SaveChanges();
                }
                else
                {
                    sACH.Tensach = tenSach;
                    sACH.Donvitinh = DonViTinh;
                    sACH.Dongia = Gia;
                    sACH.Mota = NDSach;
                    sACH.Hinhminhhoa = hinhMinhHoa;
   
                    var topics = db.CHUDEs.FirstOrDefault(k => k.TenChuDe.ToUpper() == tenChuDe.ToUpper());
                    if (topics == null)
                    {
                        int idTopic = 0;
                        while (true)
                        {
                            var chudeID = db.CHUDEs.FirstOrDefault(k => k.MaCD == idTopic);
                            if (chudeID == null)
                            {
                                break;
                            }
                            else idTopic++;

                        }
                        CHUDE topic = new CHUDE();
                        topic.TenChuDe = tenChuDe;
                        topic.MaCD = idTopic;
                        sACH.MaCD = idTopic;
                        db.CHUDEs.Add(topic);
                        db.SaveChanges();
                    }
                    else
                    {
                        sACH.MaCD = topics.MaCD;
                    }

                    var NXB = db.NHAXUATBANs.FirstOrDefault(k => k.TenNXB.ToUpper() == tenNXB.ToUpper());
                    if (NXB == null)
                    {
                        int idNXB = 0;
                        while (true)
                        {
                            var NXBID = db.NHAXUATBANs.FirstOrDefault(k => k.MaNXB == idNXB);
                            if (NXBID == null)
                            {
                                break;
                            }
                            else idNXB++;

                        }
                        NHAXUATBAN _NXB = new NHAXUATBAN();
                        _NXB.TenNXB = tenNXB;
                        _NXB.MaNXB = idNXB;
                        sACH.MaNXB = idNXB;
                        db.NHAXUATBANs.Add(_NXB);
                        db.SaveChanges();
                    }
                    else
                    {
                        sACH.MaNXB = NXB.MaNXB;
                    }

                    sACH.Ngaycapnhat = ngayCapNhap;
                    sACH.Soluongban = SLBan;
                    sACH.solanxem = SLXem;

                    var author = db.TACGIAs.FirstOrDefault(k => k.TenTG.ToUpper() == tenTG.ToUpper());
                    if (author == null)
                    {
                        int idAuthor = 0;
                        while (true)
                        {
                            var AuthorID = db.TACGIAs.FirstOrDefault(k => k.MaTG == idAuthor);
                            if (AuthorID == null)
                            {
                                break;
                            }
                            else idAuthor++;

                        }
                        TACGIA _Author = new TACGIA();
                        _Author.TenTG = tenNXB;
                        _Author.MaTG = idAuthor;
                        sACH.MaNXB = idAuthor;
                        db.TACGIAs.Add(_Author);
                        db.SaveChanges();
                    }
                    else
                    {
                        sACH.MaTG = author.MaTG;
                    }
                }
                db.SaveChanges();
                return RedirectToAction("DSSach");
            }
        }

        public ActionResult DetailSach(int id)
        {
            SACH sACH = db.SACHes.Find(id);
            return View(sACH);
        }

        public ActionResult DeleteSach(int id)
        {
            SACH sACH = db.SACHes.Find(id);
            db.SACHes.Remove(sACH);
            db.SaveChanges();
            return RedirectToAction("DSSach");
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


        public ActionResult CartUsersList()
        {
            var dth = db.DONDATHANGs;
            return View(dth.ToList());
        }

    }
}