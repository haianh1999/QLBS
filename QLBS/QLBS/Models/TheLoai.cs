using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLBS.Models
{
    public class TheLoai
    {
        [Key]
        public int MaTheLoai { get; set; }
        public string TenTheLoai { get; set; }
        public ICollection<Sach> sachs { get; set; }
    }
}