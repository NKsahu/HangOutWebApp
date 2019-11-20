using System.Web.Mvc;
using HangOut.Models;
using HangOut.Models.Common;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
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
            int count = 0;
            foreach(HG_Category menu in MenuList)
            {
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
                        List<Cart> cartCurrentItem = cartlist.FindAll(x => x.ItemId == Items.ItemID);
                        JObject objItem = new JObject();
                        objItem.Add("IID", Items.ItemID);
                        objItem.Add("ItemName", Items.Items);
                        objItem.Add("ItemPrice", Items.Price);
                        objItem.Add("ItemQuntity", Items.Qty);
                        objItem.Add("ItemImage", Items.Image);
                        objItem.Add("ItemCartValue", cartCurrentItem.Sum(x => x.Count));
                        objItem.Add("MenuId", Items.CategoryID);
                        objItem.Add("ItemIndex", ItemiIndex++);
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
            JObject jObject = JObject.FromObject(TableRowObj);
            HG_OrganizationDetails objOrg = new HG_OrganizationDetails().GetOne(TableRowObj.OrgId);
            jObject.Add("OrgName", objOrg != null ? objOrg.Name : " ");
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
            System.Int64 CID = System.Int64.Parse(Params["CID"].ToString());
            int OrgId = int.Parse(Params["OrgID"].ToString());
            System.Int64 TableorSheatId=System.Int64.Parse(Params["TORSID"].ToString());
            JObject PostResult = new JObject();
            List<Cart> ListCart = Cart.List.FindAll(x => x.CID == CID && x.OrgId==OrgId);
            if (ListCart.Count <= 0)
            {
                PostResult.Add("Status",400);
                PostResult.Add("MSG","Add Atleast one Item");
                return PostResult;
            }
            HG_Orders ObjOrders = new HG_Orders()
            {
                Create_By = CID,
                Create_Date = System.DateTime.Now,
                CID = CID,
                Update_By = CID,
                Status = "1",// order placed
                OrgId = OrgId,
                Table_or_SheatId = TableorSheatId
                };
                System.Int64 NewOID = ObjOrders.Save();
                if (NewOID > 0)
                {
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
                            Status = 1
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
                PostResult.Add("MSG",NewOID.ToString());
            }
            else
            {
                PostResult.Add("Status", 400);
                PostResult.Add("MSG", "Unable To Place Order Try Again.");
                return PostResult;
            }
            
            Cart.List.RemoveAll(x => x.CID == CID);
            return PostResult;
        }

        //Start Chef End Work
        public JArray ChefOrders(int OrgId,String Status)
        {
            JArray tableorSheatList = new JArray();
            try
            {
            List<HG_Orders> Orderlist = new HG_Orders().GetAll(OrgId: OrgId);

               if(Status.Equals("1"))
                {
                    Orderlist = Orderlist.FindAll(x => x.Status == "1");
                    Orderlist = Orderlist.OrderBy(x => x.Create_Date).ToList();
                    var  order  = Orderlist.FirstOrDefault();
                    Orderlist = Orderlist.FindAll(x => x.OID == order.OID);
                }
                else
                {
                    Orderlist = Orderlist.FindAll(x => x.Status == Status);
                }
             HG_OrganizationDetails ObjOrg = new HG_OrganizationDetails().GetOne(OrgId);
            int OrgType =int.Parse(ObjOrg.OrgTypes);
            List<HG_Tables_or_Sheat> ListTableOrSheat = new HG_Tables_or_Sheat().GetAll(OrgType, OrgId);
            List<HG_FloorSide_or_RowName> ListFloorSideorRow = new HG_FloorSide_or_RowName().GetAll(OrgType, OrgId);
            List<HG_Floor_or_ScreenMaster> ListFloorScreen = new HG_Floor_or_ScreenMaster().GetAll(OrgType, OrgId);
            string TableSheatPrefix = ObjOrg.OrgTypes == "1" ? "Table : " :"Sheat : ";
            List<HG_Items> ListfoodItems = new HG_Items().GetAll(OrgId);
                //string SideOrRowPrefix = ObjOrg.OrgTypes == "1" ? "Table" : "Sheat: ";
                int TorSIndex = 0;
              foreach(var order in Orderlist)
                {
                    HG_Tables_or_Sheat hG_Tables_Or_Sheat = ListTableOrSheat.Find(x => x.Table_or_RowID == order.Table_or_SheatId);
                    HG_FloorSide_or_RowName hG_FloorSide_Or_RowName = ListFloorSideorRow.Find(x => x.ID == hG_Tables_Or_Sheat.FloorSide_or_RowNoID);
                    HG_Floor_or_ScreenMaster hG_Floor_Or_ScreenMaster = ListFloorScreen.Find(x => x.Floor_or_ScreenID == hG_Tables_Or_Sheat.Floor_or_ScreenId);
                    JObject TableScreen = new JObject();
                    TableScreen.Add("TableScreenInfo", TableSheatPrefix + hG_Tables_Or_Sheat.Table_or_SheetName + " " + hG_Floor_Or_ScreenMaster.Name + " " + hG_FloorSide_Or_RowName.FloorSide_or_RowName);
                    TableScreen.Add("TableSeatID", hG_Tables_Or_Sheat.Table_or_RowID);
                    List<HG_OrderItem> hG_OrderItems = new HG_OrderItem().GetAll(order.OID);
                    JArray ItemsArray = new JArray();
                    int ItemIndex = 0;
                    foreach (var OrderItem in hG_OrderItems)
                    {
                        HG_Items hG_Items = ListfoodItems.Find(x => x.ItemID == OrderItem.FID);
                        JObject itemobj = new JObject();
                        itemobj.Add("OIID", OrderItem.OIID);
                        itemobj.Add("ItemID", OrderItem.FID);
                        itemobj.Add("ItemName", hG_Items.Items);
                        itemobj.Add("Quantity", OrderItem.Qty);
                        itemobj.Add("Status", OrderItem.Status);
                        itemobj.Add("IIndex", ItemIndex++);
                        ItemsArray.Add(itemobj);
                    }
                    TableScreen.Add("OrderItems", ItemsArray);
                    TableScreen.Add("TorSIndex", TorSIndex++);
                    tableorSheatList.Add(TableScreen);
                }
              
           
            }
            catch(System.Exception e)
            {

            }
            return tableorSheatList;

        }
        public JObject ChangeOrderItemStatus(String OIID, int Status,int UpdateBy)
        {
            HG_OrderItem hG_OrderItem = new HG_OrderItem().GetOne(Int64.Parse(OIID));
            JObject PostResult = new JObject();
            if (hG_OrderItem != null)
            {
                hG_OrderItem.Status = Status;

                Int64 save = hG_OrderItem.Save();
                if (save > 0)
                {
                    PostResult.Add("Status", "200");
                    PostResult.Add("Msg", "Success");
                }
                else
                {
                    PostResult.Add("Status", "400");
                    PostResult.Add("Msg", "Fail");
                }

            }
            else
            {
                PostResult.Add("Status", "400");
                PostResult.Add("Msg", "Order Item Not Found");
            }

            return PostResult;

        }
        public JObject ChangeOrdersStatus(String OID, String Status, int UpdateBy)
        {
            JObject PostResult = new JObject();

            HG_Orders hG_Order = new HG_Orders().GetOne(Int64.Parse(OID));
            List<HG_OrderItem> OrderItemList = new HG_OrderItem().GetAll(Int64.Parse(OID));
            if (hG_Order != null && OrderItemList.Count > 0)
            {
                hG_Order.Status = Status;

                Int64 save = hG_Order.Save();
                if (save > 0)
                {


                    foreach (HG_OrderItem orderItem in OrderItemList)
                    {

                        orderItem.Status = int.Parse(Status);
                        orderItem.Save();
                    }
                    PostResult.Add("Status", "200");
                    PostResult.Add("Msg", "Success");

                }
                else
                {
                    PostResult.Add("Status", "400");
                    PostResult.Add("Msg", "Fail");
                }
            }
            else
            {
                PostResult.Add("Status", "400");
                PostResult.Add("Msg", "Order Not Found.");
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
                user_obj.Password = OldPassword;
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
            Random generator = new Random();
            string  OTPNumber = generator.Next(100000, 999999).ToString("D6");
            OTPGeneretion ObjOtp = new OTPGeneretion();
            ObjOtp.MobileNO = MobileNO;
            ObjOtp.OTP = OTPNumber;
           
           if(ObjOtp.save() > 0)
            {
                // Settings settingsObj = new Settings().GetOne("Mgs");
                // APICONTACT&senderid=FOODDO&msg=APIMSG
                string Msg = "Your Otp For FooDo App Is " + OTPNumber+"";
                HttpWebRequest webRequest =(HttpWebRequest) HttpWebRequest.Create("http://host6.hemsmedia.com/app/smsapi/index.php?key=25DC260CCC0CBF&campaign=0&routeid=5&type=text&contacts="+ MobileNO+ "&senderid=FOODDO&msg="+Msg);
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

        public JArray TableAndTakAway()
        {
            List<HG_Tables_or_Sheat> list = new HG_Tables_or_Sheat().G
        }







    }
}
