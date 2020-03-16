using System.Collections.Generic;
using System;
using HangOut.Models.POS;

namespace HangOut.Models.Common
{
    public class Cart
    {
        public Int64 CID { get; set; }
        public int ItemId { get; set; }
        public int Count { get; set; }
        public int OrgId { get; set; }
        public Int64 TableorSheatOrTaleAwayId { get; set; }// SeatingId
        public Int64 OID { get; set; }// order id
        public int AddonAplied { get; set; }//0 : no ,1 :yes ( addon=serving size + addon)
        public static List<Cart> List { get; set; }

        public Cart()
        {
            AddonAplied = 0;
        }
    }
    public class ItemAddon{

        public int ItemId { get; set; }
        public List<AddOnn> addOnns { get; set; }
    }
}