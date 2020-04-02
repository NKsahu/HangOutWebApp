using System;
using System.Collections.Generic;
using HangOut.Models.MyCustomer;
using HangOut.Models.DynamicList;
using System.Web.Mvc;

namespace HangOut.Controllers.MyCustomer
{
    public class CashBackController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        // GET: CashBack
        public ActionResult CUSTCashBack(int CBID)
        {
            Cashback cashback = new Cashback();
            return View(cashback);
        }

        [HttpPost]
        public ActionResult PostCasbBack(Cashback cashback)
        {
            int OrgId = OrderType.CurrOrgId();
            if (cashback.StartDate.Date < DateTime.Now.Date)
            {
                return Json(new { msg = "Start Date Can't less than Today's Date" });
            }
            cashback.Save();
            return Json(new { data = cashback }, JsonRequestBehavior.AllowGet);
        }
    }
    
}