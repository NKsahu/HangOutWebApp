using HangOut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HangOut.Controllers
{
    public class HG_UserTypesController : Controller
    {
        // GET: HG_UserTypes
         
         public ActionResult Index()
        {
            HG_UserTypes ObjContact = new HG_UserTypes();
            List<HG_UserTypes> ListContact = ObjContact.GetAll();
            return View(ListContact);
        }

        public ActionResult CreateEdit(int ID)
        {
            HG_UserTypes ObjContact = new HG_UserTypes();
            if (ID > 0)
            {
                ObjContact = ObjContact.GetOne(ID);
            }

            return View(ObjContact);
        }
        [HttpPost]
        public ActionResult CreateEdit(HG_UserTypes ObjContact )
        {
             

            int i = ObjContact.Save();

            if (i > 0)
                return RedirectToAction("Index");
            return RedirectToAction("Error");
        }

        public ActionResult Delete(int ID)
        {
            HG_UserTypes ObjCon = new HG_UserTypes();
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
  