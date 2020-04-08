using HangOut.Models.Common;
using HangOut.Models;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
using HangOut.Models.Feedbk;
using System.Web.Mvc;
using System;
using Newtonsoft.Json;
using HangOut.Models.MyCustomer;

namespace HangOut.Controllers
{
    public class CommonApiController : Controller
    {
        // GET: CommonApi
        public JObject OrgTickUntick(int OrgId,bool Status) 
        {
            OrgSetting orgSetting = OrgSetting.Getone(OrgId);
            if (orgSetting.id == 0)
            {
                orgSetting.OrgId = OrgId;
                orgSetting.EnblDeleryChrg = 0;
                orgSetting.MinOrderAmt = 0;
                orgSetting.DeliveryCharge = 0;
                orgSetting.OrdCanlMinTime = 0;
                orgSetting.ByCash = 1;
                orgSetting.ByOnline = 1;
                orgSetting.AcptMinOrd = 1;
                orgSetting.ContactHead1 = "";
                orgSetting.Contact1 = "";
                orgSetting.ContacHead2 = "";
                orgSetting.Contact2 = "";
                orgSetting.CheckBoxStatus = Status;
                orgSetting.save();
            }
            else
            {
                orgSetting.CheckBoxStatus = Status;
                orgSetting.save();
            
}
            JObject jObject = new JObject();
            jObject.Add("Status", 200);
            return jObject;
        }
        public JObject ChangeItemAvability(Int64 ItemId, bool Status)
        {

            HG_Items ObjItem = new HG_Items().GetOne(ItemId);
            int Avaiblity = Status ? 0 : 1;
            JObject jObject = new JObject();
            if (ObjItem.ItemID > 0)
            {
                ObjItem.ItemAvaibility = Avaiblity;
                ObjItem.Save();
                jObject.Add("Status", 200);
            }
            else
            {
                jObject.Add("Status", 400);
            }
            return jObject;
        }
        //=========FEEDBACK API===========
        public JObject FeedBack(Int64 OID,int CID)
        {

            HG_Orders hG_Orders = new HG_Orders().GetOne(OID);
            HG_Tables_or_Sheat Seating = new HG_Tables_or_Sheat().GetOne(hG_Orders.Table_or_SheatId);
            JObject respose = new JObject();
            if (Seating.FDBKId > 0)// feedback applied in seating
            {
                FeedbkForm feedbkForm = FeedbkForm.GetOne(Seating.FDBKId);
                List<FeedBackQue> feedBackQues = FeedBackQue.GetAll(feedbkForm.Id);
                feedBackQues = feedBackQues.FindAll(x => x.Status);
                feedBackQues = feedBackQues.OrderBy(x => x.OrderNo).ToList();
                respose.Add("Status", 200);
                JArray jArray = new JArray();
                foreach(var question in feedBackQues)
                {
                    JObject jObject = new JObject();
                    jObject.Add("QID", question.ID);
                    jObject.Add("Title", question.Title);
                    jObject.Add("QuestionType", question.QuestionType);
                    jObject.Add("FeedBkFormID", question.FeedBkFormID);
                    if(question.Title.Contains("ITEM FEEDBACK") && (question.QuestionType==0|| question.QuestionType==3))
                    {
                        jObject["Title"] = "ITEM FEEDBACK";
                        List<HG_OrderItem> hG_OrderItems = new HG_OrderItem().GetAll(OID);
                        hG_OrderItems = hG_OrderItems.FindAll(x => x.Status == 3);
                        hG_OrderItems = hG_OrderItems.FindAll(x => x.OrdById == CID);
                        JArray itemfeedbks = new JArray();
                        for(int i=0; i<hG_OrderItems.Count; i++)
                        {
                            HG_Items Objfood = new HG_Items().GetOne(hG_OrderItems[i].FID);
                            JObject ItemJobj = new JObject();
                            ItemJobj.Add("ItmId", hG_OrderItems[i].FID);//FID is ItemId
                            ItemJobj.Add("ItmName", Objfood.Items);
                            itemfeedbks.Add(ItemJobj);
                        }
                        jObject.Add("Items", itemfeedbks);
                    }
                    if (question.QuestionType == 1)
                    {
                        List<FeedbkObj> objectivesList = FeedbkObj.GetAll(question.ID);
                        FeedbkObj firstt = objectivesList.FirstOrDefault();
                        jObject.Add("ObjectiveType", firstt.ObjectiveType);
                        jObject.Add("Objectives", JArray.FromObject(objectivesList));
                    }
                    jArray.Add(jObject);
                 }
                //=====add customer review comment in last at feedback
                JObject jObject2 = new JObject();
                jObject2.Add("QID", 0);
                jObject2.Add("Title", "");
                jObject2.Add("QuestionType",5);
                jObject2.Add("FeedBkFormID",0);
                jArray.Add(jObject2);
                respose.Add("Questions", jArray);
            }
            else
            {
                respose.Add("Status", 400);
            }
            return respose;
        }
        public static Feedbk SubmitFeedBk(Int64 OID)
        {
            Feedbk feedBack = Feedbk.GetOne(OID);
        //    public int FeedBkId { get; set; }
        //public int OrgId { get; set; }
        //public Int64 OrderId { get; set; }
        //public int FeedbkFormId { get; set; }
            if (feedBack.FeedBkId == 0)
            {
                HG_Orders hG_Orders = new HG_Orders().GetOne(OID);
                HG_Tables_or_Sheat hG_Tables_Or_Sheat = new HG_Tables_or_Sheat().GetOne(hG_Orders.Table_or_SheatId);
               
                    feedBack = new Feedbk();
                    feedBack.FeedbkFormId = hG_Tables_Or_Sheat.FDBKId;
                    feedBack.OrgId = hG_Orders.OrgId;
                    feedBack.OrderId = OID;
                    feedBack.save();
                    return feedBack;
                
            }
            else
            {
                return feedBack;
            }
         
        }
        public JObject PostFdBkResponse(string JObj)
        {
            //    public int QID { get; set; }
            //public int ResponseType { get; set; }
            //public int FeedbkFormId { get; set; }
            //public int StarCnt { get; set; }
            //public string Subject { get; set; }
            //public int LikeCnt { get; set; }
            //public int DislikeCnt { get; set; }
            //public int NormalOkCnt { get; set; }
            //public int FeedbkId { get; set; }
            //public string ObjectiveOptions { get; set; }
            //public int CID { get; set; }
            // 0:Star,1: Objective,2:Subjective,3:Like Dislike Ok,4:star-subjective,5 :remark by user
            //public int OrgId { get; set; }
            JObject result = new JObject();
            FeedbkResponse ObjRes =JsonConvert.DeserializeObject<FeedbkResponse>(JObj);
            Int64 OID = ObjRes.OID;
            Feedbk feedbk = SubmitFeedBk(OID);
            if (feedbk.FeedBkId > 0)
            {
                ObjRes.FeedbkFormId = feedbk.FeedbkFormId;
                ObjRes.FeedbkId = feedbk.FeedBkId;
                ObjRes.OrgId = feedbk.OrgId;
                ObjRes.save();
                result.Add("Status", 200);
                return result;
            }
            else
            {
                result.Add("Status", 400);
                return result;
            }
    }
        public JObject PostFdBkItems(string JObj)
        {
        //     public Int64 ItemID { get; set; }
        //public int Rating { get; set; }
        //public string Comment { get; set; }
        //public int ResponseType { get; set; }
        //public int CID { get; set; }
        //public int LikeCnt { get; set; }
        //public int DislikeCnt { get; set; }
        //public int OkCnt { get; set; }
        //public int FeedbkFormID { get; set; }
        //public int FeedBkID { get; set; }
        //public int OrgId { get; set; }
        JObject result = new JObject();
        FeedbkItem ObjFDBJItem = JsonConvert.DeserializeObject<FeedbkItem>(JObj);
            Int64 OID = ObjFDBJItem.OID;
            Feedbk feedbk = SubmitFeedBk(OID);
            if (feedbk.FeedBkId > 0)
            {
                ObjFDBJItem.FeedbkFormID = feedbk.FeedbkFormId;
                ObjFDBJItem.FeedBkID = feedbk.FeedBkId;
                ObjFDBJItem.OrgId = feedbk.OrgId;
                ObjFDBJItem.save();
                result.Add("Status", 200);
                return result;
            }
            else
            {
                result.Add("Status", 400);
                return result;
            }
        }
        //========local contact list=======
        public JObject SaveLocalContact(string Mobile,string Cname,int ContctID)
        {
            LocalContacts localContacts = new LocalContacts();
            if (ContctID > 0)
            {
                localContacts = LocalContacts.GetOne(ID: ContctID);
                localContacts.MobileNo = Mobile;
                localContacts.Cust_Name = Cname;
            }
            else
            {
                localContacts = LocalContacts.GetOne(Mobile: Mobile);
                if (localContacts != null && localContacts.ContctID > 0)
                {
                    localContacts.MobileNo = Mobile;
                    localContacts.Cust_Name = Cname;
                }
                else
                {
                       var UserInfo = Request.Cookies["UserInfo"];
                       int OrgId = int.Parse(UserInfo["OrgId"]);
                       localContacts = new LocalContacts();
                        localContacts.MobileNo = Mobile;
                        localContacts.Cust_Name = Cname;
                        localContacts.OrgId = OrgId;
                }
            }
            JObject result = new JObject();
           if(localContacts.Save() > 0)
            {
                result.Add("Status", 200);
                result.Add("ContactId", localContacts.ContctID);
            }
            else
            {
                result.Add("Status", 400);
            }
            return result;
        }

