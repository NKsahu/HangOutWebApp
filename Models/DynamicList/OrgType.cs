using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HangOut.Models.DynamicList
{
    public class OrgType
    {
        public string id { get; set; }
        public string Name { get; set; }
        public static List<OrgType> List { get; set; }
        public List<OrgType> ListOrgTypeList()
        {
            List<OrgType> list = new List<OrgType>();
            list.Add(new OrgType { id = "1", Name = "Restuarant" });
            list.Add(new OrgType { id = "2", Name = "Theater" });
            return list;
        }
    }
}