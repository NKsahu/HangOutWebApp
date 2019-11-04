using HangOut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HangOut.Controllers
{
    public class HG_Floor_or_ScreenMasterController : Controller
    {
        // GET: HG_Floor_or_ScreenMaster
        public ActionResult Index()
        {
            HG_Floor_or_ScreenMaster Objfloor = new HG_Floor_or_ScreenMaster();
            List<HG_Floor_or_ScreenMaster> listfloor = Objfloor.GetAll();
            return View(listfloor);
        }
        public ActionResult CreateEdit(int ID)
        {
            HG_Floor_or_ScreenMaster Objfloor = new HG_Floor_or_ScreenMaster();
            if (ID > 0)
            {
                Objfloor = Objfloor.GetOne(ID);
            }
            return View(Objfloor);
        }
        [HttpPost]
        public ActionResult CreateEdit(HG_Floor_or_ScreenMaster Objfloor)
        {
            int i = Objfloor.save();
            if (i > 0)
                return RedirectToAction("Index");
            return RedirectToAction("Error");

        }
        
        public ActionResult Error()
        {
            return View();
        }
    }
}