        public JObject GetNameByMobileNo(string MobileNo)
        {
            JObject result = new JObject();
            vw_HG_UsersDetails ObjUser = new vw_HG_UsersDetails().MobileAlreadyExist(MobileNo);
            if (ObjUser != null && ObjUser.UserCode > 0)
            {
                result.Add("Status", 200);
                result.Add("CName", ObjUser.UserName);
                return result;
            }
            else
            {
                LocalContacts localContacts = LocalContacts.GetOne(Mobile: MobileNo);
                
                if (localContacts != null && localContacts.ContctID > 0)
                {
                    result.Add("Status", 200);
                    result.Add("CName", localContacts.Cust_Name);
                    return result;
                }
                else
                {
                    result.Add("Status", 400);
                    return result;
                }

            }
            
        }

        public JObject getOrgType(int orgId)
        {
            HG_OrganizationDetails objOrg = new HG_OrganizationDetails().GetOne(orgId);
            if (objOrg != null && objOrg.OrgID > 0)
            {
                JObject result = new JObject();
                result.Add("Status", 200);
                result.Add("OrgType", objOrg.OrgTypes);
                result.Add("Stateid", objOrg.State);
                return result;
            }
            else
            {
                JObject result = new JObject();
                result.Add("Status", 400);
                return result;
            }
        }

