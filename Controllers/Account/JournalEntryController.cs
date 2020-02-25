using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HangOut.Models.Account;
using HangOut.Models;
namespace HangOut.Controllers
{
    public class JournalEntryController : Controller
    {
        // GET: JournalEntry
        public ActionResult CreateEdit()
        {
            JournalEntry Obj = new JournalEntry();
            return View(Obj);
        }

        // JournalEntry create edit
        [HttpPost]
        public ActionResult CreateEdit(Group Obj)
        {
            int i = Obj.Save();
            if (i > 0)
                return Json(new { data = Obj }, JsonRequestBehavior.AllowGet);
            return RedirectToAction("Error");
        }
        public ActionResult GetDetails(List<HG_OrderItem> CompletedItems)
        {
            JournalEntry jObj = new JournalEntry();
            List<JournalEntry> jObjList = new List<JournalEntry>();

            List<HG_OrganizationDetails> OrgList = new List<HG_OrganizationDetails>();
            double totalAmount = 0.00;
            for (int i = 0; i < CompletedItems.Count; i++)
            {
                jObj = new JournalEntry();
                totalAmount += CompletedItems[i].Count * CompletedItems[i].Price;
                if (CompletedItems[i].TaxInItm==5)
                {
                    Ledger LedgerDetails = Ledger.GetAllList().Where(x => x.ParentGroup == 5 && x.Name== "GST Local Sale 5%").FirstOrDefault();
                    jObj.CRLedgerId = LedgerDetails.ID;
                    jObj.JEDAmount = CompletedItems[i].CostPrice;

                    //HG_OrganizationDetails org = OrgList.FindAll(x => x.OrgID == CompletedItems[i].OrgId).FirstOrDefault();

                    //if(org.State=="17")
                    //{

                    //}

                }
                if (CompletedItems[i].TaxInItm == 12)
                {
                    Ledger LedgerDetails = Ledger.GetAllList().Where(x => x.ParentGroup == 5 && x.Name == "GST Local Sale 12%").FirstOrDefault();
                    jObj.CRLedgerId = LedgerDetails.ID;
                    jObj.JEDAmount = CompletedItems[i].CostPrice;
                }
                if (CompletedItems[i].TaxInItm == 18)
                {
                    Ledger LedgerDetails = Ledger.GetAllList().Where(x => x.ParentGroup == 5 && x.Name == "GST Local Sale 18%").FirstOrDefault();
                    jObj.CRLedgerId = LedgerDetails.ID;
                    jObj.JEDAmount = CompletedItems[i].CostPrice;
                }
                if (CompletedItems[i].TaxInItm == 28)
                {
                    Ledger LedgerDetails = Ledger.GetAllList().Where(x => x.ParentGroup == 5 && x.Name == "GST Local Sale 28%").FirstOrDefault();
                    jObj.CRLedgerId = LedgerDetails.ID;
                    jObj.JEDAmount = CompletedItems[i].CostPrice;
                }
                jObjList.Add(jObj);
            }
            jObj.Date = DateTime.Now;
            jObj.Amount = totalAmount;
            jObj.GroupId = 5;
            jObj.Narration = "";
            
            jObj.Save(jObjList);

            return Json(new { data = jObj }, JsonRequestBehavior.AllowGet);
          
        }

    }
}