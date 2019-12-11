﻿using HangOut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;




namespace HangOut.Controllers
{
    public class HG_CategoryController : Controller
    {



        // GET: HG_Category items
        public ActionResult Index()
        {
            HG_Category Objitem = new HG_Category();
            List<HG_Category> Listitem = Objitem.GetAll();
            return View(Listitem);
        }
       
        // item category create edit
        public ActionResult CreateEdit(int ID)
        {
            HG_Category Objitem = new HG_Category();
            if (ID > 0)
            {
                Objitem = Objitem.GetOne(ID);
            }

            return View(Objitem);
        }
        // item category create edit
        [HttpPost]
        public ActionResult CreateEdit(HG_Category Objitem)
        {
            if (Objitem.EntryBy == 0)
            {
             Objitem.EntryBy = System.Convert.ToInt32(Request.Cookies["UserInfo"]["UserCode"]);
            }
            if (Objitem.OrgID == 0)
            {

                var ObjOrg = Request.Cookies["UserInfo"];
                Objitem.OrgID = int.Parse(ObjOrg["OrgId"]);
            }
            if (Objitem.CategoryType == 0)
            {
                Objitem.CategoryType = 1; //item category
            }
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

        //addons category

        public ActionResult AddonsIndex()
        {
            HG_Category Objitem = new HG_Category();
            List<HG_Category> Listitem = Objitem.GetAll(CategoryType: 2);
            return View(Listitem);
        }

        // item category create edit
        public ActionResult CreateEditAddon(int ID)
        {
            HG_Category Objitem = new HG_Category();
            if (ID > 0)
            {
                Objitem = Objitem.GetOne(ID);
            }

            return View(Objitem);
        }
        // item category create edit
        [HttpPost]
        public ActionResult CreateEditAddon(HG_Category Objitem)
        {
            if (Objitem.EntryBy == 0)
            {
                Objitem.EntryBy = System.Convert.ToInt32(Request.Cookies["UserInfo"]["UserCode"]);
            }
            if (Objitem.OrgID == 0)
            {

                var ObjOrg = Request.Cookies["UserInfo"];
                Objitem.OrgID = int.Parse(ObjOrg["OrgId"]);
            }
            if (Objitem.CategoryType == 0)
            {
                Objitem.CategoryType = 2; //Addon category
            }
            int i = Objitem.Save();
            if (i > 0)
                return RedirectToAction("AddonsIndex");
            return RedirectToAction("Error");
        }
    }
}