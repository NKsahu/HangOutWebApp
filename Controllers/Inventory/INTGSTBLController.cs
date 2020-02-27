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
        public JsonResult GetAmt(int PRID)
        {
            //List<Rental> ListRentals = new Rental().GetAll();
            //Rental OnjRentals = ListRentals.Find(x => x.RantalID == PRID);
            //List<RoomTable> ListRooms = new RoomTable().GetAll();
            //RoomTable ObjRooms = ListRooms.Find(x => x.RID == OnjRentals.RID);
            //List<RantPaid> ListRentPaids = new RantPaid().GetAll();
            //RantPaid RentPaidsobj = ListRentPaids.Find(x => x.RantalID == PRID);
            //List<BlockD> listblock = new BlockD().GetAll();
            //BlockD objblock = listblock.Find(x => x.DDID == PRID);
            //List<Login> listlogin = new Login().GetAll();
            //Login OBJLogin = listlogin.Find(x => x.ID == OnjRentals.ID);
            //return Json(new { RID = ObjRooms.RID, RentAgreementFor = OnjRentals.RentAgreementFor, RoomNumber = ObjRooms.RoomNumber, RantAmt = ObjRooms.RantAmt, DDID = ObjRooms.DDID, ID = OBJLogin.ID }, JsonRequestBehavior.AllowGet);
        }
    }
}