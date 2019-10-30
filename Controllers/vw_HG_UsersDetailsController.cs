using HangOut.Models;
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
           
                ViewData["msg"] = "";
                return View("Login");
           }


        [HttpPost]
        public ActionResult LoginPost(vw_HG_UsersDetails Obj)
        {
            Obj = Obj.Checkvw_HG_UsersDetails();
             if (Obj != null)
            {
                Session["ID"] = Obj .UserCode;
                Session["Display_Name"] = Obj .UserName;
                 return RedirectToAction("Admin");
            }
               ViewData["msg"] = "Invalid User Name or Password";
            return View("Login");
        }
        public ActionResult Admin()
        {

            return View();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("vw_HG_UsersDetails");
        }
    }
}