        public JObject OrgSettng()
        {
            var UserInfo = Request.Cookies["UserInfo"];
            int Orgid = int.Parse(UserInfo["OrgId"]);
            HG_OrganizationDetails ObjOrg = new HG_OrganizationDetails().GetOne(Orgid);
            OrgSetting setting = OrgSetting.Getone(Orgid);
            JObject result = new JObject();
            double taxableAmt = (setting.ParcelAmt * setting.ParcelTax) / 100;
            double ParcelPrice= setting.ParcelAmt + taxableAmt;
            result.Add("InvoicePrint", ObjOrg.InvoicePrintting);
            result.Add("InvoiceNoOfCopy", ObjOrg.NuOfCopy);
            result.Add("OrdDisaply", ObjOrg.OrderDisplay);
            result.Add("KotPrint", ObjOrg.PrinttingType);
            result.Add("NoOfCopy", ObjOrg.Copy);
            result.Add("ParcelAmt", ParcelPrice);
            return result;
        }
        public JObject RatingApplied(int CID)
        {
            JObject result = new JObject();
            vw_HG_UsersDetails ObjUser = new vw_HG_UsersDetails().GetSingleByUserId(CID);
            if (ObjUser.UserCode > 0)
            {
                ObjUser.RateNow = 1;
                ObjUser.save();
                result.Add("Status", 200);
            }
            else
            {
                result.Add("Status", 400);
            }
            return result;
        }
       
