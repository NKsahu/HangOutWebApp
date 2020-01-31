using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HangOut.Models.Common
{
    public class FeedbkForm
    {
       public int Id { get; set; }
       public string Name { get; set; }
       public int  OrgId { get; set; }
       public bool Status { get; set; }
       public DateTime CreateDate { get; set; }
    }
}