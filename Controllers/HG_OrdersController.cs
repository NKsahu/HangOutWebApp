using HangOut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HangOut.Models.Common;
using System.Web.Mvc;

namespace HangOut.Controllers
{ [LoginFilter]
    public class HG_OrdersController : Controller
    {
        // GET: HG_Order
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult LiveSearch()
        {
            return View();
        }
        public ActionResult MakeOrder()
        {
            return View();
        }
        public ActionResult CaseRecipt()
        {
            return View();
        }
        public ActionResult PrintInvoice(int OID)
        {


            return View();
        }
        public ActionResult DashBoardOrders()
        {
            return View();
        }
        public ActionResult Ledger()
        {
            return View();
        }
        public ActionResult OrderStatus()
        {
            return View();
        }
        public ActionResult UndeliveredOrder()
        {


            return View();
        }
        public ActionResult ByCashAlert()
        {
            return View();
        }
      
    }
}