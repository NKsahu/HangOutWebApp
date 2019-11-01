using System.Web.Mvc;
using HangOut.Models;
using HangOut.Models.Common;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace HangOut.Controllers
{
    public class WebApiController : Controller
    {
        public JObject GetLogin(string Obj)
        {

            vw_HG_UsersDetails Objuser = Newtonsoft.Json.JsonConvert.DeserializeObject<vw_HG_UsersDetails>(Obj);

            vw_HG_UsersDetails LoginExist = Objuser.Checkvw_HG_UsersDetails();
            if (LoginExist == null)
            {
                LoginExist = new vw_HG_UsersDetails();
            }
            return JObject.FromObject(LoginExist);

        }

        public JObject PostRegistration(string Obj)
        {

            vw_HG_UsersDetails Objuser = Newtonsoft.Json.JsonConvert.DeserializeObject<vw_HG_UsersDetails>(Obj);

            vw_HG_UsersDetails Exist = Objuser.Checkvw_HG_UsersDetails();
            if(Exist!=null)
            {
                Objuser = new vw_HG_UsersDetails();
                Objuser.UserCode = -1; // ALREADY USER EXIST
            }
            else
            {
                Objuser.UserCode = Objuser.save();
            }


            return JObject.FromObject(Objuser);

        }
        [HttpPost]
        public JArray GetItemList(string Obj)
        {
            JObject objParams = new JObject(Obj);
            JArray jarrayObj = new JArray();
            List<HG_Items> ListItems = new HG_Items().GetAll();
            List<Cart> cartlist = Cart.List.FindAll(x => x.CID == System.Int64.Parse(objParams["CID"].ToString()));
            foreach( var Items in ListItems)
            {
                JObject objItem = new JObject();
                objItem.Add("IID", Items.ItemID);
                objItem.Add("ItemName", Items.Items);
                objItem.Add("ItemPrice", Items.Price);
                objItem.Add("ItemQuntity", Items.Qty);
                objItem.Add("ItemImage", Items.Image);
                objItem.Add("ItemCartValue", cartlist.FindAll(x => x.ItemId == Items.ItemID).Count);
                jarrayObj.Add(objItem);
            }
           return jarrayObj;
        }

    }
}
