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
        private QLBANSACHEntities db = new QLBANSACHEntities();
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
            if (num == "Default")
            {
                from = (decimal)0;
                to = (decimal)int.MaxValue;
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

            }
            ViewBag.From = from;
            ViewBag.To = to;
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

        // GET: SACHes/Create
        public ActionResult Create()
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs, "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs, "MaNXB", "TenNXB");
            return View();
        }

        // POST: SACHes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Masach,Tensach,Donvitinh,Dongia,Mota,Hinhminhhoa,MaCD,MaNXB,Ngaycapnhat,Soluongban,solanxem")] SACH sACH)
        {
            if (ModelState.IsValid)
            {
                db.SACHes.Add(sACH);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaCD = new SelectList(db.CHUDEs, "MaCD", "TenChuDe", sACH.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs, "MaNXB", "TenNXB", sACH.MaNXB);
            return View(sACH);
        }

        // GET: SACHes/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.MaCD = new SelectList(db.CHUDEs, "MaCD", "TenChuDe", sACH.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs, "MaNXB", "TenNXB", sACH.MaNXB);
            return View(sACH);
        }

        // POST: SACHes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Masach,Tensach,Donvitinh,Dongia,Mota,Hinhminhhoa,MaCD,MaNXB,Ngaycapnhat,Soluongban,solanxem")] SACH sACH)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sACH).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaCD = new SelectList(db.CHUDEs, "MaCD", "TenChuDe", sACH.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs, "MaNXB", "TenNXB", sACH.MaNXB);
            return View(sACH);
        }

        // GET: SACHes/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: SACHes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SACH sACH = db.SACHes.Find(id);
            db.SACHes.Remove(sACH);
            db.SaveChanges();
            return RedirectToAction("Index");
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
