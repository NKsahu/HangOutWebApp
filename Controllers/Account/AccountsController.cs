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

       
    }
}