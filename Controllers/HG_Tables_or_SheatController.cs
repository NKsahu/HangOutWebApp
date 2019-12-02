using HangOut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HangOut.Controllers
{
    public class HG_Tables_or_SheatController : Controller
    {
        // GET: HG_Tables INDEX
        public ActionResult Index(int Type)
        {
            HG_Tables_or_Sheat ObjTable = new HG_Tables_or_Sheat();
            List<HG_Tables_or_Sheat> listtable = ObjTable.GetAll(Type);
            return View(listtable);
        }
        public ActionResult SheetIndex(int Type)
        {
            HG_Tables_or_Sheat ObjTable = new HG_Tables_or_Sheat();
            List<HG_Tables_or_Sheat> listtable = ObjTable.GetAll(Type);
            return View(listtable);
        }
        public ActionResult SheetCreateEdit(int ID)
        {
            HG_Tables_or_Sheat ObjTable = new HG_Tables_or_Sheat();
            if (ID > 0)
            {
                ObjTable = ObjTable.GetOne(ID);

            }
            return View(ObjTable);
        }
        [HttpPost]
        public ActionResult SheetCreateEdit(HG_Tables_or_Sheat ObjTable)
        {
             Int64 i = ObjTable.save();
            if (i > 0)
                return RedirectToAction("SheetIndex",new { Type= 2});
            return RedirectToAction("Error");
        }
        public ActionResult CreateEdit(int ID)
        {
             
            HG_Tables_or_Sheat ObjTable = new HG_Tables_or_Sheat();
            if(ID>0)
            {
                ObjTable = ObjTable.GetOne(ID);

            }
            return View(ObjTable);
        }
       // [HttpPost]
        public ActionResult CreateEdit(HG_Tables_or_Sheat ObjTable)
        {
            Int64 i = ObjTable.save();
            if (i > 0)
                return Json(ObjTable,JsonRequestBehavior.AllowGet);
            return RedirectToAction("Error");
        }
        public ActionResult Delete(int ID)
        {
            HG_Tables_or_Sheat ObjTable = new HG_Tables_or_Sheat();
            int i = ObjTable.Dell(ID);
            if(i>0)
                return RedirectToAction("Index");
            return RedirectToAction("Error");
        }
        public ActionResult Error()
        {
            return View();
        }

        public ActionResult LiveSearch()
        {

            return View();
        }
    }
}