        public JObject ApplyWallet(string Jobj)
        {
            JObject obj = JObject.Parse(Jobj);
            Wallet wallet = new Wallet();
            wallet.WalletId = 0;
            wallet.CID = int.Parse(obj["CID"].ToString());
            wallet.OID = int.Parse(obj["OID"].ToString());
            wallet.CashBkId = 0;
            wallet.CashBkAmt = 0;
            wallet.DeductedAmt = double.Parse(obj["DeductedAmt"].ToString()); ;
            wallet.AmtActiveOn = DateTime.Now;
            wallet.OrgId= int.Parse(obj["OrgId"].ToString());
            wallet.Save();
            JObject result = new JObject();

            
            if (wallet.WalletId > 0)
            {
                OrdDiscntChrge ordDiscntChrge = new OrdDiscntChrge();
                ordDiscntChrge.ID = 0;
                ordDiscntChrge.Title = "Customer CashBack";
                ordDiscntChrge.OID = wallet.OID;
                ordDiscntChrge.Type = 1;
                ordDiscntChrge.Amt = wallet.DeductedAmt;
                ordDiscntChrge.Tax = 0;
                ordDiscntChrge.Remark = "";
                ordDiscntChrge.Save();
                HG_Orders hG_Orders = new HG_Orders().GetOne(wallet.OID);
                if (hG_Orders.OID > 0)
                {
                    if (hG_Orders.DisntChargeIDs != "" && hG_Orders.DisntChargeIDs != "0")
                    {
                        hG_Orders.DisntChargeIDs = hG_Orders.DisntChargeIDs + "," + ordDiscntChrge.ID;
                    }
                    else
                    {
                        hG_Orders.DisntChargeIDs = ordDiscntChrge.ID.ToString();
                    }
                    hG_Orders.Save();

                }
                result.Add("Status", 200);
            }
            else
            {
                result.Add("Status", 400);
            }
            return result;
        }
        public JArray GetWalletAmt(int CID)
        {
           var walletamt= MyWallet.GetWalletAmt(CID);
            JArray jArray = new JArray();
            foreach(var wallet in walletamt)
            {
                var leftamt = wallet.CashBkAmt - wallet.deductedAmt;
                JObject jObject = new JObject();
                jObject.Add("Used", wallet.deductedAmt.ToString("0.00"));
                jObject.Add("LeftAmt", leftamt.ToString("0.00"));
                jObject.Add("OutLetName", wallet.OutLetName);
                jArray.Add(jObject);
            }
            return jArray;
        }
        public JObject MyWalletAmt(int CID)
        {
            JObject jObject = new JObject();
            var wallet = MyWallet.GetWalletAmt(CID);
            var used = wallet.Sum(x => x.deductedAmt);
            var leftamt = wallet.Sum(x => x.CashBkAmt) - used;
            jObject.Add("Used", used.ToString("0.00"));
            jObject.Add("LeftAmt", leftamt.ToString("0.00"));
            return jObject;
        }
        public ActionResult Test()
        {
            HG_Orders.OrderAmt(55, 50);
            return Content("0");
        }
    }
}