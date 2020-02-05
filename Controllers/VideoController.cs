﻿using System;
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
        public ActionResult Popupwindow()
        {
            return View();
        }
        public JObject SaveCategory([System.Web.Http.FromBody] VideoCategory Category)
        {
            VideoCategory ObjCategory = Category;
            ObjCategory.Save();
            List<Video> videolist = ObjCategory.Videos;

            videolist = videolist.FindAll(x => x.Title != null && x.Link != null);
            foreach (var video in videolist)
            {
                video.CategoryId = ObjCategory.Id;
                video.Save();
            }
            return JObject.FromObject(ObjCategory);
        }
        public ActionResult viewstatuspopupwindow()
        {
            return View();
        }
        public JArray VideoList()
        {
            
            List<VideoCategory> videoCategories = VideoCategory.GetAll();
            JArray jArray = new JArray();
            foreach(var category in videoCategories)
            {
                JObject jObject = new JObject();
                List<Video> videolist = Video.GetAll(category.Id);
                videolist = videolist.OrderBy(x => x.SerialNumber).ToList();
                jObject.Add("CatName", category.Name);
                jObject.Add("CatId", category.Id);
                jObject.Add("Videos", JArray.FromObject(videolist));
                jArray.Add(jObject);
            }
            return jArray;
        }
    }
}