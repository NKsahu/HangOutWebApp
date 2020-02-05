using HangOut.Models.Common;
using HangOut.Models;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
using HangOut.Models.DynamicList;
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
        public JObject FeedBack(Int64 OID)
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
                respose.Add("Questions", JArray.FromObject(feedBackQues));
            }
            else
            {
                respose.Add("Status", 400);
            }
            return respose;
        }
        public JObject SubmitFeedBk(Int64 OID,Int64 SeatingId)
        {

            return new JObject();

        }





    }
}