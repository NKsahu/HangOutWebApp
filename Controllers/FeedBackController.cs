using System;
using System.Collections.Generic;
using System.Linq;
using HangOut.Models.Common;
using System.Web.Mvc;
using HangOut.Models;
using Newtonsoft.Json.Linq;

namespace HangOut.Controllers
{
    public class FeedBackController : Controller
    {
        // GET: FeedBack
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddFeedBack(int Id)
        {
            return View();
        }
        [HttpPost]
        public JObject SaveFeedBack([System.Web.Http.FromBody] FeedbkForm feedbackForm )
        {
            if (feedbackForm.OrgId <= 0)
            {
                var ObjInfo = Request.Cookies["UserInfo"];
                feedbackForm.OrgId = int.Parse(ObjInfo["OrgId"]);
            }
            feedbackForm.Save();
            int FeedbackId = feedbackForm.Id;
            var QuestionList = feedbackForm.Questions;
            QuestionList = QuestionList.FindAll(x => x.Title != null && x.Title != "");
            foreach (var question in QuestionList)
            {
                question.FeedBkFormID = FeedbackId;
                question.Save();
                int QuestionId = question.ID;
                if (question.QuestionType == 1 &&question.Objectives!=null)
                {
                    var Objectives = question.Objectives.FindAll(x => x.Name != null && x.Name != "");
                    foreach(var Objective in Objectives)
                    {
                        Objective.QuestionId = QuestionId;
                        Objective.Save();
                    }
                }

            }
            JObject response = new JObject();
            response.Add("Name", feedbackForm.Name);
            response.Add("Create", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            response.Add("Id", feedbackForm.Id);
            return response;
        }
        public JObject ListFeedBack()
        {
            var ObjInfo = Request.Cookies["UserInfo"];
            int OrgId = int.Parse(ObjInfo["OrgId"]);
            HG_OrganizationDetails orgobj = new HG_OrganizationDetails().GetOne(OrgId);
            int OrgType = orgobj.OrgTypes != null ? int.Parse(orgobj.OrgTypes) : 1;
            List<HG_Floor_or_ScreenMaster> floorOrScreens = new HG_Floor_or_ScreenMaster().GetAll(OrgType);
            List<HG_Tables_or_Sheat> tableOrSheatlist = new HG_Tables_or_Sheat().GetAll(OrgType);
            JObject FeedBackObj = new JObject();
            var feedbklist = FeedbkForm.GetAll(OrgId);
            JArray jArrayfdbk = new JArray();
            foreach (var feedback in feedbklist)
            {
                JObject jObject = JObject.FromObject(feedback);
                jObject.Add("Create", feedback.CreateDate.ToString("dd/MM/yyyy HH:mm"));
                jArrayfdbk.Add(jObject);
            }
            FeedBackObj.Add("FormList", jArrayfdbk);
            JArray jArray = new JArray();
            foreach (HG_Floor_or_ScreenMaster Floors in floorOrScreens)
            {
                JObject jObject = JObject.FromObject(Floors);
                jObject.Add("TableSheatList", JArray.FromObject(tableOrSheatlist.FindAll(x => x.Floor_or_ScreenId == Floors.Floor_or_ScreenID)));
                jArray.Add(jObject);
            }
            FeedBackObj.Add("FloorList", jArray);
            return FeedBackObj;
        }

        public int ActiveForm([System.Web.Http.FromBody] ActiveMenu activeMenu)
        {
            //var Jobj = { };
            //Jobj.OMID = MenuId;
            //Jobj.TorSIDs = TableList;
            //Jobj.OrgId = OrgId;
            int status = 0;
            int FID = activeMenu.OMID;
            int OrgId = activeMenu.OrgId;
            HG_OrganizationDetails hG_OrganizationDetails = new HG_OrganizationDetails().GetOne(OrgId);
            string OrgType = hG_OrganizationDetails.OrgTypes != null ? hG_OrganizationDetails.OrgTypes : "1";
            List<FeedbkForm> FeedBackList = FeedbkForm.GetAll(OrgId);
            FeedbkForm FeedBkObj = FeedBackList.Find(x => x.Id == FID);
            FeedBkObj.Status = true;
            FeedBkObj.Save();
            List<HG_Tables_or_Sheat> TorSlist = new HG_Tables_or_Sheat().GetAll(int.Parse(OrgType));
            var AlreadySelectedList = TorSlist.FindAll(x => x.FDBKId == FID);
            Int64[] items = activeMenu.TorSIDs;
            HashSet<Int64> hashKeys = new HashSet<Int64>(items);
            var RemovedTorSList = AlreadySelectedList.FindAll(x => !hashKeys.Contains(x.Table_or_RowID));
            List<HG_Tables_or_Sheat> OnlyApplytoTorS = TorSlist.FindAll(x => hashKeys.Contains(x.Table_or_RowID));
            foreach (var TorSobj in OnlyApplytoTorS)
            {
                TorSobj.FDBKId = FID;
                TorSobj.save();

            }

            foreach (var TorSobj in RemovedTorSList)
            {
                TorSobj.FDBKId = 0;
                TorSobj.save();
            }
            return status;
        }
        public ActionResult FeedbkShortReport()
        {
            return View();
        }
        
    }
}