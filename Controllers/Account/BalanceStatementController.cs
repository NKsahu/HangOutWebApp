﻿using System;
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
                BalanceStatement TotalBalance = BalanceStatement.GetAllForBalanceCalculation().Last();
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
                    BalanceStatement TotalBalance = BalanceStatement.GetAllForBalanceCalculation().Last();
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
    }
}