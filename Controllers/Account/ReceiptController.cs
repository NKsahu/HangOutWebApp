using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HangOut.Models.Account;

namespace HangOut.Controllers.Account
{
    public class ReceiptController : Controller
    {
        // GET: Receipt
        public ActionResult Index(int OrgId)
        {
            List<Receipt> REOBJ = Receipt.GetData(OrgId);
            
            return View(REOBJ);
        }
        public ActionResult LedgerIndex(int ID)
        {
            
            string LedgerName = Ledger.GetAll().Where(w => w.ID == ID).Select(s => s.Name).FirstOrDefault();
            List<Receipt> REOBJ = Receipt.GetLedgerWiseData(LedgerName);

            return View(REOBJ);
        }
    }
}