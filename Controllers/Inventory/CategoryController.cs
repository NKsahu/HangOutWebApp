using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HangOut.Models.Inventory;
using System.Web.Mvc;

namespace HangOut.Controllers.Inventory
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            List<INTCategory> listcategory = INTCategory.GetAll();
            return View(listcategory);
        }
        public ActionResult CreateEdit(int ID)
        {
            INTCategory Objitem = new INTCategory();
            if (ID > 0)
            {
                Objitem = Objitem.GetOne(ID);
            }

            return View(Objitem);
        }
        // category create edit
        [HttpPost]
        public ActionResult CreateEdit(INTCategory Objitem)
        {
            int i = Objitem.Save();
            if (i > 0)
                return Json(new { data = Objitem }, JsonRequestBehavior.AllowGet);
            return RedirectToAction("Error");
        }


        public ActionResult Delete(int ID)
        {
            List<INTCategory> listcategory = INTCategory.GetAll();
            listcategory = listcategory.FindAll(x => x.CatID == ID);
          
            if(listcategory!=null)
            {
                int i = INTCategory.Dell(ID);
            }
            return Json(new { data = "1" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Error()
        {
            return View();
        }
    }
}