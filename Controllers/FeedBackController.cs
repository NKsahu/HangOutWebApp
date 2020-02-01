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
            var result = feedbackForm;

            JObject response = new JObject();

            return response;
        }
    }
}