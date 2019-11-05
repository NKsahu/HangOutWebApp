﻿using HangOut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HangOut.Controllers
{
    public class HG_Tables_or_RowsController : Controller
    {
        // GET: HG_Tables_or_Rows
        public ActionResult Index()
        {
            HG_Tables_or_Rows ObjTable = new HG_Tables_or_Rows();
            List<HG_Tables_or_Rows> listtable = ObjTable.GetAll();
            return View(listtable);
        }
        public ActionResult CreateEdit(int ID)
        {
            int i = 0;
            HG_Tables_or_Rows ObjTable = new HG_Tables_or_Rows();
            if(i>0)
            {
                ObjTable = ObjTable.GetOne(ID);

            }
            return View(ObjTable);
        }
        [HttpPost]
        public ActionResult CreateEdit(HG_Tables_or_Rows ObjTable)
        {
            Int64 i = ObjTable.save();
            if (i > 0)
                return RedirectToAction("Index");
            return RedirectToAction("Error");
        }
        public ActionResult Delete(int ID)
        {
            HG_Tables_or_Rows ObjTable = new HG_Tables_or_Rows();
            int i = ObjTable.Dell(ID);
            if(i>0)
                return RedirectToAction("Index");
            return RedirectToAction("Error");
        }
        public ActionResult Error()
        {
            return View();
        }
    }
}