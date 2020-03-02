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
        public ActionResult DiscntCharges(Int64 SeatingId,int Type)
        {
            OrdDiscntChrge ordDiscntChrge = new OrdDiscntChrge();
            HG_Tables_or_Sheat SeatingObj = new HG_Tables_or_Sheat().GetOne(SeatingId);
            List<HG_Orders> orders = new HG_Orders().GetListByGetDate(DateTime.Now, DateTime.Now);
            var  ObjOrder = orders.Find(x => x.Table_or_SheatId == SeatingId && x.TableOtp == SeatingObj.Otp);
            if (ObjOrder != null &&ObjOrder.Status!="3"&&ObjOrder.Status!="4")
            {
                ordDiscntChrge.OID = ObjOrder.OID;
            }
            ordDiscntChrge.Type = Type;
            ordDiscntChrge.SeatingId = SeatingId;
            ordDiscntChrge.SeatingOtp = SeatingObj.Otp;
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
        public ActionResult SaveDiscntCharge(OrdDiscntChrge discntCharge)
        {
            if (discntCharge.Remark == null)
            {
                discntCharge.Remark = "";
            }
            if(discntCharge.Title==null||discntCharge.Title.Replace(" ", "") == "")
            {
                return Json(new { msg = "Title required" }, JsonRequestBehavior.AllowGet);
            }
            if (discntCharge.Amt > 0 && discntCharge.Tax > 0)
            {
                return Json(new { msg = "fill  Single Option" }, JsonRequestBehavior.AllowGet);
            }
            if (discntCharge.Amt <= 0 && discntCharge.Tax <= 0)
            {
                return Json(new { msg = "value cannot be zero" }, JsonRequestBehavior.AllowGet);
            }
            if (discntCharge.OID > 0)
            {
                DiscntCharge.ListDiscntChrge.Add(discntCharge);
                OrdDiscntChrge.RemoveDiscntCharge(discntCharge.SeatingId, discntCharge.SeatingOtp,discntCharge.OID);
            }
            else
            {
                DiscntCharge.ListDiscntChrge.Add(discntCharge);
            }
            return Json(new { data = discntCharge}, JsonRequestBehavior.AllowGet);
        }
        public string UpdateAmt(int ID,int Cnt)
        {
            HG_OrderItem OBJOrderItem = new HG_OrderItem().GetOne(ID);
            OBJOrderItem.Count = Cnt;
            double price = OBJOrderItem.Count * OBJOrderItem.Price;
            return price.ToString("0.00");
        }
         
    }
}