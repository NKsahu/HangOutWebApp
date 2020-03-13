using System;
using System.Collections.Generic;
using HangOut.Models.POS;
using HangOut.Models.DynamicList;
using System.Web.Mvc;

namespace HangOut.Controllers.POS
{
    public class ServingSizeController : Controller
    {
        // GET: ServingSize
        public ActionResult Index()
        {
            int OrgId = OrderType.CurrOrgId();
            List<ServingSize> servingSizes = ServingSize.GetAll(OrgId); 
            return View(servingSizes);
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
    }
}