using HangOut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HangOut.Controllers
{
    public class HG_FloorSide_or_RowNameController : Controller
    {
        // GET: HG_FloorSide 
        public ActionResult Index(int Type)
        {
            HG_FloorSide_or_RowName ObjRowName = new HG_FloorSide_or_RowName();
            List<HG_FloorSide_or_RowName> listRowName = ObjRowName.GetAll(Type);
            return View(listRowName);
        }
        //table for Row side
        public ActionResult RowIndex(int Type)
        {
            HG_FloorSide_or_RowName ObjRowName = new HG_FloorSide_or_RowName();
            List<HG_FloorSide_or_RowName> listRowName = ObjRowName.GetAll(Type);
            return View(listRowName);
        }
        //table for Row createEdit
        public ActionResult RowCreateEdit(int ID)
        {
            HG_FloorSide_or_RowName ObjRowName = new HG_FloorSide_or_RowName();
            if (ID > 0)
            {
                ObjRowName = ObjRowName.GetOne(ID);
            }
            return View(ObjRowName);
        }
        [HttpPost]
        public ActionResult RowCreateEdit(HG_FloorSide_or_RowName ObjRowName)
        {
            int i = ObjRowName.save();
            if (i > 0)
            return Json(new {data= ObjRowName }, JsonRequestBehavior.AllowGet);
            return RedirectToAction("Error");
        }
        public ActionResult CreateEdit(int ID)
        {
            HG_FloorSide_or_RowName ObjRowName = new HG_FloorSide_or_RowName();
            if( ID > 0)
            {
                ObjRowName = ObjRowName.GetOne(ID);
            }
            return View(ObjRowName);
        }
        [HttpPost]
        public ActionResult CreateEdit(HG_FloorSide_or_RowName ObjRowName)
        {
            int i = ObjRowName.save();
            if (i > 0)
                return Json(new { data = ObjRowName }, JsonRequestBehavior.AllowGet);
            return RedirectToAction("Error");

        }
        public ActionResult Error()
        {
            return View();
        }
        public ActionResult Delete(int ID)
        {
            HG_FloorSide_or_RowName ObjFscr = new HG_FloorSide_or_RowName().GetOne(ID);
            if (ObjFscr != null)
            {
                List<HG_Tables_or_Sheat> ListTorS = new HG_Tables_or_Sheat().GetAll(int.Parse(ObjFscr.Type));
                ListTorS = ListTorS.FindAll(x => x.FloorSide_or_RowNoID == ObjFscr.ID);
                if (ListTorS.Count > 0)
                {
                    return Json(new { msg = "Already Used In Seating " }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    int id = HG_FloorSide_or_RowName.Dell(ID);
                }
            }
            return Json(new { data = "1" }, JsonRequestBehavior.AllowGet);
        }


    }
}