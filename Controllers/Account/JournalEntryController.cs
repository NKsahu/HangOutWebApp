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

        // JournalEntry create edit
        [HttpPost]
        public ActionResult CreateEdit(Group Obj)
        {
            int i = Obj.Save();
            if (i > 0)
                return Json(new { data = Obj }, JsonRequestBehavior.AllowGet);
            return RedirectToAction("Error");
        }

    }
}