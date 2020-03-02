using System;
using System.Collections.Generic;
using System.Linq;
using HangOut.Models;
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
            listvideocategory = listvideocategory.OrderBy(x => x.OrderNo).ToList();
            return JArray.FromObject(listvideocategory);
        }
        public ActionResult Popupwindow()
        {
            return View();
        }
        public JObject SaveCategory([System.Web.Http.FromBody] VideoCategory Category)
        {
            VideoCategory ObjCategory = Category;
            if (ObjCategory.Id == 0)
            {
                ObjCategory.OrderNo = VideoCategory.GetAll().Count+1;
            }
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
        public JObject SaveOrderNo(int Id,int OrderNo)
        {
            //VideoCategory ObjCategory = Category;
            VideoCategory OBJvideo = VideoCategory.GetOne(Id);
            OBJvideo.OrderNo = OrderNo;
            OBJvideo.UpdateOrderNo();
            
            return JObject.FromObject(OBJvideo);
        }
        public ActionResult viewstatuspopupwindow()
        {
            return View();
        }
        public JObject VideoList()
        {
            List<VideoCategory> videoCategories = VideoCategory.GetAll();
            videoCategories = videoCategories.OrderBy(x => x.OrderNo).ToList();
            JObject response = new JObject();
            var UserInfo = Request.Cookies["UserInfo"];
            int CID = int.Parse(UserInfo["UserCode"]);
            int OrgId = int.Parse(UserInfo["OrgId"]);
            HG_OrganizationDetails ObjOrg = new HG_OrganizationDetails().GetOne(OrgId);
            List<VideoMark> videomarks = VideoMark.GetAll(CID);
            JArray jArray = new JArray();
            foreach (var category in videoCategories)
            {
                JObject jObject = new JObject();
                List<Video> videolist = Video.GetAll(category.Id);
                if (ObjOrg != null && ObjOrg.OrgID > 0)
                {
                    // 1)=========for outlet type
                    if (ObjOrg.OrgTypes == "1")
                    {
                        videolist = videolist.FindAll(x => x.Restaurant);
                    }
                    else if (ObjOrg.OrgTypes == "2")
                    {
                        videolist = videolist.FindAll(x => x.Theater);
                    }
                    //========payment mode
                    if(ObjOrg.PaymentType ==1)
                    {
                        videolist = videolist.FindAll(x => x.Prepaid);
                    }else if(ObjOrg.PaymentType==2)
                    {
                        videolist = videolist.FindAll(x => x.Postpaid);

                    }
                }
                videolist = videolist.OrderBy(x => x.SerialNumber).ToList();
                jObject.Add("CatName", category.Name);
                jObject.Add("CatId", category.Id);
                jObject.Add("Videos", JArray.FromObject(videolist));
                jArray.Add(jObject);
               
            }
             
            response.Add("CategoryList", jArray);
            response.Add("videomarks", JArray.FromObject(videomarks));
            return response;
        }
        public JObject MarkComplete(int videoId)
        {
            var UserInfo = Request.Cookies["UserInfo"];
            int CID = int.Parse(UserInfo["UserCode"]);
            VideoMark videoMark = new VideoMark();
            videoMark.CID = CID;
            videoMark.VideoID = videoId;
             videoMark.Save();
            JObject responce = new JObject();
            responce.Add("Status", 200);
            return responce;
        }
    }
}