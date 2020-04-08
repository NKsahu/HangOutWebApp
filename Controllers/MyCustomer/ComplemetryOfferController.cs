using System;
using System.Collections.Generic;
using HangOut.Models.MyCustomer;
using HangOut.Models.DynamicList;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using System.Linq;


namespace HangOut.Controllers.MyCustomer
{
    public class ComplemetryOfferController : Controller
    {
        // GET: ComplemetryOffer
        public ActionResult index()
        {
            return View();
        }
        public ActionResult CreateEdit(int CBID)
        {
            Cashback cashback = new Cashback();
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
        
        
    }
}