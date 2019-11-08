using System.Collections.Generic;
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
        public ActionResult Create(int CityId)
        {

            City city = new City().GetOne(CityId);
            return View(city);
        }
    }
}