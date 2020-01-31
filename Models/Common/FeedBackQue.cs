using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HangOut.Models.Common
{
    public class FeedBackQue
    {
       public int ID { get; set; }
      public string Title { get; set; }
      public bool Status { get; set; }
      public int QuestionType { get; set; }
    }
}