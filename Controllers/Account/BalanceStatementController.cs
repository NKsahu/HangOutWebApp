using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HangOut.Models.Account;
using HangOut.Models;

namespace HangOut.Controllers.Account
{
    public class BalanceStatementController : Controller
    {
        // GET: BalanceStatement
        public ActionResult Index(int OrgId=0)
        {
            if(OrgId>0)
            {
                List<BalanceStatement> balanceStatements = BalanceStatement.GetByOrgId(OrgId);
                return View(balanceStatements);
            }
            else
            {
                List<BalanceStatement> balanceStatements = BalanceStatement.GetAllList();
                return View(balanceStatements);
            }
         
        }
   

        public ActionResult GetDetails(List<HG_OrderItem> CompletedItems)
        {
            BalanceStatement bObj = new BalanceStatement();
            List<BalanceStatement> jObjList = new List<BalanceStatement>();
            List<HG_Orders> OrdersDetails = new List<HG_Orders>();
            List<HG_OrganizationDetails> OrgList = new List<HG_OrganizationDetails>();
            List<HG_Orders> OrdrList = new List<HG_Orders>();
            double totalAmount = 0.00;
           
            for (int i = 0; i < CompletedItems.Count; i++)
            {
                OrdrList = new HG_Orders().GetAll(CompletedItems[i].OrgId, Status: 3);
                HG_Orders Ord = OrdrList.Find(x => x.OID == CompletedItems[i].OID);
                totalAmount += Ord.DeliveryCharge;
                totalAmount += CompletedItems[i].Count * CompletedItems[i].Price;
            }
            Ledger LedgerDetails = Ledger.GetAllList().Where(x => x.DebtorType == 1 
            && x.OrgId == CompletedItems[0].OrgId).FirstOrDefault();
          
                bObj.Date = DateTime.Now;
                bObj.Amount = totalAmount;
                bObj.OrgId = LedgerDetails.OrgId;
                bObj.OrderId = CompletedItems[0].OID;
                BalanceStatement TotalBalance = BalanceStatement.GetAllForBalanceCalculation(CompletedItems[0].OrgId).Last();
                bObj.Balance = TotalBalance.Balance + totalAmount;
                HG_Orders ord = new HG_Orders().GetOne(CompletedItems[0].OID);

                if (ord.PaymentStatus == 1 || ord.PaymentStatus == 2)
                {
                    //bObj.SaveCRValue();
                    bObj.isCash = true;

                }
                else if (ord.PaymentStatus == 3)
                {
                    bObj.Narration = "Online Payment of Order No." + CompletedItems[0].OID;
                    bObj.isCash = false;
                }
                bObj.Save();
                    
            return Json(new { data = bObj }, JsonRequestBehavior.AllowGet);
        }

