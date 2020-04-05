using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Web.Mvc;
using HangOut.Models;
using System.Linq;
using HangOut.Models.DynamicList;
using HangOut.Models.MyCustomer;

namespace HangOut.Controllers
{
    public class OrderMenuController : Controller
    {
        // GET: OrderMenu
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult AddMenu()
        {

            return View();
        }

        public ActionResult CashBackSeating(int CBID)
        {

            return View();
        }
        public JObject SeatingList(int CBID,int Orgid=0)
        {
            if (Orgid == 0)
            {
                Orgid = OrderType.CurrOrgId();
            }
            HG_OrganizationDetails orgobj = new HG_OrganizationDetails().GetOne(Orgid);
            int OrgType = orgobj.OrgTypes != null ? int.Parse(orgobj.OrgTypes) : 1;
            Cashback cashback = Cashback.Getone(CBID);
            List<HG_Floor_or_ScreenMaster> floorOrScreens = new HG_Floor_or_ScreenMaster().GetAll(OrgType);
            List<HG_Tables_or_Sheat> tableOrSheatlist = new HG_Tables_or_Sheat().GetAll(OrgType);
            JObject OrderMenus = new JObject();
            List<int> Seatings = new List<int>();
            List<Cashback> cashbacks = Cashback.GetAll(Orgid,1);
            cashbacks = cashbacks.FindAll(x => x.CashBkId != cashback.CashBkId && x.CashBkStatus == 1);
            cashbacks = cashbacks.FindAll(x => x.SeatingIds != "");
            List<int> RedSeatings = new List<int>();
            foreach (var Cashbk in cashbacks)
            {
                if (Cashbk.StartDate.Date >= cashback.StartDate.Date && cashback.ValidTill == 1)
                {
                    RedSeatings.AddRange(Cashbk.SeatingIds.Split(',').Select(int.Parse).ToList());
                }
                else if (Cashbk.StartDate.Date >= cashback.StartDate.Date && Cashbk.StartDate.Date <= cashback.ValidTillDate.Date)
                {
                    RedSeatings.AddRange(Cashbk.SeatingIds.Split(',').Select(int.Parse).ToList());
                }
            }
            if (cashback.SeatingIds!="")
            {
                Seatings = cashback.SeatingIds.Split(',').Select(int.Parse).ToList();
            }
            JArray jArray = new JArray();
            foreach (HG_Floor_or_ScreenMaster Floors in floorOrScreens)
            {

                JObject jObject = JObject.FromObject(Floors);
                jObject.Add("TableSheatList", JArray.FromObject(tableOrSheatlist.FindAll(x => x.Floor_or_ScreenId == Floors.Floor_or_ScreenID)));
                jArray.Add(jObject);
            }
            OrderMenus.Add("FloorList", jArray);
            OrderMenus.Add("ExistingSeatings", JArray.FromObject(Seatings));
            OrderMenus.Add("RedSeatings", JArray.FromObject(RedSeatings));
            return OrderMenus;
        }

        [HttpPost]
        public ActionResult SaveCashBkSeating([System.Web.Http.FromBody] CashBkSeating cashBkseating)
        {
            Cashback cashback = Cashback.Getone(cashBkseating.CashBkId);
            if (cashback.CashBkId > 0)
            {
                string SeatingIds;
               if (cashBkseating.Seatings == null)
                {
                     SeatingIds = "";
                }
                else
                {
                     SeatingIds = String.Join(",", cashBkseating.Seatings);
                }
                cashback.SeatingIds = SeatingIds;
                cashback.Save();
            }
            else
            {
                return Json(new { msg = "Could not Find CashBack " });
            }

            return Content("1");
        }
    }
}