using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using WebBanSach.Models;

namespace WebBanSach.Controllers
{
    public class TrangChuController : Controller
    {
        private QLBANSACHEntities1 db = new QLBANSACHEntities1();

        public ActionResult Home()
        {
            var sACHes = db.SACHes.Include(s => s.CHUDE).Include(s => s.NHAXUATBAN);
            return View(sACHes.ToList());
        }

        public ActionResult SearchPage(string searchQuery, string num)
        {
            var sachs = db.SACHes.Include(s => s.CHUDE).Include(s => s.NHAXUATBAN);
            if (!string.IsNullOrEmpty(searchQuery))
            {
                sachs = sachs.Where(s => s.Tensach.ToUpper().Contains(searchQuery.ToUpper()));
                decimal from = (decimal)0; decimal to = (decimal)int.MaxValue;
                string messageFilter = "";
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
                else if (num == "500000")
                {
                    from = (decimal)500.0000;
                    to = (decimal)700.0000;
                }
                else if (num == "700000")
                {
                    from = (decimal)700.0000;
                    to = (decimal)int.MaxValue;
                    messageFilter = "Trở lên";

                }
                ViewBag.From = from;

                if (messageFilter == "Trở lên")
                {
                    ViewBag.To = messageFilter;
                }
                else
                {
                    ViewBag.To = to;
                }
                ViewBag.SearchString = searchQuery;
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


        public ActionResult ADPartial()
        {
            var AD = db.QUANGCAOs;
            return PartialView(AD);
        }

    }
}