using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HangOut.Models.POS
{
    public class AddOnn
    {
        public int TitleId { get; set; }
        public string TemplateName { get; set; }
        public string AddOnTitle {get;set;}
        public int Min { get; set; }
       public int Max { get; set; }
        public int AddonCatId { get; set; }// addon category id
      public  List< AddOnItems> AddOnItems { get; set; }
        public AddOnn()
        {
            AddOnItems = new List<AddOnItems>();
        }
    }
    public class AddOns
    {
       public string TemplateName { get; set; }
       public  List<AddOnn> AddonnList { get; set; }
        public AddOns()
        {
            AddonnList = new List<AddOnn>();
        }
    }
}