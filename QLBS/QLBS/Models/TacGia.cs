using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLBS.Models
{
    public class TacGia
    {
        [Key]
        public int MaTacGia { get; set; }
        public string TenTacGia { get; set; }
        public ICollection<Sach> sachs { get; set; }
    }
}