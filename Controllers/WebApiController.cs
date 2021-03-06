﻿using System.Web.Mvc;
using HangOut.Models;
using HangOut.Models.Common;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
using HangOut.Models.DynamicList;
using System;
using System.Net;
using HangOut.Models.POS;
using paytm;
using HangOut.Models.MyCustomer;
using HangOut.Controllers.Account;

namespace HangOut.Controllers
{
    public class WebApiController : Controller
    {
        
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
                if (Objuser.OrgID > 0)
                {
                    HG_OrganizationDetails OrgObj = new HG_OrganizationDetails().GetOne(Objuser.OrgID);
                    Objuser.orgType = OrgObj.OrgTypes;
                    if (Objuser.IsHeadChef)
                        Objuser.UserType = "HCH";// head chef
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
        public void SendMsgCustomer(Int64 UserId,Int64 OrderNo=0,HG_Orders ObjOrder=null)
        {
            vw_HG_UsersDetails ObjUser = new vw_HG_UsersDetails().GetSingleByUserId((int)UserId);
            string[] topics = { UserId.ToString() };
            string Msg = "";
            string Title = "";
            if (ObjUser.RateNow == 0 &&ObjUser.UserType=="CUST")
            {

                // topics.Add(OrgId.ToString());
                Title = "foodDo";
                Msg = "Hey, give us a Hi-five. Click this notification";
                PushNotification.SendNotification(topics, Msg, Title, OID: 0,UserRating:1);
            }
            if (ObjUser.UserType == "CUST" && ObjOrder != null)
            {
                HG_Tables_or_Sheat ObjSeating = new HG_Tables_or_Sheat().GetOne(ObjOrder.Table_or_SheatId);
                if (ObjSeating.FDBKId > 0)
                {
                    Title = "foodDo";
                    Msg = "Just few seconds for Outlet feedback. Click here";
                    PushNotification.SendNotification(topics, Msg, Title, OID: OrderNo);
                }

            }
            // no notifiation only send orderId 




        }
        public JObject DeliveredToCustomer(string OID,string CustId)
        {
            Int64 Oid = Int64.Parse(OID);
            Int64 CustmerId = Int64.Parse(CustId);
            JObject result = new JObject();
            HG_Orders ObjOrder = new HG_Orders().GetOne(Oid);
            if (ObjOrder != null)
            {
                ObjOrder.OrderApprovlSts = 1;
                ObjOrder.Create_By = CustmerId;
                ObjOrder.Save();
                result.Add("Status", 200);
            }
            return result;
        }

        //=====================DISPALY ITEMS ADDCART AND MENU ITEMS============
        [HttpPost]
        public JArray GetItemList(string Obj)
        {
            JObject objParams = JObject.Parse(Obj);
            JArray JMenuArray = new JArray();
            Int64 CID = Int64.Parse(objParams.GetValue("CID").ToString());
            Int32 OrgId = Int32.Parse(objParams.GetValue("OrgId").ToString());
            List<HG_Items> ListItems = new HG_Items().GetAll(OrgId);
            ListItems = ListItems.FindAll(x => x.ItemAvaibility == 0);// only available items
            Int64 SeatingId =Int64.Parse(objParams.GetValue("TSTWID").ToString());
            HG_Tables_or_Sheat ObjTorS = new HG_Tables_or_Sheat().GetOne(SeatingId);
            List<Cashback> Cashbacks = Cashback.GetAll(OrgId, 1);// only actives
            Cashbacks = Cashbacks.FindAll(x => x.CashBkStatus == 1);// only running
            Cashbacks = Cashbacks.FindAll(x => x.CampeignType == 3);//offer cashback only
            Cashbacks = Cashbacks.FindAll(x => x.SeatingIds != "");
            Cashbacks = Cashbacks.FindAll(x => x.StartDate.Date <= DateTime.Now.Date && x.ValidTillDate.Date >= DateTime.Now.Date).ToList();
            for (int i = 0; i < Cashbacks.Count; i++)
            {
                List<int> seats = Cashbacks[i].SeatingIds.Split(',').Select(int.Parse).ToList();
                int seat = seats.Find(x => x == SeatingId);
                if (seat > 0)
                {
                    if (Cashbacks[i].OfferType == 1)
                    {
                        JObject offerItems = new JObject();
                        OfferTitle offerTitle = OfferTitle.GetOne(Cashbacks[i].CashBkId,1);
                        JObject JobjMenu = new JObject();
                        JArray jarrayItem = new JArray();
                        JobjMenu.Add("MenuId", offerTitle.TitleId);
                        JobjMenu.Add("OfferType", 1);
                        JobjMenu.Add("MaxOrdQty", offerTitle.MaxOrdQty);
                        JobjMenu.Add("Name", "Offer");
                        JobjMenu.Add("CBID", Cashbacks[i].CashBkId);
                        JobjMenu.Add("MenuIndex", 0);
                        int ItemiIndex = 0;
                        int menucnt = 0;
                        int CurrCount = 0;
                        double Price = offerTitle.FinalPrice;
                        if (offerTitle.KeepFixPrice==false)
                        {
                            Price = offerTitle.TotalItemPrice;
                        }
                        JObject objItem = new JObject();
                        objItem.Add("IID", offerTitle.TitleId);
                        objItem.Add("ItemName", offerTitle.Name);
                        objItem.Add("ItemPrice", Price);
                        objItem.Add("ItemQuntity", 0);
                        objItem.Add("ItemImage", "");
                        objItem.Add("ItemCartValue", CurrCount);
                        objItem.Add("MenuId", 0);
                        objItem.Add("ItemIndex", ItemiIndex++);
                        objItem.Add("ItemMode", 0);
                        objItem.Add("CostPrice", Price);// without gst
                        objItem.Add("Tax", 0);
                        objItem.Add("Info", "");
                        objItem.Add("MaxOrdQty", offerTitle.MaxOrdQty);
                        objItem.Add("KeepFixPrice", offerTitle.KeepFixPrice?1:0);
                        objItem.Add("Offer", "Offer");
                        menucnt++;
                        
                       
                        JArray Addons = new JArray();
                        foreach (var Menuobj in offerTitle.OfferMenus)
                        {
                            JObject jObject = new JObject();
                            jObject.Add("TitleId", Menuobj.MenuId);
                            jObject.Add("AddOnTitle", Menuobj.Name);
                            jObject.Add("Min", Menuobj.Min);
                            jObject.Add("Max", Menuobj.Max);
                            jObject.Add("CatOrItmId", offerTitle.TitleId);
                            jObject.Add("IsComplementry", Menuobj.IsComplementry);
                            JArray ItemAddon = new JArray();
                            foreach(var Addonitm in Menuobj.itemOffers)
                            {
                                JObject jObjectItm = new JObject();
                                jObjectItm.Add("AddOnItemId", Addonitm.ItemOfferId);
                                jObjectItm.Add("ItemId", Addonitm.ItemId);
                                jObjectItm.Add("CostPrice", Addonitm.TotalItemPrice);
                                jObjectItm.Add("Tax", 0);
                                jObjectItm.Add("Price", Addonitm.TotalItemPrice);
                                jObjectItm.Add("AddonID", offerTitle.TitleId);
                                jObjectItm.Add("DelStatus", false);
                                jObjectItm.Add("Title", Addonitm.ItemName);
                                jObjectItm.Add("Min", 0);
                                jObjectItm.Add("Max", 0);
                                ItemAddon.Add(jObjectItm);
                            }
                            jObject.Add("AddOnItemList", ItemAddon);
                            Addons.Add(jObject);
                        }
                        objItem.Add("Addons", Addons);
                        jarrayItem.Add(objItem);
                        JobjMenu.Add("MenuItemCount", menucnt);
                        JobjMenu.Add("MenuItems", jarrayItem);
                        JobjMenu.Add("MenuItmPrice", 0);
                        JMenuArray.Add(JobjMenu);
                    }
                    if (Cashbacks[i].OfferType == 2)
                    {
                        JObject offerItems = new JObject();
                        OfferTitle offerTitle = OfferTitle.GetOneByItems(Cashbacks[i].CashBkId);
                         JObject JobjMenu = new JObject();
                        JArray jarrayItem = new JArray();
                        JobjMenu.Add("MenuId", offerTitle.TitleId);
                        JobjMenu.Add("OfferType", 2);
                        JobjMenu.Add("Name", "Offer");
                        JobjMenu.Add("MaxOrdQty", offerTitle.MaxOrdQty);
                        JobjMenu.Add("Offer", "Offer");
                        JobjMenu.Add("CBID", Cashbacks[i].CashBkId);
                        JobjMenu.Add("MenuIndex", 0);
                        int ItemiIndex = 0;
                        int menucnt = 0;
                        int CurrCount = 0;
                        JObject objItem = new JObject();
                        objItem.Add("IID", offerTitle.TitleId);
                        objItem.Add("ItemName", offerTitle.Name);
                        objItem.Add("ItemPrice", offerTitle.FinalPrice);
                        objItem.Add("ItemQuntity", 0);
                        objItem.Add("ItemImage", "");
                        objItem.Add("ItemCartValue", CurrCount);
                        objItem.Add("MenuId", 0);
                        objItem.Add("ItemIndex", ItemiIndex++);
                        objItem.Add("ItemMode", 0);
                        objItem.Add("CostPrice", offerTitle.FinalPrice);// without gst
                        objItem.Add("Tax", 0);
                        objItem.Add("Info","");
                        objItem.Add("MaxOrdQty", offerTitle.MaxOrdQty);
                        objItem.Add("Offer", "Offer");
                        menucnt++;
                        jarrayItem.Add(objItem);
                        JobjMenu.Add("MenuItemCount", menucnt);
                        JobjMenu.Add("MenuItems", jarrayItem);
                        JobjMenu.Add("MenuItmPrice", 0);
                        JArray ItemArra = new JArray();
                        foreach(var itemobj in offerTitle.itemOffers)
                        {
                            JObject jObject = new JObject();
                            jObject.Add("ItmeId", itemobj.ItemId);
                            jObject.Add("Name", itemobj.ItemName);
                            jObject.Add("Count", itemobj.Min);
                            ItemArra.Add(jObject);
                        }
                        JobjMenu.Add("InfoItemList", ItemArra);
                        JMenuArray.Add(JobjMenu);
                    }
                    break;
                }
            }

            //  List<HG_Orders> TodaysOrder = new HG_Orders().GetListByGetDate(DateTime.Now, DateTime.Now);
            //HG_Orders ObjOrder = TodaysOrder.Find(x => x.Table_or_SheatId == TableSheatTakeWayId && x.TableOtp == ObjTorS.Otp);
            double CurrentTableAmt = 0.00;
            string Cmobile = "";
            string Cname = "";
            int ContactId = 0;
            if (ObjTorS.Type !="3")// not takeaway
            {
                OrderMenu ObjMenu = OrderMenu.Getone(ObjTorS.OMID);
                List<OrderMenuCategory> ListCategry = OrderMenuCategory.GetAll(ObjMenu.id);
                List<OrdMenuCtgItems> ListMenuItems = OrdMenuCtgItems.GetAll(ObjMenu.id);
                ListCategry = ListCategry.FindAll(x => x.Status == true);
                ListCategry = ListCategry.OrderBy(x => x.OrderNo).ToList();
                ListMenuItems = ListMenuItems.FindAll(x => x.Status == true);
                HashSet<int> ItemIdHash = new HashSet<int>(ListItems.Select(x => x.ItemID).ToArray());
                ListMenuItems = ListMenuItems.FindAll(x => ItemIdHash.Contains((int)x.ItemId));
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
                            int CurrCount = 0;
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
                            objItem.Add("CostPrice", Items.CostPrice);// without gst
                            objItem.Add("Tax", Items.Tax);
                            objItem.Add("Info", Items.ItemDiscription);
                            //check addon or Serving Size or Both apply in current item
                            List<AddOnn> Addons=  AddOns.GetAddonsAndMultiSSize(Items);
                            if (Addons.Count > 0)
                            {
                                objItem.Add("Addons", JArray.FromObject(Addons));
                            }
                            jarrayItem.Add(objItem);
                            MenuItemPrice += Items.Price * CurrCount;
                        }
                        JobjMenu.Add("MenuItemCount", OrderMenuItems.Count);
                        JobjMenu.Add("MenuItems", jarrayItem);
                        JobjMenu.Add("MenuItmPrice", MenuItemPrice);
                        JobjMenu.Add("TableAmt", CurrentTableAmt);
                        JobjMenu.Add("ContactId", ContactId);
                        JobjMenu.Add("Mobile", Cmobile);
                        JobjMenu.Add("CName", Cname);
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
                            int CurrCount = 0;
                            JObject objItem = new JObject();
                            objItem.Add("IID", Items.ItemID);
                            objItem.Add("ItemName", Items.Items);
                            objItem.Add("ItemPrice", Items.Price);
                            objItem.Add("ItemQuntity", Items.Qty);
                            objItem.Add("ItemImage", Items.Image);
                            objItem.Add("ItemCartValue", CurrCount);
                            objItem.Add("MenuId", Items.CategoryID);
                            objItem.Add("ItemIndex", ItemiIndex++);
                            objItem.Add("CostPrice", Items.CostPrice);// without gst
                            objItem.Add("Tax", Items.Tax);
                            objItem.Add("ItemMode", Items.ItemMode);
                            objItem.Add("Info", Items.ItemDiscription);
                            //check addon or Serving Size or Both apply in current item
                            List<AddOnn> Addons = AddOns.GetAddonsAndMultiSSize(Items);
                            if (Addons.Count > 0)
                            {
                                objItem.Add("Addons", JArray.FromObject(Addons));
                            }
                            jarrayItem.Add(objItem);
                            MenuItemPrice += Items.Price * CurrCount;
                        }
                        JobjMenu.Add("MenuItemCount", ItemListByMenu.Count);
                        JobjMenu.Add("MenuItems", jarrayItem);
                        JobjMenu.Add("MenuItmPrice", MenuItemPrice);
                        JobjMenu.Add("TableAmt", CurrentTableAmt);
                        JobjMenu.Add("ContactId", ContactId);
                        JobjMenu.Add("Mobile", Cmobile);
                        JobjMenu.Add("CName", Cname);
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
            int CustID = Int32.Parse(ParaMeters["CID"].ToString());
            int ItemId = Convert.ToInt32(ParaMeters["ItemId"].ToString());
            int Cnt = Convert.ToInt32(ParaMeters["Cnt"].ToString());
            Int64 TableSheatTakeWayId = Int64.Parse(ParaMeters.GetValue("TSTWID").ToString());
            HG_Items ObjSingleItem = new HG_Items().GetOne(ItemId);
                Cart ObjCart = Cart.List.Find(x => x.CID == CustID && x.ItemId == ItemId && x.TableorSheatOrTaleAwayId == TableSheatTakeWayId);
                if (ObjCart != null)
                {
                    ObjCart.Count = Cnt;
                    Cart.List.RemoveAll(x => x.CID == CustID && x.ItemId == ItemId && x.TableorSheatOrTaleAwayId == TableSheatTakeWayId);
                    if (ObjCart.Count != 0)
                        Cart.List.Add(ObjCart);
                }
                else
                { Cart.List.Add(new Cart() { CID = CustID, ItemId = ItemId, Count = Cnt, TableorSheatOrTaleAwayId = TableSheatTakeWayId }); }
            double TotalFinlAmt = 0;
            double Totaltax = 0.00;
            double Subtotal = 0.00;
            int Count = 0;
            List<Cart> CurrItemsOfUser = Cart.List.FindAll(x => x.CID == CustID && x.TableorSheatOrTaleAwayId == TableSheatTakeWayId);
            int CurrCount = CurrItemsOfUser.Count;
            foreach (Cart CartObj in CurrItemsOfUser)
            {
                HG_Items ObjItem = new HG_Items().GetOne(CartObj.ItemId);
                TotalFinlAmt += CartObj.Count * ObjItem.Price;
                Totaltax += OrgType.TotalTax(ObjItem.CostPrice, ObjItem.Tax, CartObj.Count); //ObjItem.Tax* CartObj.Count;
                Count += CartObj.Count;
                Subtotal+= CartObj.Count * ObjItem.CostPrice;
            }
            Cart CurrentItemobj = Cart.List.Find(x => x.CID == CustID && x.ItemId == ItemId && x.TableorSheatOrTaleAwayId == TableSheatTakeWayId);
            if (Cnt == 0)
                return Count + ","+TotalFinlAmt.ToString("0.00") + "," + ItemId+","+"0"+","+ Totaltax.ToString("0.00") + ","+Subtotal.ToString("0.00");
            return Count + "," + TotalFinlAmt.ToString("0.00") + "," + "0" + "," + CurrentItemobj.Count + "," +Totaltax.ToString("0.00") + ","+ Subtotal.ToString("0.00");
        }
        
