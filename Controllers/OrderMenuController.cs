using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HangOut.Controllers
{
    public class OrderMenuController : Controller
    {
        // GET: OrderMenu
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult AddMenu()
        {

            return View();
        }

        public ActionResult CashBackSeating(int CBID)
        {

            return View();
        }
    }
}