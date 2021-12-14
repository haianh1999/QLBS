using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLBS.Models
{
    public class Account
    {
        [Key]
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RoleID { get; set; }
    }
}