using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HangOut.Models.Account;

namespace HangOut.Controllers
{
    public class JournalEntryController : Controller
    {
        // GET: JournalEntry
        public ActionResult CreateEdit()
        {
            JournalEntry Obj = new JournalEntry();
            return View(Obj);
        }
    }
}