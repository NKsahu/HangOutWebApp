using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HangOut.Models.Common;

namespace HangOut.Controllers
{
    
    public class SettingsController : Controller
    {
        // GET: Settings
        public ActionResult Index()
        {

            return View(new Settings().GetAll());
        }
        public ActionResult Create(int SettingId = 0)
        {
            Settings ObjSettings = new Settings();
            if (SettingId > 0)
                ObjSettings = ObjSettings.GetOne(SettingId);
            return View(ObjSettings);
        }

        [HttpPost]
        public ActionResult Create(Settings ObjSettings)
        {
            if (ObjSettings.SettingId > 0)
            {
                if (ObjSettings.Save() < 0)
                    return Json(new { msg = "Error in save" });
            }
            else
            {
                if (ObjSettings.Save() < 0)
                {
                    return Json(new { msg = "Error in Update" });
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult GetKeyValue(string id)
        {
            string value = "0.00";
            if (id != null && id == "MarginPrice")
            {
                Settings ObjSetting = new Settings().GetAll().Find(x => x.KeyName.Equals(id));
                if (ObjSetting != null)
                {
                    value = ObjSetting.KeyValue;
                }
            }
            return Content(value);
        }
    }
}