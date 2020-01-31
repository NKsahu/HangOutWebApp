using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HangOut.Models.Common
{
    public class FeedbkObj
    {
        public int id { get; set; }
        public string  Name { get; set; }
        public int ObjectiveType { get; set; }
        public int QuestionId { get; set; }
    }
}