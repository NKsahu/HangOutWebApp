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
    }
}