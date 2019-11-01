using HangOut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HangOut.Controllers
{
    public class HG_ItemsController : Controller
    {
         
        
            // GET: HG_Items

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
            Objitem.EntryBy =System.Convert.ToInt32(Request.Cookies["UserInfo"]["UserCode"]);
            int i = Objitem.Save();
            if (i > 0 && FoodImg!=null)
            {
                FoodImg.SaveAs(System.IO.Path.Combine(Server.MapPath("~/FoodImg/"), i + ".jpg"));
            }
            if (Objitem.Image.Equals(""))
            {
                Objitem.Image = "/FoodImg/"+i+".jpg";
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

    }
}
