using HangOut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HangOut.Controllers
{
    public class HG_OrganizationDetailsController : Controller
    {
        // GET: HG_OrganizationDetails
        public ActionResult Index()
        {
            HG_OrganizationDetails Objitem = new HG_OrganizationDetails();
            List<HG_OrganizationDetails> Listitem = Objitem.GetAll();
            return View(Listitem);
        }

        public ActionResult CreateEdit(int ID)
        {
            HG_OrganizationDetails Objitem = new HG_OrganizationDetails();
            if (ID > 0)
            {
                Objitem = Objitem.GetOne(ID);
            }

            return View(Objitem);
        }
        [HttpPost]
        public ActionResult CreateEdit(HG_OrganizationDetails Objitem)
        {


            int i = Objitem.Save();

            if (i > 0)
                return RedirectToAction("Index");
            return RedirectToAction("Error");
        }

        public ActionResult Delete(int ID)
        {
            HG_OrganizationDetails ObjCon = new HG_OrganizationDetails();
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
