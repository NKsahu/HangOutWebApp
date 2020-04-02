using System;
using System.Collections.Generic;
using HangOut.Models.MyCustomer;
using System.Web.Mvc;

namespace HangOut.Controllers.MyCustomer
{
    public class CashBackController : Controller
    {
        // GET: CashBack
        public ActionResult CUSTCashBack(int CBID)
        {
            Cashback cashback = new Cashback();
            return View(cashback);
        }
    }
}