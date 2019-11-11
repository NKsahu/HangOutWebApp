using System.Web.Mvc;
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
            
            List<HG_Category> MenuList = new HG_Category().GetAll(OrgId: OrgId);
            List<HG_Items> ListItems = new HG_Items().GetAll(OrgId);
            List<Cart> cartlist = Cart.List.FindAll(x => x.CID == CID && x.OrgId==OrgId);
            JArray JMenuArray = new JArray();
            foreach(HG_Category menu in MenuList)
            {
                List<HG_Items> ItemListByMenu = ListItems.FindAll(x => x.CategoryID == menu.CategoryID);
                if (ItemListByMenu.Count > 0)
                {
                    JObject JobjMenu = new JObject();
                    JArray jarrayItem = new JArray();
                    JobjMenu.Add("MenuId", menu.CategoryID);
                    JobjMenu.Add("Name", menu.Category);
                    foreach (var Items in ItemListByMenu)
                    {
                        List<Cart> cartCurrentItem = cartlist.FindAll(x => x.ItemId == Items.ItemID);
                        JObject objItem = new JObject();
                        objItem.Add("IID", Items.ItemID);
                        objItem.Add("ItemName", Items.Items);
                        objItem.Add("ItemPrice", Items.Price);
                        objItem.Add("ItemQuntity", Items.Qty);
                        objItem.Add("ItemImage", Items.Image);
                        objItem.Add("ItemCartValue", cartCurrentItem.Sum(x => x.Count));
                        objItem.Add("MenuId", Items.CategoryID);
                        jarrayItem.Add(objItem);
                    }
                    JobjMenu.Add("MenuItemCount", ItemListByMenu.Count);
                    JobjMenu.Add("MenuItems", jarrayItem);
                    JMenuArray.Add(JobjMenu);
                }
            }

          
            return JMenuArray;
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
            HG_Tables_or_Sheat TableRowObj = new HG_Tables_or_Sheat().GetOne(TableId);
            return JObject.FromObject(TableRowObj);
        }
        public JArray CartList(string CID)
        {
            return JArray.FromObject( Cart.List.FindAll(x => x.CID == int.Parse(CID)));
        }
        public JArray StateList()
        {
            List<State> StateList = new State().GetAll();
            return JArray.FromObject(StateList);
        }
         [HttpPost]
        public JArray CityListByStateId(int StateId)
        {
            List<City> citylist = new City().GetAllByState(StateId);
            return JArray.FromObject(citylist);
        }
        [HttpPost]
        public JArray GetTheaterListByCityCode(string CityCode)
        {
            List<HG_OrganizationDetails> listorgonization = new HG_OrganizationDetails().GetAll(2);//2 for get list for theater
            listorgonization = listorgonization.FindAll(x => x.City == CityCode);
            return JArray.FromObject(listorgonization);
                
        }
        [HttpPost]
        public JArray GetScreenListByTheaterCode(int OrgID)
        {
            List<HG_Floor_or_ScreenMaster> listScreen = new HG_Floor_or_ScreenMaster().GetAll(2);//2 for get list for theater
            listScreen = listScreen.FindAll(x => x.OrgID == OrgID);
            return JArray.FromObject(listScreen);
        }
        [HttpPost]
        public JArray GetRowListbyScreenCode(int OrgID)
        {
            List<HG_FloorSide_or_RowName> listFloor = new HG_FloorSide_or_RowName().GetAll(2);//2 get list forlist for row
            listFloor = listFloor.FindAll(x => x.OrgID == OrgID);
            return JArray.FromObject(listFloor);
        }
        [HttpPost]
        public JArray GetSheetNumberBYRowList(int OrgID)
        {
            List<HG_Tables_or_Sheat> listSheet = new HG_Tables_or_Sheat().GetAll(2);// 2 for list of sheets
            listSheet = listSheet.FindAll(x => x.OrgId == OrgID);
            return JArray.FromObject(listSheet);
        }



    }
}
