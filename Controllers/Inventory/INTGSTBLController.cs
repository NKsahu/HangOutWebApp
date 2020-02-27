using System.Collections.Generic;
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
               Obj.iNTItems = INTItems.GetAll(ID);
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
                foreach (var Subitem in Obj.iNTItems)
                {
                    Subitem.IParentId = Obj.GSID;
                    Subitem.Save();
                }
                if (i > 0)
                    return Json(new { data = Obj }, JsonRequestBehavior.AllowGet);
            return RedirectToAction("Error");
        }

        public ActionResult AddSubItem()
        {
            INTItems iNTItems = new INTItems();
            return View(iNTItems);
        }
        public JsonResult GETID()
        {
            int UnitID = int.Parse(Request.QueryString["ParentId"]);
            List<INTUnits> ListUnit = INTUnits.GetAll();
            ListUnit = ListUnit.FindAll(x => x.ParentId == UnitID);
            return Json(new { ListUnit });
        }
    }
}