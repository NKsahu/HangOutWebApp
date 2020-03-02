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
        public ActionResult Index()
        {
            List<Accounts> accounts = Accounts.GetAll();
            return View(accounts);
        }
    }
}