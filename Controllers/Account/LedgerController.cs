using HangOut.Models;
using HangOut.Models.Account;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace HangOut.Controllers.Account
{
    public class LedgerController : Controller
    {
        // GET: Ledger
        public ActionResult Index()
        {
            List<Ledger> ledger = Ledger.GetAll();
            return View(ledger);

        }
        // GET: TheaterLedger
        public ActionResult TheaterLedgerIndex()
        {
            List<Ledger> ledger = Ledger.GetAllTheatersList();
            return View(ledger);

        }
        // GET: AccountsList
        public ActionResult GetAccountList()
        {
            List<Ledger> ledger = Ledger.GetAllTheatersList();
            return View(ledger);
        }

        public ActionResult CreateEdit(int ID)
        {
            Ledger Obj = new Ledger();
            if (ID > 0)
            {
                Obj = Obj.GetOne(ID,0);
            }

            return View(Obj);
        }
        // Ledger create edit
        [HttpPost]
        public ActionResult CreateEdit(Ledger Obj)

        {

            Ledger LedgerObj = new Ledger();

            LedgerObj = LedgerObj.GetOne(0, Obj.OrgId);
            if (Obj.OrgId != 0)
            {
                if (Obj.ID != LedgerObj.ID)
                {
                    return Json(new { msg = "Already Created" });
                }
            }

            DateTime isnulldate =  default(DateTime);
          
            if (Obj.CalculationStartFrom.Equals(isnulldate))
            {              
                Obj.CalculationStartFrom = DateTime.Now;
            }

            if (Obj.LisenceRenewalDate.Equals(isnulldate))
            {
                Obj.LisenceRenewalDate = DateTime.Now;
            }
            
            if (Obj.ShortName==null)
            {
                Obj.ShortName = "";
            }
            if(Obj.ParentGroup!=1)
            {
                if (Obj.AccountNumber == null && (Obj.DebtorType == 0 || Obj.DebtorType == 1 || Obj.DebtorType == 2))
                {
                    Obj.AccountNumber = "";
                }
                else
                {
                    return Json(new { msg = "Please Enter Account Number" });
                }
            }
            else if(Obj.AccountNumber == null)
            {
               return Json(new { msg = "Please Enter Account Number" });
            }

            if (Obj.ParentGroup != 1)
            {
                if (Obj.IFSCCode == null && (Obj.DebtorType == 0 || Obj.DebtorType == 1 || Obj.DebtorType == 2))
                {
                    Obj.IFSCCode = "";
                }
                else
                {
                    return Json(new { msg = "Please Enter IFSC Code" });
                }
            }
            else if (Obj.IFSCCode == null)
            {
                return Json(new { msg = "Please Enter IFSC Code" });
            }


            if (Obj.ParentGroup != 1)
            {
                if (Obj.BankName == null && (Obj.DebtorType == 0 || Obj.DebtorType == 1 || Obj.DebtorType == 2))
                {
                    Obj.BankName = "";
                }
                else
                {
                    return Json(new { msg = "Please Enter Bank Name" });
                }
            }
            else if (Obj.BankName == null)
            {
                return Json(new { msg = "Please Enter Bank Name" });
            }


            if (Obj.ParentGroup != 1)
            {
                if (Obj.Branch == null && (Obj.DebtorType == 0 || Obj.DebtorType == 1 || Obj.DebtorType == 2))
                {
                    Obj.Branch = "";
                }

                else
                {
                    return Json(new { msg = "Please Enter Branch" });
                }
            }
            else if (Obj.Branch == null)
            {
                return Json(new { msg = "Please Enter Branch" });
            }

            if (Obj.Name == null || Obj.Name.Replace(" ","") == "")
            {
                return Json(new { msg = "Please Enter Name" });
            }
            if (Obj.Email == null)
            {
                Obj.Email = "";
            }
            if (Obj.Remarks == null)
            {
                Obj.Remarks = "";
            }
            if (Obj.DebtorType==1 && Obj.OrgId==0)
            {
                return Json(new { msg = "Please Select Organization Name" });

            }
            if (Obj.ParentGroup == 0)
            {
                return Json(new { msg = "Please Select Group Type" });
            }

            if (Obj.DebtorType == 2)
            {
                Obj.OrgId = 0;
                Obj.MarginOnCash = 0;
                Obj.TaxOnAboveMargin = 0;
                Obj.MarginOnline = 0;
                Obj.TaxOnAboveMarginOnline = 0;
                Obj.PaymentDay = 0;
                Obj.PaymentFrequency = 0;
                Obj.CollectionDay = 0;
                Obj.CollectionFrequency = 0;
                Obj.TDSApplicable = 0;
                Obj.ParentGroup = 0;


            }
            if (Obj.ParentGroup == 1)
            {
                Obj.DebtorType = 0;
                Obj.OrgId = 0;
                Obj.MarginOnCash = 0;
                Obj.TaxOnAboveMargin = 0;
                Obj.MarginOnline = 0;
                Obj.TaxOnAboveMarginOnline = 0;
                Obj.PaymentDay = 0;
                Obj.PaymentFrequency = 0;
                Obj.CollectionDay = 0;
                Obj.CollectionFrequency = 0;
                Obj.TDSApplicable = 0;

            }
            if (Obj.MobileNo2 != null)
            {
                if (Obj.MobileNo2.Length > 0 && Obj.MobileNo2.Length < 10)
                {
                    return Json(new { msg = "Please Enter Valid Mobile Number" });
                }
            }
            else
            {
                Obj.MobileNo2 = "";
            }
        
            if (Obj.MobileNo1 != null)
            {
                if (Obj.MobileNo1.Length > 0 && Obj.MobileNo1.Length < 10)
                {
                    return Json(new { msg = "Please Enter Valid Mobile Number" });
                }
            }
            else
            {
                Obj.MobileNo1 = "";
            }
            try
            {
                int i = Obj.Save();
                if (i > 0)
                    return Json(new { data = Obj }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message.ToString() });
            }


            return Json(new { data = Obj }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Delete(int ID)
        {
            List<Ledger> LedgerList = Ledger.GetAll();
            LedgerList = LedgerList.FindAll(x => x.OrgId == ID);

            List<BalanceStatement> BList = BalanceStatement.GetByOrgId(ID);
            BList = BList.FindAll(x => x.OrgId == ID);

            List<Commission> CommissionList = Commission.GetAllCommissions();
            CommissionList = CommissionList.FindAll(x => x.OrgId == ID);

            List<Sale> SList = Sale.GetAllSales();
            SList = SList.FindAll(x => x.OrgId == ID);

            List<Accounts> AccountList = Accounts.GetAll();
            AccountList = AccountList.FindAll(x => x.AOrgId == ID);

            List<Accounts> AccountDetailsList = Accounts.GetAllDetails();
            AccountList = AccountList.FindAll(x => x.ADOrgId == ID);

            List<Receipt> ReceiptList = Receipt.GetAllList(ID,0);
           

            if (LedgerList != null)
            {
                int i = Ledger.Dell(ID);
            }

            if (BList != null)
            {
                int i = BalanceStatement.Dell(ID);
            }

            if (CommissionList != null)
            {
                int i = Commission.Dell(ID);
            }

            if (SList != null)
            {
                int i = Sale.Dell(ID);
            }

            if (AccountList != null)
            {
                int i = Accounts.Dell(ID);
            }
            if (AccountDetailsList != null)
            {
                int i = Accounts.DellAccountDetails(ID);
            }
            if(ReceiptList!=null)
            {
                int i = Receipt.Dell(ID);
            }
            return Json(new { data = "1" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Error()
        {
            return View();
        }

      
    }
}