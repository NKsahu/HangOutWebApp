using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HangOut.Models.Inventory
{
    public class INTGSTBL
    {
        public int GSID { get; set; }
        public int CatID { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int Unit { get; set; }
        public double Qty { get; set; }
    }
}