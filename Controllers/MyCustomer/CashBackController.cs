using System;
using System.Collections.Generic;
using HangOut.Models.MyCustomer;
using HangOut.Models.DynamicList;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace HangOut.Controllers.MyCustomer
{
    public class CashBackController : Controller
    {
        public ActionResult Index()
        {
            List<Cashback> Cashbks = Cashback.GetAll(OrderType.CurrOrgId(),1);
            Cashbks = Cashbks.FindAll(x => x.CampeignType == 1);
            return View(Cashbks);
        }
        // GET: CashBack
        public ActionResult CUSTCashBack(int CBID)
        {
            Cashback cashback = new Cashback();
            cashback.CampeignType = 1;
            if (CBID > 0)
            {
                cashback = Cashback.Getone(CBID);
                if (cashback.ValidTill == 1)
                {
                    cashback.ValidTillDate = DateTime.Now;
                    cashback.ValidTillStr = cashback.ValidTillDate.ToString("dd-MM-yyyy");
                }
            }

            return View(cashback);
        }

        [HttpPost]
        public ActionResult PostCasbBack(Cashback cashback)
        {
            int OrgId = OrderType.CurrOrgId();
            Cashback OldCashBk = new Cashback();
            try
            {
                cashback.StartDate = DateTime.ParseExact(cashback.StrStartDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
               cashback.ValidTillDate = DateTime.ParseExact(cashback.ValidTillStr, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                if (cashback.ValidTill == 1)
                {
                    cashback.ValidTillDate = cashback.StartDate.AddYears(20);
                }
                if (cashback.CashBkId==0&& cashback.StartDate.Date < DateTime.Now.Date)
                {
                    return Json(new { msg = "Start Date Can't less than Today's Date" });
                }
                if(cashback.StartDate.Date> cashback.ValidTillDate.Date)
                {
                    return Json(new { msg = "Start Date Can't Greater than ValidTillDate Date" });
                }
                else if (cashback.CashBkId > 0)
                {
                    OldCashBk = Cashback.Getone(cashback.CashBkId);
                   if (OldCashBk.StartDate.Date<=DateTime.Now.Date &&OldCashBk.StartDate.Date!= cashback.StartDate.Date)
                    {

                        return Json(new { msg = "Can't Modify Start Date" });
                    }
                    if (OldCashBk.StartDate.Date <= DateTime.Now.Date && OldCashBk.StartDate.Date>=cashback.StartDate.Date && OldCashBk.StartDate.Date != cashback.StartDate.Date)
                    {

                        return Json(new { msg = "Can't Modify Start Date" });
                    }
                    if(OldCashBk.StartDate.Date > DateTime.Now.Date && cashback.StartDate.Date < DateTime.Now.Date)
                    {
                        return Json(new { msg = "Can't Modify Start Date" });
                    }
                }
                if (cashback.RaiseDynamic == false && cashback.BilAmt > 0 &&cashback.MaxCBLimit==2)//limited amt conditions
                {
                    double MaxCashBackAmt = (cashback.Percentage * cashback.BilAmt*2)/100;
                    if(cashback.MaxAmt< MaxCashBackAmt)
                    {
                        return Json(new { msg = "Max CashBack Amt should be "+ MaxCashBackAmt.ToString("0.00") + " greather than Minimun Bill Amt Of "+cashback.Percentage+" %"});
                        }
                }
                if (cashback.CashBkId == 0)
                {
                    cashback.OrgID = OrgId;
                    cashback.CashBkStatus = 1;
                    cashback.TerminateSts = 1;
                    cashback.SeatingIds = "";
                    var value = DateTime.Now.ToString("ddMMyyyyHHmmss");
                    cashback.CBUniqId = Int64.Parse(value);
                }
                if (OldCashBk.CashBkId > 0 && OldCashBk.CashBkStatus==1 &&OldCashBk.SeatingIds!="")
                {
                    List<int> Seatings = cashback.SeatingIds.Split(',').Select(int.Parse).ToList();
                    List<int> RedSeatings = Cashback.GetRedSeatings(cashback);
                    int Cnt = 0;
                    foreach (var SeatingId in Seatings)
                    {
                        var seating = RedSeatings.Find(x => x == SeatingId);
                        if (seating > 0)
                        {
                            Cnt += 1;
                        }
                    }
                    if (Cnt > 0)
                    {
                        return Json(new { msg = "Another mutual campaign applied on specified table(s)" });
                    }
                }
                cashback.Save();
                JObject response = new JObject();
                response.Add("CashBkId", cashback.CashBkId);
                response.Add("StartDate", cashback.StartDate.ToString("dd-MM-yyyy"));
                response.Add("EndDate", cashback.ValidTillDate.ToString("dd-MM-yyyy"));
                response.Add("TerminateStsID", cashback.TerminateSts);
                response.Add("TStatus", TerminatSts(cashback.TerminateSts));
                response.Add("CBSts", CBSts(cashback.CashBkStatus));
                response.Add("CBStsID", cashback.TerminateSts);
                response.Add("UID", cashback.CBUniqId);
                response.Add("OfferType", cashback.OfferType);
                return Json(new { data = response.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { msg = e.Message });
            }

        }
        public ActionResult ChangeSts(int CBID,int Sts)
        {
            Cashback cashback = Cashback.Getone(CBID);
            if (Sts == 1 && cashback.SeatingIds!="")
            {
                List<int> Seatings = cashback.SeatingIds.Split(',').Select(int.Parse).ToList();
                List<int> RedSeatings = new List<int>();
                RedSeatings = Cashback.GetRedSeatings(cashback);
                int Cnt = 0;
                foreach(var SeatingId in Seatings)
                {
                    var seating = RedSeatings.Find(x => x == SeatingId);
                    if (seating > 0)
                    {
                        Cnt += 1;
                    }
                }
                if (Cnt > 0)
                {
                    return Content("0");
                }
               
            }
            cashback.CashBkStatus = Sts;
            cashback.Save();
            return Content("1");
        }
        public ActionResult ChangeTermSts(int CBID,int Sts)
        {
            Cashback cashback = Cashback.Getone(CBID);
            cashback.TerminateSts = Sts;
            cashback.Save();
            return Content("1");
        }
        public static string TerminatSts(int Sts)
        {
            if (Sts == 1)
            {
                return "Active";
            }
            else
            {
                return "Terminated";
            }
        }
        public static string CBSts(int Sts)
        {
            if (Sts == 1)
            {
                return "Running";
            }
            else
            {
                return "Pause";
            }
        }

        
    }
}