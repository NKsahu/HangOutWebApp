using HangOut.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using HangOut.Models.Common;
using System;
using System.Data;
using System.Data.OleDb;
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
            if (Objitem.InvoiceTitle == null)
            {
                Objitem.InvoiceTitle = "";

            }
            if (Objitem.invoicePhone == null)
            {
                Objitem.invoicePhone = "";
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
            HG_OrganizationDetails ObjOrg = new HG_OrganizationDetails().GetOne(OrgId);
            OrgSetting OrgSeting = OrgSetting.Getone(OrgId);
            if (OrgSeting.OrgId==0)
            {
                OrgSeting = new OrgSetting();
                OrgSeting.OrgId = OrgId;
                if (ObjOrg.OrgTypes =="1"&& OrgSeting.ByCash==0)//restruant
                {
                    OrgSeting.ByCash = 2;//yes
                }
                else if(ObjOrg.OrgTypes=="2" && OrgSeting.ByOnline == 0)
                {
                    OrgSeting.ByOnline = 2;//yes
                }
            }
            return View(OrgSeting);
        }
        public JsonResult SaveSetting(OrgSetting ObjSetting)
        {
            if (ObjSetting.AcptMinOrd == 0||ObjSetting.EnblDeleryChrg==0)
            {
                ObjSetting.DeliveryCharge = 0.00;
            }
            if (ObjSetting.ContactHead1 == null)
            {
                ObjSetting.ContactHead1 = "";

            }
            if (ObjSetting.Contact1 == null)
            {
                ObjSetting.Contact1 = "";

            }
            if (ObjSetting.ContacHead2 == null)
            {
                ObjSetting.ContacHead2 = "";
            }
            if (ObjSetting.Contact2 == null)
            {
                ObjSetting.Contact2 = "";
            }
            ObjSetting.save();
            return  Json(new { data="1" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Upload(int OrgID, System.Web.HttpPostedFileBase UplXl)
        {
            string msg = "Uploaded Succesfully";
            if (OrgID <= 0)
            {
                return Json(new { msg = "Select Organization First" });
            }
            if (UplXl==null)
            {
                return Json(new { msg = "Upload Excel File First" });
            }
            try
            {
                UplXl.SaveAs(System.IO.Path.Combine(Server.MapPath("~/Image/"), UplXl.FileName));
                var DT = ReadExl.ReadExcelFileDT("~/Image/" + UplXl.FileName);
                if (DT.Rows.Count > 0)
                {
                    HG_OrganizationDetails ObjOrg = new HG_OrganizationDetails().GetOne(OrgID);
                    int OrgType = int.Parse(ObjOrg.OrgTypes);
                    List<HG_Tables_or_Sheat> ListTorS = new HG_Tables_or_Sheat().GetAll(OrgType, OrgID);
                    List<HG_Floor_or_ScreenMaster> ListFlrScr = new HG_Floor_or_ScreenMaster().GetAll(OrgType, OrgID);
                    List<HG_FloorSide_or_RowName> ListFsideorRowName = new HG_FloorSide_or_RowName().GetAll(OrgType, OrgID);
                    for(int i=1;i<DT.Rows.Count;i++)
                    {
                        string FlrOrScrName =(DT.Rows[i][0]==null?"": DT.Rows[i][0].ToString());
                        string FlrSideOrRowName = (DT.Rows[i][1] == null ? "" : DT.Rows[i][1].ToString());
                        string TableorSheatName = (DT.Rows[i][2] == null ? "" : DT.Rows[i][2].ToString());
                        string QrCode = (DT.Rows[i][3] == null ? "" : DT.Rows[i][3].ToString().Replace(" ", ""));
                        string QrCodeOld = QrCode;
                        HG_Tables_or_Sheat TorSAlreadyObj = new HG_Tables_or_Sheat().GetOne(QrOcde: QrCode);
                        if (TorSAlreadyObj != null && TorSAlreadyObj.QrCode != "0" && TorSAlreadyObj.QrCode !=""&& TorSAlreadyObj.Table_or_RowID > 0)
                        {
                            QrCode = "0";
                        }
                        var ObjFlrScr = ListFlrScr.Find(x => x.Name.ToUpper().Contains(FlrOrScrName.ToUpper()));
                        var ObjFsideOrRoName = ListFsideorRowName.Find(x => x.FloorSide_or_RowName.ToUpper().Contains(FlrSideOrRowName.ToUpper()));
                        if (ObjFlrScr == null && FlrOrScrName.Replace(" ","")!="")
                        {
                            ObjFlrScr = new HG_Floor_or_ScreenMaster();
                            ObjFlrScr.Name = FlrOrScrName;
                            ObjFlrScr.Type= ObjOrg.OrgTypes;
                            ObjFlrScr.OrgID = OrgID;
                            ObjFlrScr.save();
                            ListFlrScr.Add(ObjFlrScr);
                        }
                        if (ObjFsideOrRoName == null && FlrSideOrRowName.Replace(" ","")!="")
                        {
                            ObjFsideOrRoName = new HG_FloorSide_or_RowName();
                            ObjFsideOrRoName.FloorSide_or_RowName = FlrSideOrRowName;
                            ObjFsideOrRoName.OrgID = OrgID;
                            ObjFsideOrRoName.Type = ObjOrg.OrgTypes;
                            ObjFsideOrRoName.save();
                            ListFsideorRowName.Add(ObjFsideOrRoName);
                        }
                        if(ObjFsideOrRoName!=null&& ObjFlrScr != null)
                        {
                        var ObjTblOrShtExit = ListTorS.Find(x => x.Table_or_SheetName.ToUpper() == TableorSheatName.ToUpper() &&(x.Floor_or_ScreenId== ObjFlrScr.Floor_or_ScreenID) &&(x.FloorSide_or_RowNoID== ObjFsideOrRoName.ID));
                        if (ObjTblOrShtExit == null && TableorSheatName.Replace(" ","")!="")
                        {
                            HG_Tables_or_Sheat hG_Tables_Or_Sheat = new HG_Tables_or_Sheat();
                            hG_Tables_Or_Sheat.OrgId = OrgID;
                            hG_Tables_Or_Sheat.Type = ObjOrg.OrgTypes;
                            hG_Tables_Or_Sheat.Table_or_SheetName = TableorSheatName;
                            hG_Tables_Or_Sheat.QrCode = QrCode;
                            hG_Tables_Or_Sheat.Floor_or_ScreenId = ObjFlrScr.Floor_or_ScreenID;
                            hG_Tables_Or_Sheat.FloorSide_or_RowNoID = ObjFsideOrRoName.ID;
                            hG_Tables_Or_Sheat.save();
                        }
                        else if(ObjTblOrShtExit!=null&& (ObjTblOrShtExit.QrCode != QrCodeOld) && (QrCode != ""))
                        {
                            ObjTblOrShtExit.QrCode = QrCode;
                            ObjTblOrShtExit.save();
                        }
                        }
                        else
                        {
                            msg = "Uploaded Succesfully. With Some Data Missing";
                        }
                    }
                }
                else
                {
                    return Json(new { msg = "No Any Row Founds"});
                }
            }
            catch(Exception e)
            {
                return Json(new { msg = "Error " + e.Message });
            }
            return Json(new { msg = msg }, JsonRequestBehavior.AllowGet);
        }
    }
}
