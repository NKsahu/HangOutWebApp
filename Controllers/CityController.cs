using System.Collections.Generic;
using System.Web.Mvc;
using HangOut.Models.Common;
using HangOut.Models;
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
        //==================Tehsil/taluka===================
        public ActionResult IndexTehsil()
        {
            List<District> listTehsil = new District().GetAllByStsCity(0, 0, true);
            return View(listTehsil);
        }
        public ActionResult CreateTeshsile( int ID)
        {
            District district = new District();
            if (ID > 0)
            {
                district = new District().GetOne(ID);
            }
            return View(district);
        }
        [HttpPost]
        public ActionResult CreateTehsil(District district)
        {
            if (district.StateId == 0)
            {
                return Json(new { msg = "Please Select State" });
            }
            if (district.CityId == 0)
            {
                return Json(new { msg = "Please Select City" });
            }
            int i = district.save();
            if (i > 0)
                return Json(new { data = district }, JsonRequestBehavior.AllowGet);
            //    return RedirectToAction("Index", "City", new { Type = 1 });
            return RedirectToAction("Error");

        }

        public ActionResult DeleteCity(int ID)
        {
            List<HG_OrganizationDetails> ListOrg = new HG_OrganizationDetails().GetAll();
            City ObjCity = new City().GetOne(ID);
            if (ObjCity.CityId > 0)
            {
                List<District> districtList = new District().GetAllByStsCity(ObjCity.StateId, ObjCity.CityId);
                if (districtList.Count > 0)
                {
                    return Json(new { msg = "Data already used" }, JsonRequestBehavior.AllowGet);

                }
            }
            var CityContains = ListOrg.FindAll(x => x.City == ObjCity.CityId.ToString());
            if (CityContains.Count > 0)
            {
                return Json(new { msg = "Data already used" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                int i = City.Dell(ID);
            }
            return Json(new { data = "1" }, JsonRequestBehavior.AllowGet);
        }
    
    public ActionResult DeleteTehsil(int ID)
    {
        List<HG_OrganizationDetails> ListOrg = new HG_OrganizationDetails().GetAll();
        var CityContains = ListOrg.FindAll(x => x.DistrictId == ID);
        if (CityContains.Count > 0)
        {
            return Json(new { msg = "Data already used" }, JsonRequestBehavior.AllowGet);
        }
        else
        {
                int i = District.Dell(ID);
        }
        return Json(new { data = "1" }, JsonRequestBehavior.AllowGet);
    }
  }
}