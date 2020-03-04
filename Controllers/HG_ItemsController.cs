using HangOut.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using HangOut.Models.Common;
using System;
using System.Linq;

namespace HangOut.Controllers
{
    [LoginFilter]
    public class HG_ItemsController : Controller
    {


        // GET: HG_Items index

        public ActionResult Index()
        {
            HG_Items Objitem = new HG_Items();
            List<HG_Items> Listitem = Objitem.GetAll();
            return View(Listitem);
        }

        public ActionResult CreateEdit(int ID)
        {
            HG_Items Objitem = new HG_Items();
            if (ID > 0)
            {
                Objitem = Objitem.GetOne(ID);
            }
            return View(Objitem);
        }
        [HttpPost]
        public ActionResult CreateEdit(HG_Items Objitem, System.Web.HttpPostedFileBase FoodImg)
        {
            if (Objitem.Qty == null)
            {
                Objitem.Qty = "";

            }
            if (Objitem.ItemDiscription == null)
            {
                Objitem.ItemDiscription = "";
            }
            if (Objitem.Type == 0)
            {
                Objitem.Type = 1;
            }
            if (Objitem.OrgID == 0)
            {
                var OrgObj = Request.Cookies["UserInfo"];
                Objitem.OrgID = int.Parse(OrgObj["OrgId"]);
            }
            if (Objitem.ApplyAddOn == 2&&Objitem.AddOnCatId==0)
            {
                return Json(new { msg = "Select Addon Category " });
            }
            if (Objitem.CategoryID == 0)
            {
                return Json(new { msg = "Select Item Category Name" });
            }
            Objitem.EntryBy = Convert.ToInt32(Request.Cookies["UserInfo"]["UserCode"]);

            //check for category change and apply it to OrderMenu Section
            if (Objitem.ItemID > 0)
            {
                HG_Items OldObjItem = new HG_Items().GetOne(Objitem.ItemID);
                if (OldObjItem.CategoryID != Objitem.CategoryID)
                {
                  List<OrdMenuCtgItems> ListItemsinCategory = OrdMenuCtgItems.GetAll(ItemId:Objitem.ItemID);
                    List<OrderMenuCategory> MenuCategoryList = OrderMenuCategory.GetAll(CategoryId: Objitem.CategoryID);
                    foreach (var CtgItem in ListItemsinCategory)
                  {
                        foreach(var MenuCategory in MenuCategoryList)
                        {
                            List<OrdMenuCtgItems> MenuCategoryItems = OrdMenuCtgItems.GetAll(MenuCatTblId: MenuCategory.id);
                            if (MenuCategoryItems.Count > 0)
                            {
                                var ItemExist = MenuCategoryItems.Find(x => x.ItemId == CtgItem.ItemId);
                                if (ItemExist == null)
                                {
                                    CtgItem.OrderNo = MenuCategoryItems.Count + 1;
                                    CtgItem.OrdMenuCatId = MenuCategory.id;
                                    CtgItem.OderMenuId = MenuCategory.OrderMenuid;
                                    CtgItem.save();
                                }
                            }

                        }
                    }      
                }
            }
            int i = Objitem.Save();
            if (i > 0 && FoodImg != null)
            {
                FoodImg.SaveAs(System.IO.Path.Combine(Server.MapPath("~/FoodImg/"), i + ".jpg"));
            }
            if (Objitem.Image.Equals(""))
            {
                Objitem.Image = "/FoodImg/" + i + ".jpg";
                if (Objitem.Save() < 1)
                    return Json(new { msg = "Error in Update Items" });
            }
           
            HG_Category hG_Category = new HG_Category().GetOne(Objitem.CategoryID);
            Objitem.Categoryname = hG_Category.Category;
            return Json(new { data =Objitem}, JsonRequestBehavior.AllowGet);
           
        }

        public ActionResult Delete(System.Int64 ID)
        {
            List<HG_OrderItem> orderList = new HG_OrderItem().GetAll();
            orderList = orderList.FindAll(x => x.FID == ID);
            if (orderList.Count > 0)
            {
                return Json(new { msg = "Item Already Used in Orders" },JsonRequestBehavior.AllowGet);
            }
            List<OrdMenuCtgItems> listitem = OrdMenuCtgItems.GetAll(ItemId: ID);
            if (listitem.Count > 0)
            {
                return Json(new { msg = "Item Already Used in Order Menu " },JsonRequestBehavior.AllowGet);
            }
            else
            {
                int i = HG_Items.Dell(ID);
            }
            return Json(new { data = "1" },JsonRequestBehavior.AllowGet);
        }
        public ActionResult Error()
        {
            return View();
        }

        //Addon Items Index
        public ActionResult AddOnItmIndex(int CatId)
        {
            HG_Items Objitem = new HG_Items();
            List<HG_Items> Listitem = Objitem.GetAll(Type: 2);
            Listitem = Listitem.FindAll(x => x.CategoryID == CatId);
            return View(Listitem);
        }
        // Addon Items Create

        public ActionResult CreateEditAddOn(int ID,int CatId=0)
        {
            HG_Items Objitem = new HG_Items();
            if (ID > 0)
            {
                Objitem = Objitem.GetOne(ID);
            }
            else
            {
                HG_Category hG_Category = new HG_Category().GetOne(CatId);
                Objitem.CategoryID = hG_Category.CategoryID;
                Objitem.OrgID = hG_Category.OrgID;
            }
            return View(Objitem);
        }

