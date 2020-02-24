using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace HangOut.Models.DynamicList
{
    public class OrgType
    {
        public string id { get; set; }
        public string Name { get; set; }
        public static int[] mystates { get; set; }
        public static List<OrgType> List { get; set; }
        public List<OrgType> ListOrgTypeList()
        {
            List<OrgType> list = new List<OrgType>();
            list.Add(new OrgType { id = "1", Name = "Restuarant" });
            list.Add(new OrgType { id = "2", Name = "Theater" });
            return list;
        }
        public static List<SelectListItem> PaymentType()
        {
            List<SelectListItem> sl = new List<SelectListItem>();
            SelectListItem slobj = new SelectListItem();
            slobj.Value = "1";
            slobj.Text = "Prepaid";
            SelectListItem slobj2 = new SelectListItem();
            slobj2.Value = "2";
            slobj2.Text = "PostPaid";
            sl.Add(slobj);
            sl.Add(slobj2);

            return sl;
        }

        public static string PaymentMode(int PMode)
        {
            string PayMode = "Unpaid";
            if (PMode == 1)
            {
                PayMode = "Cash";

            }
            else if (PMode == 2)
            {

                PayMode = "Bank/Wallet";
            }
            else if (PMode == 3)
            {
                PayMode = "FoodDo";
            }

            return PayMode;
        }
        public static string OrderStatus(string Sts)
        {
            string PayMode = "Placed";
             if (Sts == "2")
            {
                PayMode = "Processing";
            }
            else if (Sts == "3")
            {
                PayMode = "Completed";
            }
            else if(Sts=="4")
            {
                PayMode = "Canceled";
            }
            return PayMode;
        }
        public static double TotalTax(double Amt, double Tax, int Cnt)
        {
            double total = 0.00;

            total = ((Amt * Tax) / 100) * Cnt;
            return total;
        }
        public static bool  DeliveryChargeAply(int AppType,OrgSetting setting)
        {
            bool Sts = false;
            //1 customer ,2 captain , 3 admin panel
            if(AppType==1&& setting.ApplyInCustomerApp)
            {
                Sts = true;
            }
            else if (AppType == 2 && setting.ApplyInCaptainApp)
            {
                Sts = true;
            }
            else if (AppType == 3 && setting.ApplyInAdminPanel)
            {
                Sts = true;
            }
            else
            {
                Sts = false;
            }
            return Sts;
        }
        public static string ItemMode(string mode)
        {
            if (mode == "1")
            {
                return "VEG";
            }
            else
            {
                return "NON-VEG";
            }
        }

        //public static int[] states ()
        //{
        //    mystates[0] = 17;
        //    return mystates;
        //}
    }
}