using System;
using System.Collections.Generic;
using HangOut.Models.MyCustomer;
using HangOut.Models.DynamicList;
using System.Web.Mvc;
namespace HangOut.Controllers.MyCustomer
{
    public class ComplemetryOfferController : Controller
    {
        // GET: ComplemetryOffer
        public ActionResult index()
        {
            List<Cashback> Cashbks = Cashback.GetAll(OrderType.CurrOrgId(), 1);
            Cashbks = Cashbks.FindAll(x => x.CampeignType == 2);
            return View(Cashbks);
        }
        public ActionResult CreateEdit(int CBID)
        {
            Cashback cashback = new Cashback();
            cashback.CampeignType = 2;
            if (CBID > 0)
            {
                cashback = Cashback.Getone(CBID);
                cashback.CampeignType = 2;
                if (cashback.ValidTill == 1)
                {
                    cashback.ValidTillDate = DateTime.Now;
                    cashback.ValidTillStr = cashback.ValidTillDate.ToString("dd-MM-yyyy");
                }
            }

            return View(cashback);
        }
        public ActionResult EditItems(int CBID)
        {

            return View();
        }
        
    }
}