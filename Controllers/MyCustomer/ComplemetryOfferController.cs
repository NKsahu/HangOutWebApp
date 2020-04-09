using System;
using System.Collections.Generic;
using HangOut.Models.MyCustomer;
using HangOut.Models.DynamicList;
using System.Web.Mvc;
using HangOut.Models;
using Newtonsoft.Json.Linq;

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
            OfferObj offerobj = new OfferObj();
            if (CBID > 0)
            {
                offerobj = OfferObj.GetAll(CBID);
            }
            return View(offerobj);
        }
        public ActionResult SaveOfferItem(OfferObj offerObj)
        {
            if (offerObj.itemOffers==null|| offerObj.itemOffers.Count == 0)
            {
                return Json(new { msg = "Add Atleast one Item" });
            }
            if (offerObj.Min == 0 || offerObj.Max == 0)
            {
                return Json(new { msg = "Qty cannot be zero" });
            }
            foreach(var offeritem in offerObj.itemOffers)
            {
                offeritem.CashBkId = offerObj.CBID;
                offeritem.Min = offerObj.Min;
                offeritem.Max = offerObj.Max;
                offeritem.Save();
            }

            JObject response = new JObject();
            response.Add("Status", 1);
            return Json(new { data = response.ToString() }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddOfrItem(int Itemid)
        {
            HG_Items objitem = new HG_Items().GetOne(Itemid);
            ItemOffer itemOffers = new ItemOffer();
            itemOffers.ItemId = objitem.ItemID;
            itemOffers.ItemName = objitem.Items;
            return View("OfferItm", itemOffers);
        }

    }
}