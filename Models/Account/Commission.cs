using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HangOut.Models.Account
{
    public class Commission
    {
        public int CID { get; set; }
        public double CommissionAmount { get; set; }
        public double TaxOnCommission { get; set; }
        public int EntryNo { get; set; }
        public int BalanceStatementId { get; set; }



    }
}