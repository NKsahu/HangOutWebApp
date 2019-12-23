using HangOut.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace HangOut.Controllers
{
    [HangOut.Models.Common.LoginFilter]
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
            if (Objitem.Email == null)
            {
                Objitem.Email="";
            }
            if (Objitem.PANNO == null)
            {
                Objitem.PANNO = "";
            }
            if (Objitem.IvoiceHeading == null)
            {
                Objitem.IvoiceHeading = "";
            }
            if (Objitem.Address == null)
            {
                Objitem.Address = "";
            }
            if (Objitem.AddressLin2 == null)
            {
                Objitem.AddressLin2 = "";
            }
            if (Objitem.AddressLine3 == null)
            {
                Objitem.AddressLine3 = "";
            }
            if (Objitem.Licence2 == null)
            {
                Objitem.Licence2 = "";
            }
            if (Objitem.License3 == null)
            {
                Objitem.License3 = "";
            }
            if (Objitem.Logo == null)
            {
                Objitem.Logo = "";
            }
            if (Objitem.Cell == null)
            {
                Objitem.Cell = "";
            }
            if (Objitem.WebSite == null)
            {
                Objitem.WebSite = "";
            }
            if (Objitem.GSTNO == null)
            {
                Objitem.GSTNO = "";
            }
            if (Objitem.BankName == null)
            {
                Objitem.BankName = "";
            }
            if (Objitem.AcType == null)
            {
                Objitem.AcType = "";
            }
            if (Objitem.ACNO == null)
            {
                Objitem.ACNO = "";
            }
            if (Objitem.City == null)
            {
                Objitem.City = "0";
            }
            if (Objitem.PrintRemark == null)
            {
                Objitem.PrintRemark = "";
            }
            
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
        public ActionResult PrintSetup(int Id)
        {
            //var OrgId =int.Parse(Request.Cookies["UserInfo"]["OrgId"]);
            HG_OrganizationDetails Objitem = new HG_OrganizationDetails();
            if (Id > 0)
            {
                Objitem = Objitem.GetOne(Id);
            }

            return View(Objitem);
        }

        public JsonResult SetOrderStatus(bool OrderStatus)
        {
            var ObjOrg = Request.Cookies["UserInfo"];
            int OrgId = int.Parse(ObjOrg["OrgId"]);
            HG_OrganizationDetails OrgObj = new HG_OrganizationDetails().GetOne(OrgId);

            if (OrgObj.OrgID > 0)
            {
                OrgObj.CustomerOrdering = OrderStatus;
                OrgObj.Save();

                return Json(new { msg = "1" }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new { msg = "0" }, JsonRequestBehavior.AllowGet);
            }

        }

        
        public ActionResult OrgSettingEdit(int OrgId)
        {
            OrgSetting OrgSeting = OrgSetting.Getone(OrgId);
            if (OrgSeting.OrgId==0)
            {
                OrgSeting = new OrgSetting();
                OrgSeting.OrgId = OrgId;
            }
            return View(OrgSeting);
        }

        public JsonResult SaveSetting(OrgSetting ObjSetting)
        {
            ObjSetting.save();
            return  Json(new { data="1" }, JsonRequestBehavior.AllowGet);
        }
    }
}
