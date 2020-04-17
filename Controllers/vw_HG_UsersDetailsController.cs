using HangOut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using HangOut.Models.Common;

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
             if (Obj != null && Obj.UserType!="CUST"&&Obj.UserType!="CA"&& Obj.UserType!="CH")
            {
                HttpCookie cookie = new HttpCookie("UserInfo");
                cookie.Values.Add("UserCode", Obj.UserCode.ToString());
                cookie.Values.Add("UserName", Obj.UserName);
                cookie.Values.Add("UserType", Obj.UserType);
               cookie.Values.Add("OrgId", Obj.OrgID.ToString());
                Response.Cookies.Add(cookie);
              // return RedirectToAction("Admin");
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
            
            if (Request.Cookies["UserInfo"] != null)
            {
                var c = new HttpCookie("UserInfo");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }
            return RedirectToAction("LogInUrl");

        }
        public ActionResult GetUserList()
        {
            vw_HG_UsersDetails Objuser = new vw_HG_UsersDetails();
            List<vw_HG_UsersDetails> listUser = Objuser.GetAll();
            return View(listUser);
        }
        
        public ActionResult LogInUrl()
        {
            return Json(new { url = "/vw_HG_UsersDetails/vw_HG_UsersDetails" },JsonRequestBehavior.AllowGet );
        }
        public ActionResult GetCustomerList()
        {
            vw_HG_UsersDetails Objuser = new vw_HG_UsersDetails();
            List<vw_HG_UsersDetails> listUser = Objuser.GetAll(Type:"CUST");
            return View(listUser);
        }
        public ActionResult CreateEdit( int ID)
        {
            vw_HG_UsersDetails Objuser = new vw_HG_UsersDetails();
            if (ID > 0)
            {
                Objuser = Objuser.GetSingleByUserId(ID);
            }
            return View(Objuser);
        }
        [HttpPost]
        public ActionResult CreateEdit(vw_HG_UsersDetails Objuser)
        {
            if (Objuser.EMail == null)
            {
                Objuser.EMail = "";
            }
            if(Objuser.UserType!="A"&& Objuser.UserType != "SA")
            {
                if (Objuser.OrgID <= 0)
                {
                    return Json(new { msg = "Please Select Organization First" });
                }
            }
            vw_HG_UsersDetails ObjUserAlreadyExist = new vw_HG_UsersDetails().MobileAlreadyExist(Objuser.UserId);
            if (ObjUserAlreadyExist.UserCode > 0 && ObjUserAlreadyExist.UserCode!= Objuser.UserCode)
            {
                return Json(new { msg = "User Id Already Taken" });
            }
            HG_UserTypes objUserType = new HG_UserTypes().GetOne(0, Objuser.UserType);
            int i = Objuser.save();
            Objuser.UserType = objUserType.UserTypeName;
            if (i>0)
                 return Json(new { data = Objuser }, JsonRequestBehavior.AllowGet);
            return RedirectToAction("Error");
             
             
        }
        public ActionResult Error()
        { return View(); }

        public ActionResult Account()
        {
            
            return View();
        }
        public ActionResult Inventry()
        {
            return View();
        }
        public ActionResult UserStatus()
        {
            return View();
        }

        public  ActionResult privacyPolicy()
        {
            return View();
        }
        [LoginFilter]
        public ActionResult CashBack()
        {

            return View();
        }
        public ActionResult Pos()
        {
            return View();
        }
        
    }
}