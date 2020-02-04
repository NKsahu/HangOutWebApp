using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HangOut.Models.Feedbk
{
    public class FeedbkItem
    {
       public Int64  ItemID { get; set; }
       public int Rating { get; set; }
       public string Comment { get; set; }
       public int FeedbkFormID { get; set; }
       public int FeedBkID { get; set; }
       public int  ResponseType { get; set; }
       public DateTime CreateOn { get; set; }
       public int CID { get; set; }

    }
}