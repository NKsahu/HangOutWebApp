﻿using System.Collections.Generic;
using System.Web.Mvc;
using HangOut.Models.Common;
namespace HangOut.Controllers
{
    public class CityController : Controller
    {
        // GET: City
        public ActionResult Index(int Type,int StateCode=0)
        {
            List<City> cities = new List<City>();
            if (Type != 1)
            {
                cities = new City().GetAllByState(StateCode);
            }
             return View(cities);
        }
        public ActionResult CreateEdit(int CityId)
        {

            City city = new City();
           
            if (CityId>0)
            {
                city = city.GetOne(CityId);
            }
            city.Type = 0;
            return View(city);
        }
        [HttpPost]
        public ActionResult CreateEdit(City city)
        {
            int i = city.save();
            if (i > 0)
                return Json(new { data=city }, JsonRequestBehavior.AllowGet);
            //    return RedirectToAction("Index", "City", new { Type = 1 });
           return RedirectToAction("Error");
        }
        public ActionResult Error()
        {
            return View();
        }

        public ActionResult CreateTeshsile(int CityId)
        {
            City city = new City();
           
            if (CityId > 0)
            {
                city = city.GetOne(CityId);
            }
            city.Type = 1;
            city.StateId = 0;
            return View(city);
        }
    }
}