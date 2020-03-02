using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
namespace HangOut.Models.Account
{
    public class Commission
    {
        public int CID { get; set; }
        public double CommissionAmount { get; set; }
        public double TaxOnCommission { get; set; }
        public int EntryNo { get; set; }
        public int BalanceStatementId { get; set; }
        public int OrgId { get; set; }


        public int Save()
        {
            int Row = 0;
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Quary = "";

                Quary = "Insert Into ACCommission Values (@CID,@CommissionAmount,@TaxOnCommission,@EntryNo,@BalanceStatementId,@OrgId);SELECT SCOPE_IDENTITY();";


                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@BID", this.CID);
                cmd.Parameters.AddWithValue("@Date", this.CommissionAmount);
                cmd.Parameters.AddWithValue("@Amount", this.TaxOnCommission);
                cmd.Parameters.AddWithValue("@Narration", this.EntryNo);
                cmd.Parameters.AddWithValue("@OrderId", this.BalanceStatementId);
                cmd.Parameters.AddWithValue("@OrgId", this.OrgId);
       

                Row = Convert.ToInt32(cmd.ExecuteScalar());
                this.CID = Row;



            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return Row;

        }
    }
}