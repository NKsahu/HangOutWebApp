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
            double totalAmount = 0.00;
            for (int i = 0; i < CompletedItems.Count; i++)
            {
               
                totalAmount += CompletedItems[i].Count * CompletedItems[i].Price;
            }
            Ledger LedgerDetails = Ledger.GetAllList().Where(x => x.DebtorType == 1 
            && x.OrgId == CompletedItems[0].OrgId).FirstOrDefault();

            if (LedgerDetails.CalculationStartFrom.Date >= DateTime.Now.Date)
            {
                bObj.Date = DateTime.Now;
                bObj.Amount = totalAmount;
                bObj.OrgId = LedgerDetails.OrgId;
                bObj.OrderId = CompletedItems[0].OID;
                BalanceStatement TotalBalance = BalanceStatement.GetAllForBalanceCalculation();
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

               
            }
            return Json(new { data = bObj }, JsonRequestBehavior.AllowGet);
        }
    }
}