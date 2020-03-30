using System;
using Newtonsoft.Json.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using HangOut.Models;
using HangOut.Models.POS;
using HangOut.Models.DynamicList;
using System.Linq;

namespace HangOut.Controllers.POS
{
    public class AdminApiController : Controller
    {
        // GET: AdminApi
        public JObject GetSeating(int OrgId=0)
        {
            List<HG_Orders> Orderlist = new List<HG_Orders>();
            List<HG_Items> ListItems = new List<HG_Items>();
            List<HG_Category> MenuList = new List<HG_Category>();
            if (OrgId <= 0)
            {
                OrgId = OrderType.CurrOrgId();
            }
            if (OrgId > 0)
            {
                Orderlist = new HG_Orders().GetListByGetDate(DateTime.Now, DateTime.Now);
                Orderlist = Orderlist.FindAll(x => x.Status != "3" && x.Status != "4");
                ListItems = new HG_Items().GetAll(OrgId);
                ListItems = ListItems.FindAll(x => x.ItemAvaibility == 0);// only available items
                MenuList = new HG_Category().GetAll(OrgId: OrgId);
            }
            List<Seating> Listseating = Seating.GetSeating(OrgId);
            JObject JobjResonse = new JObject();
            JArray SeatingArray = new JArray();
            JArray TakeAwayItem = new JArray();
            foreach (var ObjSeating in Listseating)
            {
                JObject jObject = new JObject();
                jObject.Add("Table_or_SheetName", ObjSeating.SeatName);
                jObject.Add("Table_or_RowID", ObjSeating.SeatId);
                jObject.Add("Otp", ObjSeating.Otp);
                JArray MenuJarray = new JArray();
                var order = Orderlist.Find(x => x.Table_or_SheatId == ObjSeating.SeatId && x.TableOtp == ObjSeating.Otp);
                if (order != null && order.OID > 0)
                {
                    jObject.Add("CurrOID", order.OID);
                    jObject.Add("Status", 3);//table is booked
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
                    jObject.Add("CurrOID", 0);
                    jObject.Add("Status", 1);//table is free;
                }
                
                double CurrentTableAmt = 0.00;
                string Cmobile = "";
                string Cname = "";
                int ContactId = 0;
                if (order != null)
                {
                    if (order.ContactId > 0)
                    {
                        LocalContacts localContacts = LocalContacts.GetOne(order.ContactId);
                        Cmobile = localContacts.MobileNo;
                        Cname = localContacts.Cust_Name;
                        ContactId = localContacts.ContctID;
                    }
                    else
                    {
                        vw_HG_UsersDetails ObjUser = new vw_HG_UsersDetails().GetSingleByUserId((int)order.CID);
                        if (ObjUser != null && ObjUser.UserCode > 0 && ObjUser.UserType == "CUST")
                        {
                            Cmobile = ObjUser.UserId;
                            Cname = ObjUser.UserName;
                            ContactId = -1;// minus show dont edit this. Order Palced By Customer
                        }
                    }
                    if (order.PaymentStatus == 0)
                    {
                        var OrderItems = new HG_OrderItem().GetAll(order.OID);
                        OrderItems = OrderItems.FindAll(x => x.Status != 4);// not ccanceled items
                        CurrentTableAmt = order.DeliveryCharge;
                        for (var i = 0; i < OrderItems.Count; i++)
                        {
                            CurrentTableAmt += OrderItems[i].Count * OrderItems[i].Price;
                        }
                    }

                }
                if (ObjSeating.FSIS>0)// not takeaway
                {
                    jObject.Add("Type", 1);//Seat or table;
                    List<OrderMenuCategory> ListCategry = OrderMenuCategory.GetAll(ObjSeating.OMID);
                    List<OrdMenuCtgItems> ListMenuItems = OrdMenuCtgItems.GetAll(ObjSeating.OMID);
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
                                List<AddOnn> Addons = AddOns.GetAddonsAndMultiSSize(Items);
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
                            MenuJarray.Add(JobjMenu);
                        }

                    }
                }
                else
                {
                    jObject.Add("Type", 3);//take away
                    if (TakeAwayItem.Count == 0)
                    {
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
                            TakeAwayItem.Add(JobjMenu);
                        }
                    }
                    }
                    MenuJarray = TakeAwayItem;
                }

                jObject.Add("MenuItems", MenuJarray);
                SeatingArray.Add(jObject);
            }
            JobjResonse.Add("Seating", SeatingArray);
            return JobjResonse;
        }
    }
}