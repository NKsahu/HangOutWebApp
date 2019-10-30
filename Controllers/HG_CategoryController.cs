using HangOut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;




namespace HangOut.Controllers
{
    public class HG_CategoryController : Controller
    {
        // GET: HG_Category
        public ActionResult Index()
        {
            HG_Category Objitem = new HG_Category();
            List<HG_Category> Listitem = Objitem.GetAll();
            return View(Listitem);
        }

        public ActionResult CreateEdit(int ID)
        {
            HG_Category Objitem = new HG_Category();
            if (ID > 0)
            {
                Objitem = Objitem.GetOne(ID);
            }

            return View(Objitem);
        }
        [HttpPost]
        public ActionResult CreateEdit(HG_Category Objitem)
        {


            int i = Objitem.Save();

            if (i > 0)
                return RedirectToAction("Index");
            return RedirectToAction("Error");
        }

        public ActionResult Delete(int ID)
        {
            HG_Category ObjCon = new HG_Category();
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