        // Insert Into Balance Stement After Registration from Calaculation start Date 
        public ActionResult InsertIntoBalanceStementAfterRegistration(DateTime CalaculationStartFrom,int OrgId)
        {

            BalanceStatement bObj = new BalanceStatement();

          
            List<BalanceStatement> jObjList = new List<BalanceStatement>();
            List<HG_Orders> OrdersDetails = new List<HG_Orders>();
            List<HG_OrganizationDetails> OrgList = new List<HG_OrganizationDetails>();          
            List<HG_Orders> OrdrList = new List<HG_Orders>();
            OrdrList = new HG_Orders().GetAll(OrgId, Status: 3);

            foreach (var ord in OrdrList)
            {
                double totalAmt = 0.00;
                List<HG_OrderItem> orderitemlist = new HG_OrderItem().GetAll(ord.OID);
                orderitemlist = orderitemlist.FindAll(x => x.Status == 3 && x.OrderDate.Date>= CalaculationStartFrom.Date);//only completed items
                totalAmt += ord.DeliveryCharge;
                for (int i = 0; i < orderitemlist.Count; i++)
                {
                    totalAmt += orderitemlist[i].Count + orderitemlist[i].Price;
                }        
                  Ledger LedgerDetails = Ledger.GetAllList().Where(x => x.DebtorType == 1
                                       && x.OrgId == OrgId).FirstOrDefault();

                    bObj.Date = ord.Create_Date;
                    bObj.Amount = totalAmt;
                    bObj.OrgId = LedgerDetails.OrgId;
                    bObj.OrderId = orderitemlist[0].OID;

                    BalanceStatement TotalBalance = BalanceStatement.GetAllForBalanceCalculation(OrgId).Last();
                    bObj.Balance = TotalBalance.Balance + totalAmt;
                    HG_Orders order = new HG_Orders().GetOne(orderitemlist[0].OID);

                    if (ord.PaymentStatus == 1 || ord.PaymentStatus == 2)
                    {
                        //bObj.SaveCRValue();
                        bObj.isCash = true;

                    }
                    else if (ord.PaymentStatus == 3)
                    {
                        bObj.Narration = "Online Payment of Order No." + orderitemlist[0].OID;
                        bObj.isCash = false;
                    }
                    bObj.Save();


                
            }
            return Json(new { data = bObj }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult mergeAndSendToAcoount(int OrgId)
        {
            BalanceStatement BSObj = new BalanceStatement();

            Commission CommissionLastrecord = Commission.GetAllCommissions().Where(w=> w.OrgId == OrgId).Last();

            Sale SaleLastrecord = Sale.GetAllSales().Where(w=>w.OrgId == OrgId).Last();

            BSObj.Narration = "Online Payment received Entry No."+ SaleLastrecord.EntryNo;
            BSObj.CRAmount = SaleLastrecord.SaleAmount;

            BalanceStatement TotalBalance = BalanceStatement.GetAllForBalanceCalculation(OrgId).Last();
            BSObj.Balance = TotalBalance.Balance - BSObj.CRAmount;

            BSObj.Date = DateTime.Now;
            BSObj.OrgId = OrgId;
            BSObj.EntryNo = SaleLastrecord.EntryNo;
            BSObj.SaveOpeningValue();

            BalanceStatement Obj = new BalanceStatement();
           

            Obj.Narration = "Commission Invoice No."+ CommissionLastrecord.EntryNo;
            Obj.Amount = CommissionLastrecord.CommissionAmount;
            BalanceStatement TotalBalances = BalanceStatement.GetAllForBalanceCalculation(OrgId).Last();
            Obj.Balance = TotalBalances.Balance + Obj.Amount;
            Obj.Date = DateTime.Now;
            Obj.OrgId = OrgId;
            Obj.EntryNo = CommissionLastrecord.EntryNo;
            Obj.SaveOpeningValue();


            BalanceStatement Obj1 = new BalanceStatement();


            Obj1.Narration = "Opening Balance";
            Obj1.Amount = 0.00;
            BalanceStatement TBalances = BalanceStatement.GetAllForBalanceCalculation(OrgId).Last();
            Obj1.Balance = TBalances.Balance;
            Obj1.Date = DateTime.Now;
            Obj1.OrgId = OrgId;
            Obj1.SaveOpeningValue();

            return Json(new { data = BSObj }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetAllorganization()
        {
          
            List<Ledger> LedgerDetails = Ledger.GetAllList().Where(x => x.DebtorType == 1 
            && x.PaymentDay== Convert.ToInt32(DateTime.Now.DayOfWeek)).ToList();

            foreach (var org in LedgerDetails)
            { 
                GetAllCommission(org.OrgId);
            }
            return Json(new { data = LedgerDetails }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult GetAllCommission(int OrgId)
        {
            // Sale table objects//
            Sale SObj = new Sale();
            Sale S1Obj = new Sale();
            //======================//

            // Commission table objects//
            Commission CObj = new Commission();
            Commission CMObj = new Commission();
            //==================================


            int LastEntry = 0;

            int LastSaleEntry = 0;

            List<BalanceStatement> GetRecords = new List<BalanceStatement>();
            BalanceStatement FindLastEntryNo = new BalanceStatement();

            
            try
            {
               FindLastEntryNo = BalanceStatement.GetAllForBalanceCalculation(OrgId)
                               .Where(w => w.EntryNo > 0 && w.Narration.Contains("Commission Invoice No.")).Last();
            }
            catch(Exception ex)
            {
                ex.ToString();
            }

    
            if(FindLastEntryNo.EntryNo==0)
            {
                 GetRecords = BalanceStatement.GetAllForBalanceCalculation(OrgId);
            }
            else
            {
              GetRecords = BalanceStatement.GetAllForBalanceCalculation(OrgId).Where(w => w.BID> FindLastEntryNo.BID).ToList();

            }


            BalanceStatement LastRecords = BalanceStatement.GetAllForBalanceCalculation(OrgId).Last();

            Ledger LedgerDetails = Ledger.GetAllList().Where(x => x.DebtorType == 1
              && x.OrgId == OrgId).FirstOrDefault();

            foreach (var data in GetRecords)
            {  
              if(data.Narration!= "Opening Balance")
              {
                
                CObj.CommissionAmount = data.CRAmount;
                SObj.SaleAmount = data.Amount;
                if (data.TaxOnCash > 0)
                {
                  CObj.TaxOnCommission = (data.CRAmount*data.TaxOnCash)/100;
                }
                if (data.TaxOnOnline > 0)
                {
                    CObj.TaxOnCommission = (data.CRAmount * data.TaxOnOnline) / 100;
                }                         
               // CObj.BalanceStatementId = data.BID;
                CObj.OrgId = data.OrgId;
                SObj.OrgId = data.OrgId;
               //Save to commision table
                CObj.Save();
                //======================//
                // Save to Sale Table
                SObj.Save();
               //====================//
              }
            }


                int GetLastCalculationRecord = Commission.GetAllCommissions().Where(w => w.OrgId == OrgId).Select(s => s.BalanceStatementId).Max();

                int CommissionId = Commission.GetAllCommissions()
                   .Where(w => w.BalanceStatementId == GetLastCalculationRecord && w.OrgId == OrgId).Select(s => s.CommisionId).FirstOrDefault();

                double TotalTaxOnCommission = Commission.GetAllCommissions()
                        .Where(w => w.CommisionId > CommissionId && w.OrgId == OrgId).Select(s => s.TaxOnCommission).Sum();

                int GetMaxBID = Sale.GetAllSales().Where(w => w.OrgId == OrgId).Select(s => s.BalanceStatementId).Max();

                int SaleId = Sale.GetAllSales()
                    .Where(w => w.BalanceStatementId == GetMaxBID && w.OrgId == OrgId).Select(s => s.SaleId).FirstOrDefault();

                double TotalSale = Sale.GetAllSales().Where(w => w.SaleId > SaleId && w.OrgId == OrgId).Select(s => s.SaleAmount).Sum();

                double totalAmount = Commission.GetAllCommissions()
                                   .Where(w => w.CommisionId > CommissionId && w.OrgId == OrgId).Select(s => s.CommissionAmount).Sum();

            try
            {

                LastEntry = Commission.GetAllCommissions()
                                  .Where(w => w.EntryNo > 0 && w.OrgId == OrgId).Select(s => s.EntryNo).Max();

                LastSaleEntry = Sale.GetAllSales()
                                  .Where(w => w.EntryNo > 0 && w.OrgId == OrgId).Select(s => s.EntryNo).Max();


            }
            catch (Exception ex)
            {
                ex.ToString();
            }

                CMObj.EntryNo = LastEntry + 1;
                CMObj.CommissionAmount = totalAmount;
                CMObj.TaxOnCommission = TotalTaxOnCommission;
                CMObj.BalanceStatementId = LastRecords.BID;
                CMObj.OrgId = LastRecords.OrgId;
                CMObj.Save();


                S1Obj.EntryNo = LastSaleEntry + 1;
                S1Obj.SaleAmount = TotalSale;
                S1Obj.BalanceStatementId = LastRecords.BID;
                S1Obj.OrgId = LastRecords.OrgId;
                S1Obj.Save();
          
            mergeAndSendToAcoount(LastRecords.OrgId);
            EntryToAccount(LastRecords.OrgId);
            return Json(new { data = CObj }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult EntryToAccount(int OrgId)
        {
            Accounts Obj = new Accounts();

            Accounts AObj = new Accounts();

            Accounts AObj1 = new Accounts();

            Accounts ADObj = new Accounts();

            List<Accounts> ADObjList = new List<Accounts>();

            Commission AllCommissions = Commission.GetAllCommissions().Where(w=> w.OrgId == OrgId).Last();

            BalanceStatement LastRecords = BalanceStatement.GetAllForBalanceCalculation(OrgId).Last();

            BalanceStatement FirstRecords = BalanceStatement.GetAllForBalanceCalculation(OrgId).Where(w=>w.Narration=="Opening Balance").Last();

            Sale AllSales = Sale.GetAllSales().Where(w=> w.OrgId == OrgId).Last();

            Obj.Date = FirstRecords.Date;
            Obj.Narration = "Opening Balance";
            Obj.AOrgId = OrgId;
            if (AllSales==null)
            {
                Obj.DRAmount = 0.00;
            }
            else
            {
                Obj.DRAmount = LastRecords.Balance;
            }
            Obj.SaveGeneral();

            AObj.DRAmount = AllSales.SaleAmount;
            AObj.Narration = "Online Payment received Entry No."+ AllSales.EntryNo;
            AObj.Balance = AllSales.SaleAmount;
            AObj.Date = DateTime.Now;
            AObj.AOrgId = OrgId;
            AObj.SaveGeneral();

            //===============================================
            HG_OrganizationDetails Objitem = new HG_OrganizationDetails();
            HG_OrganizationDetails State = Objitem.GetAll(OrgId).FirstOrDefault();

            Ledger LedgerDetails = Ledger.GetAllList().Where(x => x.DebtorType == 1
           && x.OrgId == OrgId).FirstOrDefault();

            AObj1.CRAmount = AllCommissions.CommissionAmount;
            AObj1.Narration = "Commission Invoice No." + AllCommissions.EntryNo;
            AObj1.Balance = AllSales.SaleAmount - AllCommissions.CommissionAmount;
            AObj1.Date = DateTime.Now;
            AObj1.AOrgId = OrgId;

            // entry to Account Details
            if (State.State=="17")
            {

               for(int i=1;i<=2;i++)
                {
                    ADObj = new Accounts();
                    if (i==1)
                    {
                        ADObj.ADDate = DateTime.Now;
                        ADObj.ADAmount = AllCommissions.CommissionAmount * (LedgerDetails.TaxOnAboveMargin / 2) / 100;
                        ADObj.CRLedgerId = 13;
                        ADObj.ADGroupId = 3;
                        ADObj.AOrgId = OrgId;
                        ADObjList.Add(ADObj);
                    }
                    else if(i==2)
                    {
                        ADObj.ADDate = DateTime.Now;
                        ADObj.ADAmount = AllCommissions.CommissionAmount * (LedgerDetails.TaxOnAboveMargin / 2) / 100;
                        ADObj.CRLedgerId = 17;
                        ADObj.ADGroupId = 3;
                        ADObj.AOrgId = OrgId;
                        ADObjList.Add(ADObj);
                    }
                }
            }
            else
            {
                ADObj.ADDate = DateTime.Now;
                ADObj.ADAmount = (AllCommissions.CommissionAmount * LedgerDetails.TaxOnAboveMargin) / 100;
                ADObj.CRLedgerId = 21;
                ADObj.ADGroupId = 3;
                ADObj.AOrgId = OrgId;
                ADObjList.Add(ADObj);
            }

            //=============================
            AObj1.Save(ADObjList);

            return Json(new { data = AllCommissions }, JsonRequestBehavior.AllowGet);
        }
        }
}