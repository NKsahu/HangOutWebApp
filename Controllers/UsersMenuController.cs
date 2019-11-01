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
            Listuser = Listuser.FindAll(x => x.ParentMenuId == 0);
            return View(Listuser);
        }
        [HttpGet]
        public ActionResult Create(int Menu_Id = 0)
        {
            UsersMenu menu = new UsersMenu();
            if (Menu_Id > 0)
            {
                menu = menu.GetOne(Menu_Id);
                if (menu.MultipleUserType == null || menu.MultipleUserType.Length == 0)
                {
                    menu.MultipleUserType = menu.User_Types.Split(',');
                }
            }
            return View(menu);
        }
        [HttpPost]
        public ActionResult Create(UsersMenu ObjMenu)
        {
            if (ObjMenu.MenuDisplayName == null || ObjMenu.MenuDisplayName == "" || ObjMenu.MenuDisplayName == "0")
            {
                return Json(new { msg = "Please Enter Valid Menu Name" });
            }
            string UserTypesstring = string.Join(",", ObjMenu.MultipleUserType);
            ObjMenu.User_Types = UserTypesstring;
            ObjMenu.Save();
            return RedirectToAction("index");
        }

        public ActionResult SubMenuIndex(int Sid)
        {
            List<UsersMenu> Listuser =new UsersMenu().GetAll();
            List<UsersMenu> MenuList = Listuser.FindAll(x => x.ParentMenuId == Sid);
            MenuList = MenuList.OrderBy(x => x.MenuOrderNo).ToList();
            return View(MenuList);
        }
    }
}