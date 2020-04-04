using System;
using System.Collections.Generic;
using HangOut.Models.MyCustomer;
using HangOut.Models.DynamicList;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace HangOut.Controllers.MyCustomer
{
    public class CashBackController : Controller
    {
        public ActionResult Index()
        {
            List<Cashback> Cashbks = Cashback.GetAll(OrderType.CurrOrgId());
            return View(Cashbks);
        }
        // GET: CashBack
        public ActionResult CUSTCashBack(int CBID)
        {
            Cashback cashback = new Cashback();
            if (CBID > 0)
            {
                cashback = Cashback.Getone(CBID);
            }

            return View(cashback);
        }

        [HttpPost]
        public ActionResult PostCasbBack(Cashback cashback)
        {
            int OrgId = OrderType.CurrOrgId();
            try
            {
                cashback.StartDate = DateTime.ParseExact(cashback.StartDate.ToString("dd-MM-yyyy"), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                cashback.ValidTillDate = DateTime.ParseExact(cashback.ValidTillDate.ToString("dd-MM-yyyy"), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                if (cashback.StartDate.Date < DateTime.Now.Date)
                {
                    return Json(new { msg = "Start Date Can't less than Today's Date" });
                }
                if (cashback.CashBkId == 0)
                {
                    cashback.CashBkStatus = 1;
                    cashback.TerminateSts = 1;
                    cashback.SeatingIds = "";
                }
                cashback.Save();
                JObject response = new JObject();
                response.Add("CashBkId", cashback.CashBkId);
                response.Add("StartDate", cashback.StartDate.ToString("dd/MM/yyyy"));
                response.Add("TerminateStsID", cashback.TerminateSts);
                response.Add("TStatus", TerminatSts(cashback.TerminateSts));
                response.Add("CBSts", CBSts(cashback.CashBkStatus));
                response.Add("CBStsID", cashback.TerminateSts);
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