using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using HangOut.Models.Common;
namespace HangOut.Controllers
{
    public class VideoController : Controller
    {
        // GET: Video
        public ActionResult Index()
        {

            return View();
        }

        public JArray Categorylist()
        {
            List<VideoCategory> listvideocategory = VideoCategory.GetAll();
            return JArray.FromObject(listvideocategory);
        }
    }
}