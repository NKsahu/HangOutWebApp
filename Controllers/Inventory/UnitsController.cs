using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HangOut.Models.Inventory;
namespace HangOut.Controllers.Inventory
{
    public class UnitsController : Controller
    {
        // GET: Units
        public ActionResult Index()
        {
             
            List<INTUnits> listunit = INTUnits.GetAll();
            listunit = listunit.FindAll(x => x.ParentId == 0);
           
                return View(listunit);
        }
        public  ActionResult ParentIndex()
        {
            List<INTUnits> listunit = INTUnits.GetAll();
            listunit = listunit.FindAll(x => x.ParentId > 0);
            return View(listunit);
        }
        public ActionResult CreateEdit( int ID)
        {
            INTUnits ObjUnits = new INTUnits();
            if (Request.QueryString["ParentId"] != "0")
            {
                ObjUnits.ParentId = int.Parse(Request.QueryString["ParentId"]);
            }
            if(ID>0)
            {
                ObjUnits = ObjUnits.GetOne(ID);
            }
            return View(ObjUnits);
        }
        [HttpPost]
        public ActionResult CreateEdit(INTUnits ObjUnits)
        {
            int i = ObjUnits.Save();
            if (i > 0)
                return Json(new { data = ObjUnits }, JsonRequestBehavior.AllowGet);
            return RedirectToAction("Error");
        }
        public ActionResult Error()
        {
            return View();
        }
    }
}