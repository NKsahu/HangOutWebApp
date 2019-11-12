using System.Web;
using System.Collections.Generic;


namespace HangOut.Models.DynamicList
{
    public class OrgType
    {
        public string id { get; set; }
        public string Name { get; set; }
        public static List<OrgType> List;
        public List<OrgType> OrgTypeList()
        {
            List<OrgType> list = new List<OrgType>();
            list.Add(new OrgType { id = "1", Name = "Restaurant" });
            list.Add(new OrgType { id = "2", Name = "Theater" });

            return list;
        }


        public bool IsAccess()
        {
            var CurrOrgID = HttpContext.Current.Request.Cookies["UserInfo"]["UserType"];

            if (CurrOrgID != "SA" &&CurrOrgID!="A" && CurrOrgID != "ONR")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
   

}