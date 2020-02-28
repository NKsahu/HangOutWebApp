using HangOut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HangOut.Models.Common;
using System.Web.Mvc;

namespace HangOut.Controllers
{ 
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
        public ActionResult PrintKot(Int64 OID)
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
        public ActionResult DiscntCharge(Int64 SeatingId)
        {
            OrdDiscntChrge ordDiscntChrge = new OrdDiscntChrge();
            return View(ordDiscntChrge);
        }
        public ActionResult LocalContactIndex()
        {

            List<LocalContacts> listcontact = LocalContacts.GetAll();
           //listcontact.Distinct();
            return View(listcontact);
        }
        public ActionResult TestPrinting()
        {
            return View();
        }
        public ActionResult EditOrder(Int64 OID)
        {
            return View();
        }
        [HttpPost]
        public ActionResult DiscntCharge(DiscntCharge discntCharge)
        {

           return Json(new { data = discntCharge}, JsonRequestBehavior.AllowGet);
        }
    }
}