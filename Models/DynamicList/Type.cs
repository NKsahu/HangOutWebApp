using System.Web;
using System.Collections.Generic;


namespace HangOut.Models.DynamicList
{
    public class Type
    {
        public string id { get; set; }
        public string Name { get; set; }
        public static List<Type> List;
        public List<Type> OrgTypeList()
        {
            List<Type> list = new List<Type>();
            list.Add(new Type { id = "1", Name = "Tables" });
            list.Add(new Type { id = "2", Name = "Sheats" });
            list.Add(new Type { id = "3", Name = "Take Away" });
            return list;
        }
        public bool IsAccess()
        {
            var CurrOrgID = HttpContext.Current.Request.Cookies["UserInfo"]["UserType"];

            if (CurrOrgID != "SA" &&CurrOrgID!="A")
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