using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HangOut.Models.MyCustomer
{
    public class Cashback
    {
        public int CashBkId { get; set; }
        public int OrgID { get; set; }
        public DateTime StartDate { get; set; }
        public int ValidTill { get; set; }// 1 unspecify , 2 specify ValidTillDate 
        public DateTime ValidTillDate { get; set; }

        public int CashBkType { get; set; }  // 1 :Percentage

        public double MaxAmt { get; set; }

        public int MinBilAmtType { get; set; }//1 dynamic , 2 manual amt
        public double BilAmt { get; set; }

        public Cashback()
        {
            StartDate = DateTime.Now;
            ValidTillDate= DateTime.Now;
        }

        public int Save()
        {
            int R = 0;
            DBCon dBCon = new DBCon();

            return R;
        }

    }
}