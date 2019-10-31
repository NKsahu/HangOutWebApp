using HangOut.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HangOut.Controllers
{
    public class UsersMenuController : Controller
    {
        // GET: UsersMenu
        public ActionResult Index()
        {
            UsersMenu Objuser = new UsersMenu();
            List<UsersMenu> Listuser = Objuser.GetAll();
            return View();
        }
        public ActionResult CreateEdit(int ID)
        {
            UsersMenu Objuser = new UsersMenu();
            if(ID>0)
            {
                Objuser = Objuser.GetOne(ID);
            }
            return View(Objuser);
        }
        [HttpPost]
        public ActionResult CreateEdit(UsersMenu objuser)
        {
            int i = 0;
            if(i >  0)
            
               return  RedirectToAction("Index");
            return RedirectToAction("Error");
            
        }
        public ActionResult Error()
        {
            return View();
        }
    }
}