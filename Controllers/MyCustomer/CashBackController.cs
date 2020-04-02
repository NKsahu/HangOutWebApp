﻿using System;
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
            if (cashback.StartDate.Date < DateTime.Now.Date)
            {
                return Json(new { msg = "Start Date Can't less than Today's Date" });
            }
            cashback.Save();
            JObject response = new JObject();
            response.Add("CashBkId", cashback.CashBkId);
            response.Add("StartDate", cashback.StartDate.ToString("dd/MM/yyyy"));
            return Json(new { data = response }, JsonRequestBehavior.AllowGet);
        }
    }
    
}