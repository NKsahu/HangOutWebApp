using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HangOut.Models.POS
{
    public class AddOnItems
    {
        public int AdddOnItemId { get; set; }
        public Int64 ItemId { get; set; }
        public double CostPrice {get;set;}
        public double Tax { get; set; }
        public double Price { get; set; }
        public int AddonID { get; set; }
        //===
        public string Title { get; set; }
    }
}