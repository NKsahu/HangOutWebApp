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
            if (Obj.Name == null || Obj.Name.Replace(" ","") == "")
            {
                return Json(new { msg = "Please Enter Name" });
            }
            if (Obj.Email == null)
            {
                Obj.Email = "";
            }
            if (Obj.Remarks == null)
            {
                Obj.Remarks = "";
            }
            if(Obj.DebtorType == 0 )
            {
                return Json(new { msg = "Please Select Debtor Type" });
            }
            if (Obj.DebtorType==1 && Obj.OrgId==0)
            {
                return Json(new { msg = "Please Select Organization Name" });

            }
       
            if (Obj.DebtorType == 2)
            {
                Obj.OrgId = 0;
                Obj.MarginOnCash = 0;
                Obj.TaxOnAboveMargin = 0;
                Obj.MarginOnline = 0;
                Obj.TaxOnAboveMarginOnline = 0;
                Obj.PaymentDay = 0;
                Obj.PaymentFrequency = 0;
                Obj.CollectionDay = 0;
                Obj.CollectionFrequency = 0;
                Obj.TDSApplicable = 0;
       
            }
            if(Obj.MobileNo1.Length>0 && Obj.MobileNo1.Length<10)
            {
                return Json(new { msg = "Please Enter Valid Mobile Number" });
            }
            if (Obj.MobileNo2 != null)
            {
                if (Obj.MobileNo2.Length > 0 && Obj.MobileNo2.Length < 10)
                {
                    return Json(new { msg = "Please Enter Valid Mobile Number" });
                }
            }
            else
            {
                Obj.MobileNo2 = "";
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