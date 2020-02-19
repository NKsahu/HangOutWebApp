using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HangOut.Models.Inventory;
using Newtonsoft.Json.Linq;

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
            else
            {
                Obj.iNTItems.Add(new INTItems { IParentId = ID, IQty = 0, ItemID = 0, IUnitID = 0 });
            }

            return View(Obj);
        }
        // inventory goods and service create edit
        [HttpPost]
        public ActionResult CreateEdit(INTGSTBL Obj)
        {
            if (Obj.Typeid == 1)
            {


                if (Obj.Qty == 0)
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
            if (ID > 0)
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
        public JObject SaveItems([System.Web.Http.FromBody] INTGSTBL intgst)
        {
            INTGSTBL OBJINTGSTBL = intgst;
            OBJINTGSTBL.Save();
            List<INTItems> itemlist = OBJINTGSTBL.iNTItems;
            foreach (var item in itemlist)
            {
                item.ItemID = OBJINTGSTBL.GSID;
                item.Save();
            }
            return JObject.FromObject(OBJINTGSTBL);
        }
    }
}