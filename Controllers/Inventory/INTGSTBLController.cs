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
            int i = Obj.Save();
            if (i > 0)
                return Json(new { data = Obj }, JsonRequestBehavior.AllowGet);
            return RedirectToAction("Error");
        }
        public ActionResult Delete(int ID)
        {
            List<INTGSTBL> listGoodService = INTGSTBL.GetAll();
            listGoodService = listGoodService.FindAll(x => x.GSID == ID);

            if (listGoodService != null)
            {
                int i = INTGSTBL.Dell(ID);
            }
            return Json(new { data = "1" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Error()
        {
            return View();
        }
    }
}