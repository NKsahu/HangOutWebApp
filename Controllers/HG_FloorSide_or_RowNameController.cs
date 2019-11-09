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
                return RedirectToAction("RowIndex",new { Type=2});
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
                return RedirectToAction("Index",new {Type=1 });
            return RedirectToAction("Error");
             
        }
        public ActionResult Error()
        {
            return View();
        }
    }
}