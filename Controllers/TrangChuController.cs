using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebBanSach.Models;

namespace WebBanSach.Controllers
{
    public class TrangChuController : Controller
    {
        private QLBANSACHEntities db = new QLBANSACHEntities();

        public ActionResult Home()
        {
            var sACHes = db.SACHes;
            return View(sACHes.ToList());
        }

        public ActionResult SearchPage(string SearchString)
        {
            var sachs = db.SACHes.Include(s => s.CHUDE).Include(s => s.NHAXUATBAN);
            if (!string.IsNullOrEmpty(SearchString))
            {
                sachs = sachs.Where(s => s.Tensach.ToUpper().Contains(SearchString.ToUpper()));
                ViewBag.SearchString = SearchString;
            }
            return View(sachs.ToList());
        }

        public ActionResult ChiTiet(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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