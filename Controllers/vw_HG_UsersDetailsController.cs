﻿using HangOut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace HangOut.Controllers
{
    //
    public class vw_HG_UsersDetailsController : Controller
    {
        // GET: vw_HG_UsersDetails
        public ActionResult vw_HG_UsersDetails()
        {
            // GET: Home
            if (Request.Cookies["UserInfo"] != null)
            {
                return RedirectToAction("Admin");
            }
            return View("Login");
           }


        [HttpPost]
        public ActionResult LoginPost(vw_HG_UsersDetails Obj)
        {
            Obj = Obj.Checkvw_HG_UsersDetails();
             if (Obj != null)
            {
                Session["ID"] = Obj .UserCode;
                HttpCookie cookie = new HttpCookie("UserInfo");
                cookie.Values.Add("UserCode", Obj.UserCode.ToString());
                cookie.Values.Add("UserName", Obj.UserName);
                cookie.Values.Add("UserType", Obj.UserType);
                Response.Cookies.Add(cookie);
                return Json(new { url = "/vw_HG_UsersDetails/Admin" });
            }
            else
            {
                return Json(new { msg = "Invalid Credential" });
            }
           
        }
        public ActionResult Admin()
        {
            if (Request.Cookies["UserInfo"] != null)
            {
                return View();
            }
            else
            {
             return   RedirectToAction("vw_HG_UsersDetails");
            }
                
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            if (Request.Cookies["UserInfo"] != null)
            {
                var c = new HttpCookie("UserInfo");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }
            return RedirectToAction("vw_HG_UsersDetails");
        }
        public ActionResult GetUserList()
        {
            vw_HG_UsersDetails Objuser = new vw_HG_UsersDetails();
            List<vw_HG_UsersDetails> listUser = Objuser.GetAll();
            return View(listUser);
        }
        public ActionResult CreateEdit( int ID)
        {
            vw_HG_UsersDetails Objuser = new vw_HG_UsersDetails();
            Objuser = Objuser.GetSingleByUserId(ID);
            return View(Objuser);
        }
        [HttpPost]
        public ActionResult CreateEdit(vw_HG_UsersDetails Objuser)
        {
            int i = Objuser.save();
                if(i>0)
            
                return RedirectToAction("GetUserList");
                return RedirectToAction("Error");
             
             
        }
        public ActionResult Error()
        { return View(); }
    }
}