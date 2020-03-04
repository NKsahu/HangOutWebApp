using HangOut.Models;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
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
        public ActionResult DiscntCharges(Int64 SeatingId,int Type=0,int ID=0,Int64 OID=0)
        {
            OrdDiscntChrge ordDiscntChrge = new OrdDiscntChrge();
            if (SeatingId > 0)
            {
                HG_Tables_or_Sheat SeatingObj = new HG_Tables_or_Sheat().GetOne(SeatingId);
                List<HG_Orders> orders = new HG_Orders().GetListByGetDate(DateTime.Now, DateTime.Now);
                var ObjOrder = orders.Find(x => x.Table_or_SheatId == SeatingId && x.TableOtp == SeatingObj.Otp);
                if (ObjOrder != null && ObjOrder.Status != "3" && ObjOrder.Status != "4")
                {
                    ordDiscntChrge.OID = ObjOrder.OID;
                }
                ordDiscntChrge.Type = Type;
                ordDiscntChrge.SeatingId = SeatingId;
                ordDiscntChrge.SeatingOtp = SeatingObj.Otp;
            }
            else if (ID > 0)
            {
                ordDiscntChrge = OrdDiscntChrge.GetOne(ID);
                Type = ordDiscntChrge.Type;
            }
            else if (OID > 0 && Type != 0)
            {
                ordDiscntChrge.Type = Type;
                ordDiscntChrge.OID = OID;
            }
           
          
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
        public JsonResult SaveEditOrder(Int64 OID,int PMode)
        {
            var UserInfo = Request.Cookies["UserInfo"];
            var UserType = UserInfo["UserType"];
            HG_Orders ObjOrder = new HG_Orders().GetOne(OID);
            
            
            if (UserType != "SA")
            {
                if (ObjOrder.Create_Date < DateTime.Now.AddDays(-2).Date)
                {
                    return Json(new { msg = "Can't Modify Order After 2 days" });
                }
                if (ObjOrder.PaymentStatus == 3 &&PMode!= ObjOrder.PaymentStatus)
                {
                    return Json(new { msg = "Can't change Payment mode" });
                }
                if (ObjOrder.PaymentStatus != 3 && PMode == 3)
                {
                    return Json(new { msg = "Can't change Payment To foodDo" });
                }
            }
            ObjOrder.PaymentStatus = PMode;
            var OrderItems = new HG_OrderItem().GetAll(OID);
            OrderItems = OrderItems.FindAll(x => x.TickedNo == 0);
            if (OrderItems.Count > 0)
            {
                List<HG_Ticket> list = new HG_Ticket().GetAll(ObjOrder.OrgId,onDate:ObjOrder.Create_Date);
                HG_Ticket objticket = new HG_Ticket() { OrgId = ObjOrder.OrgId, OID = ObjOrder.OID, TicketNo = list.Count + 1, DeliveryCharge = 0 };
                int Ticketno = objticket.save();
            }
            ObjOrder.Save();
            return Json(new { data = OID }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveDiscntCharge(OrdDiscntChrge discntCharge)
        {
            double AmtToAdded = 0.00;
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
            if (discntCharge.ID > 0)// modify discnt charges
            {
                discntCharge.Save();
            }
            else if (discntCharge.OID > 0 && discntCharge.SeatingId == 0)//edit Order
            {
                discntCharge.Save();
                HG_Orders hG_Orders = new HG_Orders().GetOne(discntCharge.OID);
                if (hG_Orders.OID > 0)
                {
                    if (hG_Orders.DisntChargeIDs != "" && hG_Orders.DisntChargeIDs != "0")
                    {
                        hG_Orders.DisntChargeIDs = hG_Orders.DisntChargeIDs + "," + discntCharge.ID;
                    }
                    else
                    {
                        hG_Orders.DisntChargeIDs = discntCharge.ID.ToString();
                    }
                    hG_Orders.Save();
                }
            }
            else if (discntCharge.OID > 0 && discntCharge.SeatingId>0)
            {
                DiscntCharge.ListDiscntChrge.Add(discntCharge);
                OrdDiscntChrge.RemoveDiscntCharge(discntCharge.SeatingId, discntCharge.SeatingOtp,discntCharge.OID);
            }
            else
            {
                DiscntCharge.ListDiscntChrge.Add(discntCharge);
            }
            return Json(new { data = discntCharge }, JsonRequestBehavior.AllowGet);
        }
        public JObject UpdateAmt(int ID,int Cnt,int Pmode)
        {
            HG_OrderItem OBJOrderItem = new HG_OrderItem().GetOne(ID);
            var UserInfo = Request.Cookies["UserInfo"];
            var UserType = UserInfo["UserType"];
            JObject result = new JObject();
            if (UserType != "SA")
            {
                if (OBJOrderItem.OrderDate < DateTime.Now.AddDays(-2).Date)
                {
                    result.Add("Status", 400);
                    result.Add("MSG", "Can't Modify Order After 2 days");
                    return result;
                }
                if (Pmode==3)
                {
                    result.Add("Status", 400);
                    result.Add("MSG", "Can't change Order in foodDo mode");
                    return result;
                }
            }
            double price = 0.0;
            if (Cnt >= 0)
            {
                 if(OBJOrderItem.Status==4 &&Cnt>0)
                {
                    OBJOrderItem.Status = 3;
                }
                OBJOrderItem.Count = Cnt;
                OBJOrderItem.Save();
                price = OBJOrderItem.Count * OBJOrderItem.Price;
                result.Add("Status", 200);
                result.Add("MSG", price.ToString("0.00"));
            }
            else
            {
                result.Add("Status", 400);
                result.Add("MSG", "Quantity Can't be minus");
                return result;
            }
            return result;
        }
         
    }
}