        [HttpPost]
        public ActionResult CreateEditAddOn(HG_Items Objitem)
        {
            if (Objitem.Qty == null)
            {
                Objitem.Qty = "";

            }
            if (Objitem.ItemDiscription == null)
            {
                Objitem.ItemDiscription = "";
            }
            if (Objitem.Type == 0)
            {
                Objitem.Type = 2; //addon items
            }
            if (Objitem.OrgID == 0)
            {
                var OrgObj = Request.Cookies["UserInfo"];
                Objitem.OrgID = int.Parse(OrgObj["OrgId"]);
            }
            if (Objitem.CategoryID == 0)
            {
                return Json(new { msg = "Select Item Category Name" });
            }
            Objitem.EntryBy = System.Convert.ToInt32(Request.Cookies["UserInfo"]["UserCode"]);
            int i = Objitem.Save();
            return Json(new {Objitem},JsonRequestBehavior.AllowGet);
        }
        public ActionResult UplExl()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Upload(int OrgID, System.Web.HttpPostedFileBase UplXl)
        {
            string Msg = "Uploaded Successfully";
            if (OrgID <= 0)
            {
                return Json(new { msg = "Select Organization First" });
            }
            if (UplXl == null)
            {
                return Json(new { msg = "Upload Excel File First" });
            }
            try
            {
                var userInfo = Request.Cookies["UserInfo"];
                var EntryBy = int.Parse(userInfo["UserCode"]);
                UplXl.SaveAs(System.IO.Path.Combine(Server.MapPath("~/Image/"), UplXl.FileName));
                var DT = ReadExl.ReadExcelFileDT("~/Image/" + UplXl.FileName);
                if (DT.Rows.Count > 0)
                {
                    HG_OrganizationDetails ObjOrg = new HG_OrganizationDetails().GetOne(OrgID);
                    List<HG_Category> ListCategory = new HG_Category().GetAll(ObjOrg.OrgID);
                    List<HG_Items> ListItem = new HG_Items().GetAll(ObjOrg.OrgID);
                    for (int i = 1; i < DT.Rows.Count; i++)
                    {
                        string CategoryName = (DT.Rows[i][0] == null ? "" : DT.Rows[i][0].ToString());
                        string ItmName = (DT.Rows[i][1] == null ? "" : DT.Rows[i][1].ToString().Trim());
                        string ItmMode = (DT.Rows[i][2] == null ? "1" : DT.Rows[i][2].ToString().Replace(" ", ""));
                        string Discriptn = (DT.Rows[i][3] == null ? "" : DT.Rows[i][3].ToString());
                        string CostPriceStr = (DT.Rows[i][4] == null ? "0.0" : DT.Rows[i][4].ToString().Replace(" ", ""));
                        string Taxstr = (DT.Rows[i][5] == null ? "0.0" : DT.Rows[i][5].ToString().Replace(" ", ""));
                        double CostPrice = double.Parse(CostPriceStr);
                        double Tax = double.Parse(Taxstr);
                        double TaxAmt = (CostPrice * Tax) / 100;
                        double price = CostPrice + TaxAmt;
                        if(ItmMode.ToUpper()== "VEG")
                        {
                            ItmMode = "1";
                        }
                        else if(ItmMode.ToUpper()== "NON-VEG")
                        {
                            ItmMode = "2";
                        }
                        HG_Category ObjCategory = ListCategory.Find(x => x.Category.ToUpper() == CategoryName.ToUpper());
                        if (ObjCategory == null && CategoryName.Replace(" ", "") != "")
                        {
                            ObjCategory = new HG_Category() { Category = CategoryName, OrgID = ObjOrg.OrgID, CategoryID = 0, CategoryType = 1, EntryBy = EntryBy };
                            ObjCategory.Save();
                            ListCategory.Add(ObjCategory);
                        }
                        HG_Items ObjItem = ListItem.Find(x => x.Items.ToUpper() == ItmName.ToUpper());
                        if (ObjItem == null && ItmName.Replace(" ", "") != "" && ObjCategory != null)
                        {
                            ObjItem = new HG_Items() { ItemID = 0, CategoryID = ObjCategory.CategoryID, Items = ItmName, ItemMode = ItmMode, ItemDiscription = Discriptn, CostPrice = CostPrice, Tax = Tax, Price = price, ApplyAddOn = 1, Image = ""
                            , EntryBy = EntryBy, OrgID = OrgID, Qty = "", Type = 1, Status = true

                            };
                            ObjItem.Save();
                            ListItem.Add(ObjItem);
                        }
                        else if (ObjItem != null && ItmName.Replace(" ", "") != "" && ObjCategory != null)
                        {
                            ObjItem.Items = ItmName;
                            ObjItem.ItemMode = ItmMode;
                            if ((ObjItem.CostPrice != CostPrice) || (ObjItem.Tax != Tax))
                            {
                                ObjItem.CostPrice = CostPrice;
                                ObjItem.Tax = Tax;
                                ObjItem.Price = price;
                            }
                            ObjItem.ItemDiscription = Discriptn;
                            ObjItem.Save();
                            ListItem.Add(ObjItem);
                        }
                        else if (ItmName.Replace(" ", "") == ""|| ObjCategory==null)
                        {
                            Msg = "Uploaded Successfully With Some Data Missing";
                        }
                    }
                }
                else
                {
                    return Json(new { msg = "No Any Row Founds" });
                }
            }
            catch (Exception e)
            {
                return Json(new { msg = "Error " + e.Message });
            }

            return Json(new { msg = Msg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ItemAvaiBility()
        {
            HG_Items Objitem = new HG_Items();
            List<HG_Items> Listitem = Objitem.GetAll();
            Listitem = Listitem.OrderByDescending(x => x.ItemAvaibility).ToList();
            return View(Listitem);
        }
        public ActionResult AddItemToOrder()
        {
            return View();
        }
    }
}