        public JArray GetMenulist(string Obj)
        {    
            JObject ParaMeters = JObject.Parse(Obj);
            int OrgId = Convert.ToInt32(ParaMeters["OID"].ToString());
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
                HashSet<Int64> MenuItemIdsHash = new HashSet<Int64>(ListCatItems.Select(x => x.ItemId).ToArray());
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
                //ListOfItem = ListOfItem.FindAll(x =>!MenuItemIdsHash.Contains(x.ItemID));
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
        public JObject DeliveryCharge(string Obj)
        {
            JObject ParaMeters = JObject.Parse(Obj);
            Int64 CustID = Int64.Parse(ParaMeters["CID"].ToString());
            Int32 OrgId = Convert.ToInt32(ParaMeters["OrgId"].ToString());
            Int64 TableSheatTakeWayId = Int64.Parse(ParaMeters.GetValue("TSTWID").ToString());
            int AppType =3;//1 customer ,2 captain , 3 admin panel
            List<Cart> ListCart = Cart.List.FindAll(x => x.CID == CustID && x.TableorSheatOrTaleAwayId == TableSheatTakeWayId);
            List<HG_Items> ItemList = new HG_Items().GetAll(OrgId);
            OrgSetting orgSetting = OrgSetting.Getone(OrgId);
            JObject result = new JObject();
            double DeliveryChargeAmt = 0.00;
            if (orgSetting.EnblDeleryChrg == 1 && OrgType.DeliveryChargeAply(AppType, orgSetting))
            {
                double TotalCostPrice = 0.00;
                for (int i = 0; i <ListCart.Count; i++)
                {
                    HG_Items ObjItem = ItemList.Find(x => x.ItemID == ListCart[i].ItemId);
                    TotalCostPrice += ObjItem.CostPrice * ListCart[i].Count;
                }
                 if (orgSetting.AcptMinOrd == 1 && orgSetting.DeleryChrgType == 0 && orgSetting.MinOrderAmt > TotalCostPrice)
                {
                    DeliveryChargeAmt = orgSetting.DeliveryCharge;
                }
                else if (orgSetting.AcptMinOrd == 1 && orgSetting.DeleryChrgType == 1)// delivery charge fixed type
                {
                    DeliveryChargeAmt = orgSetting.DeliveryCharge;
                }
            }
            result.Add("ChargeAmt", DeliveryChargeAmt);
            result.Add("DeliveryType", orgSetting.DeleryChrgType);
            result.Add("MinAmt", orgSetting.MinOrderAmt);
            return result;
        }
        public JObject MakeOfflineOrd(string Obj)
        {
            Int64 CID = 0;
            try
            {
                JObject jObjectlist = JObject.Parse(Obj);
                string AppType = jObjectlist.GetValue("AppType").ToString();
                List<Cart> cartlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Cart>>(jObjectlist["OrderList"].ToString());
               // AddRange(localCarts.OrderList);
                Cart.List.AddRange(cartlist);
                Cart cart = cartlist.First();
                JObject jObject = new JObject();
                CID = cart.CID;
                jObject.Add("CID", cart.CID);
                jObject.Add("OrgID", cart.OrgId);
                jObject.Add("TORSID", cart.TableorSheatOrTaleAwayId);
                jObject.Add("AppType", AppType);
                int Status = jObjectlist["Status"] != null ? int.Parse(jObjectlist["Status"].ToString()) : 1;//"1":Order Placed,"2":Processing,3:"Completed" ,"4" :"Cancelled"
                int CustomerOrdering = jObjectlist["OrdingSts"] != null ? int.Parse(jObjectlist["OrdingSts"].ToString()) : 0;
                int PaymtSts = jObjectlist["PaymtType"] != null ? int.Parse(jObjectlist["PaymtType"].ToString()) : 0;//payment mode type
                int ContactId = jObjectlist["ContactId"] != null ? int.Parse(jObjectlist["ContactId"].ToString()) : 0;// local contact id
                int CashBKid= jObjectlist["CashBKid"] != null ? int.Parse(jObjectlist["CashBKid"].ToString()) : 0;// CashBack id
                int OfferDishCBID = jObjectlist["OfferDishCBID"] != null ? int.Parse(jObjectlist["OfferDishCBID"].ToString()) : 0;// CashBack id
                jObject.Add("Status", Status);
                jObject.Add("OrdingSts", CustomerOrdering);
                jObject.Add("PaymtType", PaymtSts);
                jObject.Add("ContactId", ContactId);
                jObject.Add("CashBKid", CashBKid);
                jObject.Add("OfferDishCBID", OfferDishCBID);
                JObject result = PostOrder(jObject.ToString());
                if (result.GetValue("Status").ToString() == "400")
                {
                    Cart.List.RemoveAll(x => x.CID == cart.CID  && x.TableorSheatOrTaleAwayId == cart.TableorSheatOrTaleAwayId);
                }
                return result;
            }
            catch(Exception e)
            {
                Cart.List.RemoveAll(x => x.CID == CID);
                JObject PostResult = new JObject();
                PostResult.Add("Status", 400);
                PostResult.Add("MSG", e.Message.ToString());
                
                return PostResult;
            }
            
        }
        // make order
        public JObject PostOrder(string Obj)
        {
            BalanceStatementController balanceStatement = new BalanceStatementController();
            JObject Params = JObject.Parse(Obj);
            Int64 CID = Int64.Parse(Params["CID"].ToString());
            int OrgId = int.Parse(Params["OrgID"].ToString());
            Int64 TableorSheatId=Int64.Parse(Params["TORSID"].ToString());
            int Status =Params["Status"]!=null?int.Parse(Params["Status"].ToString()):1;//"1":Order Placed,"2":Processing,3:"Completed" ,"4" :"Cancelled"
            int CustomerOrdering= Params["OrdingSts"] != null ? int.Parse(Params["OrdingSts"].ToString()) : 0;
            int AppType = Params["AppType"] != null ? int.Parse(Params["AppType"].ToString()) : 1;//1 customer ,2 captain , 3 admin panel
            int PaymtSts=Params["PaymtType"]!=null? int.Parse(Params["PaymtType"].ToString()) :0;//payment mode type
            int ContactId=Params["ContactId"]!=null? int.Parse(Params["ContactId"].ToString()) : 0;// local contact id
            int CashBKid = Params["CashBKid"] != null ? int.Parse(Params["CashBKid"].ToString()) : 0;// CashBack id
            int OfferDishCBID = Params["OfferDishCBID"] != null ? int.Parse(Params["OfferDishCBID"].ToString()) : 0;// CashBack id
            double DeliveryChargeAmt = 0.00;
            int ItemPrepaireBy = 0;
            HG_Tables_or_Sheat ObjTorS = new HG_Tables_or_Sheat().GetOne(TableorSheatId);
            HG_OrganizationDetails ObjOrg = new HG_OrganizationDetails().GetOne(OrgId);
            List<HG_Items> ItemList = new HG_Items().GetAll(OrgId);
            List<HG_Orders> ListOfOrder = new HG_Orders().GetListByGetDate(DateTime.Now, DateTime.Now);
            ListOfOrder = ListOfOrder.FindAll(x => x.OrgId == OrgId);
            HG_Orders ObjOrders = ListOfOrder.Find(x => x.Table_or_SheatId == TableorSheatId && x.TableOtp == ObjTorS.Otp);
            JObject PostResult = new JObject();
            List<Cart> ListCart = Cart.List.FindAll(x => x.CID == CID  && x.TableorSheatOrTaleAwayId==TableorSheatId);
            ListCart = ListCart.FindAll(x => x.Count > 0);
            OrgSetting orgSetting = OrgSetting.Getone(ObjOrg.OrgID);
            Int64 OID = 0;
            if (ObjOrders==null||ObjOrders.Status=="3"|| ObjOrders.Status == "4"){// if order is completed or Canceled then Take New order

                OID = 0;
            }
            else if (ObjOrders != null && ObjOrders.PaymentStatus != 0)
            {
                PostResult.Add("Status", 400);
                PostResult.Add("MSG", "Can't Modify Order After Payment ! First Complete Pending Order");
                return PostResult;
            }
            else
            {
                OID = ObjOrders.OID;
            }
            //check customer ordering enable
            if (ObjOrg.CustomerOrdering==false&& CustomerOrdering==0)
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
            if (orgSetting.EnblDeleryChrg == 1 &&OrgType.DeliveryChargeAply(AppType,orgSetting))
            {
                double TotalCostPrice = 0.00;
                for(int i=0;i< ListCart.Count; i++)
                {
                    HG_Items ObjItem = ItemList.Find(x => x.ItemID == ListCart[i].ItemId);
                    TotalCostPrice += ObjItem.CostPrice * ListCart[i].Count;
                }
                if (orgSetting.AcptMinOrd==0&&orgSetting.MinOrderAmt > TotalCostPrice)
                {
                    PostResult.Add("Status", 400);
                    PostResult.Add("MSG", "Minimum ticket value for this outlet is Rs "+ orgSetting.MinOrderAmt.ToString("0.00"));
                    return PostResult;
                }
                else if(orgSetting.AcptMinOrd==1 && orgSetting.DeleryChrgType==0&& orgSetting.MinOrderAmt > TotalCostPrice)
                {
                    DeliveryChargeAmt = orgSetting.DeliveryCharge;
                }
                else if(orgSetting.AcptMinOrd == 1 && orgSetting.DeleryChrgType ==1)// delivery charge fixed type
                {
                    DeliveryChargeAmt= orgSetting.DeliveryCharge;
                }
            }
            if (ObjOrg.OrderDisplay == 2)// check KOT mode enable
            {
                Status = 3;// mark complete all items
            }
            if (Status == 3 && (AppType == 2 || AppType == 3)){
                ItemPrepaireBy =(int) CID;
            }
            if (PymentPageOpen.ListPytmPgOpen.Find(x => x.OID==OID) != null)
            {
                PostResult.Add("Status", 400);
                PostResult.Add("MSG", "Can't Change Order After Redirect To Payment Page");
                return PostResult;
            }


            //=========Order Logic Here===================
            string OrderSts = "1"; //placed by defualt
            if (Status == 3 && PaymtSts > 0)
            {
                if (OID > 0)
                {
                    List<HG_OrderItem> ListOrderItems = new HG_OrderItem().GetAll(OID);
                    if (ListOrderItems.Count > 0)
                    {
                        var CanclAndComleted = ListOrderItems.FindAll(x => x.Status == 3 || x.Status == 4);
                        if (CanclAndComleted.Count == ListOrderItems.Count)
                        {
                            OrderSts = "3";//completed
                        }
                    }
                }
                else
                {
                    OrderSts = "3";//completed
                }
                
            }
            Int64 NewOID = 0;
            if (OID > 0)
            {
                NewOID = OID;
                ObjOrders.Status = OrderSts;
                ObjOrders.Update_By = CID;
                ObjOrders.DisntChargeIDs = ObjOrders.DisntChargeIDs;
                ObjOrders.Update_Date = DateTime.Now;
                ObjOrders.DeliveryCharge = ObjOrders.DeliveryCharge + DeliveryChargeAmt;
                ObjOrders.PaymentStatus = PaymtSts;
                ObjOrders.PayReceivedBy = (int)CID;
                ObjOrders.ContactId = ContactId<=0?0:ContactId;// -1 contact id for Customer Order foodo app
                ObjOrders.OfferDishCBID = OfferDishCBID > 0 ? OfferDishCBID : ObjOrders.OfferDishCBID;
                ObjOrders.Save();
            }
            else
            {
                ObjOrders = new HG_Orders()
                {
                    Create_By = CID,
                    Create_Date = DateTime.Now,
                    Update_Date = DateTime.Now,
                    CID = CID,
                    Update_By = CID,
                    Status = OrderSts,
                    OrgId = OrgId,
                    Table_or_SheatId = TableorSheatId,
                    PaymentStatus = PaymtSts,
                    TableOtp = ObjTorS.Otp,
                    PayReceivedBy = (int)CID,
                    OrderApprovlSts = 0,
                    DeliveryCharge = DeliveryChargeAmt,
                   ContactId = ContactId <= 0 ? 0 : ContactId// -1 contact id for Customer Order foodo app
            };
                NewOID= ObjOrders.Save();
                ChampeignCharge.ChargeCamp(NewOID, CashBKid, OrgId);
            }
                if (NewOID > 0)
                {
                List<HG_Ticket> list = new HG_Ticket().GetAll(OrgId);
                HG_Ticket objticket = new HG_Ticket() {OrgId=OrgId,OID=NewOID,TicketNo=list.Count+1,DeliveryCharge=DeliveryChargeAmt };
                int Ticketno = objticket.save();
                foreach (Cart Item in ListCart)
                    {
                    HG_Items ObjItem = ItemList.Find(x => x.ItemID == Item.ItemId);
                    string IsAdon = "0";
                    if (Item.IsAddon == 1 || Item.IsParcel == 1)
                    {
                        IsAdon = "1";
                    }
                    if (OfferDishCBID > 0)
                    {
                        ObjItem.Price = 0;
                        ObjItem.CostPrice = 0;
                        ObjItem.Tax = 0;
                    }
                    HG_OrderItem OrderItem = new HG_OrderItem()
                    {
                        FID = ObjItem.ItemID,
                        Price = ObjItem.Price,
                        Count = Item.Count,
                        IsAddon = IsAdon,
                        OID = NewOID,
                        Status = Status,
                        TickedNo = Ticketno,
                        OrgId = OrgId,
                        ChefSeenBy = ItemPrepaireBy,
                        OrderDate = DateTime.Now,
                        UpdatedBy=0,
                        UpdationDate=DateTime.Now,
                        OrdById = CID,
                        TaxInItm=ObjItem.Tax,
                        CostPrice=ObjItem.CostPrice
                    };
                    if (OrderItem.Save() <= 0)
                    {
                        HG_Orders order = new HG_Orders();
                        order.DeleteOrderAndOrderItem(NewOID, false);
                        PostResult.Add("Status", 400);
                        PostResult.Add("MSG", "Can't Confirm Order Try After Some Time.");
                        return PostResult;
                    }
                    //check addon items exist
                    if (Item.itemAddons != null && Item.itemAddons.AddonItemId.Count > 0)
                    {
                        string AddonItemId = String.Join(",", Item.itemAddons.AddonItemId.Select(x => x.ToString()).ToArray());
                        List<AddOnItems> AddonItems = AddOnItems.GetAll(AddonItemId);
                        foreach (AddOnItems addOnItems in AddonItems)
                        {
                            OrderAdonItm OrdAddonItm = new OrderAdonItm();
                            OrdAddonItm.OID = NewOID;
                            OrdAddonItm.OIID = OrderItem.OIID;
                            OrdAddonItm.AdddOnItemId = addOnItems.AddOnItemId;
                            OrdAddonItm.ItemId = addOnItems.ItemId;
                            OrdAddonItm.Tax = addOnItems.Tax;
                            OrdAddonItm.CostPrice = addOnItems.CostPrice;
                            OrdAddonItm.Price = addOnItems.Price;
                            OrdAddonItm.Save();
                        }
                    }
                    if (Item.IsParcel == 1)
                    {//make one parcel entry
                        double taxableAmt = (orgSetting.ParcelAmt * orgSetting.ParcelTax) / 100;
                        double ParcelPrice = orgSetting.ParcelAmt + taxableAmt;
                        OrderAdonItm OrdAddonItm = new OrderAdonItm();
                        OrdAddonItm.OID = NewOID;
                        OrdAddonItm.OIID = OrderItem.OIID;
                        OrdAddonItm.AdddOnItemId = 0;
                        OrdAddonItm.ItemId = 0;
                        OrdAddonItm.Tax = orgSetting.ParcelTax;
                        OrdAddonItm.CostPrice = orgSetting.ParcelAmt;
                        OrdAddonItm.Price = orgSetting.ParcelAmt+taxableAmt;
                        OrdAddonItm.Save();
                    }

                }
                
                OrdDiscntChrge.RemoveDiscntCharge(ObjOrders.Table_or_SheatId, ObjOrders.TableOtp,ObjOrders.OID);
                if (OrderSts == "3")
                {
                    ObjTorS.Status = 1;// free table
                    ObjTorS.Otp = OTPGeneretion.Generate();
                    ObjTorS.save();
                }
                PostResult.Add("Status", 200);
                PostResult.Add("MSG",NewOID.ToString()+","+Ticketno.ToString()+","+PaymtSts.ToString());
                PostResult.Add("DeliveryChrge", DeliveryChargeAmt);
                PostResult.Add("OrderSts", OrderSts);
                //send firebase massage new ticket assign
                string[] topics = { OrgId.ToString(), "0" };
                if (ObjOrg.PaymentType == 2 &&ObjTorS.Type!="3")// postpaid
                {
                    SendMsgChef(OrgId, NewOID,Ticketno);

                   PushNotification.NewOrderMsg(topics, NewOID, Ticketno);
                }
                else if (PaymtSts > 0 && ObjTorS.Type != "3")
                {
                    SendMsgChef(OrgId, NewOID,Ticketno);
                    PushNotification.NewOrderMsg(topics, NewOID, Ticketno);
                }

                if (ObjOrg.OrderDisplay == 2 && AppType != 3 && ObjOrg.PrinttingType == 2 && (PaymtSts > 0 || ObjOrg.PaymentType == 2))
                {
                    PendingPrints.SaveKotPrint(ObjOrders, ObjOrg.Copy, Ticketno);
                }
                if(ObjOrders.Status=="3")
                {
                    //for balance Statement
                    List<HG_OrderItem> OrdrItmsList = new List<HG_OrderItem>();
                    OrdrItmsList = new HG_OrderItem().GetAll(ObjOrders.OID);
                    List<HG_OrderItem> CompletedItems = OrdrItmsList.FindAll(x => x.Status == 3);
                    //==============
                    //=======balanceStatement========
                    try
                    {
                        balanceStatement.GetDetails(CompletedItems);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            else
            {
                PostResult.Add("Status", 400);
                PostResult.Add("MSG", "Unable To Place Order Try Again.");
                return PostResult;
            }
            Cart.List.RemoveAll(x => x.CID == CID &&x.TableorSheatOrTaleAwayId==TableorSheatId);
            return PostResult;
        }

        //cancel order
        public JObject CancelOrder(Int64 OID ,int UpdatedBy = 0)
        {
            JObject result = new JObject();
            HG_Orders hG_Orders = new HG_Orders().GetOne(OID);
            OrdNotice ordNotice = OrdNotice.GetOne(OID);
            if (hG_Orders.OID < 1)
            {
                result.Add("Status", 400);
                result.Add("MSG", "Order Not Found");
                return result;
            }
            if (hG_Orders.Status == "4")
            {
                result.Add("Status", 400);
                result.Add("MSG", "Already Canceled");
                return result;
            }
            if (hG_Orders.Status == "3"&& (ordNotice==null||ordNotice.OID==0))
            {
                result.Add("Status", 400);
                result.Add("MSG", "Can't Cancel Order. Order Already Completed");
                return result;
            }
            if (hG_Orders.PaymentStatus ==1 &&(ordNotice == null || ordNotice.OID == 0))
            {
                result.Add("Status", 400);
                result.Add("MSG", "Can't Cancel Order. Payment Has Been Done");
                return result;
            }
            if (hG_Orders.PaymentStatus ==2 || hG_Orders.PaymentStatus==3)
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
                hG_Orders.PaymentStatus = 0;
                hG_Orders.DeliveryCharge = 0;
                HG_Tables_or_Sheat ObjTorS = new HG_Tables_or_Sheat().GetOne(hG_Orders.Table_or_SheatId);
                if (ObjTorS != null &&hG_Orders.Create_Date.Date==DateTime.Now.Date&&hG_Orders.Status!="3")
                {
                    ObjTorS.Status = 1;// free table
                    ObjTorS.Otp = OTPGeneretion.Generate();
                    ObjTorS.save();
                }
                var OrderItem = new HG_OrderItem().GetAll(hG_Orders.OID);
                foreach(var ObjOitem in OrderItem)
                {
                    ObjOitem.Status = 4;//cancel all items
                    ObjOitem.UpdatedBy = UpdatedBy;
                    ObjOitem.Save();

                }
                hG_Orders.Save();
                OrdNotice.ChangeAlertSts(OID, 1, 1);
                result.Add("Status", 200);
                return result;
            }
        }
        public JObject ShowOrderItems(int TOrSId,int OrgId=0)
        {
            JObject jObject = new JObject();
            List<HG_Orders> ListOrders = new HG_Orders().GetListByGetDate(DateTime.Now, DateTime.Now);
            ListOrders = ListOrders.FindAll(x => x.Table_or_SheatId == TOrSId &&(x.Status=="1"||x.Status=="2"));// placed or Processing
            ListOrders= ListOrders.FindAll(x => x.PaymentStatus == 0);
            HG_Orders order  = ListOrders.FirstOrDefault();
            List<HG_OrderItem> listitems = new List<HG_OrderItem>();
            double TotalPrice = 0.00;
            double CostPrice = 0.00;
            double Totaltax = 0.00;
            List<HG_Items> items = new HG_Items().GetAll(OrgId);
            if (order != null)
            {
                listitems.AddRange(new HG_OrderItem().GetAll(order.OID));
                TotalPrice = order.DeliveryCharge;
            }
            JArray jArray = new JArray();
            foreach (var OrderItm in listitems)
            {
                TotalPrice += (OrderItm.Count * OrderItm.Price);
                CostPrice += (OrderItm.Count * OrderItm.CostPrice);
                Totaltax += OrgType.TotalTax(OrderItm.CostPrice, OrderItm.TaxInItm, OrderItm.Count);

                JObject jobj = JObject.FromObject(OrderItm);
                HG_Items hG_Items = items.Find(x => x.ItemID == OrderItm.FID);
                jobj.Add("ItemName", hG_Items.Items);
                if (OrderItm.IsAddon == "1")
                {
                    List<OrderAdonItm> listaddonitems = OrderAdonItm.GetAll(OrderItm.OIID);
                    foreach (var addonitm in listaddonitems)
                    {
                        Totaltax += OrgType.TotalTax(addonitm.CostPrice, addonitm.Tax, OrderItm.Count);
                        CostPrice += (OrderItm.Count * addonitm.CostPrice);
                        TotalPrice += (OrderItm.Count * addonitm.Price);
                    }
                    jobj.Add("AddonItems", JArray.FromObject(listaddonitems));
                }
                jArray.Add(jobj);
            }
            if (listitems.Count == 0)
            {
                HG_Tables_or_Sheat objTorS = new HG_Tables_or_Sheat().GetOne(TOrSId);
                jObject.Add("Status", 400);
                jObject.Add("TorSsts", objTorS.Status);
                jObject.Add("MSG", "Make Order First");
            }
            else
            {
                jObject.Add("Status", 200);
                jObject.Add("ListItems", jArray);
                jObject.Add("CostPrice", CostPrice.ToString("0.00"));
                jObject.Add("Tax", Totaltax.ToString("0.00"));
                jObject.Add("Total",TotalPrice.ToString("0.00"));
                jObject.Add("DeliveryChage", order.DeliveryCharge);
                jObject.Add("OID", order.OID);
            }
            

            return jObject;
        }
        public JObject CompleteOrder(int PaymentType,int UpdatedBy, Int64 OID = 0, int TorSid = 0,int AppType=0)
        {
            JObject jObject = new JObject();
            JournalEntryController journalControllerObj = new JournalEntryController();
            BalanceStatementController balanceStatement = new BalanceStatementController();
            HG_Orders order = new HG_Orders();
            HG_Tables_or_Sheat obj = new HG_Tables_or_Sheat();
            List<HG_OrderItem> OrdrItmsList = new List<HG_OrderItem>();
            if (OID > 0)
            {
                order = new HG_Orders().GetOne(OID);
                obj = new HG_Tables_or_Sheat().GetOne(order.Table_or_SheatId);
                
            }
            else if (TorSid > 0)
            {
                obj = new HG_Tables_or_Sheat().GetOne(TorSid);
              var  OrderList = new HG_Orders().GetListByGetDate(DateTime.Now, DateTime.Now);
                order = OrderList.Find(x => x.Table_or_SheatId == TorSid && x.TableOtp==obj.Otp);
            }
            HG_OrganizationDetails ObjOrg = new HG_OrganizationDetails().GetOne(obj.OrgId);
            bool Status = false;
            int ChangeOtpTbl = 0;
            Int64 OrdId = 0;
            if(order!=null&&order.OID>0)
            {
                if (ObjOrg.PaymentType == 1)// prepaid case only accept payment 
                {
                    //send msg to chef
                    order.PaymentStatus = PaymentType;
                    order.Update_By = UpdatedBy;
                    order.PayReceivedBy = UpdatedBy;
                    Status = true;
                    OrdrItmsList=new HG_OrderItem().GetAll(order.OID);
                    List<HG_OrderItem> CompletedItems = OrdrItmsList.FindAll(x => x.Status == 3);
                     var CompltedOrCacelOdrItms = OrdrItmsList.FindAll(x => x.Status == 3 || x.Status == 4);
                    if (CompltedOrCacelOdrItms.Count == OrdrItmsList.Count)
                    {
                        obj.Status = 1;// free table
                        obj.Otp = OTPGeneretion.Generate();
                        obj.save();
                        order.Status = "3";//completed
                        ChangeOtpTbl = 1;
                        
                        //=======Journal Entry======
                        //try
                        //{
                        //    balanceStatement.GetDetails(CompletedItems);
                        //}
                        //catch (Exception ex)
                        //{

                        //}
                        ///==============
                       
                        if (obj.Type != "3")
                        {
                            SendMsgCustomer(order.CID, order.OID,order);
                        }
                        
                    }
                    else if(obj.Type != "3")
                    {
                        SendMsgChef(ObjOrg.OrgID, order.OID);
                        string[] topics = { "0", ObjOrg.OrgID.ToString() };
                        PushNotification.NewOrderMsg(topics, order.OID, 0);
                    }
                    if (ObjOrg.OrderDisplay == 2 && AppType != 3 &&ObjOrg.PrinttingType==2)
                    {
                        PendingPrints.SaveKotPrint(order, ObjOrg.Copy,hG_OrderItems: OrdrItmsList);
                    }
                    order.Save();
                    Wallet.AddToWallet(order, AppType);
                }
                else
                {
                    if (order.OID > 0 && obj.Table_or_RowID > 0)
                    {
                        var OrderItems = new HG_OrderItem().GetAll(order.OID);
                        order.Update_By = UpdatedBy;
                        order.PayReceivedBy = UpdatedBy;
                        order.PaymentStatus = PaymentType;// update payment status
                        //for balance Statement
                        List<HG_OrderItem> CompletedItems = OrdrItmsList.FindAll(x => x.Status == 3);
                        //==============
                        var CompletedOrCancelItems = OrderItems.FindAll(x => x.Status == 3 || x.Status == 4);// completed or canceled
                        if (OrderItems.Count == CompletedOrCancelItems.Count)
                        {
                            obj.Status = 1;// free table
                            obj.Otp = OTPGeneretion.Generate();
                            obj.save();
                            ChangeOtpTbl = 1;
                            order.Status = "3";//3 order completed
                            
                            //=======Journal Entry======
                            //try
                            //{
                            //    balanceStatement.GetDetails(CompletedItems);
                            //}
                            //catch (Exception ex)
                            //{

                            //}
                            ///==============
                            if (obj.Type != "3")
                            {
                                SendMsgCustomer(order.CID, order.OID,order);
                            }

                        }
                        Status = true;
                        order.Save();
                        Wallet.AddToWallet(order, AppType);
                    }
                    else
                    {
                        Status = false;
                    }
                }
                OrdId = order.OID;
                if (AppType != 3 && ObjOrg.InvoicePrintting == 2)
                {
                    PendingPrints.SaveInvoicePrint(order, ObjOrg.NuOfCopy);
                }
            }
            if (Status)
            {
              
                jObject.Add("Status", 200);
                jObject.Add("MSG", obj.Otp);
                jObject.Add("OID", OrdId);
                jObject.Add("OrdSts", order.Status);
                jObject.Add("ChangeOtp", ChangeOtpTbl);
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
          var  CurrentOrder = Orders.FindAll(x => x.Status != "3" &&x.Status!="4");//not completed not canceled
            CurrentOrder = CurrentOrder.FindAll(x => x.OrgId == OrgId);
            HG_OrganizationDetails orgobj = new HG_OrganizationDetails().GetOne(OrgId);
            List<HG_OrderItem> hG_OrderItems = new List<HG_OrderItem>();
            if (orgobj != null && orgobj.PaymentType == 1)// prepaid and is chef orders
            {
                if (IsChef == 1)
                {
                    CurrentOrder = CurrentOrder.FindAll(x => x.PaymentStatus > 0);// only seen paid orders
                    OrderToShow = CurrentOrder;
                    foreach (var Order in CurrentOrder)
                    {
                        var OrderItms = new HG_OrderItem().GetAll(Order.OID);
                        hG_OrderItems.AddRange(OrderItms);
                    }
                }
                else
                {
                  
                    CurrentOrder = CurrentOrder.FindAll(x => x.PaymentStatus ==0);// only seen unpaid orders
                    OrderToShow = CurrentOrder;

                    foreach(var Order in CurrentOrder)
                    {
                        var OrderItms = new HG_OrderItem().GetAll(Order.OID);
                        hG_OrderItems.AddRange(OrderItms);
                    }
                    
                }
            }
            else// postpaid
            {
                if (IsChef == 1)
                {
                    //Orders = Orders.FindAll(x => x.Status=="1");// only seen placed ordersw
                    foreach(var Order in CurrentOrder)
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
                    foreach (var Order in CurrentOrder)
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
                ShowOrderItems = ShowOrderItems.FindAll(x => x.Status != 4);//not canceled
                double ToTalAmt = order.DeliveryCharge;
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
        //Start Chef End Work=== single chef
        public JArray ChefOrders(int OrgId,int ChefId,int Status)
        {
            JArray tableorSheatList = new JArray();
            try
            {
                List<HG_Orders> Orderlist = new List<HG_Orders>();
                HG_OrganizationDetails ObjOrg = new HG_OrganizationDetails().GetOne(OrgId);
                int OrgType = int.Parse(ObjOrg.OrgTypes);
                List<HG_OrderItem> OrderItemList = new HG_OrderItem().GetAllByOrg(OrgId,ChefId);
                if (Status == 0)
                {
                    OrderItemList = OrderItemList.FindAll(x => x.Status != 3 && x.Status != 4);
                    OrderItemList = OrderItemList.FindAll(x => x.OrderDate.Date == DateTime.Now.Date).ToList();
                    OrderItemList = OrderItemList.OrderBy(x => x.TickedNo).ToList();
                    if (ObjOrg.PaymentType == 1)
                    {
                        var OrderItems = OrderItemList;
                        foreach (var Orderitm in OrderItems)
                        {
                            HG_Orders order = new HG_Orders().GetOne(Orderitm.OID);
                            if (order != null && order.PaymentStatus != 0)
                            {
                                Orderlist.Add(order);
                                OrderItemList = OrderItemList.FindAll(x=>x.TickedNo==Orderitm.TickedNo);
                                break;
                            }
                        }
                    }
                    else
                    {
                        var ObjItem = OrderItemList.First();
                       OrderItemList = OrderItemList.FindAll(x => x.TickedNo == ObjItem.TickedNo);
                        HG_Orders order = new HG_Orders().GetOne(ObjItem.OID);
                        Orderlist.Add(order);
                    }
                }
            List<HG_Items> ListfoodItems = new HG_Items().GetAll(OrgId);
                int TorSIndex = 0;
                foreach (var order in Orderlist)
                {
                    string Seating = "";
                    HG_Tables_or_Sheat hG_Tables_Or_Sheat =new HG_Tables_or_Sheat().GetOne(order.Table_or_SheatId);
                    if (hG_Tables_Or_Sheat != null&& hG_Tables_Or_Sheat.Table_or_RowID>0)
                    {
                        HG_Floor_or_ScreenMaster hG_Floor_Or_ScreenMaster = new HG_Floor_or_ScreenMaster().GetOne(hG_Tables_Or_Sheat.Floor_or_ScreenId);
                        if (hG_Floor_Or_ScreenMaster != null&& hG_Floor_Or_ScreenMaster.Floor_or_ScreenID>0)
                        {
                            Seating = hG_Floor_Or_ScreenMaster.Name;
                        }
                        HG_FloorSide_or_RowName hG_FloorSide_Or_RowName = new HG_FloorSide_or_RowName().GetOne(hG_Tables_Or_Sheat.FloorSide_or_RowNoID);
                        if (hG_FloorSide_Or_RowName != null && hG_FloorSide_Or_RowName.ID>0)
                        {
                            Seating += " " + hG_FloorSide_Or_RowName.FloorSide_or_RowName;
                        }
                        Seating += " " + hG_Tables_Or_Sheat.Table_or_SheetName;
                    }
                    JObject TableScreen = new JObject();
                    var hG_OrderItems = OrderItemList.FindAll(x => x.OID == order.OID);
                    JArray ItemsArray = new JArray();
                    int ticketno = 0;
                    int ItemIndex = 0;
                    double TotalAmount = 0.00;
                    Int64 OrderById = 0;
                    List<vw_HG_UsersDetails> ListUsers = new vw_HG_UsersDetails().GetAll(OrgId: OrgId);
                    foreach (var OrderItem in hG_OrderItems)
                    {
                       
                        double Amount = OrderItem.Price * OrderItem.Count;
                        TotalAmount += Amount;
                        ticketno = OrderItem.TickedNo;
                        HG_Items hG_Items = ListfoodItems.Find(x => x.ItemID == OrderItem.FID);
                        JObject itemobj = new JObject();
                        itemobj.Add("OIID", OrderItem.OIID);
                        itemobj.Add("ItemID", OrderItem.FID);
                        itemobj.Add("ItemName", hG_Items.Items);
                        itemobj.Add("Quantity",OrderItem.Count);
                        itemobj.Add("Status", OrderItem.Status);
                        itemobj.Add("IIndex", ItemIndex++);
                        itemobj.Add("ItmAmt", Amount);
                        ItemsArray.Add(itemobj);
                        OrderById = OrderItem.OrdById;
                    }
                    OrdNotice OrderNotice = OrdNotice.GetOne(order.OID);
                    string name = Seating + " Ticket no. :" + ticketno;
                    double deliveryCharge = 0.00;
                    vw_HG_UsersDetails objOrderBy = new vw_HG_UsersDetails().GetSingleByUserId((int)OrderById);
                    if (OrderNotice!=null&& OrderNotice.OID>0)
                    {
                        if (order.DeliveryCharge > 0)
                        {
                            List<HG_Ticket> Tickets = HG_Ticket.GetByOID(order.OID);
                            var ObjTicket = Tickets.Find(x => x.TicketNo == ticketno);
                            if (ObjTicket != null && ObjTicket.TicketNo > 0)
                            {
                                deliveryCharge = ObjTicket.DeliveryCharge;
                            }
                        }
                        vw_HG_UsersDetails ObjUser = new vw_HG_UsersDetails();
                        if (order.PayReceivedBy > 0)
                        {
                            ObjUser = new vw_HG_UsersDetails().GetSingleByUserId(order.PayReceivedBy);
                        }
                        if (ObjUser != null && ObjUser.UserType != "CA"&&ObjUser.UserType!="ONR")// not captain not OWN
                        {
                            TableScreen.Add("Amt", TotalAmount);
                        }
                        else if (ObjUser == null||ObjUser.UserCode<=0)// if CASH PAY BY CUSTOMER
                        {
                            TableScreen.Add("Amt", TotalAmount);
                        }
                        else
                        {
                            TableScreen.Add("Amt", 0);
                        }
                        
                    }
                    else
                    {
                        TableScreen.Add("Amt", 0);
                    }
                    TableScreen.Add("PymtMode", order.PaymentStatus);
                    TableScreen.Add("deliveryCharge", deliveryCharge);
                    TableScreen.Add("TableScreenInfo", name);
                    TableScreen.Add("TableSeatID", hG_Tables_Or_Sheat.Table_or_RowID);
                    TableScreen.Add("TicketNo", ticketno);
                    TableScreen.Add("OID", order.OID);
                    TableScreen.Add("OrderItems", ItemsArray);
                    TableScreen.Add("TorSIndex", TorSIndex++);
                    TableScreen.Add("CustomerMobile", objOrderBy.UserId);
                    TableScreen.Add("CustomerName", objOrderBy.UserName);
                    TableScreen.Add("ChefName", "");
                    TableScreen.Add("OrdTime", order.Create_Date.ToString("dd-MM hh:mm tt"));
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
            catch(Exception e)
            {

            }
            return tableorSheatList;

        }
        public JArray HedChefOrders(int OrgId)
        {
            JArray tableorSheatList = new JArray();
            try
            {
                List<HG_Orders> Orderlist = new List<HG_Orders>();
                HG_OrganizationDetails ObjOrg = new HG_OrganizationDetails().GetOne(OrgId);
                int OrgType = 1;
                if (ObjOrg != null && ObjOrg.OrgTypes == "2")
                {
                    OrgType = 2;
                }
                List<HG_Tables_or_Sheat> ListTorS = new HG_Tables_or_Sheat().GetAll(OrgType,OrgId);
                List<HG_FloorSide_or_RowName> ListFsorRowNa = new HG_FloorSide_or_RowName().GetAll(OrgType,OrgId);
                List<HG_Floor_or_ScreenMaster> ListFrorScr = new HG_Floor_or_ScreenMaster().GetAll(OrgType,OrgId);
                List<HG_OrderItem> OrderItems = new HG_OrderItem().GetAllByOrg(OrgId, ChefId: 0, Status: "(Status=1 or Status=2)");
                List<HG_Orders> PendingOrders = new HG_Orders().GetAll(OrgId:OrgId,Status:1);
                if (ObjOrg.PaymentType == 1)
                {
                    PendingOrders = PendingOrders.FindAll(x => x.PaymentStatus > 0);
                }
                List<vw_HG_UsersDetails> ListUsers = new vw_HG_UsersDetails().GetAll(OrgId: OrgId);
                HashSet<Int64> OIDHash = new HashSet<Int64>(PendingOrders.Select(x => x.OID).ToArray());
                OrderItems = OrderItems.FindAll(x => OIDHash.Contains(x.OID));
                List<HG_Items> ListfoodItems = new HG_Items().GetAll(OrgId);
                var GroupByTicketNo = OrderItems.GroupBy(x =>new { x.TickedNo,x.OID});
                int TorSIndex = 0;
                foreach (var TicktObj in GroupByTicketNo)
                {
                    JObject TableScreen = new JObject();
                    JArray ItemsArray = new JArray();
                    var hG_OrderItems = TicktObj.ToList();
                    int ticketno = 0;
                    int ItemIndex = 0;
                    int ChefSeenId = 0;
                    double TotalAmount = 0.00;
                    Int64 OID = 0;
                    Int64 OrderById = 0;
                    foreach (var OrderItem in hG_OrderItems)
                    {
                        double Amount = OrderItem.Price * OrderItem.Count;
                        TotalAmount += Amount;
                        ticketno = OrderItem.TickedNo;
                        HG_Items hG_Items = ListfoodItems.Find(x => x.ItemID == OrderItem.FID);
                        JObject itemobj = new JObject();
                        itemobj.Add("OIID", OrderItem.OIID);
                        itemobj.Add("ItemID", OrderItem.FID);
                        itemobj.Add("ItemName", hG_Items.Items);
                        itemobj.Add("Quantity",OrderItem.Count);
                        itemobj.Add("Status", OrderItem.Status);
                        itemobj.Add("IIndex", ItemIndex++);
                        itemobj.Add("OID", OrderItem.OID);
                        itemobj.Add("ItmAmt", Amount);
                        ItemsArray.Add(itemobj);
                        OID = OrderItem.OID;
                        ChefSeenId = OrderItem.ChefSeenBy;
                        OrderById = OrderItem.OrdById;
                    }
                    var Chefname = "";
                    if (ChefSeenId > 0)
                    {
                        var ObjChef = ListUsers.Find(x => x.UserCode == ChefSeenId);
                        if (ObjChef != null)
                            Chefname = ObjChef.UserName;
                    }
                    var objOrder = PendingOrders.Find(x => x.OID == OID);
                    string Seating = "";
                    var ObjTorS = ListTorS.Find(x => x.Table_or_RowID == objOrder.Table_or_SheatId);
                    if (ObjTorS != null)
                    {
                        var ObjFlrOrScr = ListFrorScr.Find(x => x.Floor_or_ScreenID == ObjTorS.Floor_or_ScreenId);
                        if (ObjFlrOrScr != null)
                        {
                            Seating = ObjFlrOrScr.Name;
                        }
                        var ObjFsideOrRowName = ListFsorRowNa.Find(x => x.ID == ObjTorS.FloorSide_or_RowNoID);
                        if (ObjFsideOrRowName != null)
                        {
                            Seating += " " + ObjFsideOrRowName.FloorSide_or_RowName;
                        }
                        Seating += " " + ObjTorS.Table_or_SheetName;
                    }
                    //Seating += " Ticket No:" + TicktObj.Key;
                    OrdNotice OrderNotice = OrdNotice.GetOne(objOrder.OID);
                    vw_HG_UsersDetails objOrderBy = new vw_HG_UsersDetails().GetSingleByUserId((int)OrderById);
                    string name = Seating + " Ticket No. :" + ticketno ;
                    //if (Chefname != "")
                    //{
                    //    name += " Chef " + Chefname;
                    //}
                    double deliveryCharge = 0.00;
                    if (objOrder.DeliveryCharge > 0)
                    {
                        List<HG_Ticket> Tickets = HG_Ticket.GetByOID(objOrder.OID);
                        var ObjTicket = Tickets.Find(x => x.TicketNo == ticketno);
                        if (ObjTicket != null && ObjTicket.TicketNo > 0)
                        {
                            deliveryCharge = ObjTicket.DeliveryCharge;
                        }
                    }
                    if (OrderNotice != null && OrderNotice.OID > 0)
                    {

                        vw_HG_UsersDetails ObjUser = new vw_HG_UsersDetails();
                        if (objOrder.PayReceivedBy > 0)
                        {
                            ObjUser = new vw_HG_UsersDetails().GetSingleByUserId(objOrder.PayReceivedBy);
                        }
                        if (ObjUser != null && ObjUser.UserType != "CA" && ObjUser.UserType != "ONR")// not captain not OWN
                        {
                            TableScreen.Add("Amt", TotalAmount);
                        }
                        else if (ObjUser == null || ObjUser.UserCode <= 0)// if CASH PAY BY CUSTOMER
                        {
                            TableScreen.Add("Amt", TotalAmount);
                        }
                        else
                        {
                            TableScreen.Add("Amt", 0);
                        }
                    }
                    else
                    {
                        TableScreen.Add("Amt", 0);
                    }
                    TableScreen.Add("PymtMode", objOrder.PaymentStatus);
                    TableScreen.Add("deliveryCharge", deliveryCharge);
                    TableScreen.Add("TableScreenInfo", name);
                    TableScreen.Add("TableSeatID", objOrder.Table_or_SheatId);
                    TableScreen.Add("TicketNo", ticketno);
                    TableScreen.Add("OID", objOrder.OID);
                    TableScreen.Add("OrderItems", ItemsArray);
                    TableScreen.Add("TorSIndex", TorSIndex++);
                    TableScreen.Add("CustomerMobile", objOrderBy.UserId);
                    TableScreen.Add("CustomerName", objOrderBy.UserName);
                    TableScreen.Add("ChefName", Chefname);
                    TableScreen.Add("OrdTime", objOrder.Create_Date.ToString("dd-MM hh:mm tt"));
                    tableorSheatList.Add(TableScreen);
                }
            }
            catch (Exception e)
            {

            }
            return tableorSheatList;

        }
        // chef past completed and cancel orders
        public JArray ChefComCaclOrd(int OrgId, int ChefId, int Status)
        {
            JArray tableorSheatList = new JArray();
            try
            {
                List<vw_HG_UsersDetails> ListUsers = new vw_HG_UsersDetails().GetAll(OrgId:OrgId);
                List<HG_OrderItem> OrderItemList = new HG_OrderItem().GetAllByOrg(OrgId, ChefId:0,TodayOnly:true);
                OrderItemList = OrderItemList.FindAll(x => x.Status == Status);
                if (ChefId > 0)
                {
                    OrderItemList = OrderItemList.FindAll(x => x.ChefSeenBy == ChefId);
                }
                OrderItemList = OrderItemList.OrderByDescending(x => x.UpdationDate).ToList();
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
                    int ChefSeenId = 0;
                    var OrderItmListTicketWise = TicketitmList.ToList();
                    foreach (var OrderItem in OrderItmListTicketWise)
                    {
                        HG_Items hG_Items = ListfoodItems.Find(x => x.ItemID == OrderItem.FID);
                        JObject itemobj = new JObject();
                        itemobj.Add("OIID", OrderItem.OIID);
                        itemobj.Add("ItemID", OrderItem.FID);
                        itemobj.Add("ItemName", hG_Items.Items);
                        itemobj.Add("Quantity",OrderItem.Count);
                        itemobj.Add("Status", OrderItem.Status);
                        itemobj.Add("IIndex", ItemIndex++);
                        ItemsArray.Add(itemobj);
                        ticketno = OrderItem.TickedNo;
                        OrdId = OrderItem.OID;
                        ChefSeenId = OrderItem.ChefSeenBy;
                    }
                    var ChefObj = ListUsers.Find(x => x.UserCode == ChefSeenId);
                    HG_Orders order = new HG_Orders().GetOne(OrdId);
                    string Seating = " ";
                    HG_Tables_or_Sheat hG_Tables_Or_Sheat = ListTableOrSheat.Find(x => x.Table_or_RowID == order.Table_or_SheatId);
                    if (hG_Tables_Or_Sheat != null)
                    {
                        HG_Floor_or_ScreenMaster hG_Floor_Or_ScreenMaster = ListFloorScreen.Find(x => x.Floor_or_ScreenID == hG_Tables_Or_Sheat.Floor_or_ScreenId);
                        if (hG_Floor_Or_ScreenMaster != null)
                        {
                            Seating = hG_Floor_Or_ScreenMaster.Name;
                        }
                        HG_FloorSide_or_RowName hG_FloorSide_Or_RowName = ListFloorSideorRow.Find(x => x.ID == hG_Tables_Or_Sheat.FloorSide_or_RowNoID);
                        if (hG_FloorSide_Or_RowName != null)
                        {
                            Seating += " " + hG_FloorSide_Or_RowName.FloorSide_or_RowName;
                        }
                        Seating += " " + hG_Tables_Or_Sheat.Table_or_SheetName;
                    }
                    JObject TableScreen = new JObject();
                    string name =Seating+ "Ticket no. : " + ticketno;
                    TableScreen.Add("TableScreenInfo", name);
                    TableScreen.Add("TableSeatID", "0");// UNUSED
                    TableScreen.Add("TicketNo", ticketno);
                    TableScreen.Add("OID", order.OID);
                    TableScreen.Add("OrderItems", ItemsArray);
                    TableScreen.Add("TorSIndex", TorSIndex++);
                    TableScreen.Add("ChefName", (ChefObj != null?ChefObj.UserName:""));
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
            BalanceStatementController balanceStatement = new BalanceStatementController();
            var OrderItemList = OrderItemListAll.FindAll(x => x.TickedNo == TickedNo);
            OrderItemList = OrderItemList.FindAll(X => X.Status != 3 && X.Status != 4); //NOT ALREADY CANCEL NOT COMPLETED
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
                OrderitemObj.ChefSeenBy = UpdateBy;
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
                 OrderItemListAll = new HG_OrderItem().GetAll(OID);
                if (order.DeliveryCharge > 0)
                {
                    var TockenItems = OrderItemListAll.FindAll(x => x.TickedNo == TickedNo);
                    var CancelItems = TockenItems.FindAll(x => x.Status == 4);//all canceled list
                    if (TockenItems.Count == CancelItems.Count)
                    {
                        List<HG_Ticket> Tickets = HG_Ticket.GetByOID(order.OID);
                       var NotCanceled = Tickets.FindAll(x => x.TicketNo != TickedNo);
                        order.DeliveryCharge = NotCanceled.Sum(x => x.DeliveryCharge);
                        var CancelTicket = Tickets.Find(x => x.TicketNo == TickedNo);
                        if (CancelTicket != null && CancelTicket.TicketNo > 0)
                        {
                            CancelTicket.DeliveryCharge = 0;
                            CancelTicket.save();
                        }
                    }

                }
                if (ObjOrg.PaymentType==1)// prepaid
                {
                    //for balance Statement
                    List<HG_OrderItem> CompletedItems = OrderItemListAll.FindAll(x => x.Status == 3);
                    //==============
                    var completedOrCancelorderItems = OrderItemListAll.FindAll(x => x.Status == 3 || x.Status == 4);//cancel and Completed
                    if (OrderItemListAll.Count == completedOrCancelorderItems.Count)
                    {
                        order.Status = "3";
                        TorSObj.Status = 1;
                        TorSObj.Otp = OTPGeneretion.Generate();
                        TorSObj.save();// free table  
                       //=============Balance Statement==============
                                 
                        try
                        {
                            balanceStatement.GetDetails(CompletedItems);
                        }
                        catch (Exception ex)
                        {

                        }               
                        //============================================
                        order.Update_By = UpdateBy;
                        if (TorSObj.Type != "3")
                        {
                            SendMsgCustomer(order.CID, order.OID,order);
                        }
                        
                        OrdNotice.ChangeAlertSts(OID, 0, 1);
                    }
                  
                }
                else
                {//postpaid
                    //order.Status = "2";// processing
                    var completedOrCancelorderItems = OrderItemListAll.FindAll(x => x.Status == 3 || x.Status == 4);//cancel and Completed
                    if (OrderItemListAll.Count == completedOrCancelorderItems.Count && order.PaymentStatus != 0)
                    {
                        //for balance Statement
                        List<HG_OrderItem> CompletedItems = OrderItemListAll.FindAll(x => x.Status == 3);
                        //==============
                        order.Status ="3";
                        TorSObj.Status = 1;
                        TorSObj.Otp = OTPGeneretion.Generate();
                        TorSObj.save();
                        //=============Balance Statement==============

                        try
                        {
                            balanceStatement.GetDetails(CompletedItems);
                        }
                        catch (Exception ex)
                        {

                        }
                        //============================================
                        if (TorSObj.Type != "3")
                        {
                            SendMsgCustomer(order.CID, order.OID,order);
                        }
                        OrdNotice.ChangeAlertSts(OID, 0, 1);
                    }
                    order.Update_By = UpdateBy;
                }
                order.Save();
                Wallet.AddToWallet(order);
                PostResult.Add("Status", 200);
            }
            else
            {
                PostResult.Add("Status", 400);
            }
            return PostResult;

        }
        public void SendMsgChef(int OrgId,Int64 OrdNo,int Ticket=0)
        {
            string[] topics = { OrgId.ToString() };
           // topics.Add(OrgId.ToString());
            string Msg = "Order Place  Order No " + OrdNo.ToString();
            if (Ticket > 0)
            {
                Msg += "and Ticket No: " + Ticket;
            }
            string Title = "New Order Placed";
            try
            {
                PushNotification.SendNotification(topics, Msg, Title);
            }
            catch(Exception e)
            {

            }
             
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
                //string Msg = "YOUR OTP FOR foodDo APP IS " + OTPNumber+"";
                string Msg = OTPNumber ;
                //HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create("http://app.telcob.cloud/app/smsapi/index.php?key=55DD67927E1B3E&campaign=0&routeid=4&type=text&contacts=" + MobileNO + "&senderid=FOODDO&msg=" + Msg);
                HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create("https://api.msg91.com/api/v5/otp?authkey=312759As2icJb85e19debcP1&template_id=5e1ac072d6fc051c9d245ab7&mobile=" + MobileNO + "&invisible=1&otp=" + Msg);

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


        //===================================TABLE AND SCREEN SECTION =================
        public JArray TableAndTakeAway(int Type,int OrderById,int FloorId=0,int OrgId=0)
        {
            JArray jArray = new JArray();
            DateTime fromdate = DateTime.Now;
            if (OrgId == 0)
            {
                var UserInfo = Request.Cookies["UserInfo"];
                OrgId = int.Parse(UserInfo["OrgId"]);
            }
            List<HG_Tables_or_Sheat> list = new List<HG_Tables_or_Sheat>();
            List<HG_FloorSide_or_RowName> FloorSideRowList = new List<HG_FloorSide_or_RowName>();
            List<HG_Floor_or_ScreenMaster> FloorScrenList = new List<HG_Floor_or_ScreenMaster>();
            List<HG_Orders> Orderlist = new List<HG_Orders>();
            if (OrgId > 0)
            {
                 list = new HG_Tables_or_Sheat().GetAllWithTakeAwya(Type);
                FloorSideRowList = new HG_FloorSide_or_RowName().GetAll(Type, OrgId);
                 FloorScrenList = new HG_Floor_or_ScreenMaster().GetAll(Type, OrgId);
                Orderlist = new HG_Orders().GetListByGetDate(fromdate, DateTime.Now);
            }
            if (OrgId > 0)
            {
                Orderlist = Orderlist.FindAll(x => x.OrgId == OrgId);
            }
            if (FloorId > 0)
            {
                list = list.FindAll(x => x.Floor_or_ScreenId == FloorId);
            }
            foreach(var objtable in list)
            {
                HG_Orders order = Orderlist.Find(x =>x.Table_or_SheatId == objtable.Table_or_RowID &&x.Status!="3" && x.Status != "4");
                JObject jObject = new JObject();
                jObject = JObject.FromObject(objtable);
                if (order != null && order.OID > 0)
                {
                    jObject.Add("CurrOID",order.OID);
                    if (objtable.Status == 1)
                    {
                        jObject["Status"] = 3;// processing
                    }
                    if (order.ContactId > 0)
                    {
                        LocalContacts localContacts = LocalContacts.GetOne(order.ContactId);
                        jObject.Add("SeatingUser", localContacts.Cust_Name + " (" + localContacts.MobileNo + ")");
                    }
                    else
                    {
                        vw_HG_UsersDetails ObjUser = new vw_HG_UsersDetails().GetSingleByUserId((int)order.CID);
                        if (ObjUser != null && ObjUser.UserCode > 0 && ObjUser.UserType == "CUST")
                        {
                            jObject.Add("SeatingUser", ObjUser.UserName + " (" + ObjUser.UserId + ")");
                        }
                    }
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
        public JArray FloorScreen(int OrgID)
        {
            HG_OrganizationDetails orgonization = new HG_OrganizationDetails().GetOne(OrgID);

            List<HG_Floor_or_ScreenMaster> floorlist = new HG_Floor_or_ScreenMaster().GetAll(int.Parse(orgonization.OrgTypes), OrgID);
            return JArray.FromObject(floorlist);
        }
        //dropdown captain end
        public JArray GetTableInfo(int OrgId, int OrgType)
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
                TableScreen.Add("TableOrSheatName", ForSname + " " + FsideOrRname + " " + TableObj.Table_or_SheetName);
                TableScreen.Add("TableSeatID", TableObj.Table_or_RowID);
                TablesOrSheatList.Add(TableScreen);

            }
            return TablesOrSheatList;
        }

        public JObject ScanRestTable(string Obj)
        {
            JObject ParaMeters = JObject.Parse(Obj);
            string QrCode = ParaMeters.GetValue("TID").ToString();
            int CID = int.Parse(ParaMeters.GetValue("CID").ToString());
            
            string Type = ParaMeters.GetValue("Type").ToString();
            int AppType= ParaMeters["AppType"] != null ? int.Parse(ParaMeters["AppType"].ToString()) : 1;//1 customer ,2 captain , 3 admin panel
            HG_Tables_or_Sheat TableRowObj = new HG_Tables_or_Sheat().GetOne(QrOcde: QrCode);
            if (TableRowObj.Type != Type)
            {
                TableRowObj = new HG_Tables_or_Sheat();
            }
            JObject jObject = JObject.FromObject(TableRowObj);
            HG_OrganizationDetails objOrg = new HG_OrganizationDetails().GetOne(TableRowObj.OrgId);
            jObject.Add("OrgName", objOrg != null ? objOrg.Name : " ");
            jObject.Add("OrderingStatus", objOrg.CustomerOrdering);
            jObject.Add("PaymentType", objOrg.PaymentType);
            jObject.Add("OID", 0);
            vw_HG_UsersDetails ObjUser = new vw_HG_UsersDetails().GetSingleByUserId(CID);
            if (ObjUser != null && ObjUser.UserCode > 0 && ObjUser.JoinByOrg == 0 && objOrg.OrgID>0)
            {
                if (objOrg.OrgID > 0)
                {
                    ObjUser.JoinByOrg = objOrg.OrgID;
                    ObjUser.save();
                }
            }
            if (Customer.IsJoined(CID, objOrg.OrgID) == false && TableRowObj.Table_or_RowID>0)
            {
                new Customer { CID = CID, OrgId = objOrg.OrgID }.save();
            }
            OrgSetting orgSetting = OrgSetting.Getone(objOrg.OrgID);
            if (orgSetting.EnblDeleryChrg == 1 && orgSetting.AcptMinOrd == 1 && OrgType.DeliveryChargeAply(AppType, orgSetting))
            {
                if (orgSetting.DeleryChrgType == 1)//fixed charge{
                {
                    jObject.Add("MinOrdAmt", orgSetting.MinOrderAmt);
                    jObject.Add("DeliveryChrge", orgSetting.DeliveryCharge);
                    jObject.Add("DeliveryType", orgSetting.DeleryChrgType);
                }
                else if (orgSetting.DeleryChrgType == 0)
                {
                    jObject.Add("MinOrdAmt", orgSetting.MinOrderAmt);
                    jObject.Add("DeliveryChrge", orgSetting.DeliveryCharge);
                    jObject.Add("DeliveryType", orgSetting.DeleryChrgType);
                }
            }
            jObject.Add("VerifyBy", orgSetting.CrxVerification);
            if (objOrg.OrgID > 0 &&objOrg.WalletAmt>0)
            {
                //===========CASHBACK========================================================
                JArray CashBks = new JArray();
                List<Cashback> Cashbacks = Cashback.GetAll(objOrg.OrgID, 1);// only actives
                Cashbacks = Cashbacks.FindAll(x => x.CashBkStatus == 1);// only running
                Cashbacks = Cashbacks.FindAll(x => x.SeatingIds != "");
                Cashbacks = Cashbacks.FindAll(x => x.StartDate.Date <= DateTime.Now.Date && x.ValidTillDate.Date >= DateTime.Now.Date).ToList();
                Cashbacks = Cashbacks.FindAll(x => x.CampeignType != 3);// not offers item based ,fixed choice
                foreach(var cashback in Cashbacks)
                {
                    List<int> seats = cashback.SeatingIds.Split(',').Select(int.Parse).ToList();
                    int seat = seats.Find(x => x == TableRowObj.Table_or_RowID);
                    if (seat>0 && cashback.CampeignType==1)
                    {
                        JObject Jobj = new JObject();
                        Jobj.Add("CBPercentage", cashback.Percentage.ToString("0.00"));
                        Jobj.Add("MaxCB", cashback.MaxAmt.ToString("0.00"));
                        Jobj.Add("CashBKid", cashback.CashBkId);
                        Jobj.Add("MaxCbLimit", cashback.MaxCBLimit);
                        Jobj.Add("CashBkType", cashback.CampeignType);
                        string MinBillAmt = "";
                        if (cashback.RaiseDynamic)
                        {
                            var AggStudy = GetOrder.GetTotalAmt(objOrg.OrgID);
                            double DynamicValue = AggStudy + (AggStudy - cashback.BilAmt) * (cashback.Percentage * 2 / 100);
                            MinBillAmt= DynamicValue > cashback.BilAmt ? DynamicValue.ToString("0.00") : cashback.BilAmt.ToString("0.00");
                        }
                        else
                        {
                            MinBillAmt= cashback.BilAmt.ToString("0.00");
                        }
                        Jobj.Add("MinBillAmt", MinBillAmt);
                        JObject JojCashbk = new JObject();
                        JojCashbk.Add("Title", "CashBack Offer");
                        JojCashbk.Add("Description", "Get "+ cashback.Percentage.ToString("0.00") +"% on Min Bill Amount "+ MinBillAmt);
                        jObject.Add("CashBk", Jobj);
                        CashBks.Add(JojCashbk);

                    }
                    else if(seat>0 && cashback.CampeignType == 2)// item offers
                    {
                        JObject JojCashbk = new JObject();
                        JojCashbk.Add("Title", "Complementry Dish Offer");
                        JojCashbk.Add("Description", "Get A Complementry Dish on a Bill Amt Of Rs " + cashback.BilAmt.ToString("0.00") +" or more");
                        CashBks.Add(JojCashbk);
                    }
                }
                jObject.Add("CashBkList", CashBks);
            }
            return jObject;
        }
        [HttpPost]
        public JArray GetSheetNumberBYRowList(int OrgID)
        {
            List<HG_Tables_or_Sheat> listSheet = new HG_Tables_or_Sheat().GetAll(2);// 2 for list of sheets
            listSheet = listSheet.FindAll(x => x.OrgId == OrgID);
            return JArray.FromObject(listSheet);
        }
        public JObject GetDeliveryCharge(int OrgId,int AppType)
        {
            JObject jObject = new JObject();
            OrgSetting orgSetting = OrgSetting.Getone(OrgId);
            if (orgSetting.EnblDeleryChrg == 1 && orgSetting.AcptMinOrd == 1 && OrgType.DeliveryChargeAply(AppType, orgSetting))
            {
                if (orgSetting.DeleryChrgType == 1)//fixed charge{
                {
                    jObject.Add("MinOrdAmt", orgSetting.MinOrderAmt);
                    jObject.Add("DeliveryChrge", orgSetting.DeliveryCharge);
                    jObject.Add("DeliveryType", orgSetting.DeleryChrgType);
                }
                else if (orgSetting.DeleryChrgType == 0)
                {
                    jObject.Add("MinOrdAmt", orgSetting.MinOrderAmt);
                    jObject.Add("DeliveryChrge", orgSetting.DeliveryCharge);
                    jObject.Add("DeliveryType", orgSetting.DeleryChrgType);
                }
            }
            return jObject;
        }
        public JArray PastOrderMainList(int CID,int status=0)
        {
            JArray Info = new JArray();
            List<HG_Orders> OrderList = new HG_Orders().GetAll(CID: CID);
          //  OrderList = OrderList.FindAll(x => x.OrderByIds.Contains(CID.ToString()));
            OrderList = OrderList.FindAll(x => x.Status != "4");
            if (status > 0 && status == 1)//ongoing orders
            {
                OrderList = OrderList.FindAll(x => x.Status == "1" || x.Status == "2");
                OrderList = OrderList.FindAll(x => x.Create_Date.Date == DateTime.Now.Date).ToList();
            }
            else if (status > 0 && status == 3)//completed
            {
                OrderList = OrderList.FindAll(x => x.Status == "3");
            }
        //    List<HG_OrderItem> OrderItem=new HG_OrderItem().GetAll(CID)
            if(OrderList.Count>0)
            {
                foreach (HG_Orders orders in OrderList)
                {
                    HG_OrganizationDetails hG_OrganizationDetails = new HG_OrganizationDetails().GetOne(orders.OrgId);
                    List<HG_OrderItem> OrderItemList = new HG_OrderItem().GetAll(orders.OID);
                    OrderItemList = OrderItemList.FindAll(x => x.Status != 4);// not canceled items
                    double price = orders.DeliveryCharge;
                    double CostPrice = 0.00;
                    double tax = 0.00;
                    HashSet<int> Token = new HashSet<int>();
                    for(int i=0;i< OrderItemList.Count; i++)
                    {
                        price += (OrderItemList[i].Count * OrderItemList[i].Price);
                        CostPrice+= (OrderItemList[i].Count * OrderItemList[i].CostPrice);
                        tax += OrgType.TotalTax(OrderItemList[i].CostPrice, OrderItemList[i].TaxInItm, OrderItemList[i].Count);
                        Token.Add(OrderItemList[i].TickedNo);
                    }
                  //  CostPrice= price
                    JObject Object = new JObject();
                    Object.Add("Date", orders.Create_Date.ToString("ddd, MMM-dd-yyyy"));
                    Object.Add("OrganizationName", hG_OrganizationDetails.Name);
                    Object.Add("OutLetType", hG_OrganizationDetails.PaymentType);
                    Object.Add("CostPrice", CostPrice.ToString("0.00"));
                    Object.Add("Tax", tax.ToString("0.00"));
                    Object.Add("TotalAmount", price.ToString("0.00"));
                    Object.Add("TicketNo", string.Join(",", Token));
                    Object.Add("OID", orders.OID);
                    Object.Add("Status", orders.Status);
                    Object.Add("DeliveryChrge", orders.DeliveryCharge);
                    Object.Add("PayStatus",OrgType.PaymentMode(orders.PaymentStatus));
                    if (orders.Status!="3"&& orders.PaymentStatus == 0 && hG_OrganizationDetails.PaymentType==1)//prepaid
                    {
                        Object.Add("LastOrdTime", (DateTime.Now-orders.Update_Date).TotalMinutes);
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
                OrgSetting orgSetting = OrgSetting.Getone(orders.OrgId);
                HG_Tables_or_Sheat ObjTorS = new HG_Tables_or_Sheat().GetOne(orders.Table_or_SheatId);
                List<HG_OrderItem> hG_OrderItems = new HG_OrderItem().GetAll(orders.OID);
                List<HG_Items> ListfoodItems = new HG_Items().GetAll(orders.OrgId);
                double price = orders.DeliveryCharge;
                double tax = 0.00;
                double CostPrice = 0.00;
                HashSet<int> Token = new HashSet<int>();
                Object.Add("Date", orders.Create_Date.ToString("ddd, MMM-dd-yyyy"));
                Object.Add("OrganizationName", hG_OrganizationDetails.Name);
                Object.Add("OrgId", hG_OrganizationDetails.OrgID);
                Object.Add("OrgType", hG_OrganizationDetails.OrgTypes);
                Object.Add("SeatOrTblid", orders.Table_or_SheatId);
                Object.Add("TableSeatname", ObjTorS.Table_or_SheetName);
                Object.Add("OutLetType", hG_OrganizationDetails.PaymentType);
                Object.Add("OID", orders.OID);
                Object.Add("Status", orders.Status); 
                Object.Add("PayStatus",OrgType.PaymentMode(orders.PaymentStatus));
                Object.Add("OrdAprvalSts", orders.OrderApprovlSts);
                if (hG_OrganizationDetails.OrgTypes == "1")//restuarnt
                {
                    Object.Add("ByCash", (orgSetting.ByCash ==1 ||orgSetting.ByCash==0)? "YES" : "NO");//
                    Object.Add("ByOnline", (orgSetting.ByOnline == 1 || orgSetting.ByOnline == 0) ? "YES" : "NO");//
                }
                else if (hG_OrganizationDetails.OrgTypes == "2")// theater
                {
                    Object.Add("ByOnline", (orgSetting.ByOnline ==1||orgSetting.ByOnline==0) ? "YES" : "NO");
                    Object.Add("ByCash", (orgSetting.ByCash == 1 || orgSetting.ByCash == 0) ? "YES" : "NO");
                }
                //cancel order condition
                if (orders.Status!="3"&&orders.PaymentStatus == 0 && hG_OrganizationDetails.PaymentType == 1)//prepaid
                {
                    Object.Add("LastOrdTime", (DateTime.Now - orders.Update_Date).TotalMinutes);
                }
                Object.Add("ContactH1", orgSetting.ContactHead1==null?"": orgSetting.ContactHead1);
                Object.Add("Contact1", orgSetting.Contact1==null?"": orgSetting.Contact1);
                Object.Add("ContactH2", orgSetting.ContacHead2==null?"" : orgSetting.ContacHead2);
                Object.Add("Contact2", orgSetting.Contact2==null?"" : orgSetting.Contact2);
                foreach (var OrderItem in hG_OrderItems)
                {
                    HG_Items hG_Items = ListfoodItems.Find(x => x.ItemID == OrderItem.FID);
                    JObject itemobj = new JObject();
                    Token.Add(OrderItem.TickedNo);
                    itemobj.Add("OIID", OrderItem.OIID);
                    itemobj.Add("ItemID", OrderItem.FID);
                    itemobj.Add("ItemName", hG_Items.Items);
                    itemobj.Add("Quantity",OrderItem.Count);
                    itemobj.Add("Status", OrderItem.Status);
                    itemobj.Add("VegNonVeg", hG_Items.ItemMode);
                    itemobj.Add("IsAddon", OrderItem.IsAddon);// is addon item 0 no ,1 yes
                    double taxprice= OrgType.TotalTax(OrderItem.CostPrice, OrderItem.TaxInItm, OrderItem.Count);
                    double CostPriceItm= (OrderItem.Count * OrderItem.CostPrice);
                    double TotlPrice= (OrderItem.Count * OrderItem.Price);
                    if (OrderItem.IsAddon == "1")
                    {
                        List<OrderAdonItm> listaddonitems = OrderAdonItm.GetAll(OrderItem.OIID);
                        foreach(var addonitm in listaddonitems)
                        {
                            taxprice += OrgType.TotalTax(addonitm.CostPrice, addonitm.Tax, OrderItem.Count);
                            CostPriceItm += (OrderItem.Count * addonitm.CostPrice);
                            TotlPrice += (OrderItem.Count * addonitm.Price);
                        }
                        itemobj.Add("AddonItems", JArray.FromObject(listaddonitems));
                    }
                    itemobj.Add("Tax", taxprice.ToString("0.00"));
                    itemobj.Add("CostPrice", CostPriceItm.ToString("0.00"));
                    itemobj.Add("Amount", TotlPrice.ToString("0.00"));
                    if (OrderItem.Status != 4)
                    {
                        tax += taxprice;
                        CostPrice += CostPriceItm;
                        price += TotlPrice;
                    }
                    Info.Add(itemobj);
                }
                Object.Add("TicketNo", string.Join(",", Token));
                Object.Add("CostPrice", CostPrice.ToString("0.00"));
                Object.Add("Tax", tax.ToString("0.00"));
                Object.Add("TotalAmount", price.ToString("0.00"));
                Object.Add("DeliveryChrge", orders.DeliveryCharge);
                if (orders.DisntChargeIDs != "" && orders.DisntChargeIDs != "0")
                {
                    List<OrdDiscntChrge> Discnt = OrdDiscntChrge.GetAll(orders.DisntChargeIDs);
                    JArray DiscntArray = new JArray();
                    for (int j = 0; j < Discnt.Count; j++)
                    {
                        JObject discnJoj = new JObject();
                        discnJoj.Add("DiscntTitle", Discnt[j].Title);
                        discnJoj.Add("DiscntAmt", Discnt[j].Amt.ToString("0.00"));
                        DiscntArray.Add(discnJoj);
                    }
                    Object.Add("DiscntList", DiscntArray);
                }
                if (Status == "1" &&orders.PaymentStatus==0)
                {
                    WalletAmt walletAmt = WalletAmt.GetUnusedWalletAmt((int)orders.CID, orders.OrgId);
                    double MyCashBk = walletAmt.CashBkAmt - walletAmt.DeductedAmt;
                    if (MyCashBk > 0)
                    {
                        Object.Add("MyCashBkAmt", MyCashBk.ToString("0.00"));
                    }
                    Cashback cashback = Cashback.GetAppliedCashBk(orders.OrgId, orders.Table_or_SheatId, 2);// complementry offers
                    if (cashback != null && price> cashback.BilAmt &&orders.OfferDishCBID==0)
                    {
                        Object.Add("ComplimentryDish",JObject.FromObject(OfferObj.GetAll(cashback.CashBkId)));
                    }
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
            tableorderlist = tableorderlist.FindAll(x => x.Status != 3 && x.Status != 4);
            tableorderlist = tableorderlist.FindAll(x => x.TickedNo == TicketNO);
            foreach (var OrderItem in tableorderlist)
            {
                OrderItem.ChefSeenBy = 0;
                OrderItem.Status = 1;
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
        
        

        [HttpPost]
        public JObject JoinFoodDoTeam(string Obj)
        {
            JObject objParams = JObject.Parse(Obj);
            JArray JMenuArray = new JArray();
            int CID = int.Parse(objParams.GetValue("CID").ToString());
            int JoinType = int.Parse(objParams.GetValue("JoinType").ToString());
            string ProductType = objParams.GetValue("ProductType").ToString();
            JObject result = new JObject();
            List<JoinFoodDo> JoinedList = JoinFoodDo.GetAll(CID);
            if (JoinedList.Count > 0)
            {
                JoinFoodDo joinFoodDoObj = JoinedList.Find(x => x.JoinType == JoinType);
                if (joinFoodDoObj != null)
                {
                    result.Add("Status", 400);
                    result.Add("MSG", "Already Joined");
                    return result; ;
                }
            }
            JoinFoodDo joinFoodDo = new JoinFoodDo();
            joinFoodDo.JoinedUserd = CID;
            joinFoodDo.JoinType = JoinType;
            joinFoodDo.ProductType = ProductType;
            joinFoodDo.JoinDate = DateTime.Now;
           
            if (joinFoodDo.save() > 0)
            {
                result.Add("Status", 200);
                result.Add("MSG", "Joined successfully. You will receive call back soon");
            }
            else
            {
                result.Add("Status", 400);
                result.Add("MSG", "Try after some time");
            }
            return result;
        }

        public JObject CustomerQueNumber(string Obj)
        {
            JObject ParamObj = JObject.Parse(Obj);
            int CID = int.Parse(ParamObj.GetValue("CID").ToString());
            Int64 TorSId = Int64.Parse(ParamObj.GetValue("TorSid").ToString());
            int OrgId = int.Parse(ParamObj.GetValue("OrgId").ToString());
            HG_OrganizationDetails ObjOrg = new HG_OrganizationDetails().GetOne(OrgId);
            int Type = int.Parse(ObjOrg.OrgTypes);
            List<HG_Tables_or_Sheat> ListTorS = new HG_Tables_or_Sheat().GetAll(Type, OrgId);
            HG_Tables_or_Sheat ObjTorS = ListTorS.Find(x => x.Table_or_RowID == TorSId);
            ListTorS = ListTorS.FindAll(x => x.Floor_or_ScreenId == ObjTorS.Floor_or_ScreenId);
            HashSet<Int64> TorShash = new HashSet<Int64>(ListTorS.Select(x => x.Table_or_RowID).ToArray());
            List<HG_Orders> TodayOrderList = new HG_Orders().GetListByGetDate(DateTime.Now, DateTime.Now);
            TodayOrderList = TodayOrderList.FindAll(x => x.OrgId == OrgId);
            TodayOrderList = TodayOrderList.FindAll(x => x.Status != "3" && x.Status!="4");
            TodayOrderList = TodayOrderList.FindAll(x => TorShash.Contains(x.Table_or_SheatId));
            TodayOrderList = TodayOrderList.OrderBy(x => x.Create_Date).ToList();
            var countnumber = TodayOrderList.FindIndex(x => x.Table_or_SheatId == TorSId);
            countnumber= countnumber+1;
            JObject result = new JObject();
            if (countnumber == 0)
            {
                result.Add("Position", countnumber);
                result.Add("MSG", "Completed");
            }
            else
            {
                result.Add("Position", countnumber);
                result.Add("MSG", "One the Way");
            }
            return result;
        }

        [HttpPost]
        public string GetCheckSum(string CID, string OID, string Amount, string email, string mobile,Int64 RealOID = 0)
        {

            String merchantKey = "yB4HRdQ0vcb9XBrI";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("CALLBACK_URL", "https://securegw.paytm.in/theia/paytmCallback?ORDER_ID=" + OID + "");
            parameters.Add("CHANNEL_ID", "WAP");
            parameters.Add("CUST_ID", CID);
            parameters.Add("INDUSTRY_TYPE_ID", "Retail105");
            parameters.Add("MID", "foodDo62634269971979");
            parameters.Add("ORDER_ID", OID);
            parameters.Add("TXN_AMOUNT", Amount);
            parameters.Add("WEBSITE", "APPSTAGING");
            string checksum = CheckSum.generateCheckSum(merchantKey, parameters);
            PymentPageOpen.ListPytmPgOpen.Add(new PymentPageOpen { OID = RealOID, CID = CID });
            return checksum;

        }
        public JObject CancelPageOpen(Int64 OID)
        {
            JObject result = new JObject();
            PymentPageOpen.ListPytmPgOpen.RemoveAll(x => x.OID == OID);
            result.Add("Status", 200);
            return result;
        }
        public JObject PaytmPayMentStatus(string paytmResn)
        {
            PaytmResn paytmResnObj = Newtonsoft.Json.JsonConvert.DeserializeObject<PaytmResn>(paytmResn);
           // PaytmResn paytmResnObj = new PaytmResn();
            //paytmResnObj.id=0
            JObject result = new JObject();
            if (paytmResnObj.save() > 0 )
            {
                //BY  FOODDO PAYMENT
                CompleteOrder(3, (int)paytmResnObj.CID,paytmResnObj.OID,AppType:1);
                result.Add("Status",200);
            }
            else
            {
                result.Add("Status",400);
            }
            return result;
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
        public JArray TehsilList(int StateId,int CityId)
        {
            List<District> citylist = new District().GetAllByStsCity(StateId,CityId);
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
        public JObject SettingPrivacyPolicy(string KeyName)
        {
            List<Settings> listsettings = new Settings().GetAll();
            Settings settingsObj = listsettings.Find(x => x.KeyName == KeyName);
            return JObject.FromObject(settingsObj);
        }
        public JObject ByCashPayment(string Obj)
        {
            JObject ParamObj = JObject.Parse(Obj);
            int CID = int.Parse(ParamObj.GetValue("CID").ToString());
            Int64 OID = Int64.Parse(ParamObj.GetValue("OID").ToString());
            HG_Orders ObjOrder = new HG_Orders().GetOne(OID);
            JObject result = new JObject();
            OrdNotice ordNotice = new OrdNotice();
            ordNotice.OID = OID;
            ordNotice.Status = 0;
            ordNotice.Type = 0;//payment by cash
            ordNotice.CID = CID;
            ordNotice.Orgid = ObjOrder.OrgId;
            if (ordNotice.save() > 0)
            {
                result.Add("Status", 200);
                CompleteOrder(1, CID, OID,AppType:1);
            }
            else
            {
                result.Add("Status", 400);
            }
            return result;
        }
        public JObject CountByCashUnverify()
        {
            var CookiObj = Request.Cookies["UserInfo"];
            var orgId = int.Parse(CookiObj["OrgId"]);
            List<OrdNotice> List = OrdNotice.GetAll(1);
            List=List.FindAll(x => x.Orgid == orgId);
            JObject jObject = new JObject();
            jObject.Add("Cnt", List.Count);
            return jObject;
        }
        public void OrdAcpted(Int64 OID)
        {
           OrdNotice.ChangeAlertSts(OID, 1, 1);
            HG_Orders objorder = new HG_Orders().GetOne(OID);
            var UserInfo = Request.Cookies["UserInfo"];
            var UserCode = int.Parse(UserInfo["UserCode"]);
            objorder.PayReceivedBy = UserCode;
            objorder.Save();
        }
        public JObject UnseenOrdCnt(int OrgId=0)
        {
            if (OrgId == 0)
            {
                var UserInfo = Request.Cookies["UserInfo"];
                OrgId  =int.Parse(UserInfo["OrgId"]);
            }
            int Count = HG_OrderItem.UnseenOrd(OrgId);
            JObject reslt = new JObject();
            reslt.Add("Cnt", Count);
            return reslt;
        }
        public JObject CheckForCancelOrd(int Orgid=0)
        {
            if (Orgid == 0)
            {
                var ObjCookie = Request.Cookies["UserInfo"];
                 Orgid = int.Parse(ObjCookie["OrgId"]);
            }
            
            HG_OrganizationDetails ObjOrg = new HG_OrganizationDetails().GetOne(Orgid);
           
            if (ObjOrg != null && ObjOrg.OrgID>0&& ObjOrg.PaymentType == 1)
            {
                List<HG_Orders> ListOrder = new HG_Orders().GetListByGetDate(DateTime.Now.AddDays(-1), DateTime.Now);
                ListOrder = ListOrder.FindAll(x => x.Status != "3" && x.Status != "4");
                ListOrder = ListOrder.FindAll(X => X.OrgId == Orgid);
                ListOrder = ListOrder.FindAll(x => x.PaymentStatus == 0);
                //ListOrder = ListOrder.FindAll(x => x.Update_Date > DateTime.Now.AddMinutes(-15));
                foreach(var order in ListOrder)
                {
                    double TimeDiffInMinutes = (DateTime.Now - order.Update_Date).TotalMinutes;
                    if (TimeDiffInMinutes > 10)
                    {
                        CancelOrder(order.OID, -1);// order auto cancel
                    }
                    
                }
            }
            else if (Orgid == 0)
            {
                List<HG_Orders> ListOrder = new HG_Orders().GetListByGetDate(DateTime.Now, DateTime.Now);
                ListOrder = ListOrder.FindAll(x => x.Status != "3" && x.Status != "4");
                ListOrder = ListOrder.FindAll(x => x.PaymentStatus == 0);
                //ListOrder = ListOrder.FindAll(x => x.Update_Date > DateTime.Now.AddMinutes(-15));
                List<HG_OrganizationDetails> ListOrgs = new HG_OrganizationDetails().GetAll();
                foreach (var order in ListOrder)
                {
                    HG_OrganizationDetails ObjOrnaziztn = ListOrgs.Find(x => x.OrgID == order.OrgId);
                    if(ObjOrnaziztn!=null&&ObjOrnaziztn.PaymentType==1)//prepaid
                    {
                        double TimeDiffInMinutes = (DateTime.Now - order.Update_Date).TotalMinutes;
                        if (TimeDiffInMinutes > 10)
                        {
                            CancelOrder(order.OID, -1);// order auto cancel
                        }
                    }
                   

                }
            }
            return new JObject();
        }
    }
}
