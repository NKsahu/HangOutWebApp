using HangOut.Models.Common;
using HangOut.Models;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
using HangOut.Models.Feedbk;
using System.Web.Mvc;
using System;

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
        public JObject CustomerCnt(int OrgId)
        {
            JObject result = new JObject();

            return result;
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
               // feedBackQues = feedBackQues.FindAll(x => x.Status);
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
                respose.Add("Questions", jArray);
            }
            else
            {
                respose.Add("Status", 400);
            }
            return respose;
        }
        public JObject SubmitFeedBk(Int64 OID)
        {
            Feedbk feedBack = Feedbk.GetOne(OID);
            if (feedBack.FeedBkId == 0)
            {
               // feedBack.o
            }
            return new JObject();
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

    }
}