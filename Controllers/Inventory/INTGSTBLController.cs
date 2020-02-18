using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HangOut.Models.Inventory;

namespace HangOut.Controllers.Inventory
{
    public class INTGSTBLController : Controller
    {
        // GET: INTGSTBL
        public ActionResult Index()
        {
            List<INTGSTBL> listINTGSTBL = INTGSTBL.GetAll();
            
            return View(listINTGSTBL);
        }
        public ActionResult CreateEdit(int ID)
        {
            INTGSTBL Obj = new INTGSTBL();
             
            if (ID > 0)
            {
                Obj = Obj.GetOne(ID);
            }

            return View(Obj);
        }
        // inventory goods and service create edit
        [HttpPost]
        public ActionResult CreateEdit(INTGSTBL Obj)
        {
          if(Obj.Typeid==1)
            {

           
           if( Obj.Qty==0)
            {
                return Json(new { msg = "Opening Stock Required" });
            }
            }
            int i = Obj.Save();
            if (i > 0)
                return Json(new { data = Obj }, JsonRequestBehavior.AllowGet);
            return RedirectToAction("Error");
        }
       public ActionResult ItemsCreateEdit(int ID)
        {
            INTItems iNTItems = new INTItems();
            if(ID>0)
            {
                iNTItems = iNTItems.GetOne(ID);
            }
            return View(iNTItems);
        }
        [HttpPost]
        public ActionResult ItemsCreateEdit(INTItems iNTItems)
        {
            int i = iNTItems.Save();
            if (i > 0)
                return Json(new { data = iNTItems }, JsonRequestBehavior.AllowGet);
            return RedirectToAction("Error");
        }
    }
}