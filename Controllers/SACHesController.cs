using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebBanSach.Models;
using WebGrease.Css.Extensions;

namespace WebBanSach.Controllers
{
    public class SACHesController : Controller
    {
        private QLBANSACHEntities1 db = new QLBANSACHEntities1();
        public ActionResult SachTheoChuDe()
        {
            var sachs = db.SACHes.Include(s => s.CHUDE).Include(s => s.NHAXUATBAN);     
            return View(sachs.ToList());
        }
        public ActionResult ChonSachQuaChuDe()
        {
            var chude = db.CHUDEs.ToList();
            return PartialView("ChonSachQuaChuDe", chude);
        }

        public ActionResult ChonSachQuaIDChuDe(string num, int id)
        {
            var sachs = db.SACHes.Where(p => p.CHUDE.MaCD == id);
            CHUDE cHUDE = db.CHUDEs.Find(id);
            decimal from = (decimal)0; decimal to = (decimal)int.MaxValue;
            string messageFilter ="";
            if (num == "Default")
            {
                from = (decimal)0;
                to = (decimal)int.MaxValue;
                messageFilter = "Trở lên";
            }
            else if (num == "0")
            {
                from = (decimal)0;
                to = (decimal)150.0000;
            }
            else if (num == "150000")
            {
                from = (decimal)150.0000;
                to = (decimal)300.0000;
            }
            else if (num == "300000")
            {
                from = (decimal)300.0000;
                to = (decimal)500.0000;
            }
            else if(num == "500000")
            {
                from = (decimal)500.0000;
                to = (decimal)700.0000;
            }
            else if(num == "700000")
            {
                from = (decimal)700.0000;
                to = (decimal)int.MaxValue;
                messageFilter = "Trở lên";

            }
            ViewBag.From = from;
            
            if(messageFilter == "Trở lên")
            {
                ViewBag.To = messageFilter;
            }
            else
            {
                ViewBag.To = to;
            }
            ViewBag.Topic = cHUDE.TenChuDe;
            ViewBag.maCD = cHUDE.MaCD;

            return View("SachTheoChuDe", sachs);
        }

        // GET: SACHes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SACH sACH = db.SACHes.Find(id);
            if (sACH == null)
            {
                return HttpNotFound();
            }
            return View(sACH);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
