using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanSach.Models
{
    public class CartItem
    {
        QLBANSACHEntities db = new QLBANSACHEntities();
        public int MaSach { get; set; }
        public string TenSach { get; set; }
        public string ImageFile { get; set; }
        public decimal Price { get; set; }
        public int Number { get; set; }
        public decimal Total()
        {
            return Number * Price;
        }
        public CartItem(int MaSach)
        {
            this.MaSach = MaSach;
            var sachDB = db.SACHes.Single(s => s.Masach == this.MaSach);
            this.TenSach = sachDB.Tensach;
            this.ImageFile = sachDB.Hinhminhhoa;
            this.Price = (decimal)sachDB.Dongia;
            this.Number = 1;
        }
    }
}