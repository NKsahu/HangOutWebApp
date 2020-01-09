using HangOut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using System.Net;

namespace HangOut.Controllers
{
    public class HG_Tables_or_SheatController : Controller
    {
        // GET: HG_Tables INDEX
        public ActionResult Index(int Type)
        {
            HG_Tables_or_Sheat ObjTable = new HG_Tables_or_Sheat();
            List<HG_Tables_or_Sheat> listtable = ObjTable.GetAll(Type);
            return View(listtable);
        }
        public ActionResult SheetIndex(int Type)
        {
            HG_Tables_or_Sheat ObjTable = new HG_Tables_or_Sheat();
            List<HG_Tables_or_Sheat> listtable = ObjTable.GetAll(Type);
            return View(listtable);
        }
        public ActionResult SheetCreateEdit(int ID)
        {
            HG_Tables_or_Sheat ObjTable = new HG_Tables_or_Sheat();
            if (ID > 0)
            {
                ObjTable = ObjTable.GetOne(ID);

            }
            return View(ObjTable);
        }
        [HttpPost]
        public ActionResult SheetCreateEdit(HG_Tables_or_Sheat ObjTable)
        {
            if (ObjTable.QrCode == null || ObjTable.QrCode.Replace(" ","") == "")
            {
                return Json(new { msg = "Please Enter Qr Code" });
            }
            HG_Tables_or_Sheat TorSAlreadyObj = new HG_Tables_or_Sheat().GetOne(QrOcde:ObjTable.QrCode);
            if (ObjTable.OrgId == 0)
            {
                var ObjOrg = Request.Cookies["UserInfo"];
                ObjTable.OrgId = int.Parse(ObjOrg["OrgId"]);
            }
            if(TorSAlreadyObj != null && TorSAlreadyObj.QrCode!="0" && TorSAlreadyObj.Table_or_RowID>0 && TorSAlreadyObj.Table_or_RowID!= ObjTable.Table_or_RowID)
            {
                string QrMsg = "Qr Code Already used ";
                if (TorSAlreadyObj.OrgId != ObjTable.OrgId)
                {
                    QrMsg = "Qr Code Already used For Other Organization";
                }
                return Json(new { msg =QrMsg });
            }
            Int64 i = ObjTable.save();
            if (i > 0)
            {
                HG_Floor_or_ScreenMaster ObjScr = new HG_Floor_or_ScreenMaster().GetOne(ObjTable.Floor_or_ScreenId);
                HG_FloorSide_or_RowName ObjRowName = new HG_FloorSide_or_RowName().GetOne(ObjTable.FloorSide_or_RowNoID);
                JObject jObject = JObject.FromObject(ObjTable);
                jObject.Add("ScreenName",ObjScr.Name);
                jObject.Add("RowName", ObjRowName.FloorSide_or_RowName);
                return Json(new {data=jObject.ToString() }, JsonRequestBehavior.AllowGet);
            }
                
            return RedirectToAction("Error");
        }
        public ActionResult CreateEdit(int ID)
        {

            HG_Tables_or_Sheat ObjTable = new HG_Tables_or_Sheat();
            if (ID > 0)
            {
                ObjTable = ObjTable.GetOne(ID);

            }
            return View(ObjTable);
        }
        [HttpPost]
        public ActionResult CreateEdit(HG_Tables_or_Sheat ObjTable)
        {
            if (ObjTable.QrCode == null || ObjTable.QrCode.Replace(" ","") == "")
            {
                return Json(new { msg = "Please Enter Qr Code" });
            }
            HG_Tables_or_Sheat TorSAlreadyObj = new HG_Tables_or_Sheat().GetOne(QrOcde: ObjTable.QrCode);
            if (ObjTable.OrgId == 0)
            {
                var ObjOrg = Request.Cookies["UserInfo"];
                ObjTable.OrgId = int.Parse(ObjOrg["OrgId"]);
            }
            if(TorSAlreadyObj != null && TorSAlreadyObj.QrCode!="0" && TorSAlreadyObj.Table_or_RowID>0 && TorSAlreadyObj.Table_or_RowID!= ObjTable.Table_or_RowID)
            {
                string QrMsg= "Qr Code Already used ";
                if(TorSAlreadyObj.OrgId!= ObjTable.OrgId)
                {
                    QrMsg = "Qr Code Already used For Other Organization";
                }
                return Json(new { msg = QrMsg });
            }
            Int64 i = ObjTable.save();
            if (i > 0)
            {
                HG_Floor_or_ScreenMaster ObjScr = new HG_Floor_or_ScreenMaster().GetOne(ObjTable.Floor_or_ScreenId);
                HG_FloorSide_or_RowName ObjRowName = new HG_FloorSide_or_RowName().GetOne(ObjTable.FloorSide_or_RowNoID);
                JObject jObject = JObject.FromObject(ObjTable);
                jObject.Add("ScreenName", ObjScr.Name);
                jObject.Add("RowName", ObjRowName.FloorSide_or_RowName);
                return Json(new { data = jObject.ToString() }, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Error");
        }
        public ActionResult Delete(int ID)
        {
            HG_Tables_or_Sheat ObjTable = new HG_Tables_or_Sheat().GetOne(ID);
            if (ObjTable != null)
            {
                List<HG_Orders> listord = new HG_Orders().GetAll(ObjTable.OrgId);
                listord = listord.FindAll(x => x.Table_or_SheatId == ObjTable.Table_or_RowID);
                if (listord.Count > 0)
                {
                   // Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(new { msg = "Already Used in "+listord.Count.ToString()+" Orders" },JsonRequestBehavior.AllowGet);

                }
                else
                {
                    HG_Tables_or_Sheat.Dell(ObjTable.Table_or_RowID);
                }
            }
           return Json(new { data = "1" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Error()
        {
            return View();
        }

        public ActionResult LiveSearch()
        {

            return View();
        }

        public ActionResult CreateTakeAway(int Id)
        {
            HG_Tables_or_Sheat ObjTable = new HG_Tables_or_Sheat();
            if (Id > 0)
            {
                ObjTable = ObjTable.GetOne(Id);

            }
            else
            {
                ObjTable.Table_or_RowID = 0;
                ObjTable.Type = "3";
                ObjTable.QrCode = "";
                ObjTable.Status = 1;

            }
            return View(ObjTable);
        }
        [HttpPost]
        public ActionResult SaveTakeAway(HG_Tables_or_Sheat ObjTakeAway)
        {
            if (ObjTakeAway.QrCode == null)
            {
                ObjTakeAway.QrCode = "";
            }
            if (ObjTakeAway.OrgId == 0)
            {
                var ObjOrg = Request.Cookies["UserInfo"];
                ObjTakeAway.OrgId = int.Parse(ObjOrg["OrgId"]);
            }
            Int64 i = ObjTakeAway.save();
            if (i > 0)
                return RedirectToAction("TakeAwayIndex");
            return RedirectToAction("Error");
        }
        public ActionResult TakeAwayIndex()
        {
            HG_Tables_or_Sheat ObjTable = new HG_Tables_or_Sheat();
            List<HG_Tables_or_Sheat> listtable = ObjTable.GetAll(3);
            return View(listtable);
        }

    }
}