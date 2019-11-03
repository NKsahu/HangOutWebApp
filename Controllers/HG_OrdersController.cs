using HangOut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HangOut.Controllers
{
    public class HG_OrdersController : Controller
    {
        // GET: HG_Order
        public ActionResult Index()
        {
            HG_Orders listorder = new HG_Orders();
            List<HG_Orders> objorder = listorder.GetAll();
            return View(objorder);
        }
    }
}