using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HangOut.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult ChefUnengaged()
        {

            return View();
        }
        public ActionResult AllChefOrders()
        {

            return View();
        }
        public ActionResult PastOrderFilter()
        {
            return View();
        }
        public ActionResult PastOrder()
        {

            return View();
        }
        public ActionResult JoinFoodDoFilter()
        {
           return View();
        }
        public ActionResult JoinFoodDo()
        {
            return View();
        }
    }
}