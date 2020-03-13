using System.Web;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace HangOut.Models.DynamicList
{
    public class OrderType
    {
        public string id { get; set; }
        public string Name { get; set; }
        public static List<OrderType> List;
        public List<OrderType> OrgTypeList()
        {
            List<OrderType> list = new List<OrderType>();
            list.Add(new OrderType { id = "1", Name = "Tables" });
            list.Add(new OrderType { id = "2", Name = "Sheats" });
            list.Add(new OrderType { id = "3", Name = "Take Away" });
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
        public static  List<JObject> PaymentType()
        {
            List<JObject> list = new List<JObject>();
            JObject jobj = new JObject();
            jobj.Add("Id", "1");
            jobj.Add("Name", "Prepaid");
            list.Add(jobj);
            JObject jobj2 = new JObject();
            jobj2.Add("Id", "2");
            jobj2.Add("Name", "PostPaid");
            list.Add(jobj2);
            return list;
        }
        public static HttpCookie BrwserCookie()
        {
            var UserInfo= HttpContext.Current.Request.Cookies["UserInfo"];
            return UserInfo;
        }
        public static int CurrOrgId()
        {
            int OrgId =int.Parse(BrwserCookie()["OrgId"]);
            return OrgId;
        }
       
    }
   

}