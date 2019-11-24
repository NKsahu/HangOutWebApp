using System.Collections.Generic;
namespace HangOut.Models.Common
{
    public class Cart
    {
        public System.Int64 CID { get; set; }
        public System.Int64 ItemId { get; set; }
        public int Count { get; set; }
        public System.Int64 OrgId { get; set; }
        public System.Int64 TableorSheatOrTaleAwayId { get; set; }
        public System.Int64 OID { get; set; }// order id
        public static List<Cart> List { get; set; }

      public  Cart()
        {
           

        }
    }
}