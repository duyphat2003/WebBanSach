using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebBanSach.Models;

namespace WebBanSach.Controllers
{
    public class CustomerBookController : Controller
    {
        private QLBANSACHEntities db = new QLBANSACHEntities();
        // GET: CustomerBook

        //PAGE ALL SÁCH
        public ActionResult Index()
        {
            var product = db.SACHes;
            return View(product.ToList());
        }
        public ActionResult ChiTiet(int? id)
        {
            if (id == null)
            {
                return new
                HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SACH product = db.SACHes.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // PAGE SÁCH TIỂU THUYẾT
        public ActionResult TieuThuyet()
        {
            var product_TT = db.SACHes.Where(macd => macd.MaCD == 12).ToList();
            return View(product_TT);
        }
        public ActionResult ChiTietTieuThuyet(int? id)
        {
            if (id == null)
            {
                return new
                HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SACH product = db.SACHes.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // PAGE SÁCH LẬP TRÌNH
        public ActionResult LapTrinh()
        {
            var product_LT = db.SACHes.Where(macd => macd.MaCD == 14).ToList();
            return View(product_LT);
        }
        public ActionResult ChiTietLapTrinh(int? id)
        {
            if (id == null)
            {
                return new
                HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SACH product = db.SACHes.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // PAGE TRUYỆN TRANH
        public ActionResult TruyenTranh()
        {
            var product_LT = db.SACHes.Where(macd => macd.MaCD == 13).ToList();
            return View(product_LT);
        }
        public ActionResult ChiTietTruyenTranh(int? id)
        {
            if (id == null)
            {
                return new
                HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SACH product = db.SACHes.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

    }
}