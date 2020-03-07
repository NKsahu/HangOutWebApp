using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;


namespace HangOut.Models.Account
{
    public class Receipt
    {
        public int ID { get; set; }
        public double Amount { get; set; }
        public string Particular { get; set; }
        public int CRGroupId { get; set; }
        public int DRGroupId { get; set; }
        public int CRLedgerId { get; set; }
        public int DRLedgerId { get; set; }
        public int BalanceStatementId { get; set; }
        public DateTime Date { get; set; }
        public int OrgId { get; set; }
        public int EntryNo { get; set; }
        public double Balance { get; set; }





        public int Save()
        {
            int Row = 0;
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Quary = "";

                Quary = "Insert Into ACReceipt Values (@Particular,@Amount,@CRGroupId,@CRLedgerId,@BalanceStatementId,@Date,@OrgId,@DRLedgerId,@EntryNo,@Balance,@DRGroupId);SELECT SCOPE_IDENTITY();";


                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@ID", this.ID);
                cmd.Parameters.AddWithValue("@Particular", this.Particular);
                cmd.Parameters.AddWithValue("@Amount", this.Amount);
                cmd.Parameters.AddWithValue("@CRGroupId", this.CRGroupId);
                cmd.Parameters.AddWithValue("@CRLedgerId", this.CRLedgerId);
                cmd.Parameters.AddWithValue("@BalanceStatementId", this.BalanceStatementId);
                cmd.Parameters.AddWithValue("@Date", this.Date);
                cmd.Parameters.AddWithValue("@OrgId", this.OrgId);
                cmd.Parameters.AddWithValue("@DRLedgerId", this.DRLedgerId);
                cmd.Parameters.AddWithValue("@EntryNo", this.EntryNo);
                cmd.Parameters.AddWithValue("@Balance", this.Balance);
                cmd.Parameters.AddWithValue("@DRGroupId", this.DRGroupId);

                Row = Convert.ToInt32(cmd.ExecuteScalar());
                this.ID = Row;



            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return Row;

        }
        public static List<Receipt> GetAllList(int OrgId)
        {
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<Receipt> ReceiptList = new List<Receipt>();

            try
            {
                string Quary = "Select * from ACReceipt ORDER BY Date ASC";

                if(OrgId>0)
                {
                  Quary = "Select * from ACReceipt where OrgId="+OrgId+" ORDER BY Date ASC";
                }
                cmd = new SqlCommand(Quary, con.Con);
                SDR = cmd.ExecuteReader();

                while (SDR.Read())
                {
                    Receipt OBJBS = new Receipt();
                    OBJBS.ID = SDR.GetInt32(0);
                    OBJBS.Particular = SDR.GetString(1);
                    OBJBS.Amount = SDR.GetDouble(2);
                    OBJBS.CRGroupId = SDR.GetInt32(4);
                    OBJBS.CRLedgerId = SDR.GetInt32(5);
                    OBJBS.BalanceStatementId = SDR.GetInt32(6);
                    OBJBS.Date = SDR.GetDateTime(7);
                    OBJBS.DRLedgerId = SDR.GetInt32(9);
                    OBJBS.EntryNo = SDR.GetInt32(10);
                    OBJBS.Balance = SDR.GetDouble(11);
                    OBJBS.DRGroupId = SDR.GetInt32(12);
                    ReceiptList.Add(OBJBS);
                }

            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return (ReceiptList);
        }

    }
} 