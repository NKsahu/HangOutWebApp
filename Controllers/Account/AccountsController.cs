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
        public ActionResult Index(int OrgId)
        {
            List<Accounts> accounts = new List<Accounts>();
            if (OrgId>0)
            {
                 accounts = Accounts.GetAllACDetails(OrgId);
            
            }
            return View(accounts);
        }

       
    }
}