using System.Web.Mvc;
using HangOut.Models;
using HangOut.Models.Common;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
using Newtonsoft.Json;
using System;
using System.Net;


namespace HangOut.Controllers
{
    public class WebApiController : Controller
    {
        [HttpPost]
        public JObject GetLogin(string Obj)
        {

            vw_HG_UsersDetails Objuser = Newtonsoft.Json.JsonConvert.DeserializeObject<vw_HG_UsersDetails>(Obj);
            vw_HG_UsersDetails ObjuserByMobile = Objuser.MobileAlreadyExist(Objuser.UserId);
            if (ObjuserByMobile.UserCode ==0)
            {
                Objuser.UserCode = -1;
               
            }
            else
            {
                Objuser = Objuser.Checkvw_HG_UsersDetails();
                if (Objuser==null)
                {

                    Objuser = new vw_HG_UsersDetails();
                   
                }
            }
           
            return JObject.FromObject(Objuser);

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
            JArray JMenuArray = new JArray();
            System.Int64 CID = System.Int64.Parse(objParams.GetValue("CID").ToString());
           // System.Int64 OID = System.Int64.Parse(objParams.GetValue("OID").ToString());
            System.Int32 OrgId = System.Int32.Parse(objParams.GetValue("OrgId").ToString());
             
            List<HG_Items> ListItems = new HG_Items().GetAll(OrgId);
            System.Int64 TableSheatTakeWayId = System.Int64.Parse(objParams.GetValue("TSTWID").ToString());
            List<Cart> cartlist = Cart.List.FindAll(x => x.CID == CID && x.OrgId == OrgId && x.TableorSheatOrTaleAwayId == TableSheatTakeWayId);
            HG_Tables_or_Sheat ObjTorS = new HG_Tables_or_Sheat().GetOne(TableSheatTakeWayId);
            if (ObjTorS.Type !="3")// not takeaway
            {
                OrderMenu ObjMenu = OrderMenu.Getone(ObjTorS.OMID);
                List<OrderMenuCategory> ListCategry = OrderMenuCategory.GetAll(ObjMenu.id);
                
                List<OrdMenuCtgItems> ListMenuItems = OrdMenuCtgItems.GetAll(ObjMenu.id);
                ListCategry = ListCategry.FindAll(x => x.Status == true);
                ListCategry = ListCategry.OrderBy(x => x.OrderNo).ToList();
                ListMenuItems = ListMenuItems.FindAll(x => x.Status == true);
                int count = 0;
                foreach (var OrderMenuObj in ListCategry)
                {
                    double MenuItemPrice = 0.00;
                    var OrderMenuItems = ListMenuItems.FindAll(x => x.OrdMenuCatId == OrderMenuObj.id);
                    if (OrderMenuItems.Count > 0)
                    {
                        JObject JobjMenu = new JObject();
                        JArray jarrayItem = new JArray();
                        JobjMenu.Add("MenuId", OrderMenuObj.CategoryId);
                        JobjMenu.Add("Name", OrderMenuObj.DisplayName);
                        JobjMenu.Add("MenuIndex", count++);
                        int ItemiIndex = 0;
                        OrderMenuItems = OrderMenuItems.OrderBy(x => x.OrderNo).ToList();
                        foreach (var MenuItmObj in OrderMenuItems)
                        {
                            var Items = ListItems.Find(x => x.ItemID == MenuItmObj.ItemId);
                            Cart cartCurrentItem = cartlist.Find(x => x.ItemId == Items.ItemID);
                            int CurrCount = cartCurrentItem != null ? cartCurrentItem.Count : 0;
                            JObject objItem = new JObject();
                            objItem.Add("IID", Items.ItemID);
                            objItem.Add("ItemName", Items.Items);
                            objItem.Add("ItemPrice", Items.Price);
                            objItem.Add("ItemQuntity", Items.Qty);
                            objItem.Add("ItemImage", Items.Image);
                            objItem.Add("ItemCartValue", CurrCount);
                            objItem.Add("MenuId", Items.CategoryID);
                            objItem.Add("ItemIndex", ItemiIndex++);
                            objItem.Add("ItemMode", Items.ItemMode);
                            objItem.Add("CostPrice", Items.CostPrice);
                            objItem.Add("Info", Items.ItemDiscription);
                            jarrayItem.Add(objItem);
                            MenuItemPrice += Items.Price * CurrCount;
                        }
                        JobjMenu.Add("MenuItemCount", OrderMenuItems.Count);
                        JobjMenu.Add("MenuItems", jarrayItem);
                        JobjMenu.Add("MenuItmPrice", MenuItemPrice);
                        JMenuArray.Add(JobjMenu);
                    }

                }

            }
            else
            {
                List<HG_Category> MenuList = new HG_Category().GetAll(OrgId: OrgId);
                 
                int count = 0;
                foreach (HG_Category menu in MenuList)
                {
                    double MenuItemPrice = 0.00;
                    List<HG_Items> ItemListByMenu = ListItems.FindAll(x => x.CategoryID == menu.CategoryID);
                    if (ItemListByMenu.Count > 0)
                    {
                        JObject JobjMenu = new JObject();
                        JArray jarrayItem = new JArray();
                        JobjMenu.Add("MenuId", menu.CategoryID);
                        JobjMenu.Add("Name", menu.Category);
                        JobjMenu.Add("MenuIndex", count++);
                        int ItemiIndex = 0;
                        foreach (var Items in ItemListByMenu)
                        {
                            Cart cartCurrentItem = cartlist.Find(x => x.ItemId == Items.ItemID);
                            int CurrCount = cartCurrentItem != null ? cartCurrentItem.Count : 0;
                            JObject objItem = new JObject();
                            objItem.Add("IID", Items.ItemID);
                            objItem.Add("ItemName", Items.Items);
                            objItem.Add("ItemPrice", Items.Price);
                            objItem.Add("ItemQuntity", Items.Qty);
                            objItem.Add("ItemImage", Items.Image);
                            objItem.Add("ItemCartValue", CurrCount);
                            objItem.Add("MenuId", Items.CategoryID);
                            objItem.Add("ItemIndex", ItemiIndex++);
                            objItem.Add("CostPrice", Items.CostPrice);
                            objItem.Add("ItemMode", Items.ItemMode);
                            objItem.Add("Info", Items.ItemDiscription);
                            jarrayItem.Add(objItem);
                            MenuItemPrice += Items.Price * CurrCount;
                        }
                        JobjMenu.Add("MenuItemCount", ItemListByMenu.Count);
                        JobjMenu.Add("MenuItems", jarrayItem);
                        JobjMenu.Add("MenuItmPrice", MenuItemPrice);
                        JMenuArray.Add(JobjMenu);
                    }
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
            int OrgId = System.Convert.ToInt32(ParaMeters["OrgId"].ToString());
           // Int64 OID = System.Convert.ToInt64(ParaMeters["OID"]);
            System.Int64 TableSheatTakeWayId = System.Int64.Parse(ParaMeters.GetValue("TSTWID").ToString());
            Cart ObjCart = Cart.List.Find(x => x.CID == CustID && x.ItemId == ItemId && x.OrgId == OrgId && x.TableorSheatOrTaleAwayId==TableSheatTakeWayId);
            if (ObjCart != null)
            {
                ObjCart.Count = Cnt;
                Cart.List.RemoveAll(x => x.CID == CustID && x.ItemId == ItemId && x.OrgId == OrgId && x.TableorSheatOrTaleAwayId==TableSheatTakeWayId);
                if (ObjCart.Count != 0)
                    Cart.List.Add(ObjCart);
            }
            else
                Cart.List.Add(new Cart() { CID = CustID, ItemId = ItemId, Count = Cnt, OrgId = OrgId ,TableorSheatOrTaleAwayId=TableSheatTakeWayId});
            double Amt = 0;
            int Count = 0;
            List<Cart> CurrItemsOfUser = Cart.List.FindAll(x => x.CID == CustID && x.TableorSheatOrTaleAwayId == TableSheatTakeWayId && x.OrgId == OrgId);
            int CurrCount = CurrItemsOfUser.Count;
            if (CurrCount == 1 || CurrCount == 0)
            {
                HG_Tables_or_Sheat hG_Tables_Or_Sheat = new HG_Tables_or_Sheat().GetOne(TableSheatTakeWayId);
                hG_Tables_Or_Sheat.Status = CurrCount==0?1:3;// Free:Progress(Occupied)
                hG_Tables_Or_Sheat.save();
            }
            foreach (Cart CartObj in CurrItemsOfUser)
            {
                HG_Items ObjItem = new HG_Items().GetOne((int)CartObj.ItemId);
                Amt += CartObj.Count * ObjItem.Price;
                Count += CartObj.Count;
            }
            Cart CurrentItemobj = Cart.List.Find(x => x.CID == CustID && x.ItemId == ItemId && x.OrgId == OrgId && x.TableorSheatOrTaleAwayId == TableSheatTakeWayId);

            if (Cnt == 0)
                return Count + "," + Amt + "," + ItemId+","+"0";
            return Count + "," + Amt + "," + "0"+"," + CurrentItemobj.Count;
        }
        [HttpPost]
        public JObject GetCart(string Obj)
        {
            JObject ParaMeters = JObject.Parse(Obj);
            System.Int64 CustID = System.Int64.Parse(ParaMeters["CID"].ToString());
            System.Int32 OrgId = System.Convert.ToInt32(ParaMeters["OrgId"].ToString());
           // Int64 OID= System.Int64.Parse(ParaMeters["OID"].ToString());
            System.Int64 TableSheatTakeWayId = System.Int64.Parse(ParaMeters.GetValue("TSTWID").ToString());
            HG_OrganizationDetails objOrg = new HG_OrganizationDetails().GetOne(OrgId);
            double TotalPrice = 0.00;
            List<Cart> CartItems = Cart.List.FindAll(x => x.CID == CustID && x.OrgId==OrgId && x.TableorSheatOrTaleAwayId==TableSheatTakeWayId);
            List<HG_Items> ListItems = new HG_Items().GetAll(OrgId);
            JArray jArray = new JArray();
            foreach (Cart Mycart in CartItems)
            {
                HG_Items item = ListItems.Find(x => x.ItemID == Mycart.ItemId);
                JObject ObjItem = new JObject();
                    ObjItem.Add("IID", item.ItemID);
                    ObjItem.Add("ItemName", item.Items);
                    ObjItem.Add("ItemPrice", item.Price);
                    ObjItem.Add("ItemQuntity", item.Qty);
                    ObjItem.Add("ItemImage", item.Image);
                    ObjItem.Add("ItemCartValue", Mycart.Count);
                ObjItem.Add("ItemMode", item.ItemMode);
                    TotalPrice += Mycart.Count * item.Price;
                    jArray.Add(ObjItem);

            }
            JObject ViewCartItem = new JObject();
            ViewCartItem.Add("TotalPrice", TotalPrice);
            ViewCartItem.Add("ListGetCart", jArray);
            ViewCartItem.Add("OrderingStatus", objOrg.CustomerOrdering);
            return ViewCartItem;
        }

        public JArray GetMenulist(string Obj)
        {    
            JObject ParaMeters = JObject.Parse(Obj);
            int OrgId = System.Convert.ToInt32(ParaMeters["OID"].ToString());
            List<HG_Category> listcategory = new HG_Category().GetAll(OrgId:OrgId);
            return  JArray.FromObject(listcategory);
        }
        public JObject OrderMenuList(int Orgid)
        {
            HG_OrganizationDetails orgobj = new HG_OrganizationDetails().GetOne(Orgid);
            int OrgType = orgobj.OrgTypes != null ? int.Parse(orgobj.OrgTypes) : 1;
            List<HG_Floor_or_ScreenMaster> floorOrScreens = new HG_Floor_or_ScreenMaster().GetAll(OrgType);
            List<HG_Tables_or_Sheat> tableOrSheatlist = new HG_Tables_or_Sheat().GetAll(OrgType);
            JObject OrderMenus = new JObject();
            OrderMenus.Add("MenuList",JArray.FromObject(OrderMenu.GetAll(Orgid)));
            JArray jArray = new JArray();
            foreach (HG_Floor_or_ScreenMaster Floors in floorOrScreens)
            {

                JObject jObject = JObject.FromObject(Floors);
                jObject.Add("TableSheatList", JArray.FromObject(tableOrSheatlist.FindAll(x=>x.Floor_or_ScreenId== Floors.Floor_or_ScreenID)));
                jArray.Add(jObject);
            }
            OrderMenus.Add("FloorList", jArray);
            return OrderMenus;
        }
        public JArray ShowOMCategories(int OMId)
        {
            JArray jArray = new JArray();
            if (OMId == 0)
            {
                List<HG_Category> listcategory = new HG_Category().GetAll();
                List<HG_Items> ListItems = new HG_Items().GetAll();
                foreach(var category in listcategory)
                {
                    List<HG_Items> Items = ListItems.FindAll(x=>x.CategoryID==category.CategoryID);
                    if (Items.Count > 0)
                    {
                        JObject jObject = JObject.FromObject(category);
                        jObject.Add("CheckSts", false);
                        jObject.Add("id", 0);
                        jObject.Add("MenuId", 0);
                        jObject.Add("MenuName", " ");
                        JArray itemJarray = new JArray();
                        foreach (var item in Items)
                        {
                           JObject ItmjObject = JObject.FromObject(item);
                            ItmjObject.Add("id", 0);
                            ItmjObject.Add("CheckSts",false);
                            itemJarray.Add(ItmjObject);
                        }
                        jObject.Add("ItemList", itemJarray);
                        jArray.Add(jObject);
                    }
                    
                }


            }
            else
            {
               List<OrderMenu> orderMenulist = OrderMenu.GetAll();
                OrderMenu obj = orderMenulist.Find(x => x.id == OMId);
                List<OrderMenuCategory> orderMenuCategories = OrderMenuCategory.GetAll(obj.id);
                List<OrdMenuCtgItems> ListCatItems = OrdMenuCtgItems.GetAll(obj.id);
                orderMenuCategories = orderMenuCategories.OrderBy(x => x.OrderNo).ToList();
                List<HG_Items> ListOfItem = new HG_Items().GetAll();
                List<HG_Category> categoryList = new HG_Category().GetAll();
                HashSet<int> CategoryHash = new HashSet<int>(orderMenuCategories.Select(x => x.CategoryId).ToArray());
                foreach (var ordecategory in orderMenuCategories)
                {
                    JObject jobj = new JObject();
                    jobj.Add("id", ordecategory.id);
                    jobj.Add("CategoryID", ordecategory.CategoryId);
                    jobj.Add("Category", ordecategory.DisplayName);
                    jobj.Add("CheckSts", ordecategory.Status);
                    jobj.Add("MenuId", obj.id);
                    jobj.Add("MenuName",obj.MenuName);
                    var ListCateItems = ListCatItems.FindAll(x => x.OrdMenuCatId == ordecategory.id);
                    ListCateItems = ListCateItems.OrderBy(x => x.OrderNo).ToList();
                    JArray OrderCategoryItems = new JArray();
                    foreach(var Categoryitm in ListCateItems)
                    {
                        var Item = ListOfItem.Find(x => x.ItemID == Categoryitm.ItemId);
                        JObject itemObj = JObject.FromObject(Item);
                        itemObj.Add("CheckSts", Categoryitm.Status);
                        itemObj.Add("id", Categoryitm.id);
                        OrderCategoryItems.Add(itemObj);
                        //  list[i].CategoryID + '" value="' + list[i].Category + '
                        //id  ItemList[j].ItemID   ItemList[j].Items  CheckSts
                    }
                    HashSet<Int64> SelectedItms = new HashSet<Int64>(ListCateItems.Select(x => x.ItemId).ToArray());
                    var UnSelectedItmList = ListOfItem.FindAll(x =>!SelectedItms.Contains(x.ItemID)&& x.CategoryID== ordecategory.CategoryId);
                    foreach (var Item in UnSelectedItmList)
                    {
                      //  var Item = ListOfItem.Find(x => x.ItemID == Categoryitm.ItemId);
                        JObject itemObj = JObject.FromObject(Item);
                        itemObj.Add("CheckSts", false);
                        itemObj.Add("id", 0);
                        OrderCategoryItems.Add(itemObj);
                        //  list[i].CategoryID + '" value="' + list[i].Category + '
                        //id  ItemList[j].ItemID   ItemList[j].Items  CheckSts
                    }

                    jobj.Add("ItemList", OrderCategoryItems);
                    jArray.Add(jobj);
                }

                categoryList = categoryList.FindAll(x => !CategoryHash.Contains(x.CategoryID));
                foreach (var ordecategory in categoryList)
                {
                    JObject jobj = new JObject();
                    jobj.Add("id", 0);
                    jobj.Add("CategoryID", ordecategory.CategoryID);
                    jobj.Add("Category", ordecategory.Category);
                    jobj.Add("CheckSts", false);
                    jobj.Add("MenuId", obj.id);
                    jobj.Add("MenuName", obj.MenuName);
                    JArray OrderCategoryItems = new JArray();
                    var ItemList = ListOfItem.FindAll(x => x.CategoryID == ordecategory.CategoryID);
                    foreach (var Item in ItemList)
                    {
                        //  var Item = ListOfItem.Find(x => x.ItemID == Categoryitm.ItemId);
                        JObject itemObj = JObject.FromObject(Item);
                        itemObj.Add("CheckSts", false);
                        itemObj.Add("id", 0);
                        OrderCategoryItems.Add(itemObj);
                        //  list[i].CategoryID + '" value="' + list[i].Category + '
                        //id  ItemList[j].ItemID   ItemList[j].Items  CheckSts
                    }
                    if (ItemList.Count > 0)
                    {
                        jobj.Add("ItemList", OrderCategoryItems);
                        jArray.Add(jobj);

                    }
                   
                }


            }
            return jArray;
        }
        [HttpPost]
        public int SaveOrderMenu([System.Web.Http.FromBody] OrderMenu ordermenu)
        {
          // OrderMenu orderMenu = JsonConvert.DeserializeObject<OrderMenu>(objMenu);
          
                ordermenu.id = ordermenu.save();

                foreach(OrderMenuCategory orderMenuCategory in ordermenu.OderMenuCategry)
                {

                    orderMenuCategory.OrderMenuid = ordermenu.id;
                    orderMenuCategory.id = orderMenuCategory.save();
                    foreach(var OrderItem in orderMenuCategory.OrdCatItems)
                    {
                        OrderItem.OderMenuId = ordermenu.id;
                        OrderItem.OrdMenuCatId = orderMenuCategory.id;
                        OrderItem.id = OrderItem.save();
                    }

                }
            return ordermenu.id;
        }
        public int ActiveMenu([System.Web.Http.FromBody] ActiveMenu activeMenu)
        {
            //var Jobj = { };
            //Jobj.OMID = MenuId;
            //Jobj.TorSIDs = TableList;
            //Jobj.OrgId = OrgId;
            int status = 0;
            int MenuId = activeMenu.OMID;
            int OrgId = activeMenu.OrgId;
            HG_OrganizationDetails hG_OrganizationDetails = new HG_OrganizationDetails().GetOne(OrgId);
            string OrgType = hG_OrganizationDetails.OrgTypes !=null ? hG_OrganizationDetails.OrgTypes : "1";
            List <OrderMenu> orderMenulist = OrderMenu.GetAll();
            OrderMenu orderMenu = orderMenulist.Find(x => x.id == MenuId);
            orderMenu.Status = true;
            orderMenu.save();
            List<HG_Tables_or_Sheat> TorSlist = new HG_Tables_or_Sheat().GetAll(int.Parse(OrgType));
            var AlreadySelectedList = TorSlist.FindAll(x => x.OMID == MenuId);
            Int64[] items = activeMenu.TorSIDs;
            HashSet<Int64> hashKeys = new HashSet<Int64>(items);
            var RemovedTorSList = AlreadySelectedList.FindAll(x => !hashKeys.Contains(x.Table_or_RowID));
            List<HG_Tables_or_Sheat>  OnlyApplytoTorS = TorSlist.FindAll(x => hashKeys.Contains(x.Table_or_RowID));
            foreach (var TorSobj in OnlyApplytoTorS)
            {
                TorSobj.OMID = MenuId;
                TorSobj.save();

            }
            
            foreach (var TorSobj in RemovedTorSList)
            {
                TorSobj.OMID = 0;
                TorSobj.save();

            }
            return status;
        }
        public JObject ScanRestTable(string Obj)
        {
            JObject ParaMeters = JObject.Parse(Obj);
            string QrCode =ParaMeters.GetValue("TID").ToString();
            int CID = int.Parse(ParaMeters.GetValue("CID").ToString());
            string Type = ParaMeters.GetValue("Type").ToString();
            HG_Tables_or_Sheat TableRowObj = new HG_Tables_or_Sheat().GetOne(QrOcde: QrCode);
            if(TableRowObj.Type!= Type)
            {
                TableRowObj = new HG_Tables_or_Sheat();
            }
            List<HG_Orders> CustOrdrList = new HG_Orders().GetListByGetDate(DateTime.Now, DateTime.Now);
            JObject jObject = JObject.FromObject(TableRowObj);
            HG_Orders orders = CustOrdrList.Find(x => x.Table_or_SheatId == TableRowObj.Table_or_RowID && x.Status!="3");
            if (orders == null)
            {
                jObject.Add("OID", 0);
            }
            else
            {
                jObject.Add("OID", orders.OID);
            }
            HG_OrganizationDetails objOrg = new HG_OrganizationDetails().GetOne(TableRowObj.OrgId);
            jObject.Add("OrgName", objOrg != null ? objOrg.Name : " ");
            jObject.Add("OrderingStatus", objOrg.CustomerOrdering);
            jObject.Add("PaymentType", objOrg.PaymentType);
            return jObject;
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

        public JObject SettingPrivacyPolicy(string KeyName)
        {
            List<Settings> listsettings = new Settings().GetAll();
           Settings settingsObj = listsettings.Find(x => x.KeyName == KeyName);
            return JObject.FromObject(settingsObj);
        }
        // make order
        public JObject PostOrder(string Obj)
        {
            JObject Params = JObject.Parse(Obj);
            Int64 CID = Int64.Parse(Params["CID"].ToString());
            int OrgId = int.Parse(Params["OrgID"].ToString());
            Int64 TableorSheatId=Int64.Parse(Params["TORSID"].ToString());
          //  Int64 OID =Int64.Parse(Params["OID"].ToString());
            int Status =Params["Status"]!=null?int.Parse(Params["Status"].ToString()):1;//"1":Order Placed,"2":Processing,3:"Completed" ,"4" :"Cancelled"
            HG_Tables_or_Sheat ObjTorS = new HG_Tables_or_Sheat().GetOne(TableorSheatId);
            List<HG_Orders> ListOfOrder = new HG_Orders().GetListByGetDate(DateTime.Now, DateTime.Now);
            ListOfOrder = ListOfOrder.FindAll(x => x.OrgId == OrgId);
            HG_Orders ObjOrders = ListOfOrder.Find(x => x.Table_or_SheatId == TableorSheatId && x.TableOtp == ObjTorS.Otp);
           
            JObject PostResult = new JObject();
            List<Cart> ListCart = Cart.List.FindAll(x => x.CID == CID && x.OrgId==OrgId && x.TableorSheatOrTaleAwayId==TableorSheatId);
            // HG_Orders ObjOrders = new HG_Orders().GetOne(OID);
            Int64 OID = 0;
            if (ObjOrders==null||ObjOrders.Status=="3"|| ObjOrders.Status == "4" ||ObjOrders.PaymentStatus!=0){// if order is completed or Order then Take New order

                OID = 0;
            }
            else
            {
                OID = ObjOrders.OID;
            }
            //check customer ordering enable
            HG_OrganizationDetails OrgObj = new HG_OrganizationDetails().GetOne(OrgId);
            if (OrgObj.CustomerOrdering==false)
            {
                PostResult.Add("Status", 400);
                PostResult.Add("MSG", "Customer Ordering is UnActive");
                return PostResult;
            }
            if (ListCart.Count <= 0)
            {
                PostResult.Add("Status",400);
                PostResult.Add("MSG","Add Atleast one Item");
                return PostResult;
            }
            Int64 NewOID = 0;
            if (OID > 0)
            {
                NewOID = OID;
                ObjOrders.Status ="1";// Placed
                ObjOrders.Update_By = CID;
                ObjOrders.OrderByIds = ObjOrders.OrderByIds + CID.ToString() + ",";
                ObjOrders.Update_Date = DateTime.Now;
                ObjOrders.Save();
            }
            else
            {
                HG_Orders ObjOrder = new HG_Orders()
                {
                    Create_By = CID,
                    Create_Date =DateTime.Now,
                    CID = CID,
                    Update_By = CID,
                    Status = "1",//Placed
                    OrgId = OrgId,
                    Table_or_SheatId = TableorSheatId,
                    PaymentStatus = 0,// unpaid
                    TableOtp = ObjTorS.Otp,
                    OrderByIds=CID.ToString()+","

                };
                NewOID= ObjOrder.Save();
            }
                if (NewOID > 0)
                {
                List<HG_Ticket> list = new HG_Ticket().GetAll(OrgId);
                HG_Ticket objticket = new HG_Ticket() {OrgId=OrgId,OID=NewOID,TicketNo=list.Count+1 };
                int Ticketno = objticket.save();
                    foreach (Cart Item in ListCart)
                    {
                    HG_Items ObjItem = new HG_Items().GetOne(ItemID: Item.ItemId);
                    HG_OrderItem OrderItem = new HG_OrderItem()
                    {
                        FID = ObjItem.ItemID,
                        Price = ObjItem.Price,
                        Count = Item.Count,
                        Qty = ObjItem.Qty,
                        OID = NewOID,
                        Status = Status,
                        TickedNo = Ticketno,
                        OrgId = OrgId,
                        ChefSeenBy = 0,
                        OrderDate = DateTime.Now,
                        OrdById = CID,
                        TaxInItm=ObjItem.Tax
                        };
                        if (OrderItem.Save() <= 0)
                        {
                            HG_Orders order = new HG_Orders();
                            order.DeleteOrderAndOrderItem(NewOID,false);
                        PostResult.Add("Status", 400);
                        PostResult.Add("MSG", "Can't Confirm Order Try After Some Time.");
                        return PostResult;
                        }
                    }
                PostResult.Add("Status", 200);
                PostResult.Add("MSG",NewOID.ToString()+","+Ticketno.ToString());
            }
            else
            {
                PostResult.Add("Status", 400);
                PostResult.Add("MSG", "Unable To Place Order Try Again.");
                return PostResult;
            }
            Cart.List.RemoveAll(x => x.CID == CID && x.OrgId==OrgId &&x.TableorSheatOrTaleAwayId==TableorSheatId);
            return PostResult;
        }

        //cancel order
        public JObject CancelOrder(Int64 OID ,int UpdatedBy = 0)
        {
            JObject result = new JObject();
            HG_Orders hG_Orders = new HG_Orders().GetOne(OID);
            if (hG_Orders.OID < 1)
            {
                result.Add("Status", 400);
                result.Add("MSG", "Order Not Found");
                return result;
            }
            if (hG_Orders.Status == "3")
            {
                result.Add("Status", 400);
                result.Add("MSG", "Can't Cancel Order. Order Already Completed");
                return result;
            }
            if (hG_Orders.PaymentStatus > 0)
            {
                result.Add("Status", 400);
                result.Add("MSG", "Can't Cancel Order. Payment Has Been Done");
                return result;
            }
            else
            {
                if (UpdatedBy == 0)
                {
                    var UserInfo = Request.Cookies["UserInfo"];
                    UpdatedBy = int.Parse(UserInfo["UserCode"]);
                }
                hG_Orders.Status = "4";//CANCEL ORDER
                hG_Orders.Update_By = UpdatedBy;
                var OrderItem = new HG_OrderItem().GetAll(hG_Orders.OID);
                foreach(var ObjOitem in OrderItem)
                {
                    ObjOitem.Status = 4;//cancel all items
                    ObjOitem.UpdatedBy = UpdatedBy;
                    ObjOitem.Save();

                }
                result.Add("Status", 200);
                return result;
            }
        }
        public JObject ShowOrderItems(int TOrSId,int OrgId=0)
        {
            JObject jObject = new JObject();
            List<HG_Orders> ListOrders = new HG_Orders().GetListByGetDate(DateTime.Now, DateTime.Now);
            ListOrders = ListOrders.FindAll(x => x.Table_or_SheatId == TOrSId &&(x.Status=="1"||x.Status=="2"));// placed or Processing
            ListOrders = ListOrders.FindAll(x => x.PaymentStatus == 0);
            List<HG_OrderItem> listitems = new List<HG_OrderItem>();
            
            List<HG_Items> items = new HG_Items().GetAll(OrgId);
            foreach (var OrderObj in ListOrders)
            {
                listitems.AddRange(new HG_OrderItem().GetAll(OrderObj.OID));
                
            }
            double TotalPrice = 0.00;
            JArray jArray = new JArray();
            foreach (var OrderItm in listitems)
            {
                TotalPrice += (OrderItm.Count * OrderItm.Price);
                JObject jobj = JObject.FromObject(OrderItm);
                HG_Items hG_Items = items.Find(x => x.ItemID == OrderItm.FID);
                jobj.Add("ItemName", hG_Items.Items);
                jArray.Add(jobj);
            }
            if (listitems.Count == 0)
            {
                jObject.Add("Status", 400);
                jObject.Add("MSG", "Make Order First");
            }
            else
            {
                jObject.Add("Status", 200);
                jObject.Add("ListItems", jArray);
                jObject.Add("Total", TotalPrice);

            }
            

            return jObject;
        }
        public JObject CompleteOrder(int PaymentType,int UpdatedBy, int OID = 0, int TorSid = 0)
        {
            JObject jObject = new JObject();
            List<HG_Orders> OrderList = new List<HG_Orders>();
            HG_Tables_or_Sheat obj = new HG_Tables_or_Sheat();
            List<HG_OrderItem> OrdrItmsList = new List<HG_OrderItem>();
            if (OID > 0)
            {
                HG_Orders order = new HG_Orders().GetOne(OID);
                obj = new HG_Tables_or_Sheat().GetOne(order.Table_or_SheatId);
                OrderList.Add(order);
            }
            else if (TorSid > 0)
            {
                OrderList = new HG_Orders().GetListByGetDate(DateTime.Now, DateTime.Now);
                OrderList = OrderList.FindAll(x => x.Table_or_SheatId == TorSid && (x.Status == "1" || x.Status == "2"));
                obj = new HG_Tables_or_Sheat().GetOne(TorSid);

            }
            HG_OrganizationDetails ObjOrg = new HG_OrganizationDetails().GetOne(obj.OrgId);
            bool Status = false;
            foreach(var order in OrderList)
            {
                if (ObjOrg.PaymentType == 1)// prepaid case only accept payment 
                {
                    order.PaymentStatus = PaymentType;
                    order.Update_By = UpdatedBy;
                    order.PayReceivedBy = UpdatedBy;
                    order.Save();
                    Status = true;
                    OrdrItmsList=new HG_OrderItem().GetAll(order.OID);
                    var CompltedOrCacelOdrItms = OrdrItmsList.FindAll(x => x.Status == 3 || x.Status == 4);
                    if (CompltedOrCacelOdrItms.Count == OrdrItmsList.Count)
                    {
                        obj.Status = 1;// free table
                        obj.Otp = OTPGeneretion.Generate();
                        obj.save();
                        order.Status = "3";//completed
                        order.Save();

                    }
                }
                else
                {
                    if (order.OID > 0 && obj.Table_or_RowID > 0)
                    {
                        var OrderItems = new HG_OrderItem().GetAll(order.OID);
                        
                        order.Update_By = UpdatedBy;
                        order.PayReceivedBy = UpdatedBy;
                        order.PaymentStatus = PaymentType;// update payment status
                        order.Save();
                        //
                        var CompletedOrCancelItems = OrderItems.FindAll(x => x.Status == 3 || x.Status == 4);// completed or canceled
                        if (OrderItems.Count == CompletedOrCancelItems.Count)
                        {
                            obj.Status = 1;// free table
                            obj.Otp = OTPGeneretion.Generate();
                            obj.save();

                            order.Status = "3";//3 order completed
                            order.Save();
                          
                        }
                        Status = true;
                    }
                    else
                    {
                        Status = false;
                        
                    }
                }

            }

            if (Status)
            {
              
                jObject.Add("Status", 200);
                jObject.Add("MSG", obj.Otp);
            }
            else
            {
                jObject.Add("Status", 400);
                jObject.Add("MSG", "Order Number Not Found");
            }
            
            return jObject;
        }

        public JArray ShowOrderByStatus(string Obj)
        {
            JObject Params = JObject.Parse(Obj);
            int OrgId = int.Parse(Params["OrgId"].ToString());
            int IsChef = int.Parse(Params["IsChef"].ToString());
            List<HG_Orders> Orders = new HG_Orders().GetListByGetDate(DateTime.Now, DateTime.Now);
            List<HG_Orders> OrderToShow = new List<HG_Orders>();
            Orders = Orders.FindAll(x => x.Status != "3");//not completed
            Orders = Orders.FindAll(x => x.OrgId == OrgId);
            HG_OrganizationDetails orgobj = new HG_OrganizationDetails().GetOne(OrgId);
            List<HG_OrderItem> hG_OrderItems = new List<HG_OrderItem>();
            if (orgobj != null && orgobj.PaymentType == 1)// prepaid and is chef orders
            {
                if (IsChef == 1)
                {
                    Orders = Orders.FindAll(x => x.PaymentStatus > 0);// only seen paid orders
                    OrderToShow = Orders;
                }
                else
                {
                    Orders = Orders.FindAll(x => x.PaymentStatus ==0);// only seen unpaid orders
                    OrderToShow = Orders;
                }
            }
            else// postpaid
            {
                if (IsChef == 1)
                {
                    //Orders = Orders.FindAll(x => x.Status=="1");// only seen placed ordersw
                    foreach(var Order in Orders)
                    {
                        var OrderItms = new HG_OrderItem().GetAll(Order.OID);
                        hG_OrderItems.AddRange(OrderItms);
                        // var completedorCancel = OrderItms.FindAll(x => x.Status != 3);
                        OrderItms = OrderItms.FindAll(x => x.Status != 3 && x.Status != 4);
                        if (OrderItms.Count>0)
                        {
                            OrderToShow.Add(Order);
                        }
                    }
                }
                else
                {
                    foreach (var Order in Orders)
                    {
                        var OrderItms = new HG_OrderItem().GetAll(Order.OID);
                        hG_OrderItems.AddRange(OrderItms);
                       var  Completeditems = OrderItms.FindAll(x => x.Status == 3 || x.Status == 4);//all completed or cancel
                        if (OrderItms.Count== Completeditems.Count)
                        {
                            OrderToShow.Add(Order);
                        }
                    }
                   
                }
            }
            JArray jArray = new JArray();
            int TYpe = int.Parse(orgobj.OrgTypes);
            List<HG_Tables_or_Sheat> ListTorS = new HG_Tables_or_Sheat().GetAll(TYpe, OrgId);
            List<HG_FloorSide_or_RowName> ListFlorSideOrRow = new HG_FloorSide_or_RowName().GetAll(TYpe, OrgId);
            List<HG_Floor_or_ScreenMaster> ListFlrScrn = new HG_Floor_or_ScreenMaster().GetAll(TYpe, OrgId);
            foreach(var order in OrderToShow)
            {
                JObject jObject = new JObject();
                jObject = JObject.FromObject(order);
              var  ShowOrderItems = hG_OrderItems.FindAll(x => x.OID == order.OID);
                double ToTalAmt = 0.00;
                foreach(var item in ShowOrderItems)
                {
                    ToTalAmt += item.Count * item.Price;
                }
                jObject.Add("AMT",ToTalAmt);
                string Seating = " ";
                HG_Tables_or_Sheat objTorS = ListTorS.Find(x => x.Table_or_RowID==order.Table_or_SheatId);
                if (objTorS != null)
                {
                    string TorSName = objTorS.Table_or_SheetName;
                    HG_Floor_or_ScreenMaster objFs = ListFlrScrn.Find(x => x.Floor_or_ScreenID == objTorS.Floor_or_ScreenId);
                    if (objFs != null)
                    {
                        Seating += objFs.Name;
                        HG_FloorSide_or_RowName ObjFsRn = ListFlorSideOrRow.Find(x => x.ID == objTorS.FloorSide_or_RowNoID);
                        if (ObjFsRn != null)
                        {
                            Seating += " " + ObjFsRn.FloorSide_or_RowName;
                        }
                    }
                    Seating += " " + TorSName;
                }
                jObject.Add("TableorSheatName", Seating);
                jArray.Add(jObject);
            }
            return jArray;
        }


        //Start Chef End Work
        public JArray ChefOrders(int OrgId,int ChefId,int Status)
        {
            JArray tableorSheatList = new JArray();
            try
            {
                List<HG_Orders> Orderlist = new List<HG_Orders>();

                List<HG_OrderItem> OrderItemList = new HG_OrderItem().GetAllByOrg(OrgId,ChefId);
                if (Status == 0)
                {
                    OrderItemList = OrderItemList.FindAll(x => x.Status != 3 && x.Status != 4);
                    OrderItemList = OrderItemList.FindAll(x => x.OrderDate.Date == DateTime.Now.Date).ToList();
                    OrderItemList = OrderItemList.OrderBy(x => x.TickedNo).ToList();
                    var ObjItem = OrderItemList.First();
                    OrderItemList = OrderItemList.FindAll(x => x.TickedNo == ObjItem.TickedNo);
                    HG_Orders order = new HG_Orders().GetOne(ObjItem.OID);
                    Orderlist.Add(order);
                }
                else
                {

                    OrderItemList = OrderItemList.FindAll(x => x.Status==Status &&x.ChefSeenBy==ChefId);
                    OrderItemList = OrderItemList.FindAll(x => x.OrderDate.Date == DateTime.Now.Date).ToList();
                    // HashSet<int> HashOID = new HashSet<int>(OrderItemList.Select(x => x.TickedNo).ToArray());
                    var GroupByTicketNo = OrderItemList.GroupBy(x => x.TickedNo);
                    foreach(var Ticket in GroupByTicketNo)
                    {
                        var Firstorder = Ticket.First();
                        Orderlist.Add(new HG_Orders().GetOne(Firstorder.OID));
                    }
                  //  Orderlist = new HG_Orders().GetAll(OrgId);
                    //Orderlist = Orderlist.FindAll(x => HashOID.Contains(x.OID));
                }
                Orderlist = Orderlist.FindAll(x => x.Create_Date.Date == DateTime.Now.Date).ToList();
                HG_OrganizationDetails ObjOrg = new HG_OrganizationDetails().GetOne(OrgId);
            int OrgType =int.Parse(ObjOrg.OrgTypes);
                if (ObjOrg.PaymentType == 1)//if prepaid than visible after payment completed
                {
                    Orderlist = Orderlist.FindAll(x => x.PaymentStatus != 0);
                }
                List<HG_Tables_or_Sheat> ListTableOrSheat = new HG_Tables_or_Sheat().GetAllWithTakeAwya(OrgType, OrgId);//GetAll(OrgType, OrgId);
            List<HG_FloorSide_or_RowName> ListFloorSideorRow = new HG_FloorSide_or_RowName().GetAll(OrgType, OrgId);
            List<HG_Floor_or_ScreenMaster> ListFloorScreen = new HG_Floor_or_ScreenMaster().GetAll(OrgType, OrgId);
            List<HG_Items> ListfoodItems = new HG_Items().GetAll(OrgId);
                int TorSIndex = 0;
              foreach(var order in Orderlist)
                {
                    string FloorOrScreenName = "";
                    HG_Tables_or_Sheat hG_Tables_Or_Sheat = ListTableOrSheat.Find(x => x.Table_or_RowID == order.Table_or_SheatId);
                    HG_FloorSide_or_RowName hG_FloorSide_Or_RowName = ListFloorSideorRow.Find(x => x.ID == hG_Tables_Or_Sheat.FloorSide_or_RowNoID);
                    if (hG_FloorSide_Or_RowName == null)
                    {
                        hG_FloorSide_Or_RowName = new HG_FloorSide_or_RowName();
                        hG_FloorSide_Or_RowName.FloorSide_or_RowName = " ";
                    }
                    else
                    {
                        HG_Floor_or_ScreenMaster hG_Floor_Or_ScreenMaster = ListFloorScreen.Find(x => x.Floor_or_ScreenID == hG_Tables_Or_Sheat.Floor_or_ScreenId);
                        if (hG_Floor_Or_ScreenMaster == null)
                        {
                            hG_Floor_Or_ScreenMaster = new HG_Floor_or_ScreenMaster();
                            hG_Floor_Or_ScreenMaster.Name = "";
                        }
                        else
                        {
                            FloorOrScreenName = hG_Floor_Or_ScreenMaster.Name;
                        }

                    }
                    JObject TableScreen = new JObject();
                    
                    var hG_OrderItems = OrderItemList.FindAll(x => x.OID == order.OID);
                    JArray ItemsArray = new JArray();
                    
                    int ticketno = 0;
                    int ItemIndex = 0;
                    foreach (var OrderItem in hG_OrderItems)
                    {
                        HG_Items hG_Items = ListfoodItems.Find(x => x.ItemID == OrderItem.FID);
                        JObject itemobj = new JObject();
                        itemobj.Add("OIID", OrderItem.OIID);
                        itemobj.Add("ItemID", OrderItem.FID);
                        itemobj.Add("ItemName", hG_Items.Items);
                        itemobj.Add("Quantity", OrderItem.Qty+"*"+OrderItem.Count);
                        itemobj.Add("Status", OrderItem.Status);
                        itemobj.Add("IIndex", ItemIndex++);
                        ItemsArray.Add(itemobj);
                        ticketno = OrderItem.TickedNo;
                    }
                    string name = FloorOrScreenName + "-" + hG_FloorSide_Or_RowName.FloorSide_or_RowName + "-" + hG_Tables_Or_Sheat.Table_or_SheetName + " " + "Ticket no. : " + ticketno;
                    TableScreen.Add("TableScreenInfo", name);
                    TableScreen.Add("TableSeatID", hG_Tables_Or_Sheat.Table_or_RowID);
                    TableScreen.Add("TicketNo", ticketno);
                    TableScreen.Add("OID", order.OID);
                    TableScreen.Add("OrderItems", ItemsArray);
                    TableScreen.Add("TorSIndex", TorSIndex++);
                    tableorSheatList.Add(TableScreen);
                    if (tableorSheatList.Count == 1&& Status==0)
                    {
                        OrderItemList = OrderItemList.FindAll(x => x.Status == 1);
                        foreach (var Orderitem in OrderItemList)
                        {
                            Orderitem.ChefSeenBy = ChefId;
                            Orderitem.Status = 2;// processing
                            Orderitem.UpdatedBy = ChefId;
                            Orderitem.UpdationDate = DateTime.Now;
                            Orderitem.Save();
                        }

                    }
                }
              
           
            }
            catch(System.Exception e)
            {

            }
            return tableorSheatList;

        }
        public JArray ChefComCaclOrd(int OrgId, int ChefId, int Status)
        {
            JArray tableorSheatList = new JArray();
            try
            {
               
                List<HG_OrderItem> OrderItemList = new HG_OrderItem().GetAllByOrg(OrgId, ChefId);
               
                    OrderItemList = OrderItemList.FindAll(x => x.Status == Status && x.ChefSeenBy == ChefId);
                    OrderItemList = OrderItemList.FindAll(x => x.OrderDate.Date == DateTime.Now.Date).ToList();
                    // HashSet<int> HashOID = new HashSet<int>(OrderItemList.Select(x => x.TickedNo).ToArray());
                    var GroupByTicketNo = OrderItemList.GroupBy(x => x.TickedNo);
                HG_OrganizationDetails ObjOrg = new HG_OrganizationDetails().GetOne(OrgId);
                int OrgType = int.Parse(ObjOrg.OrgTypes);
                List<HG_Tables_or_Sheat> ListTableOrSheat = new HG_Tables_or_Sheat().GetAll(OrgType, OrgId);
                List<HG_FloorSide_or_RowName> ListFloorSideorRow = new HG_FloorSide_or_RowName().GetAll(OrgType, OrgId);
                List<HG_Floor_or_ScreenMaster> ListFloorScreen = new HG_Floor_or_ScreenMaster().GetAll(OrgType, OrgId);
                List<HG_Items> ListfoodItems = new HG_Items().GetAll(OrgId);
                int TorSIndex = 0;
                foreach (var TicketitmList in GroupByTicketNo)
                {
                    JArray ItemsArray = new JArray();
                    int ItemIndex = 0;
                    int ticketno = 0;
                    Int64 OrdId = 0;
                    var OrderItmListTicketWise = TicketitmList.ToList();
                    foreach (var OrderItem in OrderItmListTicketWise)
                    {
                        HG_Items hG_Items = ListfoodItems.Find(x => x.ItemID == OrderItem.FID);
                        JObject itemobj = new JObject();
                        itemobj.Add("OIID", OrderItem.OIID);
                        itemobj.Add("ItemID", OrderItem.FID);
                        itemobj.Add("ItemName", hG_Items.Items);
                        itemobj.Add("Quantity", OrderItem.Qty + "*" + OrderItem.Count);
                        itemobj.Add("Status", OrderItem.Status);
                        itemobj.Add("IIndex", ItemIndex++);
                       
                        ItemsArray.Add(itemobj);
                        ticketno = OrderItem.TickedNo;
                        OrdId = OrderItem.OID;
                    }
                    HG_Orders order = new HG_Orders().GetOne(OrdId);
                    HG_Tables_or_Sheat hG_Tables_Or_Sheat = ListTableOrSheat.Find(x => x.Table_or_RowID == order.Table_or_SheatId);
                    HG_FloorSide_or_RowName hG_FloorSide_Or_RowName = ListFloorSideorRow.Find(x => x.ID == hG_Tables_Or_Sheat.FloorSide_or_RowNoID);
                    HG_Floor_or_ScreenMaster hG_Floor_Or_ScreenMaster = ListFloorScreen.Find(x => x.Floor_or_ScreenID == hG_Tables_Or_Sheat.Floor_or_ScreenId);
                    JObject TableScreen = new JObject();
                    string name = hG_Floor_Or_ScreenMaster.Name + "-" + hG_FloorSide_Or_RowName.FloorSide_or_RowName + "-" + hG_Tables_Or_Sheat.Table_or_SheetName + " " + "Ticket no. : " + ticketno;
                    TableScreen.Add("TableScreenInfo", name);
                    TableScreen.Add("TableSeatID", hG_Tables_Or_Sheat.Table_or_RowID);
                    TableScreen.Add("TicketNo", ticketno);
                    TableScreen.Add("OID", order.OID);
                    TableScreen.Add("OrderItems", ItemsArray);
                    TableScreen.Add("TorSIndex", TorSIndex++);
                    tableorSheatList.Add(TableScreen);
                    
                }


            }
            catch (System.Exception e)
            {

            }
            return tableorSheatList;

        }
        // this method used for update item status by chef
        public JObject ChangeOrderItemStatus(string CheckedID, int TickedNo, int UpdateBy,int OID)
        {
            HG_Orders order = new HG_Orders().GetOne(OID);
            List<HG_OrderItem> OrderItemListAll  = new HG_OrderItem().GetAll(OID);
           var OrderItemList = OrderItemListAll.FindAll(x => x.TickedNo == TickedNo);
            HashSet<Int64> OIIDHash = new HashSet<Int64>();
            OIIDHash.Add(0);
            if (CheckedID.Contains(","))
            {
                var OIDSarray = CheckedID.Replace(" ", "").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach(var OIds in OIDSarray)
                {
                    OIIDHash.Add(Int64.Parse(OIds));
                }
            }
            bool status = false;
            JObject PostResult = new JObject();
            foreach(HG_OrderItem OrderitemObj in OrderItemList)
            {

                if (OIIDHash.Contains(OrderitemObj.OIID)){

                    OrderitemObj.Status = 3;//completed
                }
                else
                {
                    OrderitemObj.Status = 4;// canceld
                }
                OrderitemObj.UpdatedBy = UpdateBy;
                OrderitemObj.Save();
                if (OrderitemObj.OIID > 0)
                {
                    status = true;
                }
             }
            if (status)
            {
                HG_OrganizationDetails ObjOrg = new HG_OrganizationDetails().GetOne(order.OrgId);
                HG_Tables_or_Sheat TorSObj = new HG_Tables_or_Sheat().GetOne(order.Table_or_SheatId);
                if (ObjOrg.PaymentType==1)// prepaid
                {
                    order.Status ="3";// completed
                    order.Update_By = UpdateBy;
                    // free table 
                    TorSObj.Status = 1;
                    TorSObj.Otp = OTPGeneretion.Generate();
                    TorSObj.save();
                }
                else
                {//postpaid
                    order.Status = "2";// processing
                    var completedOrCancelorderItems = OrderItemListAll.FindAll(x => x.Status == 3 || x.Status == 4);//cancel and Completed
                    if (OrderItemListAll.Count == completedOrCancelorderItems.Count && order.PaymentStatus != 0)
                    {
                        order.Status ="3";
                        TorSObj.Status = 1;
                        TorSObj.Otp = OTPGeneretion.Generate();
                        TorSObj.save();
                    }
                    order.Update_By = UpdateBy;
                }
                order.Save();
                PostResult.Add("Status", 200);
            }
            else
            {
                PostResult.Add("Status", 400);
            }
            return PostResult;

        }

        //End Chef End Work

        public JObject ChangePassWord(string Obj)
        {
            JObject ParaMeters = JObject.Parse(Obj);
            System.Int32 UserCode = int.Parse(ParaMeters["UserCode"].ToString());
            string OldPassword =ParaMeters["OldPass"].ToString();
            string NewPassword = ParaMeters["NewPass"].ToString();
            JObject JsonResult = new JObject();
            vw_HG_UsersDetails user_obj = new vw_HG_UsersDetails().GetSingleByUserId(UserCode);
            if (user_obj.Password.Equals(OldPassword))
            {
                user_obj.Password = NewPassword;
                int check = user_obj.save();
                if (check > 0)
                {
                    JsonResult.Add("Status", 200);
                    JsonResult.Add("Msg", "Password Change Successful");
                }
                else
                {
                    JsonResult.Add("Status", 400);
                    JsonResult.Add("Msg", "Password Not Change.");
                }
            }
            
            else
            {
                JsonResult.Add("Status", 400);
                JsonResult.Add("Msg", "Old Password Incorrect.Please type correct old password");
            }
            return JsonResult;
        }
        public JObject ForgotPassword(string Obj)
        {
            JObject Result = new JObject();
            JObject ParaMeters = JObject.Parse(Obj);
            string MobileNo = ParaMeters["MobileNo"].ToString();
            string NewPassword = ParaMeters["Pass"].ToString();
            vw_HG_UsersDetails ObjUser = new vw_HG_UsersDetails().MobileAlreadyExist(MobileNo);
            if (ObjUser.UserCode > 0)
            {
                ObjUser.Password = NewPassword;
                if (ObjUser.save() > 0)
                {
                    Result.Add("Status",200);
                    Result.Add("MSG", "Change Successfully");
                }
                else
                {
                    Result.Add("Status", 400);
                    Result.Add("MSG", "Unable To Change  Password Try after some time ");
                }
            }
            else
            {
                Result.Add("Status", 400);
                Result.Add("MSG", "Invalid Mobile Number ");

            }
            return Result;
        }
        public JArray GetTableInfo(int OrgId,int OrgType)
        {
            List<HG_Tables_or_Sheat> ListTableOrSheat = new HG_Tables_or_Sheat().GetAll(OrgType, OrgId);
            List<HG_FloorSide_or_RowName> ListFloorSideorRow = new HG_FloorSide_or_RowName().GetAll(OrgType, OrgId);
            List<HG_Floor_or_ScreenMaster> ListFloorScreen = new HG_Floor_or_ScreenMaster().GetAll(OrgType, OrgId);
            JArray TablesOrSheatList = new JArray();
            foreach (var TableObj in ListTableOrSheat)
            {
                HG_FloorSide_or_RowName hG_FloorSide_Or_RowName = ListFloorSideorRow.Find(x => x.ID == TableObj.FloorSide_or_RowNoID);
                HG_Floor_or_ScreenMaster hG_Floor_Or_ScreenMaster = ListFloorScreen.Find(x => x.Floor_or_ScreenID == TableObj.Floor_or_ScreenId);
                JObject TableScreen = new JObject();
                string ForSname = hG_Floor_Or_ScreenMaster != null ? hG_Floor_Or_ScreenMaster.Name : "";
                string FsideOrRname = hG_FloorSide_Or_RowName != null ? hG_FloorSide_Or_RowName.FloorSide_or_RowName : "";
                TableScreen.Add("TableOrSheatName",ForSname + " "+ FsideOrRname+" "+ TableObj.Table_or_SheetName);
                TableScreen.Add("TableSeatID", TableObj.Table_or_RowID);
                TablesOrSheatList.Add(TableScreen);

            }
            return TablesOrSheatList;
        }
        public JObject OTPVerification(string MobileNO,string OtpNu)
        {
            OTPGeneretion ObjOtp = new OTPGeneretion();
            ObjOtp = ObjOtp.GetOne(MobileNO, OtpNu);
            JObject result = new JObject();
            if (ObjOtp.ID > 0)
            {
                result.Add("Status", 200);
                ObjOtp.Dell(ObjOtp.ID);

            }
            else
            {
                result.Add("Status", 400);
            }
            return JObject.FromObject(result);
        }
       
        public JObject OTPGenerator(string MobileNO,int Type)
        {
            JObject Result = new JObject();
            /* Type 1 means User registartion 2 means forgot password */
            if (Type == 1)//change registartion
            {
                vw_HG_UsersDetails ObjUser= new vw_HG_UsersDetails().MobileAlreadyExist(MobileNO);
                if (ObjUser.UserCode > 0)
                {
                    Result.Add("Status", 500);
                    return JObject.FromObject(Result);
                }
                
            }
            OTPGeneretion ObjOtp = new OTPGeneretion();
            string OTPNumber = OTPGeneretion.Generate().ToString();
            ObjOtp.MobileNO = MobileNO;
            ObjOtp.OTP = OTPNumber;
           
           if(ObjOtp.save() > 0)
            {
                // Settings settingsObj = new Settings().GetOne("Mgs");
                // APICONTACT&senderid=FOODDO&msg=APIMSG
                string Msg = "YOUR OTP FOR foodDo APP IS " + OTPNumber+"";
                HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create("http://app.telcob.cloud/app/smsapi/index.php?key=55DD67927E1B3E&campaign=0&routeid=4&type=text&contacts=" + MobileNO + "&senderid=FOODDO&msg=" + Msg);
                webRequest.Method = "GET";
                WebResponse webResp = webRequest.GetResponse();
                Result.Add("Status", 200);
            }
           else
            {
                Result.Add("Status",400);
            }
            return JObject.FromObject(Result);
        }

        public JArray TableAndTakeAway(int Type,int OrderById,int FloorId=0,int OrgId=0)
        {
            JArray jArray = new JArray();
            DateTime fromdate = DateTime.Now;
            List<HG_Tables_or_Sheat> list = new HG_Tables_or_Sheat().GetAllWithTakeAwya(Type);
            List<HG_FloorSide_or_RowName> FloorSideRowList = new HG_FloorSide_or_RowName().GetAll(Type, OrgId);
            List<HG_Floor_or_ScreenMaster> FloorScrenList = new HG_Floor_or_ScreenMaster().GetAll(Type, OrgId);
            List<HG_Orders> Orderlist = new HG_Orders().GetListByGetDate(fromdate, DateTime.Now);
            if (FloorId > 0)
            {
                list = list.FindAll(x => x.Floor_or_ScreenId == FloorId);
            }
            foreach(var objtable in list)
            {
                HG_Orders order = Orderlist.Find(x => x.CID == OrderById && x.Table_or_SheatId == objtable.Table_or_RowID &&x.Status!="3");
                JObject jObject = new JObject();
                jObject = JObject.FromObject(objtable);
                if (order != null && order.OID > 0)
                {
                    jObject.Add("CurrOID",order.OID);
                    
                }
                else
                {
                    jObject.Add("CurrOID",0);
                    jObject["Status"]= 1;

                }
                string Seating = "";
                HG_Floor_or_ScreenMaster floor_Or_ScreenMaster = FloorScrenList.Find(x => x.Floor_or_ScreenID == objtable.Floor_or_ScreenId);
                if (floor_Or_ScreenMaster != null)
                {
                    Seating += floor_Or_ScreenMaster.Name;
                    HG_FloorSide_or_RowName OBJFsideOrRowName = FloorSideRowList.Find(x => x.ID == objtable.FloorSide_or_RowNoID);
                    if (OBJFsideOrRowName != null)
                    {
                        Seating += " " + OBJFsideOrRowName.FloorSide_or_RowName;
                    }
                }
                Seating += " " + objtable.Table_or_SheetName;
                // public int Status { get; set; }// {"1":free,"2":"BOOKED",3:"PROGRESS"}
                //  public string Table_or_SheetName { get; set; }
                jObject["Table_or_SheetName"] = Seating;
                jArray.Add(jObject);
            }
            return jArray;
        }



        public JArray PastOrderMainList(int CID,int status=0)
        {
            JArray Info = new JArray();
            
            List<HG_Orders> OrderList = new HG_Orders().GetAll(CID: CID);
            if (status > 0 && status == 1)//ongoing orders
            {
                OrderList = OrderList.FindAll(x => x.Status == "1" || x.Status == "2");
                OrderList = OrderList.FindAll(x => x.Create_Date.Date == DateTime.Now.Date).ToList();
            }
            else if (status > 0 && status == 3)//completed
            {
                OrderList = OrderList.FindAll(x => x.Status == "3");
                OrderList = OrderList.FindAll(x => x.Create_Date.Date == DateTime.Now.Date).ToList();
            }

            if(OrderList.Count>0)
            {

                foreach (HG_Orders orders in OrderList)
                {
                   
                    HG_OrganizationDetails hG_OrganizationDetails = new HG_OrganizationDetails().GetOne(orders.OrgId);
                    List<HG_OrderItem> OrderItemList = new HG_OrderItem().GetAll(orders.OID);
                    double price = 0.00;
                    HashSet<int> Token = new HashSet<int>();
                    for(int i=0;i< OrderItemList.Count; i++)
                    {
                        price += (OrderItemList[i].Count * OrderItemList[i].Price);
                        Token.Add(OrderItemList[i].TickedNo);
                    }
                    JObject Object = new JObject();
                    Object.Add("Date", orders.Create_Date.ToString("ddd, MMM-dd-yyyy"));
                    Object.Add("OrganizationName", hG_OrganizationDetails.Name);
                    Object.Add("TotalAmount", price);
                    Object.Add("TicketNo", string.Join(",", Token));
                    Object.Add("OID", orders.OID);
                    Object.Add("Status", orders.Status);
                    if (orders.PaymentStatus == 1)
                    {
                        Object.Add("PayStatus", "CASH");
                    }
                    else if (orders.PaymentStatus == 2)
                    {
                        Object.Add("PayStatus", "ONLINE");
                    }
                    else if (orders.PaymentStatus == 3)
                    {
                        Object.Add("PayStatus", "foodDo");
                    }
                    else
                    {
                        Object.Add("PayStatus", "UNPAID");
                    }

                    Info.Add(Object);
                }
            }
            
            return Info;
        }



        public JObject PastOrderSubList(int OID, string Status)
        {

            JObject Object = new JObject();
            JArray Info = new JArray();
            HG_Orders orders = new HG_Orders().GetOne(OID);

            if (orders != null && orders.Status == Status)
            {
                HG_OrganizationDetails hG_OrganizationDetails = new HG_OrganizationDetails().GetOne(orders.OrgId);
                List<HG_OrderItem> hG_OrderItems = new HG_OrderItem().GetAll(orders.OID);
                List<HG_Items> ListfoodItems = new HG_Items().GetAll(orders.OrgId);
                double price = 0.00;
                HashSet<int> Token = new HashSet<int>();
                for (int i = 0; i < hG_OrderItems.Count; i++)
                {
                    price += (hG_OrderItems[i].Count * hG_OrderItems[i].Price);
                    Token.Add(hG_OrderItems[i].TickedNo);
                }
                Object.Add("Date", orders.Create_Date.ToString("ddd, MMM-dd-yyyy"));
                Object.Add("OrganizationName", hG_OrganizationDetails.Name);
                Object.Add("TotalAmount", price);
                Object.Add("TicketNo", string.Join(",", Token));
                Object.Add("OID", orders.OID);
                Object.Add("Status", orders.Status);
                if (orders.PaymentStatus == 1)
                {
                    Object.Add("PayStatus", "CASH");
                }
                else if (orders.PaymentStatus == 2)
                {
                    Object.Add("PayStatus", "ONLINE");
                }
                else if (orders.PaymentStatus == 3)
                {
                    Object.Add("PayStatus", "foodDo");
                }
                else
                {
                    Object.Add("PayStatus", "UNPAID");
                }

                foreach (var OrderItem in hG_OrderItems)
                {
                    HG_Items hG_Items = ListfoodItems.Find(x => x.ItemID == OrderItem.FID);
                    JObject itemobj = new JObject();
                    itemobj.Add("OIID", OrderItem.OIID);
                    itemobj.Add("ItemID", OrderItem.FID);
                    itemobj.Add("ItemName", hG_Items.Items);
                    itemobj.Add("Quantity", OrderItem.Qty + "*" + OrderItem.Count);
                    itemobj.Add("Status", OrderItem.Status);
                    itemobj.Add("Amount", OrderItem.Price);
                    Info.Add(itemobj);
                }


            }
            Object.Add("OrderDetails", Info);

            return Object;
        }

        public JObject TableOTPCheker(string  TableID , int TableOTP)
        {
            JObject jsonResult = new JObject();
            HG_Tables_or_Sheat listtable = new HG_Tables_or_Sheat().GetOne(Int64.Parse(TableID));
       
            if(listtable.Otp == TableOTP)
            {
                jsonResult.Add("Status", 200);
                jsonResult.Add("Msg", "  Successful");
            }
            else
            {
                jsonResult.Add("Status", 400);
                jsonResult.Add("Msg", "  UNSuccessful");
            }

            return jsonResult;
        }

        public JObject ONLINEOFFLINE(int CHEFID, int TicketNO, int OrgId, bool ChefStatus)
        {
            JObject jObject = new JObject();
            List<HG_OrderItem> tableorderlist = new HG_OrderItem().GetAllByOrg(OrgId, ChefId: CHEFID);
            tableorderlist = tableorderlist.FindAll(x => x.TickedNo == TicketNO);
            foreach (var OrderItem in tableorderlist)
            {
                OrderItem.ChefSeenBy = 0;
                OrderItem.Save();
            }
            vw_HG_UsersDetails userdetails = new vw_HG_UsersDetails().GetSingleByUserId(CHEFID);
            userdetails.CurrentStatus = ChefStatus;
            Int64 save = userdetails.save();
            if (save > 0)
            {
                jObject.Add("Status", 200);

            }
            else
            {
                jObject.Add("Status", 400);

            }

            return jObject;
        }

        // THIS FUNCTION SET SAFE ONLINE OFFLINE MODES
        public JObject CheckChefOnlineOffline(int CHEFID)
        {
            JObject jObject = new JObject();
          
            vw_HG_UsersDetails userdetails = new vw_HG_UsersDetails().GetSingleByUserId(CHEFID);
            if(userdetails!=null)
            {
                if (userdetails.CurrentStatus)
                {
                    jObject.Add("Status", 200);
                    jObject.Add("Msg", "Online");

                }
                else
                {
                    jObject.Add("Status", 400);
                    jObject.Add("Msg", "Offline");
                }
            }
            else
            {
                jObject.Add("Status", 600);
                jObject.Add("Msg", "UserNotExits");
            }
            return jObject;
        }
        public JArray FloorScreen(int OrgID)
        {
            HG_OrganizationDetails orgonization = new HG_OrganizationDetails().GetOne(OrgID);
          
            List<HG_Floor_or_ScreenMaster> floorlist = new HG_Floor_or_ScreenMaster().GetAll(int.Parse(orgonization.OrgTypes), OrgID);
            return JArray.FromObject(floorlist);
        }
       






    }
}
