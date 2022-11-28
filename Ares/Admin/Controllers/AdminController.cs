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
using static System.Net.Mime.MediaTypeNames;

namespace WebBanSach.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        QLBANSACHEntities1 db = new QLBANSACHEntities1();
        // GET: Admin/Admin

        // Xử lý thông báo
        public ActionResult DSThongBao()
        {
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login");    // Trả lại view Login Page
            }
            else
            {
                var listcate = db.THONGBAOs.ToList();    // lấy dữ liệu từ bảng Thông báo trả về kiểu dữ liệu danh sách
                return View(listcate.ToList());   // Trả lại view có dữ liệu đã có được ở trên
            }
        }
        public ActionResult CreateThongBao()
        {
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login");    // Trả lại view Login Page
            }
            else
            {
                return View();  // Trả lại view CreateThongBao
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateThongBao(THONGBAO tHONGBAO, string content)
        {
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login");   // Trả lại view Login Page
            }
            else
            {
                try
                {
                    // Tạo một ID mới không trùng trong bảng thông báo
                    int id = 0;
                    while (true)
                    {
                        var thongbaoID = db.THONGBAOs.FirstOrDefault(k => k.ID == id);
                        if (thongbaoID == null)
                        {
                            break;
                        }
                        else id++;

                    }
                    tHONGBAO.ID = id;
                    tHONGBAO.Content = content; // Content trong bảng thông báo bằng với biến content (biến này lấy dữ liệu từ input)
                    db.THONGBAOs.Add(tHONGBAO);  // Thêm vào bảng thông bảo
                    db.SaveChanges();  // Lưu dữ liệu
                    return RedirectToAction("DSThongBao");  // Hướng đến action method DSThongBao trả lại view của action method này
                }
                catch
                {
                    return Content("ERROR !!!"); // Trả lại view có nội dung Error !!!
                }
            }
        }

        public ActionResult EditThongBao(int id)
        {
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login");  // Trả lại view Login Page
            }
            else
            {
                THONGBAO tHONGBAO = db.THONGBAOs.Find(id); // Tìm thông báo nào có mã bằng với id
                return View(tHONGBAO);  // Trả lại view với dữ liệu của tHONGBAO có mã bằng với id
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditThongBao(int id, string noidung)
        {
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login");   // Trả lại view Login Page
            }
            else
            {
                THONGBAO tHONGBAO = db.THONGBAOs.Find(id);  // Tìm thông báo nào có mã bằng với id
                tHONGBAO.Content = noidung;      // Content trong bảng thông báo bằng với biến noidung (biến này lấy dữ liệu từ input)
                db.SaveChanges();  // Lưu dữ liệu
                return RedirectToAction("DSThongBao"); // Hướng đến action method DSThongBao trả lại view của action method này
            }
        }

        public ActionResult DetailThongBao(int id)
        {
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login");    // Trả lại view Login Page
            }
            THONGBAO tHONGBAO = db.THONGBAOs.Find(id); // Tìm thông báo nào có mã bằng với id
            return View(tHONGBAO);  // Trả lại view với dữ liệu của tHONGBAO có mã bằng với id
        }

        public ActionResult DeleteThongBao(int id)
        {
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login");   // Trả lại view Login Page
            }
            THONGBAO tHONGBAO = db.THONGBAOs.Find(id);  // Tìm thông báo nào có mã bằng với id
            db.THONGBAOs.Remove(tHONGBAO);     // Xóa dữ liệu tHONGBAO
            db.SaveChanges();  // Lưu dữ liệu
            return RedirectToAction("DSThongBao");   // Hướng đến action method DSThongBao trả lại view của action method này
        }

        // Xử lý tác giả
        public ActionResult DSTacGia()
        {
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login");  // Trả lại view Login Page
            }
            else
            {
                var listcate = db.TACGIAs.ToList(); // lấy dữ liệu từ bảng TÁC GIẢ trả về kiểu dữ liệu danh sách
                return View(listcate.ToList());  // Trả lại view có dữ liệu đã có được ở trên
            }
        }
        //Xử lý Thể loại
        public ActionResult DSTheloai()
        {
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login");   // Trả lại view Login Page
            }
            else
            {
                var listcate = db.CHUDEs.ToList();  // lấy dữ liệu từ bảng CHỦ ĐỀ trả về kiểu dữ liệu danh sách
                return View(listcate.ToList());      // Trả lại view có dữ liệu đã có được ở trên
            }
        }
        public ActionResult CreateCD()
        {
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login"); // Trả lại view Login Page
            }
            else
            {
                return View();    // Trả lại view CreateCD
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCD(CHUDE chude)
        {
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login");      // Trả lại view Login Page
            }
            else
            {
                try
                {
                    if (string.IsNullOrEmpty(chude.TenChuDe) || chude.TenChuDe.Length > 50)
                        ModelState.AddModelError(string.Empty, "Vui lòng nhập tên thể loại của sách");
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
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login");  // Trả lại view Login Page
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
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login");    // Trả lại view Login Page
            }
            else
            {
                try
                {
                    CHUDE cHUDE = db.CHUDEs.Find(id);
                    if (string.IsNullOrEmpty(cHUDE.TenChuDe) || cHUDE.TenChuDe.Length > 50)
                        ModelState.AddModelError(string.Empty, "Vui lòng nhập tên thể loại của sách");
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

                 catch
                {
                    return Content("ERROR !!!");
                }
            }

        }

        public ActionResult DetailCD(int id)
        {
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login");  // Trả lại view Login Page
            }
            CHUDE cHUDE = db.CHUDEs.Find(id);
            return View(cHUDE);
        }

        public ActionResult DeleteCD(int id)
        {
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login"); // Trả lại view Login Page
            }
            CHUDE cHUDE = db.CHUDEs.Find(id);

            var sACH = db.SACHes.Where(p => p.MaCD == id);
            foreach(var item in sACH)
            {
                item.MaCD = null;
            }
            db.CHUDEs.Remove(cHUDE);
            db.SaveChanges();
            return RedirectToAction("DSTheloai");
        }

        // Xử lý sách
        public ActionResult DSSach()
        {
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login");  // Trả lại view Login Page
            }
            else
            {
                var listcate = db.SACHes.ToList();   // lấy dữ liệu từ bảng SÁCH trả về kiểu dữ liệu danh sách
                return View(listcate.ToList());      // Trả lại view có dữ liệu đã có được ở trên
            }
        }
        public ActionResult CreateSach()
        {
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login"); // Trả lại view Login Page
            }
            else
            {
                return View();  // Trả lại view CreateSach
            }
        }


        public ActionResult ChuDePartial(int id)
        {
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login");  // Trả lại view Login Page
            }
            var chude = db.CHUDEs;
            if (id != -1)
            {
                CHUDE cHUDE = db.CHUDEs.Find(id);
                ViewBag.Topic = cHUDE.TenChuDe;
            }
            else
                ViewBag.Topic = "none";
            return PartialView(chude);
        }
        public ActionResult NXBPartial(int id)
        {
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login");   // Trả lại view Login Page
            }
            var NXB = db.NHAXUATBANs;
            if (id != -1)
            {
                NHAXUATBAN nHAXUATBAN = db.NHAXUATBANs.Find(id);
                ViewBag.NXB = nHAXUATBAN.TenNXB;
            }
            else
                ViewBag.NXB = "none";
            return PartialView(NXB);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSach(SACH sACH, string content, string tenChuDe, string tenChuDeOption, string tenTG, string tenNXB, string tenNXBOption, DateTime time)
        {
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login");  // Trả lại view Login Page
            }
            else
            {
                try
                {
                    //Xét có dữ liệu nào để null hoặc để trống hay không
                    if (string.IsNullOrEmpty(sACH.Tensach) || sACH.Tensach.Length > 100)
                        ModelState.AddModelError(string.Empty, "Vui lòng nhập tên sách");

                    if (string.IsNullOrEmpty(sACH.Donvitinh) || sACH.Donvitinh.Length > 50)
                        ModelState.AddModelError(string.Empty, "Vui lòng nhập đơn vị tính");

                    if (string.IsNullOrEmpty(sACH.Dongia.ToString()) && sACH.Dongia > 0)
                        ModelState.AddModelError(string.Empty, "Vui lòng nhập giá sách");

                    if (string.IsNullOrEmpty(content))
                        ModelState.AddModelError(string.Empty, "Vui lòng nhập nội dung");

                    if (string.IsNullOrEmpty(sACH.Hinhminhhoa) || sACH.Hinhminhhoa.Length > 50)
                        ModelState.AddModelError(string.Empty, "Vui lòng nhập tên ảnh của sách");

                    if (string.IsNullOrEmpty(tenChuDe) || tenChuDe.Length > 50)
                        ModelState.AddModelError(string.Empty, "Vui lòng nhập tên thể loại của sách");

                    if (string.IsNullOrEmpty(sACH.Soluongban.ToString()) && sACH.Soluongban > 0)
                        ModelState.AddModelError(string.Empty, "Vui lòng nhập giá sách");

                    if (string.IsNullOrEmpty(tenTG) || tenTG.Length > 50)
                        ModelState.AddModelError(string.Empty, "Vui lòng nhập tên tác giả");

                    if (string.IsNullOrEmpty(tenNXB) || tenNXB.Length > 100)
                        ModelState.AddModelError(string.Empty, "Vui lòng nhập tên nhà xuất bản");

                    if (string.IsNullOrEmpty(time.ToString()))
                        ModelState.AddModelError(string.Empty, "Vui lòng nhập ngày cập nhật");

                    sACH.solanxem = 0;
                    sACH.Mota = content;
                    //Tạo ra id của sách, chủ đề, tác giả, nhà xuất bản không trùng với id của sách, chủ đề, tác giả, nhà xuất bản trong database 
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




                    if (tenChuDeOption == "Other")
                    {
                        CHUDE topic = new CHUDE();
                        topic.MaCD = idTopic;
                        var topics = db.CHUDEs.FirstOrDefault(k => k.TenChuDe.ToUpper() == tenChuDe.ToUpper());
                        if (topics == null)
                        {
                            topic.TenChuDe = tenChuDe;
                            sACH.MaCD = idTopic;
                            db.CHUDEs.Add(topic);
                            db.SaveChanges();
                        }
                        else
                        {
                            sACH.MaCD = topics.MaCD;  // Nếu tên chủ đề trùng với tên chủ đề trong bảng CHỦ ĐỀ thì 
                        }
                    }
                    else
                    {
                        var topics = db.CHUDEs.FirstOrDefault(k => k.TenChuDe.ToUpper() == tenChuDeOption.ToUpper());
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

                    if (tenNXBOption == "Other")
                    {
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
                    }
                    else
                    {
                        var NXB = db.NHAXUATBANs.FirstOrDefault(k => k.TenNXB.ToUpper() == tenNXBOption.ToUpper());
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
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login");  // Trả lại view Login Page
            }
            else
            {
                SACH sACH = db.SACHes.Find(id);
                return View(sACH);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSach(int id, string tenSach, string DonViTinh, decimal Gia, string NDSach, string hinhMinhHoa, string tenChuDe, string tenChuDeOption, string tenNXB, string tenNXBOption, DateTime ngayCapNhap, int SLBan, int SLXem, string tenTG)
        {
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login");    // Trả lại view Login Page
            }
            else
            {
                try
                {
                    SACH sACH = db.SACHes.Find(id);
                    if (string.IsNullOrEmpty(tenSach) || tenSach.Length > 100)
                        ModelState.AddModelError(string.Empty, "Trống hoặc vượt quá số lượng");
                    if (string.IsNullOrEmpty(DonViTinh) || DonViTinh.Length > 50)
                        ModelState.AddModelError(string.Empty, "Trống hoặc vượt quá số lượng");
                    if (string.IsNullOrEmpty(hinhMinhHoa) || hinhMinhHoa.Length > 50)
                        ModelState.AddModelError(string.Empty, "Trống hoặc vượt quá số lượng");
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

                        int idTopic = 0;
                        int idNXB = 0;
                        while (true)
                        {
                            var topicID = db.CHUDEs.FirstOrDefault(k => k.MaCD == idTopic);
                            var nxbID = db.NHAXUATBANs.FirstOrDefault(k => k.MaNXB == idNXB);
                            if (nxbID == null && topicID == null)
                            {
                                break;
                            }
                            if (topicID != null)
                            {
                                idTopic++;
                            }
                            if (nxbID != null)
                            {
                                idNXB++;
                            }
                        }
                        if (tenChuDeOption == "Other")
                        {
                            CHUDE topic = new CHUDE();
                            topic.MaCD = idTopic;
                            var topics = db.CHUDEs.FirstOrDefault(k => k.TenChuDe.ToUpper() == tenChuDe.ToUpper());
                            if (topics == null)
                            {
                                topic.TenChuDe = tenChuDe;
                                sACH.MaCD = idTopic;
                                db.CHUDEs.Add(topic);
                                db.SaveChanges();
                            }
                            else
                            {
                                sACH.MaCD = topics.MaCD;
                            }
                        }
                        else
                        {
                            var topics = db.CHUDEs.FirstOrDefault(k => k.TenChuDe.ToUpper() == tenChuDeOption.ToUpper());
                            sACH.MaCD = topics.MaCD;
                        }

                        if (tenNXBOption == "Other")
                        {
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
                        }
                        else
                        {
                            var NXB = db.NHAXUATBANs.FirstOrDefault(k => k.TenNXB.ToUpper() == tenNXBOption.ToUpper());
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
                catch
                {
                    return Content("ERROR !!!");
                }
            }
        }

        public ActionResult DetailSach(int id)
        {
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login");   // Trả lại view Login Page
            }
            SACH sACH = db.SACHes.Find(id);
            return View(sACH);
        }

        public ActionResult DeleteSach(int id)
        {
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login");  // Trả lại view Login Page
            }
            SACH sACH = db.SACHes.Find(id);
            db.SACHes.Remove(sACH);
            db.SaveChanges();
            return RedirectToAction("DSSach");
        }

        // Login, Logout & Register 

        public ActionResult Login()
        {
            return View();   // Trả lại view Login
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

        public ActionResult Error()
        {
            return View();   // Trả lại view Error
        }

        public ActionResult Register()
        {
            if (Session["Manager"] == null)
            {
                return View("Login");        // Trả lại view Login Page
            }
            else
            {
                QUANLY qUANLY = (QUANLY)Session["Manager"];
                string level = qUANLY.VaiTro.ToUpper();
                if (level == "CAO CẤP" || level == "CAO")
                {
                    return View();     // Trả lại view Register
                }
                else
                {
                    return RedirectToAction("Error");
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(QUANLY user)
        {
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login");   // Trả lại view Login Page
            }
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
                   return RedirectToAction("DSTheloai", "Admin");
                }

            }
            return View();
        }

        public ActionResult LogOut()
        {
            Session["Manager"] = null;
            return View("Login");  // Trả lại view Login Page
        }


        public ActionResult CartUsersList()
        {
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login"); // Trả lại view Login Page
            }
            var dth = db.DONDATHANGs;
            return View(dth.ToList());
        }

        //Xử lý quảng cáo

        public ActionResult AdsList()
        {
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login");    // Trả lại view Login Page
            }
            var QC = db.QUANGCAOs;
            return View(QC.ToList());
        }

        public ActionResult CreateAd()
        {
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login"); // Trả lại view Login Page
            }
            else
            {
                return View();    // Trả lại view CreateAd
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAd(QUANGCAO qUANGCAO, DateTime dayStart, DateTime dayEnd)
        {
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login");  // Trả lại view Login Page
            }
            else
            {
                try
                {
                    if (string.IsNullOrEmpty(qUANGCAO.TenCty) || qUANGCAO.TenCty.Length > 200)
                        ModelState.AddModelError(string.Empty, "Trống hoặc vượt quá số lượng");
                    if (string.IsNullOrEmpty(qUANGCAO.Hinhminhhoa) || qUANGCAO.Hinhminhhoa.Length > 20)
                        ModelState.AddModelError(string.Empty, "Trống hoặc vượt quá số lượng");
                    if (String.IsNullOrEmpty(qUANGCAO.Href))
                        qUANGCAO.Href = "#";
                    qUANGCAO.Ngaybatdau = dayStart;
                    qUANGCAO.Ngayhethan = dayEnd;
                    int id = 0;
                    while (true)
                    {
                        var qcID = db.QUANGCAOs.FirstOrDefault(k => k.STT == id);
                        if (qcID == null)
                        {
                            break;
                        }
                        else id++;

                    }
                    qUANGCAO.STT = id;
                    if(String.IsNullOrEmpty(qUANGCAO.TenCty))
                    {
                        qUANGCAO.TenCty = "Unknown";
                    }
                    db.QUANGCAOs.Add(qUANGCAO);
                    db.SaveChanges();
                    return RedirectToAction("AdsList");
                }
                catch
                {
                    return Content("ERROR !!!");
                }
            }
        }

        public ActionResult EditAD(int id)
        {
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login");     // Trả lại view Login Page
            }
            else
            {
                QUANGCAO qc = db.QUANGCAOs.Find(id);
                if (String.IsNullOrEmpty(qc.Href))
                    qc.Href = "#";
                return View(qc);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAD(int id, string tenCT, string hinhMinhHoa, string href, DateTime dayStart, DateTime dayEnd)
        {

            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login");  // Trả lại view Login Page
            }
            else
            {
                try
                {
                    QUANGCAO qc = db.QUANGCAOs.Find(id);
                    if (string.IsNullOrEmpty(tenCT) || tenCT.Length > 200)
                        ModelState.AddModelError(string.Empty, "Trống hoặc vượt quá số lượng");
                    if (string.IsNullOrEmpty(hinhMinhHoa) || hinhMinhHoa.Length > 20)
                        ModelState.AddModelError(string.Empty, "Trống hoặc vượt quá số lượng");
                    qc.TenCty = tenCT;
                    qc.Hinhminhhoa = hinhMinhHoa;

                    if (String.IsNullOrEmpty(qc.Href))
                        href = "#";
                    qc.Href = href;
                    qc.Ngaybatdau = dayStart;
                    qc.Ngayhethan = dayEnd;
                    db.SaveChanges();
                    return RedirectToAction("AdsList");
                }
                catch
                {
                    return Content("ERROR !!!");
                }
            }
        }

        public ActionResult DetailAD(int id)
        {
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login");  // Trả lại view Login Page
            }
            QUANGCAO qc = db.QUANGCAOs.Find(id);
            if (String.IsNullOrEmpty(qc.Href))
                qc.Href = "#";
            return View(qc);
        }

        public ActionResult DeleteAD(int id)
        {
            // Điều kiện kiểm tra có đăng nhập chưa
            if (Session["Manager"] == null)
            {
                return View("Login");  // Trả lại view Login Page
            }
            QUANGCAO qc = db.QUANGCAOs.Find(id);
            db.QUANGCAOs.Remove(qc);
            db.SaveChanges();
            return RedirectToAction("AdsList");
        }
    }
}