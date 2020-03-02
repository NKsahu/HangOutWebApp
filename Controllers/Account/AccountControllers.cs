using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HangOut.Controllers.Account
{
    public class AccountControllers : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
    }
}