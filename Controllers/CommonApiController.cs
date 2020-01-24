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
        public JObject CustomerCnt()
        {
            JObject result = new JObject();
            return result;
        }
    }
}