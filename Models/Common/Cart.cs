﻿using System.Collections.Generic;
using System;
using HangOut.Models.POS;

namespace HangOut.Models.Common
{
    public class Cart
    {
        public Int64 CID { get; set; }
        public int ItemId { get; set; }
        public int Count { get; set; }
        //public int OrgId { get; set; }
        public Int64 TableorSheatOrTaleAwayId { get; set; }// SeatingId
        public Int64 OID { get; set; }// order id
        public string ItemUUID { get; set; }
        public static List<Cart> List { get; set; }
        public double ItemPrice { get; set; }
        public ItemAddon itemAddons { get; set; }
        
    }
    public class ItemAddon{
        public String AddonItemIdCsv { get; set; }
        public List<int> AddonItemId { get; set; }


    }
}