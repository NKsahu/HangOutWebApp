﻿using HangOut.Models;
using HangOut.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace HangOut.Controllers.Account
{
    public class BalanceStatementController : Controller
    {
        // GET: BalanceStatement
        //public ActionResult Index(int OrgId=0)
        //{
        //    if(OrgId>0)
        //    {
        //        List<BalanceStatement> balanceStatements = BalanceStatement.GetByOrgId(OrgId);
        //        return View(balanceStatements);
        //    }
        //    else
        //    {
        //        List<BalanceStatement> balanceStatements = BalanceStatement.GetAllList();
        //        return View(balanceStatements);
        //    }
         
        //}
        public ActionResult Index(int OrgId)
        {
            DateTime FromDate = DateTime.ParseExact(Request.QueryString["Fdate"], "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime Todate = DateTime.ParseExact(Request.QueryString["Tdate"], "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

            List<BalanceStatement> BS = BalanceStatement.GetDataBYDate(OrgId, FromDate, Todate);
            return View(BS);
        }

        public ActionResult BalanceStatementFilter(int OrgId)
        {
            return View();
        }


        public ActionResult GetDetails(List<HG_OrderItem> CompletedItems)
        {
            BalanceStatement bObj = new BalanceStatement();
            List<BalanceStatement> jObjList = new List<BalanceStatement>();
            List<HG_Orders> OrdersDetails = new List<HG_Orders>();
            List<HG_OrganizationDetails> OrgList = new List<HG_OrganizationDetails>();
            List<HG_Orders> OrdrList = new List<HG_Orders>();
            int LastEntryNo = 0;
            double LastBalance = 0.00;

            List<PaytmResn> Onlinepayment = new List<PaytmResn>();

            double totalAmount = 0.00;
        
            HG_Orders ord = new HG_Orders().GetOne(CompletedItems[0].OID);


            if (ord.PaymentStatus == 1 || ord.PaymentStatus == 2)
                {
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

                bObj.isCash = true;
                BalanceStatement Obj = new BalanceStatement();
                HG_Orders od = new HG_Orders().GetOne(bObj.OrderId);
                Ledger LedgerDetail = Ledger.GetAllList().Where(x => x.DebtorType == 1
                        && x.OrgId == bObj.OrgId).FirstOrDefault();



                if (od.PaymentStatus == 1 || od.PaymentStatus == 2)
                {
                    double amt = (bObj.Amount * LedgerDetails.MarginOnCash) / 100;
                    Obj.CRAmount = (amt) + ((amt * LedgerDetails.TaxOnAboveMargin) / 100);
                    Obj.TaxOnCash = LedgerDetails.TaxOnAboveMargin;
                }
                else if (od.PaymentStatus == 3)
                {
                    double amt = (bObj.Amount * LedgerDetails.MarginOnline) / 100;
                    Obj.CRAmount = (amt) + ((amt * LedgerDetails.TaxOnAboveMarginOnline) / 100);
                    Obj.TaxOnOnline = LedgerDetails.TaxOnAboveMarginOnline;
                }
                Obj.Date = bObj.Date;

                // Obj.Amount = Amount;
                BalanceStatement TBalance = BalanceStatement.GetAllForBalanceCalculation(bObj.OrgId).Last();
                Obj.Balance = TBalance.Balance - Obj.CRAmount;
                Obj.OrgId = bObj.OrgId;
                Obj.OrderId = bObj.OrderId;
                Obj.Narration = "Commission of Order No." + bObj.OrderId;
                Obj.SaveCRValue();


            }
            else if (ord.PaymentStatus == 3)
                {

                double OAmount = 0.00;
                    Onlinepayment = PaytmResn.GetAll();
                    PaytmResn paytmTxn = Onlinepayment.Find(x => x.OID == CompletedItems[0].OID);

                    totalAmount += ord.DeliveryCharge;
                    totalAmount += Convert.ToDouble(paytmTxn.PaidAmount);


                Ledger LedgerDetails = Ledger.GetAllList().Where(x => x.DebtorType == 1
                && x.OrgId == CompletedItems[0].OrgId).FirstOrDefault();

                bObj.Date = DateTime.Now;
                bObj.Amount = totalAmount;
                bObj.OrgId = LedgerDetails.OrgId;
                bObj.OrderId = CompletedItems[0].OID;
                BalanceStatement TotalBalance = BalanceStatement.GetAllForBalanceCalculation(CompletedItems[0].OrgId).Last();
                bObj.Balance = TotalBalance.Balance + totalAmount;

                bObj.Narration = "Online Payment of Order No." + CompletedItems[0].OID;
                    bObj.isCash = false;
                    bObj.SaveCRValue();

                BalanceStatement Obj = new BalanceStatement();
                HG_Orders od = new HG_Orders().GetOne(bObj.OrderId);
                Ledger LedgerDetail = Ledger.GetAllList().Where(x => x.DebtorType == 1
                        && x.OrgId == bObj.OrgId).FirstOrDefault();



                if (od.PaymentStatus == 1 || od.PaymentStatus == 2)
                {
                    double amt = (bObj.Amount * LedgerDetails.MarginOnCash) / 100;
                    Obj.CRAmount = (amt) + ((amt * LedgerDetails.TaxOnAboveMargin) / 100);
                    Obj.TaxOnCash = LedgerDetails.TaxOnAboveMargin;
                }
                else if (od.PaymentStatus == 3)
                {
                    double amt = (bObj.Amount * LedgerDetails.MarginOnline) / 100;
                    Obj.CRAmount = (amt) + ((amt * LedgerDetails.TaxOnAboveMarginOnline) / 100);
                    Obj.TaxOnOnline = LedgerDetails.TaxOnAboveMarginOnline;

                    if (ord.PaymentStatus == 3 && ord.Status == "4")
                    {
                        double RefundAmt = bObj.Amount - OAmount;
                        double GetRefundAmt = (RefundAmt * 2) / 100;
                        double GetRefundAmtwithTax = GetRefundAmt + ((GetRefundAmt * LedgerDetails.TaxOnAboveMarginOnline) / 100);
                        Obj.CRAmount = Obj.CRAmount + GetRefundAmtwithTax;
                    }
                }
                Obj.Date = bObj.Date;

                // Obj.Amount = Amount;
                BalanceStatement TBalance = BalanceStatement.GetAllForBalanceCalculation(bObj.OrgId).Last();
                Obj.Balance = TBalance.Balance - Obj.CRAmount;
                Obj.OrgId = bObj.OrgId;
                Obj.OrderId = bObj.OrderId;
                Obj.Narration = "Commission of Order No." + bObj.OrderId;
                Obj.SaveCRValue();



                //===================================================================================================================
                Receipt ReceiptEntry = new Receipt();

                ReceiptEntry.BalanceStatementId = bObj.BID;
                ReceiptEntry.Date = bObj.Date;
                ReceiptEntry.Amount = bObj.Amount;
                ReceiptEntry.Particular = "Online Payment of Order No." + CompletedItems[0].OID;
                try
                {
                    LastEntryNo = Receipt.GetAllList(0, 0).Select(s => s.EntryNo).Last();
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                if (LastEntryNo > 0)
                {
                    ReceiptEntry.EntryNo = LastEntryNo + 1;
                }
                else
                {
                    ReceiptEntry.EntryNo = 1;
                }

                ReceiptEntry.OrgId = bObj.OrgId;
                string Customer = "Customer";
                string Paytm = "Paytm";
                string Bank = "BANK";
                string CurrentLiabilities = "Current Liabilities";

                Ledger CutomerLedger = Ledger.GetAllList().Where(x => x.Name.ToLower() == Customer.ToLower()).FirstOrDefault();


                Ledger Paytmledger = Ledger.GetAllList().Where(x => x.Name.ToLower() == Paytm.ToLower()).FirstOrDefault();

                ReceiptEntry.CRLedgerId = CutomerLedger.ID;
                ReceiptEntry.DRLedgerId = Paytmledger.ID;

                Group BankGroup = Group.GetAll().Where(x => x.Name.ToLower() == Bank.ToLower()).FirstOrDefault();


                Group CurrentLiabilitiesGroup = Group.GetAll().Where(x => x.Name.ToLower() == CurrentLiabilities.ToLower()).FirstOrDefault();

                ReceiptEntry.CRGroupId = CurrentLiabilitiesGroup.ID;
                ReceiptEntry.DRGroupId = BankGroup.ID;
                try
                {
                    LastBalance = Receipt.GetAllList(0, 0).Select(s => s.Balance).Last();
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                if (LastBalance > 0)
                {
                    ReceiptEntry.Balance = LastBalance + ReceiptEntry.Amount;
                }
                else
                {
                    ReceiptEntry.Balance = ReceiptEntry.Amount;
                }

                ReceiptEntry.Save();

            }
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
            List<PaytmResn> Onlinepayment = new List<PaytmResn>();
            int LastEntryNo = 0;
            double LastBalance = 0.00;

            foreach (var ord in OrdrList)
            {
                double totalAmt = 0.00;
                List<HG_OrderItem> orderitemlist = new HG_OrderItem().GetAll(ord.OID);
                orderitemlist = orderitemlist.FindAll(x => x.Status == 3 && x.OrderDate.Date>= CalaculationStartFrom.Date);//only completed items
              

                    if (ord.PaymentStatus == 1 || ord.PaymentStatus == 2)
                    {
                    for (int i = 0; i < orderitemlist.Count; i++)
                    {
                        OrdrList = new HG_Orders().GetAll(orderitemlist[i].OrgId, Status: 3);
                        HG_Orders Ord = OrdrList.Find(x => x.OID == orderitemlist[i].OID);

                        totalAmt += Ord.DeliveryCharge;
                        totalAmt += orderitemlist[i].Count * orderitemlist[i].Price;


                    }
                    BalanceStatement Obj = new BalanceStatement();
                        HG_Orders od = new HG_Orders().GetOne(ord.OID);
                        Ledger LedgerDetail = Ledger.GetAllList().Where(x => x.DebtorType == 1
                                && x.OrgId == OrgId).FirstOrDefault();

                        if (od.PaymentStatus == 1 || od.PaymentStatus == 2)
                        {
                            double amt = (totalAmt * LedgerDetail.MarginOnCash) / 100;
                            Obj.CRAmount = (amt) + ((amt * LedgerDetail.TaxOnAboveMargin) / 100);
                            Obj.TaxOnCash = LedgerDetail.TaxOnAboveMargin;
                        }
                     
                        Obj.Date = ord.Create_Date; 

                        // Obj.Amount = Amount;
                        BalanceStatement TBalance = BalanceStatement.GetAllForBalanceCalculation(OrgId).Last();
                        Obj.Balance = TBalance.Balance - Obj.CRAmount;
                        Obj.OrgId = OrgId;
                        Obj.OrderId = ord.OID;
                        Obj.Narration = "Commission of Order No." + ord.OID;
                        Obj.SaveCRValue();


                    }
                    else if (ord.PaymentStatus == 3)
                    {
                
                       Onlinepayment = PaytmResn.GetAll();
                       PaytmResn paytmTxn = Onlinepayment.Find(x => x.OID == ord.OID);

                        totalAmt += ord.DeliveryCharge;
                        totalAmt += Convert.ToDouble(paytmTxn.PaidAmount);

                   
                        if (Onlinepayment.Count > 0)
                        {
                            Ledger LedgerDetails = Ledger.GetAllList().Where(x => x.DebtorType == 1
                                                 && x.OrgId == OrgId).FirstOrDefault();

                            bObj.Date = ord.Create_Date;
                            bObj.Amount = totalAmt;
                            bObj.OrgId = LedgerDetails.OrgId;
                            bObj.OrderId = orderitemlist[0].OID;

                            BalanceStatement TotalBalance = BalanceStatement.GetAllForBalanceCalculation(OrgId).Last();
                            bObj.Balance = TotalBalance.Balance + totalAmt;

                            bObj.Narration = "Online Payment of Order No." + orderitemlist[0].OID;
                            bObj.isCash = false;

                            bObj.SaveCRValue();

                      //===============================================================================

                        BalanceStatement Obj = new BalanceStatement();
                        HG_Orders od = new HG_Orders().GetOne(bObj.OrderId);
                        Ledger LedgerDetail = Ledger.GetAllList().Where(x => x.DebtorType == 1
                                && x.OrgId == OrgId).FirstOrDefault();

                       if (od.PaymentStatus == 3)
                        {
                            double amt = (bObj.Amount * LedgerDetails.MarginOnline) / 100;
                            Obj.CRAmount = (amt) + ((amt * LedgerDetails.TaxOnAboveMarginOnline) / 100);
                            Obj.TaxOnOnline = LedgerDetails.TaxOnAboveMarginOnline;

                            if (ord.PaymentStatus == 3 && ord.Status == "4")
                            {
                                double RefundAmt = bObj.Amount - totalAmt;
                                double GetRefundAmt = (RefundAmt * 2) / 100;
                                double GetRefundAmtwithTax = GetRefundAmt + ((GetRefundAmt*LedgerDetails.TaxOnAboveMarginOnline) / 100);
                                Obj.CRAmount = Obj.CRAmount + GetRefundAmtwithTax;
                            }
                        }
                        Obj.Date = bObj.Date;

                        // Obj.Amount = Amount;
                        BalanceStatement TBalance = BalanceStatement.GetAllForBalanceCalculation(OrgId).Last();
                        Obj.Balance = TBalance.Balance - Obj.CRAmount;
                        Obj.OrgId = OrgId;
                        Obj.OrderId = bObj.OrderId;
                        Obj.Narration = "Commission of Order No." + bObj.OrderId;
                        Obj.SaveCRValue();
                    }
                   

                }
                
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

            return Json(new { data = BSObj }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetAllorganization()
        {
          
            List<Ledger> LedgerDetails = Ledger.GetAllList().Where(x => x.DebtorType == 1 
            && x.PaymentDay== Convert.ToInt32(DateTime.Now.DayOfWeek) || x.CollectionDay == Convert.ToInt32(DateTime.Now.DayOfWeek)).ToList();

            foreach (var org in LedgerDetails)
            { 
                GetAllCommission(org.OrgId);
            }
            return Json(new { data = LedgerDetails }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult PostAllorganization()
        {

            List<Ledger> LedgerDetails = Ledger.GetAllList().Where(x => x.DebtorType == 1
            && x.PaymentDay == Convert.ToInt32(DateTime.Now.DayOfWeek) || x.CollectionDay == Convert.ToInt32(DateTime.Now.DayOfWeek)).ToList();

            foreach (var org in LedgerDetails)
            {
                PostToAccount(org.OrgId);
            }
            return Json(new { data = LedgerDetails }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PostToAccount(int OrgId)
        {
            Accounts Obj = new Accounts();
            Accounts AObj = new Accounts();
            Accounts A0Obj = new Accounts();
            Accounts A1Obj = new Accounts();
            Accounts A2Obj = new Accounts();
            Accounts A3Obj = new Accounts();
            Accounts A4Obj = new Accounts();
            int LastEntryNo = 0;
            int JLastEntryNo = 0;
            int RLastEntryNo = 0;
            int PLastEntryNo = 0;

            Ledger LedgerDetails = Ledger.GetAllList().Where(w => w.OrgId == OrgId).FirstOrDefault();

               Receipt GetReceiptEntry = Receipt.GetAllList(OrgId,0)
                .Where(w => w.Date >= LedgerDetails.CalculationStartFrom).LastOrDefault();

            double GetAmountSum = Receipt.GetAllList(OrgId, 0)
              .Where(w => w.Date >= LedgerDetails.CalculationStartFrom).Select(s=>s.Amount).Sum();


            Accounts GetACBalance = Accounts.GetAllACDetails(OrgId)
                       .Where(w => w.AOrgId == OrgId).LastOrDefault();

            Obj.Date = DateTime.Now;
            Obj.Narration = "Online payments from customers";
            Obj.DRAmount = GetAmountSum;
            if (GetACBalance != null)
            {
                AObj.Balance = GetACBalance.Balance - AObj.DRAmount;
            }
            else
            {
                AObj.Balance = GetAmountSum;
            }

            Obj.AOrgId = OrgId;
            Obj.ADRLedgerId = GetReceiptEntry.DRLedgerId;
            Obj.ACRLedgerId = GetReceiptEntry.CRLedgerId;
            Obj.DRGroupId = GetReceiptEntry.DRGroupId;
            Obj.CRGroupId = 12;
            Obj.EntryType = "Receipt";
            Obj.ReceiptID = GetReceiptEntry.ID;
            try
            {
                RLastEntryNo = Accounts.GetAllACDetails(OrgId).Where(w => w.EntryType == "Receipt").Select(s => s.EntryNo).Last();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            if (RLastEntryNo > 0)
            {
                RLastEntryNo = RLastEntryNo + 1;
            }
            else
            {
                RLastEntryNo = 1;
            }
            Obj.EntryNo = RLastEntryNo;

            Obj.SaveGeneral();

            //=====================================================================================================
            AObj.Date = DateTime.Now;
            AObj.Narration = "Online payments from customers";
            AObj.CRAmount = GetAmountSum;
            if(GetACBalance!=null)
            {
                AObj.Balance = GetACBalance.Balance + AObj.CRAmount;
            }
            else
            {
                AObj.Balance = GetAmountSum;
            }
     
            AObj.AOrgId = OrgId;
            AObj.ADRLedgerId = GetReceiptEntry.CRLedgerId;
            AObj.ACRLedgerId = LedgerDetails.ID;
            AObj.DRGroupId = GetReceiptEntry.CRGroupId;
            AObj.CRGroupId = 2;
            AObj.EntryType = "Journal";
            AObj.ReceiptID = GetReceiptEntry.ID;
            try
            {
                JLastEntryNo = Accounts.GetAllACDetails(OrgId).Where(w => w.EntryType == "Journal").Select(s => s.EntryNo).Last();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            if (JLastEntryNo > 0)
            {
                JLastEntryNo = JLastEntryNo + 1;
            }
            else
            {
                JLastEntryNo = 1;
            }
            AObj.EntryNo = JLastEntryNo;

            AObj.SaveGeneral();

            //=============================================================
            if (AObj.Balance > 0)
            {
                Ledger Commission = Ledger.GetAllList().Where(w => w.Name == "Commission").FirstOrDefault();
                A0Obj.Date = DateTime.Now;
                A0Obj.Narration = "Commission Invoice";
                double cAmount = (AObj.Balance * LedgerDetails.MarginOnline) / 100;
                A0Obj.DRAmount = cAmount + (cAmount * LedgerDetails.TaxOnAboveMarginOnline) / 100;

                A0Obj.Balance = AObj.Balance - A0Obj.DRAmount;
                A0Obj.AOrgId = OrgId;
                A0Obj.ADRLedgerId = LedgerDetails.ID;
                A0Obj.ACRLedgerId = Commission.ID;

                A0Obj.DRGroupId = 2;
                A0Obj.CRGroupId = 9;
                A0Obj.EntryType = "Sale";
                A0Obj.ReceiptID = GetReceiptEntry.ID;

                try
                {
                    LastEntryNo = Accounts.GetAllACDetails(OrgId).Where(w => w.EntryType == "Sale").Select(s => s.EntryNo).Last();
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                if (LastEntryNo > 0)
                {
                    LastEntryNo = LastEntryNo + 1;
                }
                else
                {
                    LastEntryNo = 1;
                }
                A0Obj.EntryNo = LastEntryNo;

                A0Obj.SaveGeneral();

                //A0Obj.ACID = A0Obj.AID;

                //A0Obj.ADSave();

                //=============================================================

             //   Ledger Commission = Ledger.GetAllList().Where(w => w.Name == "Commission").FirstOrDefault();
                A1Obj.Date = DateTime.Now;
                A1Obj.Narration = "Theater Commission";
                A1Obj.CRAmount = (AObj.Balance * LedgerDetails.MarginOnline)/100;
             
                A1Obj.Balance = A0Obj.Balance;            
                A1Obj.AOrgId = OrgId;
                A1Obj.ADRLedgerId = LedgerDetails.ID;
                A1Obj.ACRLedgerId = Commission.ID;

                A1Obj.DRGroupId = 2;
                A1Obj.CRGroupId = 9;
                A1Obj.EntryType = "Sale";
                A1Obj.ReceiptID = GetReceiptEntry.ID;

                A1Obj.ACID = A0Obj.AID;
                A1Obj.ADSave();



                //=================================================================================
                HG_OrganizationDetails Objitem = new HG_OrganizationDetails();
                HG_OrganizationDetails State = Objitem.GetAll(OrgId).FirstOrDefault();
                if (State.State == "17")
                {
                    if (LedgerDetails.TaxOnAboveMarginOnline == 5)
                    {
                        Ledger CGST = Ledger.GetAllList().Where(w => w.Name == "CGST Local Sale 2.5%").FirstOrDefault();

                        Ledger SGST = Ledger.GetAllList().Where(w => w.Name == "SGST Local Sale 2.5%").FirstOrDefault();

                        A2Obj.Narration = "CGST Local Sale @2.5%";
                        A3Obj.Narration = "SGST Local Sale @2.5%";

                        A2Obj.ADRLedgerId = Commission.ID;
                        A2Obj.ACRLedgerId = CGST.ID;

                        A3Obj.ADRLedgerId = Commission.ID;
                        A3Obj.ACRLedgerId = SGST.ID;
                    }
                    if (LedgerDetails.TaxOnAboveMarginOnline == 12)
                    {
                        Ledger CGST = Ledger.GetAllList().Where(w => w.Name == "CGST Local Sale 6%").FirstOrDefault();

                        Ledger SGST = Ledger.GetAllList().Where(w => w.Name == "SGST Local Sale 6%").FirstOrDefault();

                        A2Obj.Narration = "CGST Local Sale @6%";
                        A3Obj.Narration = "SGST Local Sale @6%";

                        A2Obj.ADRLedgerId = Commission.ID;
                        A2Obj.ACRLedgerId = CGST.ID;

                        A3Obj.ADRLedgerId = Commission.ID;
                        A3Obj.ACRLedgerId = SGST.ID;
                    }
                    if (LedgerDetails.TaxOnAboveMarginOnline == 18)
                    {
                        Ledger CGST = Ledger.GetAllList().Where(w => w.Name == "CGST Local Sale 9%").FirstOrDefault();

                        Ledger SGST = Ledger.GetAllList().Where(w => w.Name == "SGST Local Sale 9%").FirstOrDefault();

                        A2Obj.Narration = "CGST Local Sale @9%";
                        A3Obj.Narration = "SGST Local Sale @9%";

                        A2Obj.ADRLedgerId = Commission.ID;
                        A2Obj.ACRLedgerId = CGST.ID;

                        A3Obj.ADRLedgerId = Commission.ID;
                        A3Obj.ACRLedgerId = SGST.ID;
                    }
                    if (LedgerDetails.TaxOnAboveMarginOnline == 28)
                    {
                        Ledger CGST = Ledger.GetAllList().Where(w => w.Name == "CGST Local Sale 14%").FirstOrDefault();

                        Ledger SGST = Ledger.GetAllList().Where(w => w.Name == "SGST Local Sale 14%").FirstOrDefault();

                        A2Obj.Narration = "CGST Local Sale @14%";
                        A3Obj.Narration = "SGST Local Sale @14%";

                        A2Obj.ADRLedgerId = Commission.ID;
                        A2Obj.ACRLedgerId = CGST.ID;

                        A3Obj.ADRLedgerId = Commission.ID;
                        A3Obj.ACRLedgerId = SGST.ID;
                    }


                    A2Obj.Date = DateTime.Now;
                    A3Obj.Date = DateTime.Now;
                    A2Obj.CRAmount = ((A1Obj.DRAmount * LedgerDetails.TaxOnAboveMarginOnline) / 100) / 2;
                    A3Obj.CRAmount = ((A1Obj.DRAmount * LedgerDetails.TaxOnAboveMarginOnline) / 100) / 2;

                    A2Obj.Balance = A1Obj.Balance;
                    A3Obj.Balance = A1Obj.Balance;

                    A2Obj.AOrgId = OrgId;
                    A3Obj.AOrgId = OrgId;



                    A2Obj.DRGroupId = 9;
                    A2Obj.CRGroupId = 3;

                    A3Obj.DRGroupId = 9;
                    A3Obj.CRGroupId = 3;

                    A2Obj.EntryType = "sale";
                    A2Obj.ReceiptID = GetReceiptEntry.ID;

                    A3Obj.EntryType = "sale";
                    A3Obj.ReceiptID = GetReceiptEntry.ID;


                    A2Obj.SaveGeneral();
                    A3Obj.SaveGeneral();
                }
                else
                {
                    if (LedgerDetails.TaxOnAboveMarginOnline == 5)
                    {
                         Ledger IGST = Ledger.GetAllList().Where(w => w.Name == "IGST Central Sale  5%").FirstOrDefault();                             
                         A2Obj.Narration = "IGST Central Sale @5%";               
                         A2Obj.ADRLedgerId = Commission.ID;
                         A2Obj.ACRLedgerId = IGST.ID;                   
                    }
                    if (LedgerDetails.TaxOnAboveMarginOnline == 12)
                    {
                        Ledger IGST = Ledger.GetAllList().Where(w => w.Name == "IGST Central Sale  12%").FirstOrDefault();
                        A2Obj.Narration = "IGST Central Sale @12%";                      
                        A2Obj.ADRLedgerId = Commission.ID;
                        A2Obj.ACRLedgerId = IGST.ID;
                        
                    }
                    if (LedgerDetails.TaxOnAboveMarginOnline == 18)
                    {
                        Ledger IGST = Ledger.GetAllList().Where(w => w.Name == "IGST Central Sale  18%").FirstOrDefault();            
                        A2Obj.Narration = "IGST Central Sale @18%";                
                        A2Obj.ADRLedgerId = Commission.ID;
                        A2Obj.ACRLedgerId = IGST.ID;
                       
                    }
                    if (LedgerDetails.TaxOnAboveMarginOnline == 28)
                    {
                        Ledger IGST = Ledger.GetAllList().Where(w => w.Name == "IGST Central Sale  28%").FirstOrDefault();

                        A2Obj.Narration = "IGST Central Sale @28%";                     
                        A2Obj.ADRLedgerId = Commission.ID;
                        A2Obj.ACRLedgerId = IGST.ID;                     
                    }

                    Ledger Tax = Ledger.GetAllList().Where(w => w.Name == "Commission").FirstOrDefault();
                    A2Obj.Date = DateTime.Now;          
                    A2Obj.CRAmount = (A1Obj.CRAmount * LedgerDetails.TaxOnAboveMarginOnline) / 100;

                    A2Obj.Balance = A0Obj.Balance;
                    A2Obj.AOrgId = OrgId;                 
                    A2Obj.DRGroupId = 9;
                    A2Obj.CRGroupId = 3;
             
                    A2Obj.ACID = A0Obj.AID;

                    A2Obj.ADSave();
                }



                A4Obj.Date = DateTime.Now;
                A4Obj.Narration = "Balance Amount Transfered";
                A4Obj.DRAmount = A0Obj.Balance;
                A4Obj.Balance = A0Obj.Balance- A4Obj.DRAmount;
                A4Obj.AOrgId = OrgId;
                A4Obj.ADRLedgerId = LedgerDetails.ID;
                A4Obj.ACRLedgerId = 67;
                A4Obj.DRGroupId = 2;
                A4Obj.CRGroupId = 1;
                A4Obj.EntryType = "Payment";          
                try
                {
                    PLastEntryNo = Accounts.GetAllACDetails(OrgId).Where(w => w.EntryType == "Payment").Select(s => s.EntryNo).Last();
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                if (PLastEntryNo > 0)
                {
                    PLastEntryNo = PLastEntryNo + 1;
                }
                else
                {
                    PLastEntryNo = 1;
                }
                A4Obj.EntryNo = PLastEntryNo;

                A4Obj.SaveGeneral();
            }

            return Json(new { data = AObj }, JsonRequestBehavior.AllowGet);

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

            double totalAmount = 0.00;

            double TotalSale = 0.00;

            double TotalTaxOnCommission = 0.00;

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

            if (GetRecords != null)
            {
                BalanceStatement LastRecords = BalanceStatement.GetAllForBalanceCalculation(OrgId).Last();

                Ledger LedgerDetails = Ledger.GetAllList().Where(x => x.DebtorType == 1
                  && x.OrgId == OrgId).FirstOrDefault();

               foreach (var data in GetRecords)
                 {
                    if (data.Narration != "Opening Balance")
                    {

                        CObj.CommissionAmount = data.CRAmount;
                        SObj.SaleAmount = data.Amount;
                        if (data.TaxOnCash > 0)
                        {
                            CObj.TaxOnCommission = (data.CRAmount * data.TaxOnCash) / 100;
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

                //if (GetRecords.Count != 1) // used for eliminating Opening Balance entry when there is no data for next entry
               // {
                    int GetLastCalculationRecord = Commission.GetAllCommissions().Where(w => w.OrgId == OrgId).Select(s => s.BalanceStatementId).Max();

                    int CommissionId = Commission.GetAllCommissions()
                       .Where(w => w.BalanceStatementId == GetLastCalculationRecord && w.OrgId == OrgId).Select(s => s.CommisionId).FirstOrDefault();
                    if(GetLastCalculationRecord==0)
                    {
                          TotalTaxOnCommission = Commission.GetAllCommissions()
                           .Where(w => w.CommisionId >= CommissionId && w.OrgId == OrgId).Select(s => s.TaxOnCommission).Sum();

                    totalAmount = Commission.GetAllCommissions()
                                    .Where(w => w.CommisionId >= CommissionId && w.OrgId == OrgId).Select(s => s.CommissionAmount).Sum();
                    }
                    else
                     {
                         TotalTaxOnCommission = Commission.GetAllCommissions()
                         .Where(w => w.CommisionId > CommissionId && w.OrgId == OrgId).Select(s => s.TaxOnCommission).Sum();

                             totalAmount = Commission.GetAllCommissions()
                                    .Where(w => w.CommisionId > CommissionId && w.OrgId == OrgId).Select(s => s.CommissionAmount).Sum();
                     }
                   

                    int GetMaxBID = Sale.GetAllSales().Where(w => w.OrgId == OrgId).Select(s => s.BalanceStatementId).Max();

                    int SaleId = Sale.GetAllSales()
                        .Where(w => w.BalanceStatementId == GetMaxBID && w.OrgId == OrgId).Select(s => s.SaleId).FirstOrDefault();
                    if(GetMaxBID==0)
                     {
                        TotalSale = Sale.GetAllSales().Where(w => w.SaleId >= SaleId 
                        && w.OrgId == OrgId).Select(s => s.SaleAmount).Sum();
                     }
                    else
                     {
                         TotalSale = Sale.GetAllSales().Where(w => w.SaleId > SaleId
                        && w.OrgId == OrgId).Select(s => s.SaleAmount).Sum();
                    }
                 

                 

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
               // }
            }
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

            Accounts FirstAccountRecords = Accounts.GetAll().Where(w => w.Narration == "Opening Balance").FirstOrDefault();

            if (FirstAccountRecords == null)
            {
                Obj.Date = FirstRecords.Date;
                Obj.Narration = "Opening Balance";
                Obj.AOrgId = OrgId;
                if (AllSales == null)
                {
                    Obj.DRAmount = 0.00;
                }
                else
                {
                    Obj.DRAmount = LastRecords.Balance;
                }
                Obj.SaveGeneral();
            }
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