using HangOut.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using HangOut.Models.Common;
using System;

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
            Objitem.EntryBy = System.Convert.ToInt32(Request.Cookies["UserInfo"]["UserCode"]);
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
                UplXl.SaveAs(System.IO.Path.Combine(Server.MapPath("~/Image/"), UplXl.FileName));
                var DT = ReadExl.ReadExcelFileDT("~/Image/" + UplXl.FileName);
                if (DT.Rows.Count > 0)
                {
                    HG_OrganizationDetails ObjOrg = new HG_OrganizationDetails().GetOne(OrgID);
                    for (int i = 1; i < DT.Rows.Count; i++)
                    {
                        string CategoryName = (DT.Rows[i][0] == null ? "" : DT.Rows[i][0].ToString());
                        string ItmName = (DT.Rows[i][1] == null ? "" : DT.Rows[i][1].ToString());
                        string ItmMode = (DT.Rows[i][2] == null ? "" : DT.Rows[i][2].ToString());
                        string Discriptn = (DT.Rows[i][3] == null ? "" : DT.Rows[i][3].ToString().Replace(" ", ""));
                        string CostPrice = (DT.Rows[i][3] == null ? "" : DT.Rows[i][3].ToString().Replace(" ", ""));

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

            return Json(new { data = "1" }, JsonRequestBehavior.AllowGet);
        }
    }
}
