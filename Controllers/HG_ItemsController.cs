using HangOut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HangOut.Controllers
{
    public class HG_ItemsController : Controller
    {
         
        
            // GET: HG_Items

            public ActionResult Index()
            {
                HG_Items Objitem = new HG_Items();
                List<HG_Items> Listitem = Objitem.GetAll();
                return View(Listitem);
            }

            public ActionResult CreateEdit(int ID)
            {
                HG_Items Objitem = new HG_Items();
                if (ID > 0)
                {
                Objitem = Objitem.GetOne(ID);
                }

                return View(Objitem);
            }
        [HttpPost]
        public ActionResult CreateEdit(HG_Items Objitem)
        {


            int i = Objitem.Save();

            if (i > 0)
                return RedirectToAction("Index");
            return RedirectToAction("Error");
        }

        public ActionResult Delete(int ID)
        {
            HG_Items ObjCon = new HG_Items();
            int d = ObjCon.Dell(ID);
            if (d > 0)
                return RedirectToAction("Index");
            return RedirectToAction("Error");

        }



        public ActionResult Error()
        {
            return View();
        }

    }
}
