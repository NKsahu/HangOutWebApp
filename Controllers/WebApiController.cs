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

            System.Int64 CID = System.Int64.Parse(objParams.GetValue("CID").ToString());
            System.Int64 OID = System.Int64.Parse(objParams.GetValue("OID").ToString());
            System.Int32 OrgId = System.Int32.Parse(objParams.GetValue("OrgId").ToString());
            System.Int64 TableSheatTakeWayId = System.Int64.Parse(objParams.GetValue("TSTWID").ToString());
            List<HG_Category> MenuList = new HG_Category().GetAll(OrgId: OrgId);
            List<HG_Items> ListItems = new HG_Items().GetAll(OrgId);
            List<Cart> cartlist = Cart.List.FindAll(x => x.CID == CID && x.OrgId==OrgId &&x.TableorSheatOrTaleAwayId== TableSheatTakeWayId && x.OID==OID);
            JArray JMenuArray = new JArray();
            int count = 0;
            foreach(HG_Category menu in MenuList)
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
                        int CurrCount = cartCurrentItem != null ? cartCurrentItem.Count :0;
                        JObject objItem = new JObject();
                        objItem.Add("IID", Items.ItemID);
                        objItem.Add("ItemName", Items.Items);
                        objItem.Add("ItemPrice", Items.Price);
                        objItem.Add("ItemQuntity", Items.Qty);
                        objItem.Add("ItemImage", Items.Image);
                        objItem.Add("ItemCartValue", CurrCount);
                        objItem.Add("MenuId", Items.CategoryID);
                        objItem.Add("ItemIndex", ItemiIndex++);
                        jarrayItem.Add(objItem);
                        MenuItemPrice += Items.Price * CurrCount;
                    }
                    JobjMenu.Add("MenuItemCount", ItemListByMenu.Count);
                    JobjMenu.Add("MenuItems", jarrayItem);
                    JobjMenu.Add("MenuItmPrice", MenuItemPrice);
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
            int OrgId = System.Convert.ToInt32(ParaMeters["OrgId"].ToString());
            Int64 OID = System.Convert.ToInt64(ParaMeters["OID"]);
            System.Int64 TableSheatTakeWayId = System.Int64.Parse(ParaMeters.GetValue("TSTWID").ToString());
            Cart ObjCart = Cart.List.Find(x => x.CID == CustID && x.ItemId == ItemId && x.OrgId == OrgId && x.TableorSheatOrTaleAwayId==TableSheatTakeWayId);
            if (ObjCart != null)
            {
                ObjCart.Count = Cnt;
                Cart.List.RemoveAll(x => x.CID == CustID && x.ItemId == ItemId && x.OrgId == OrgId && x.TableorSheatOrTaleAwayId==TableSheatTakeWayId && x.OID==OID);
                if (ObjCart.Count != 0)
                    Cart.List.Add(ObjCart);
            }
            else
                Cart.List.Add(new Cart() { CID = CustID, ItemId = ItemId, Count = Cnt, OrgId = OrgId ,TableorSheatOrTaleAwayId=TableSheatTakeWayId,OID=OID});
            double Amt = 0;
            int Count = 0;
            List<Cart> CurrItemsOfUser = Cart.List.FindAll(x => x.CID == CustID && x.TableorSheatOrTaleAwayId == TableSheatTakeWayId && x.OrgId == OrgId && x.OID==OID);
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
            Cart CurrentItemobj = Cart.List.Find(x => x.CID == CustID && x.ItemId == ItemId && x.OrgId == OrgId && x.TableorSheatOrTaleAwayId == TableSheatTakeWayId && x.OID==OID);

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
                    TotalPrice += Mycart.Count * item.Price;
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
                    jobj.Add("ItemList", OrderCategoryItems);
                    jArray.Add(jobj);
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
        public int ActiveMenu(string SObj)
        {
            int status = 0;
            JObject Obj = JObject.Parse(SObj);
            int MenuId =int.Parse(Obj["OMID"].ToString());
            int OrgId = int.Parse(Obj["OrgId"].ToString());
            HG_OrganizationDetails hG_OrganizationDetails = new HG_OrganizationDetails().GetOne(OrgId);
            string OrgType = hG_OrganizationDetails.OrgTypes !=null ? hG_OrganizationDetails.OrgTypes : "1";
            List <OrderMenu> orderMenulist = OrderMenu.GetAll();
            OrderMenu orderMenu = orderMenulist.Find(x => x.id == MenuId);
            orderMenu.Status = true;
            orderMenu.save();
            List<HG_Tables_or_Sheat> TorSlist = new HG_Tables_or_Sheat().GetAll(int.Parse(OrgType));
            var AlreadySelectedList = TorSlist.FindAll(x => x.OMID == MenuId);
            JArray jArray = JArray.FromObject(Obj["TorSIDs"]);
            Int64[] items = jArray.Select(jv => (Int64)jv).ToArray();
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
            System.Int64 TableId =System.Convert.ToInt64(ParaMeters.GetValue("TID").ToString());
            HG_Tables_or_Sheat TableRowObj = new HG_Tables_or_Sheat().GetOne(TableId);
            JObject jObject = JObject.FromObject(TableRowObj);
            HG_OrganizationDetails objOrg = new HG_OrganizationDetails().GetOne(TableRowObj.OrgId);
            jObject.Add("OrgName", objOrg != null ? objOrg.Name : " ");
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
            Int64 OID =Int64.Parse(Params["OID"].ToString());
            int Status =Params["Status"]!=null?int.Parse(Params["Status"].ToString()):1;//"1":Order Placed,"2":Processing,3:"Completed" ,"4" :"Cancelled"
            JObject PostResult = new JObject();
            List<Cart> ListCart = Cart.List.FindAll(x => x.CID == CID && x.OrgId==OrgId && x.TableorSheatOrTaleAwayId==TableorSheatId &&x.OID==OID);
            if (ListCart.Count <= 0)
            {
                PostResult.Add("Status",400);
                PostResult.Add("MSG","Add Atleast one Item");
                return PostResult;
            }
            System.Int64 NewOID = 0;
            if (OID > 0)
            {
                NewOID = OID;
                HG_Orders ObjOrders = new HG_Orders().GetOne(NewOID);
                ObjOrders.Status = "1";
                ObjOrders.Update_By = CID;
                ObjOrders.Update_Date = DateTime.Now;
                ObjOrders.Save();
            }
            else
            {
                HG_Orders ObjOrders = new HG_Orders()
                {
                    Create_By = CID,
                    Create_Date = System.DateTime.Now,
                    CID = CID,
                    Update_By = CID,
                    Status = "1",// order placed
                    OrgId = OrgId,
                    Table_or_SheatId = TableorSheatId,
                    PaymentStatus=0// unpaid
                    
                };
                NewOID= ObjOrders.Save();
            }
                if (NewOID > 0)
                {
                List<HG_Ticket> list = new HG_Ticket().GetAll(OrgId);
                HG_Ticket objticket = new HG_Ticket() {OrgId=OrgId,OID=OID,TicketNo=list.Count+1 };
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
            Cart.List.RemoveAll(x => x.CID == CID &&x.OID==OID && x.OrgId==OrgId);
            return PostResult;
        }
        public JObject ShowOrderItems(int OID)
        {
            JObject jObject = new JObject();
            List<HG_OrderItem> listitems = new HG_OrderItem().GetAll(OID);
            if (listitems.Count == 0)
            {
                jObject.Add("Status", 400);
                jObject.Add("MSG", "Make Order First");
            }
            else
            {
                jObject.Add("Status", 200);
                jObject.Add("ListItems", JArray.FromObject(listitems));
                jObject.Add("Total", listitems.Sum(x => x.Price));

            }
            

            return jObject;
        }
        public JObject CompleteOrder(int OID,int PaymentType,int UpdatedBy)
        {
            JObject jObject = new JObject();
            HG_Orders order = new HG_Orders().GetOne(OID);
            List<HG_OrderItem> listOrderitem = new HG_OrderItem().GetAll(order.OID);
            HG_Tables_or_Sheat obj = new HG_Tables_or_Sheat().GetOne(order.Table_or_SheatId);
            if (order.OID > 0 && obj.Table_or_RowID>0)
            {
                order.Status = "3";//3 completed
                order.Update_By = UpdatedBy;
                order.PayReceivedBy = UpdatedBy;
                order.PaymentStatus = PaymentType;// update payment status
                obj.Status = 1;// free table
                obj.Otp = OTPGeneretion.Generate();
                order.Save();
                obj.save();
                foreach(var Oitems in listOrderitem)
                {
                    Oitems.Status = 3;
                    Oitems.UpdatedBy = UpdatedBy;
                    Oitems.Save();
                }
                jObject.Add("Status", 200);
                jObject.Add("MSG", obj.Otp);
            }
            else
            {
                jObject.Add("Status", 400);
                jObject.Add("MSG", "Order No Not Found");
            }
            return jObject;
        }

        public JArray ShowOrderByStatus(string Obj)
        {
            JObject Params = JObject.Parse(Obj);
            int OrgId = int.Parse(Params["OrgId"].ToString());
            int PaymentStatus = int.Parse(Params["PayStatus"].ToString());
            List<HG_Orders> Orders = new HG_Orders().GetListByGetDate(DateTime.Now.AddDays(-1), DateTime.Now);
            Orders = Orders.FindAll(x => x.Status != "3");//not completed
            Orders = Orders.FindAll(x => x.OrgId == OrgId);
            if (PaymentStatus != -1)// unpaid and other
            {
                Orders = Orders.FindAll(x => x.PaymentStatus == PaymentStatus);
            }
            else
            {
                //chef orders==
                HG_OrganizationDetails orgobj = new HG_OrganizationDetails().GetOne(OrgId);
                if (orgobj != null && orgobj.PaymentType == 1)
                {
                    Orders = Orders.FindAll(x => x.PaymentStatus > 0);
                }
            }
            
            JArray jArray = new JArray();
            foreach(var order in Orders)
            {
                
                JObject jObject = new JObject();
                jObject = JObject.FromObject(order);
                List<HG_OrderItem> hG_OrderItems = new HG_OrderItem().GetAll(order.OID);
                double ToTalAmt = 0.00;
                foreach(var item in hG_OrderItems)
                {
                    ToTalAmt += item.Count * item.Price;
                }
                jObject.Add("AMT",ToTalAmt);
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
                    OrderItemList = OrderItemList.OrderBy(x => x.TickedNo).ToList();
                    var ObjItem = OrderItemList.First();
                    OrderItemList = OrderItemList.FindAll(x => x.TickedNo == ObjItem.TickedNo);
                    HG_Orders order = new HG_Orders().GetOne(ObjItem.OID);
                    Orderlist.Add(order);
                }
                else
                {
                    OrderItemList = OrderItemList.FindAll(x => x.Status==Status &&x.ChefSeenBy==ChefId);
                    HashSet<Int64> HashOID = new HashSet<Int64>(OrderItemList.Select(x => x.OID).ToArray());
                    Orderlist = new HG_Orders().GetAll(OrgId);
                    Orderlist = Orderlist.FindAll(x => HashOID.Contains(x.OID));
                }
                Orderlist = Orderlist.FindAll(x => x.Create_Date.Date >= DateTime.Now.Date).ToList();
                HG_OrganizationDetails ObjOrg = new HG_OrganizationDetails().GetOne(OrgId);
            int OrgType =int.Parse(ObjOrg.OrgTypes);
                if (ObjOrg.PaymentType == 1)//if prepaid than visible after payment completed
                {
                    Orderlist = Orderlist.FindAll(x => x.PaymentStatus != 0);
                }
            List<HG_Tables_or_Sheat> ListTableOrSheat = new HG_Tables_or_Sheat().GetAll(OrgType, OrgId);
            List<HG_FloorSide_or_RowName> ListFloorSideorRow = new HG_FloorSide_or_RowName().GetAll(OrgType, OrgId);
            List<HG_Floor_or_ScreenMaster> ListFloorScreen = new HG_Floor_or_ScreenMaster().GetAll(OrgType, OrgId);
           // string TableSheatPrefix = ObjOrg.OrgTypes == "1" ? "Table : " :"Sheat : ";
            List<HG_Items> ListfoodItems = new HG_Items().GetAll(OrgId);
                //string SideOrRowPrefix = ObjOrg.OrgTypes == "1" ? "Table" : "Sheat: ";
                int TorSIndex = 0;
              foreach(var order in Orderlist)
                {
                    HG_Tables_or_Sheat hG_Tables_Or_Sheat = ListTableOrSheat.Find(x => x.Table_or_RowID == order.Table_or_SheatId);
                    HG_FloorSide_or_RowName hG_FloorSide_Or_RowName = ListFloorSideorRow.Find(x => x.ID == hG_Tables_Or_Sheat.FloorSide_or_RowNoID);
                    HG_Floor_or_ScreenMaster hG_Floor_Or_ScreenMaster = ListFloorScreen.Find(x => x.Floor_or_ScreenID == hG_Tables_Or_Sheat.Floor_or_ScreenId);
                    JObject TableScreen = new JObject();
                    
                    var hG_OrderItems = OrderItemList.FindAll(x => x.OID == order.OID);
                    JArray ItemsArray = new JArray();
                    int ItemIndex = 0;
                    int ticketno = 0;
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
                    string name = hG_Floor_Or_ScreenMaster.Name + "-" + hG_FloorSide_Or_RowName.FloorSide_or_RowName + "-" + hG_Tables_Or_Sheat.Table_or_SheetName + " " + "Ticket no. : " + ticketno;
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
       // this method used for update item status by chef
        public JObject ChangeOrderItemStatus(string CheckedID, int TickedNo, int UpdateBy,int OID)
        {
            List<HG_OrderItem> OrderItemList  = new HG_OrderItem().GetAll(OID);
            OrderItemList = OrderItemList.FindAll(x => x.TickedNo == TickedNo);
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
                TableScreen.Add("TableOrSheatName","Table No : "+TableObj.Table_or_SheetName +" "+ hG_Floor_Or_ScreenMaster.Name+" "+ hG_FloorSide_Or_RowName.FloorSide_or_RowName);
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
                string Msg = "Your Otp For FooDo App Is " + OTPNumber+"";
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

        public JArray TableAndTakeAway(int Type,int OrderById)
        {
            JArray jArray = new JArray();
            DateTime fromdate = DateTime.Now;
            List<HG_Tables_or_Sheat> list = new HG_Tables_or_Sheat().GetAllWithTakeAwya(Type);
            List<HG_Orders> Orderlist = new HG_Orders().GetListByGetDate(fromdate, DateTime.Now);

            foreach(var objtable in list)
            {
                HG_Orders order = Orderlist.Find(x => x.CID == OrderById && x.Table_or_SheatId == objtable.Table_or_RowID &&x.Status!="3");
                JObject jObject = new JObject();
                jObject= JObject.FromObject(objtable);
                jObject.Add("CurrOID", order!=null?order.OID:0);
                jArray.Add(jObject);
            }
            return jArray;
        }



        public JArray PastOrderMainList(int CID)
        {
            JArray Info = new JArray();
            
            List<HG_Orders> OrderList = new HG_Orders().GetAll(CID: CID);

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


        public JArray PastOrderSubList(int OID,string Status)
        {
            JArray Info = new JArray();
           HG_Orders orders = new HG_Orders().GetOne(OID);

            if (orders!= null && orders.Status==Status)
            {
              
                    HG_OrganizationDetails hG_OrganizationDetails = new HG_OrganizationDetails().GetOne(orders.OrgId);
                    JObject Object = new JObject();

         
                    List<HG_OrderItem> hG_OrderItems = new HG_OrderItem().GetAll(orders.OID);
                    List<HG_Items> ListfoodItems = new HG_Items().GetAll(orders.OrgId);
                   
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

            return Info;
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

       






    }
}
