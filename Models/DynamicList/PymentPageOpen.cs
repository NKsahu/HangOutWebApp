using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HangOut.Models.DynamicList
{
    public class PymentPageOpen
    {
        public Int64 OID { get; set; }
        public string CheckSum { get; set; }
        public static List<PymentPageOpen> ListPytmPgOpen;
    }
}