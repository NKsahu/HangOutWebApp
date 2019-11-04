using HangOut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HangOut.Controllers
{
    public class HG_FloorSide_or_RowNameController : Controller
    {
        // GET: HG_FloorSide_or_RowName
        public ActionResult Index()
        {
            HG_FloorSide_or_RowName ObjRowName = new HG_FloorSide_or_RowName();
            List<HG_FloorSide_or_RowName> listRowName = ObjRowName.GetAll();
            return View(listRowName);
        }
        public ActionResult CreateEdit(int ID)
        {
            HG_FloorSide_or_RowName ObjRowName = new HG_FloorSide_or_RowName();
            if( ID > 0)
            {
                ObjRowName = ObjRowName.GetOne(ID);
            }
            return View(ObjRowName);
        }
        [HttpPost]
        public ActionResult CreateEdit()
        {
            return View();
        }
    }
}