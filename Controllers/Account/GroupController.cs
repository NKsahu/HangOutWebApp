using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HangOut.Models.Account;

namespace HangOut.Controllers.Account
{
    public class GroupController : Controller
    {
        // GET: Group
        public ActionResult Index()
        {
            List<Group> group = Group.GetAll();
            return View(group);
         
        }
        public ActionResult CreateEdit(int ID)
        {
            Group Obj = new Group();
            if (ID > 0)
            {
                Obj = Obj.GetOne(ID);
            }

            return View(Obj);
        }
        // Group create edit
        [HttpPost]
        public ActionResult CreateEdit(Group Obj)
         {
            int i = Obj.Save();
            if (i > 0)
                return Json(new { data = Obj }, JsonRequestBehavior.AllowGet);
            return RedirectToAction("Error");
        }


        public ActionResult Delete(int ID)
        {
            List<Group> GroupList = Group.GetAll();
            GroupList = GroupList.FindAll(x => x.ID == ID);

            if (GroupList != null)
            {
                int i = Group.Dell(ID);
            }
            return Json(new { data = "1" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Error()
        {
            return View();
        }
    }
}