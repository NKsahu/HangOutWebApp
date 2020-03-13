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
        public ActionResult CreatEdit()
        {
            return View();
        }
    }
}