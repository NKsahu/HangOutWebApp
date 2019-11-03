using System.Collections.Generic;
namespace HangOut.Models.Common
{
    public class Cart
    {
        public System.Int64 CID { get; set; }
        public System.Int64 ItemId { get; set; }
        public int Count { get; set; }
        public System.Int64 OrgId { get; set; }
        public static List<Cart> List { get; set; }

      public  Cart()
        {
            Count = 0;

        }
    }
}