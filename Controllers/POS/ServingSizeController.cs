using System;
using System.Collections.Generic;
using HangOut.Models.POS;
using HangOut.Models.DynamicList;
using System.Web.Mvc;
using HangOut.Models;
namespace HangOut.Controllers.POS
{
    public class ServingSizeController : Controller
    {
        // GET: ServingSize
        public ActionResult Index()
        {
            int OrgId = OrderType.CurrOrgId();
            List<HG_Items> ServingItems = new HG_Items().GetAll(OrgId, 2);
            return View(ServingItems);
        }
        public ActionResult CreatEdit(int ID)
        {
            ServingSize servingSize = new ServingSize();
            if (ID > 0)
            {
                servingSize = ServingSize.GetOne(ID);
            }
            return View(servingSize);
        }
        [HttpPost]
        public ActionResult CreatEdit(ServingSize servingSize)
        {
            if (servingSize.ServingId == 0)
            {
                servingSize.OrgId = OrderType.CurrOrgId();
            }
            servingSize.Save();
            return Json(new { data = servingSize }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShowServingSize()
        {
            int OrgId = OrderType.CurrOrgId();
            //List<ServingSize> servingSizes = ServingSize.GetAll(OrgId); 
            List<HG_Items> ServingItems = new HG_Items().GetAll(OrgId, 2);
            return View(ServingItems);
        }
        public ActionResult NewAddonSS(int SSID)
        {
            HG_Items ObjItem = new HG_Items().GetOne(SSID);
            AddOnItems AddOnItemList = new AddOnItems();
            AddOnItemList.ItemId = ObjItem.ItemID;
            AddOnItemList.Title = ObjItem.Items;
            AddOnItemList.CostPrice = ObjItem.CostPrice;
            AddOnItemList.Tax = ObjItem.Tax;
            AddOnItemList.Price = ObjItem.Price;
            AddOnItemList.IsServingAddon = true;
            return View("AddOnItem", AddOnItemList);
        }
    }
}