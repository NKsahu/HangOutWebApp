using System;
using System.Collections.Generic;
using System.Linq;
using HangOut.Models.Common;
using System.Web.Mvc;
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
            foreach(var question in QuestionList)
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
            return response;
        }
        public JArray ListFeedBack()
        {
            var ObjInfo = Request.Cookies["UserInfo"];
            int OrgId = int.Parse(ObjInfo["OrgId"]);

            List<FeedbkForm> formList=FeedbkForm.
        }
    }
}