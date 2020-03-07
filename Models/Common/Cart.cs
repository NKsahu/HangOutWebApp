using System.Collections.Generic;
using System;
namespace HangOut.Models.Common
{
    public class Cart
    {
        public Int64 CID { get; set; }
        public Int64 ItemId { get; set; }
        public int Count { get; set; }
        public Int64 OrgId { get; set; }
        public Int64 TableorSheatOrTaleAwayId { get; set; }
        public Int64 OID { get; set; }// order id
        public static List<Cart> List { get; set; }

      public  Cart()
        {
           

        }
    }
}