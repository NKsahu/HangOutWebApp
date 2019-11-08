﻿using System.Web.Mvc;
using HangOut.Models;
using HangOut.Models.Common;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;

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
            System.Int32 OrgId = System.Int32.Parse(objParams.GetValue("OID").ToString());
            JArray jarrayObj = new JArray();
            List<HG_Items> ListItems = new HG_Items().GetAll(OrgId);
            List<Cart> cartlist = Cart.List.FindAll(x => x.CID == CID && x.OrgId==OrgId);
            foreach (var Items in ListItems)
            {
              List<Cart> cartCurrentItem=  cartlist.FindAll(x => x.ItemId == Items.ItemID);
                JObject objItem = new JObject();
                objItem.Add("IID", Items.ItemID);
                objItem.Add("ItemName", Items.Items);
                objItem.Add("ItemPrice", Items.Price);
                objItem.Add("ItemQuntity", Items.Qty);
                objItem.Add("ItemImage", Items.Image);
                objItem.Add("ItemCartValue", cartCurrentItem.Sum(x=>x.Count));
                objItem.Add("MenuId", Items.CategoryID);
                jarrayObj.Add(objItem);
            }
            return jarrayObj;
        }
        [HttpPost]
        public string AddCart(string Obj)
        {
            JObject ParaMeters = JObject.Parse(Obj);
            System.Int64 CustID = System.Int64.Parse(ParaMeters["CID"].ToString());
            System.Int64 ItemId = System.Convert.ToInt64(ParaMeters["ItemId"].ToString());
            int Cnt = System.Convert.ToInt32(ParaMeters["Cnt"].ToString());
            int OrgId = System.Convert.ToInt32(ParaMeters["OID"].ToString());
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
        public JObject GetCart(string Obj)
        {
            JObject ParaMeters = JObject.Parse(Obj);
            System.Int64 CustID = System.Int64.Parse(ParaMeters["CID"].ToString());
            System.Int32 OrgId = System.Convert.ToInt32(ParaMeters["OID"].ToString());
            double TotalPrice = 0.00;
            List<Cart> CartItems = Cart.List.FindAll(x => x.CID == CustID && x.OrgId==OrgId);
            List<HG_Items> ListItems = new HG_Items().GetAll(OrgId);
            JArray jArray = new JArray();
            foreach (Cart Mycart in CartItems)
            {
                HG_Items Items = ListItems.Find(x => x.ItemID == Mycart.ItemId);
                JObject ObjItem = new JObject();
                ObjItem.Add("IID", Items.ItemID);
                ObjItem.Add("ItemName", Items.Items);
                ObjItem.Add("ItemPrice", Items.Price);
                ObjItem.Add("ItemQuntity", Items.Qty);
                ObjItem.Add("ItemImage", Items.Image);
                ObjItem.Add("ItemCartValue", Mycart.Count);
                TotalPrice += Mycart.Count * Items.Price;
                jArray.Add(ObjItem);

            }
            JObject ViewCartItem = new JObject();
            ViewCartItem.Add("TotalPrice", TotalPrice);
            ViewCartItem.Add("ListGetCart", jArray);
            return ViewCartItem;
        }

        public JArray GetMenulist(string Obj)
        {    
            JObject ParaMeters = JObject.Parse(Obj);
            int OrgId = System.Convert.ToInt32(ParaMeters["OID"].ToString());
            List<HG_Category> listcategory = new HG_Category().GetAll(OrgId:OrgId);
            return  JArray.FromObject(listcategory);
        }

        public JObject ScanRestTable(string Obj)
        {
            JObject ParaMeters = JObject.Parse(Obj);
            System.Int64 TableId =System.Convert.ToInt64(ParaMeters.GetValue("TID").ToString());
            HG_Tables_or_Rows TableRowObj = new HG_Tables_or_Rows().GetOne(TableId);
            return JObject.FromObject(TableRowObj);
        }
        public JArray CartList(string CID)
        {
            return JArray.FromObject( Cart.List.FindAll(x => x.CID == int.Parse(CID)));
        }
        public JArray StateList()
        {
            List<State> list = new State().GetAll();
            return JArray.FromObject(list);
        }
        public JArray CityListByStateId(string Obj)
        {
            JObject ParaMeters = JObject.Parse(Obj);
            int StateId = System.Convert.ToInt32(ParaMeters.GetValue("SID").ToString());
            List<City> citylist = new City().GetAllByState(StateId);
            return JArray.FromObject(citylist);
        }


    }
}
