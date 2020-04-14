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
            if (offerObj.Min == 0 || offerObj.Max == 0 )
            {
                return Json(new { msg = "Qty cannot be zero" });
            }
            if (offerObj.Min > offerObj.Max)
            {
                return Json(new { msg = "Min Qty cannot be Greater than Max Qty" });
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
            response.Add("CashBkId", offerObj.CBID);
            return Json(new { data = response.ToString() }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddOfrItem(int Itemid)
        {
            HG_Items objitem = new HG_Items().GetOne(Itemid);
            ItemOffer itemOffers = new ItemOffer();
            itemOffers.ItemId = objitem.ItemID;
            itemOffers.ItemName = objitem.Items;
            itemOffers.TotalItemPrice = objitem.Price;
            return View("OfferItm", itemOffers);
        }

       /////=============offer=====
       ///
       public ActionResult OfferIndex()
        {
            List<Cashback> Cashbks = Cashback.GetAll(OrderType.CurrOrgId(), 1);
            Cashbks = Cashbks.FindAll(x => x.CampeignType == 3);// only offer types
            return View(Cashbks);
        }
        public ActionResult CreaEditOffr(int CBID)
        {
            Cashback cashback = new Cashback();
            
            if (CBID > 0)
            {
                cashback = Cashback.Getone(CBID);
                
            }
            cashback.CampeignType = 3;
            cashback.ValidTill = 2;
            return View(cashback);

        }

        public ActionResult ChoiceBasedCreate(int CBID)
        {
            OfferTitle offerTitle = OfferTitle.GetOne(CBID,1);
            if (offerTitle.OfferMenus.Count == 0)
            {
                var Menu = new OfferMenu();
               // Menu.itemOffers.Add(new ItemOffer());
                offerTitle.OfferMenus.Add(Menu);
            }
            return View(offerTitle);
        }

        [HttpPost]
        public JObject SaveChoiceBased([System.Web.Http.FromBody] OfferTitle offerTitle)
        {
            JObject response = new JObject();
            if(offerTitle.Name==null|| offerTitle.Name == "")
            {
                response.Add("Status", 400);
                response.Add("MSG", "Display Name Required");
                return response;
            }
            if (offerTitle.Discription == null || offerTitle.Discription == "")
            {
                response.Add("Status", 400);
                response.Add("MSG", "Discription Is Required");
                return response;
            }
            if (offerTitle.MaxOrdQty <= 0)
            {
                response.Add("Status", 400);
                response.Add("MSG", "Qty Can't Zero");
                return response;
            }
            var Offers = offerTitle.OfferMenus.FindAll(x => x.IsComplementry==false);
            var Complementry = offerTitle.OfferMenus.FindAll(x => x.IsComplementry);
            if (Offers.Count > 0 && offerTitle.FinalPrice == 0) 
            {
                response.Add("Status", 400);
                response.Add("MSG", "Final Price Cannot be Zero");
                return response;
            }
            if(Complementry.Count== offerTitle.OfferMenus.Count && offerTitle.FinalPrice > 0)
            {
                response.Add("Status", 400);
                response.Add("MSG", "Final Price must be Zero for Complemetry Items");
                return response;
            }
            foreach (var offermenu in offerTitle.OfferMenus)
            {
                if(offermenu.Name==null|| offermenu.Name == "")
                {
                    response.Add("Status", 400);
                    response.Add("MSG", "Name Is Required");
                    return response;
                }
                if (offermenu.Min <= 0 || offermenu.Min > offermenu.Max)
                {
                    response.Add("Status", 400);
                    response.Add("MSG","Invalid Qty");
                    return response;
                }
                if(offermenu.itemOffers == null|| offermenu.itemOffers.Count == 0)
                {
                    response.Add("Status", 400);
                    response.Add("MSG", "Add Atleast on Item");
                    return response;
                }
            }
            offerTitle.Save();
            foreach (var offermenu in offerTitle.OfferMenus)
            {
                offermenu.CBID = offerTitle.CBID;
                offermenu.OfferTitleId = offerTitle.TitleId;
                offermenu.Save();
                foreach(var items in offermenu.itemOffers)
                {
                    items.CashBkId = offermenu.CBID;
                    items.MenuId = offermenu.MenuId;
                    items.Save();
                }
            }
            response.Add("Status", 200);
            response.Add("CBID", offerTitle.CBID);
            return response;
        }
        public ActionResult AddMenu()
        {
            OfferMenu OfferMenus = new OfferMenu();
            OfferMenus.Min = 1;
            OfferMenus.Max = 1;
            return View("OfferMenu",OfferMenus);
        }
       //========ITEM BASED OFFER === 

        public ActionResult ItemBasedCreate(int CBID)
        {
            OfferTitle offerTitle = OfferTitle.GetOneByItems(CBID);
            offerTitle.CBID = CBID;
            return View(offerTitle);
        }
        public ActionResult AddOfrItemBased(int Itemid)
        {
            HG_Items objitem = new HG_Items().GetOne(Itemid);
            ItemOffer itemOffers = new ItemOffer();
            itemOffers.ItemId = objitem.ItemID;
            itemOffers.ItemName = objitem.Items;
            itemOffers.TotalItemPrice = objitem.Price;
            itemOffers.Min = 1;
            return View("OfferItemBased", itemOffers);
        }
        [HttpPost]

        public ActionResult SaveItemBased(OfferTitle offerTitle)
        {
            if(offerTitle.itemOffers==null|| offerTitle.itemOffers.Count == 0)
            {
                return Json(new { msg = "Add Atleast One Item" });
            }
            if (offerTitle.MaxOrdQty == 0)
            {
                return Json(new { msg = "Min Order Qty cannot be zero" });
            }
            if (offerTitle.FinalPrice <= 0)
            {
                return Json(new { msg = "Fianl Price  cannot be zero" });
            }
            foreach (var item in offerTitle.itemOffers)
            {
                if (item.Min <= 0)
                {
                    return Json(new { msg = "Min Item Qty cannot be zero" });
                }
            }
            offerTitle.Type = 2;// item based
            offerTitle.Save();
            
            foreach (var item in offerTitle.itemOffers)
            {
                item.CashBkId = offerTitle.CBID;
                item.Max = item.Min;
                item.Save();
            }
            JObject response = new JObject();
            response.Add("OpenSeating", 2);
            response.Add("CashBkId", offerTitle.CBID);
            return Json(new { data = response.ToString() }, JsonRequestBehavior.AllowGet);
        }
    }
}