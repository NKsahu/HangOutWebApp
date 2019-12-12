using HangOut.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using HangOut.Models.Common;
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
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int ID)
        {
            HG_Items ObjCon = new HG_Items();
            int d = ObjCon.Dell(ID);
            if (d > 0)
                return RedirectToAction("Index");
            return RedirectToAction("Error");

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
        public JsonResult CreateEditAddOn(HG_Items Objitem)
        {
            if (Objitem.Qty == null)
            {
                Objitem.Qty = "";

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
            return new JsonResult(){JsonRequestBehavior= JsonRequestBehavior.AllowGet, Data=Objitem};
        }
    }
}
