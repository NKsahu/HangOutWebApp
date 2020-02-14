using HangOut.Models.Account;
using System.Collections.Generic;
using System.Web.Mvc;

namespace HangOut.Controllers.Account
{
    public class LedgerController : Controller
    {
        // GET: Ledger
        public ActionResult Index()
        {
            List<Ledger> ledger = Ledger.GetAll();
            return View(ledger);

        }
        public ActionResult CreateEdit(int ID)
        {
            Ledger Obj = new Ledger();
            if (ID > 0)
            {
                Obj = Obj.GetOne(ID);
            }

            return View(Obj);
        }
        // category create edit
        [HttpPost]
        public ActionResult CreateEdit(Ledger Obj)
        {
            if(Obj.ShortName==null)
            {
                Obj.ShortName = "";
            }
            if (Obj.Email == null)
            {
                Obj.Email = "";
            }
            if (Obj.Remarks == null)
            {
                Obj.Remarks = "";
            }
            if (Obj.DebtorType == 1)
            {
                Obj.OrgId = 0;
            }
            int i = Obj.Save();
            if (i > 0)
                return Json(new { data = Obj }, JsonRequestBehavior.AllowGet);
            return RedirectToAction("Error");
        }


        public ActionResult Delete(int ID)
        {
            List<Ledger> GroupList = Ledger.GetAll();
            GroupList = GroupList.FindAll(x => x.ID == ID);

            if (GroupList != null)
            {
                int i = Ledger.Dell(ID);
            }
            return Json(new { data = "1" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Error()
        {
            return View();
        }
    }
}