using HangOut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HangOut.Controllers
{
    public class HG_Floor_or_ScreenMasterController : Controller
    {
        // GET: HG_Floor 
        public ActionResult Index(int Type)
        {
            HG_Floor_or_ScreenMaster Objfloor = new HG_Floor_or_ScreenMaster();
            List<HG_Floor_or_ScreenMaster> listfloor = Objfloor.GetAll(Type);
            return View(listfloor);
        }
        // Get: Screen Index
        public ActionResult ScreenIndex(int Type)
        {
            HG_Floor_or_ScreenMaster Objfloor = new HG_Floor_or_ScreenMaster();
            List<HG_Floor_or_ScreenMaster> listfloor = Objfloor.GetAll(Type);
            return View(listfloor);
        }
        // Get: Screen CreateEdit
        public ActionResult ScreenCreateEdit(int ID)
        {

            HG_Floor_or_ScreenMaster Objfloor = new HG_Floor_or_ScreenMaster();
            if (ID > 0)
            {
                Objfloor = Objfloor.GetOne(ID);
            }
            return View(Objfloor);
        }
        [HttpPost]
        public ActionResult ScreenCreateEdit(HG_Floor_or_ScreenMaster Objfloor)
        {
            int i = Objfloor.save();
           List<HG_Floor_or_ScreenMaster> Listscr = new HG_Floor_or_ScreenMaster().GetAll(2);//scrn Type=2
            var ObjFlrExist = Listscr.Find(x => x.Name.ToUpper() == Objfloor.Name.ToUpper() && x.Floor_or_ScreenID != Objfloor.Floor_or_ScreenID);
            if (ObjFlrExist != null)
            {
                return Json(new { msg = "Screen Name Already Exist" }, JsonRequestBehavior.AllowGet);
            }
            if (i > 0)
                return Json(new {data= Objfloor }, JsonRequestBehavior.AllowGet);
            return RedirectToAction("Error");
        }
        public ActionResult CreateEdit(int ID)
        {
            HG_Floor_or_ScreenMaster Objfloor = new HG_Floor_or_ScreenMaster();
           
            if (ID > 0)
            {
                Objfloor = Objfloor.GetOne(ID);
            }
            return View(Objfloor);
        }
        [HttpPost]
        public ActionResult CreateEdit(HG_Floor_or_ScreenMaster Objfloor)
        {
            List<HG_Floor_or_ScreenMaster> Listscr = new HG_Floor_or_ScreenMaster().GetAll(1);//floor Type=1
            var ObjFlrExist = Listscr.Find(x => x.Name.ToUpper() == Objfloor.Name.ToUpper() && x.Floor_or_ScreenID != Objfloor.Floor_or_ScreenID);
            if (ObjFlrExist != null)
            {
                return Json(new { msg = "Floor Name Already Exist" }, JsonRequestBehavior.AllowGet);
            }
            int i = Objfloor.save();
            if (i > 0)
                return Json(new {data= Objfloor }, JsonRequestBehavior.AllowGet);
            return RedirectToAction("Error");

        }
        
        public ActionResult Error()
        {
            return View();
        }

        public ActionResult Delete(int ID)
        {
            HG_Floor_or_ScreenMaster ObjFscr = new HG_Floor_or_ScreenMaster().GetOne(ID);
            if (ObjFscr != null)
            {
                List<HG_Tables_or_Sheat> ListTorS = new HG_Tables_or_Sheat().GetAll(int.Parse(ObjFscr.Type));
                ListTorS = ListTorS.FindAll(x => x.Floor_or_ScreenId == ObjFscr.Floor_or_ScreenID);
                if (ListTorS.Count > 0)
                {
                    return Json(new { msg = "Already Used In Seating " }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    int id = HG_Floor_or_ScreenMaster.Dell(ID);
                }
            }
            return Json(new { data = "1" }, JsonRequestBehavior.AllowGet);
        }

    }
}