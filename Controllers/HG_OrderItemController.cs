using HangOut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HangOut.Controllers
{
    
    public class HG_OrderItemController : Controller
    {
        // GET: HG_OrderItem
        public ActionResult Index( Int64 OID)
        {
            // HG_OrderItem Objorder = new HG_OrderItem();
            // List<HG_OrderItem> listorder = Objorder.GetAll(OID);
            
            return View();
        }

        public ActionResult Paytm()
        {

            return View();
        }
        public ActionResult MakeOrder()
        {
            return View();
        }
        public JsonResult AddItemToOrder(Int64 OID,Int64 ItemId)
        {
            var UserInfo = Request.Cookies["UserInfo"];
            var UserId = int.Parse(UserInfo["UserCode"]);
            HG_Items ObjItem = new HG_Items().GetOne(ItemID: ItemId);
            HG_OrderItem OrderItem = new HG_OrderItem()
            {
                FID = ObjItem.ItemID,
                Price = ObjItem.Price,
                Count = 0,
                IsAddon = "0",
                OID = OID,
                Status = 4,
                TickedNo = 0,
                OrgId = ObjItem.OrgID,
                ChefSeenBy = UserId,
                OrderDate = DateTime.Now,
                UpdatedBy = 0,
                UpdationDate = DateTime.Now,
                OrdById = UserId,
                TaxInItm = ObjItem.Tax,
                CostPrice = ObjItem.CostPrice
            };
            OrderItem.Save();
            OrderItem.CostPrice = OrderItem.Price;
            OrderItem.Price = 0.00;
            OrderItem.ItemNam = ObjItem.Items;
            return Json(new { msg = OrderItem }, JsonRequestBehavior.AllowGet);
        }
    }
}