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
        public Int64 TableorSheatOrTaleAwayId { get; set; }// SeatingId
        public Int64 OrgId { get; set; }
        public Int64 OID { get; set; }// order id
        public string ItemUUID { get; set; }
        public static List<Cart> List { get; set; }
        public double ItemPrice { get; set; }
        public int IsAddon { get; set; } //0: no , 1: yes
        public ItemAddon itemAddons { get; set; }
        public Cart()
        {
            OrgId = 0;
            ItemUUID = "";
            ItemPrice = 0.00;
            IsAddon = 0;
        }
    }
    public class ItemAddon{
        public String AddonItemIdCsv { get; set; }
        public List<int> AddonItemId { get; set; }


    }
}