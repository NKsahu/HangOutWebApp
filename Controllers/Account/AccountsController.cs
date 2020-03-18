using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HangOut.Models.Account;

namespace HangOut.Controllers.Account
{
    public class AccountsController : Controller
    {
        // GET: Account
        public ActionResult Index(int OrgId,string Name)
        {
            List<Accounts> accounts = new List<Accounts>();
            if (OrgId>0)
            {
                 accounts = Accounts.GetADetails(OrgId,Name);
            
            }
            return View(accounts);
        }
        public ActionResult ACIndex(int ID)
        {
            List<Accounts> accounts = new List<Accounts>();

            string LedgerName = Ledger.GetAll().Where(w => w.ID == ID).Select(s => s.Name).FirstOrDefault();
            List<Accounts> REOBJ = Receipt.GetLedgerWiseData(ID, LedgerName);

            return View(REOBJ);
            
        }
        // GET: JournalEntryUI
        public ActionResult GetACUI()
        {
            Accounts Obj = new Accounts();
            return View(Obj);
        }

        // Entry create edit
        [HttpPost]
        public ActionResult CreateEdit()
        {
           
            return RedirectToAction("Error");
        }




    }
}