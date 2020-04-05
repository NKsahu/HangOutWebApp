﻿using System;
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
            Cashback OldCashBk = new Cashback();
            try
            {
                cashback.StartDate = DateTime.ParseExact(cashback.StartDate.ToString("dd-MM-yyyy"), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                cashback.ValidTillDate = DateTime.ParseExact(cashback.ValidTillDate.ToString("dd-MM-yyyy"), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                if (cashback.ValidTill == 1)
                {
                    cashback.ValidTillDate = cashback.StartDate;
                }
                if (cashback.CashBkId==0&& cashback.StartDate.Date < DateTime.Now.Date)
                {
                    return Json(new { msg = "Start Date Can't less than Today's Date" });
                }
                else if (cashback.CashBkId > 0)
                {
                    OldCashBk = Cashback.Getone(cashback.CashBkId);
                    if(OldCashBk.StartDate.Date!= cashback.StartDate.Date)
                    {
                        return Json(new { msg = "Can't Modify Start Date" });
                    }
                }
                if (cashback.CashBkId == 0)
                {
                    cashback.OrgID = OrgId;
                    cashback.CashBkStatus = 1;
                    cashback.TerminateSts = 1;
                    cashback.SeatingIds = "";
                    var value = DateTime.Now.ToString("ddMMyyHHmmss");
                    cashback.CBUniqId = Int64.Parse(value);
                }
                cashback.Save();
                JObject response = new JObject();
                response.Add("CashBkId", cashback.CashBkId);
                response.Add("StartDate", cashback.StartDate.ToString("dd-MM-yyyy"));
                response.Add("TerminateStsID", cashback.TerminateSts);
                response.Add("TStatus", TerminatSts(cashback.TerminateSts));
                response.Add("CBSts", CBSts(cashback.CashBkStatus));
                response.Add("CBStsID", cashback.TerminateSts);
                response.Add("UID", cashback.CBUniqId);
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
                List<Cashback> Cashbakcs = Cashback.GetAll(OrderType.CurrOrgId(), 1);// all active cashbk of current outlet
                List<int> RedSeatings = new List<int>();
                //List<Cashback> CashbakMutualy = new List<Cashback>();
                Cashbakcs = Cashbakcs.FindAll(x => x.SeatingIds != "");
                Cashbakcs = Cashbakcs.FindAll(x => x.CashBkId != CBID && x.CashBkStatus == 1);// all running cashback not Current cashbk
                foreach(var cashbak in Cashbakcs)
                {
                    if(cashbak.StartDate.Date>= cashback.StartDate.Date && cashback.ValidTill == 1)
                    {
                        RedSeatings.AddRange(cashbak.SeatingIds.Split(',').Select(int.Parse).ToList());
                    }
                    else if(cashbak.StartDate.Date >= cashback.StartDate.Date && cashbak.StartDate.Date <= cashback.ValidTillDate.Date)
                    {
                        RedSeatings.AddRange(cashbak.SeatingIds.Split(',').Select(int.Parse).ToList());
                    }
                }
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