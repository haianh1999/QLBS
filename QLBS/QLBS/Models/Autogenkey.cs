using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLBS.Models
{
    public class AutogenKey
    {
        public string generatekey(string id, int number)
        {
            string strkey = "";
            string numPart = "", strPart = "";
            strPart = id.Substring(0, number);
            numPart = id.Substring(number);
            strkey = strPart + (Convert.ToInt32(numPart) + 1);
            return strkey;
        }
    }
}