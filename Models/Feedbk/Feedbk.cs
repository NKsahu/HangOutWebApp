using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HangOut.Models.Feedbk
{
    public class Feedbk
    {
       public int  FeedBkId { get; set; }
       public int OrgId { get; set; }
       public Int64 OrderId { get; set; }
       public Int64 SeatingId { get; set; }
       public  int FeedbkFormId { get; set; }
       public DateTime CreateOn { get; set; }
       
    }
}