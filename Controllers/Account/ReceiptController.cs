using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HangOut.Models.Account;
using HangOut.Models;

namespace HangOut.Controllers.Account
{
    public class ReceiptController : Controller
    {
        // GET: Receipt
        public ActionResult Index(int OrgId)
        {
            List<Receipt> REOBJ = Receipt.GetData(OrgId);
            
            return View(REOBJ);
        }
        public ActionResult LedgerIndex(int ID)
        {
            
            string LedgerName = Ledger.GetAll().Where(w => w.ID == ID).Select(s => s.Name).FirstOrDefault();
            List<Accounts> REOBJ = Receipt.GetLedgerWiseData(ID,LedgerName);

            return View(REOBJ);
        }

        public ActionResult GenerateAllPaymentReceipt()
        {
           List<long> OrderIds = PaytmResn.GetAll().Select(s=>s.OID).Distinct().ToList();

            List<PaytmResn> Onlinepayment = PaytmResn.GetAll().Where(w=> OrderIds.Contains(w.OID)).Distinct().ToList();
            int LastEntryNo = 0;
            double LastBalance = 0.00;
            int ENo = 0;
            Receipt ReceiptEntry = new Receipt();
            ReceiptEntry.Date = Onlinepayment[0].TxtDate.AddHours(-24);
            ReceiptEntry.ReceiptType = "Rece";
            ReceiptEntry.Amount = 0.00;
            ReceiptEntry.Particular = "Opening Balance";
            ReceiptEntry.Save();

            foreach (var OP in Onlinepayment)
            {
                ReceiptEntry = new Receipt();

                ReceiptEntry.Date = OP.TxtDate;
                ReceiptEntry.ReceiptType = "Rece";
                ReceiptEntry.Amount =Convert.ToDouble(OP.PaidAmount);
                ReceiptEntry.Particular = "Online Payment of Order No." + OP.OID;
                ENo = ENo + 1;
                ReceiptEntry.EntryNo = ENo;
                HG_Orders ord = new HG_Orders().GetOne(OP.OID);
                ReceiptEntry.OrgId = ord.OrgId;
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

            return Json(new { data = ReceiptEntry }, JsonRequestBehavior.AllowGet);
        }
                      
    }
}