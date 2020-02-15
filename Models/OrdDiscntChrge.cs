using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HangOut.Models
{
    public class OrdDiscntChrge
    {
      public int   ID { get; set; }
      public string Title { get; set; }
      public Int64 OID { get; set; }
      public int Type { get; set; }// 1 discount, 2 charge
      public double TaxOrAmt { get; set; }
      

    }
}