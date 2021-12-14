using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QLBS.Models
{
    public class Sach
    {
        [Key]
        public string IDSach { get; set; }
        public string TenSach { get; set; }
        public string GiaSach { get; set; }
        public int MaTheLoai { get; set; }
        [ForeignKey("MaTheLoai")]
        public virtual TheLoai  TheLoais { get; set; }
        public int MaTacGia { get; set; }
        [ForeignKey("MaTacGia")]
        public virtual TacGia TacGias { get; set; }
    }
}