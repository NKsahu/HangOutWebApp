using System.Web.Mvc;
using HangOut.Models;
using HangOut.Models.Common;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace HangOut.Controllers
{
    public class WebApiController : Controller
    {
        [HttpPost]
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
        [HttpPost]
        public JObject PostRegistration(string Obj)
        {

            vw_HG_UsersDetails Objuser = Newtonsoft.Json.JsonConvert.DeserializeObject<vw_HG_UsersDetails>(Obj);

            vw_HG_UsersDetails Exist = Objuser.Checkvw_HG_UsersDetails();
            if (Exist != null)
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
            JObject objParams = JObject.Parse(Obj);
            System.Int64 CID = System.Int64.Parse(objParams.GetValue("CID").ToString());
            JArray jarrayObj = new JArray();

            List<HG_Items> ListItems = new HG_Items().GetAll();
            List<Cart> cartlist = Cart.List.FindAll(x => x.CID == CID);
            foreach (var Items in ListItems)
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
        [HttpPost]
        public string AddCart(string Obj)
        {
            JObject ParaMeters = JObject.Parse(Obj);
            System.Int64 CustID = System.Int64.Parse(ParaMeters["CID"].ToString());
            System.Int64 ItemId = System.Convert.ToInt64(ParaMeters["ItemID"].ToString());
            int Cnt = System.Convert.ToInt32(ParaMeters["Cnt"].ToString());
            int OrgId = System.Convert.ToInt32(ParaMeters["OrgId"].ToString());
            Cart ObjCart = Cart.List.Find(x => x.CID == CustID && x.ItemId == ItemId && x.OrgId == OrgId);
            if (ObjCart != null)
            {
                ObjCart.Count = Cnt;
                Cart.List.RemoveAll(x => x.CID == CustID && x.ItemId == ItemId && x.OrgId == OrgId);
                if (ObjCart.Count != 0)
                    Cart.List.Add(ObjCart);
            }
            else
                Cart.List.Add(new Cart() { CID = CustID, ItemId = ItemId, Count = Cnt, OrgId = OrgId });

            double Amt = 0;
            int Count = 0;


            foreach (Cart CartObj in Cart.List.FindAll(x => x.CID == CustID))
            {
                HG_Items ObjItem = new HG_Items().GetOne((int)CartObj.ItemId);
                Amt += CartObj.Count * ObjItem.Price;
                Count += CartObj.Count;
            }

            if (Cnt == 0)
                return Count + "," + Amt + "," + ItemId;
            return Count + "," + Amt + "," + "0";
        }
        [HttpPost]
        public JObject AddCart(string CID)
        {
            JObject ViewCartItem = new JObject();




            return ViewCartItem;
        }



    }
}
