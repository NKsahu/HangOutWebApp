using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
namespace HangOut.Models.Account
{
    public class Commission
    {
        public int CommisionId { get; set; }
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

                Quary = "Insert Into ACCommission Values (@CommissionAmount,@TaxOnCommission,@EntryNo,@BalanceStatementId,@OrgId);SELECT SCOPE_IDENTITY();";

                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@CommisionId", this.CommisionId);
                cmd.Parameters.AddWithValue("@CommissionAmount", this.CommissionAmount);
                cmd.Parameters.AddWithValue("@TaxOnCommission", this.TaxOnCommission);
                cmd.Parameters.AddWithValue("@EntryNo", this.EntryNo);
                cmd.Parameters.AddWithValue("@BalanceStatementId", this.BalanceStatementId);
                cmd.Parameters.AddWithValue("@OrgId", this.OrgId);
       
                Row = Convert.ToInt32(cmd.ExecuteScalar());
                this.CommisionId = Row;
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return Row;

        }
        public static List<Commission> GetAllCommissions()
        {
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<Commission> CommissionList = new List<Commission>();

            try
            {
                string Quary = "Select * from ACCommission";
                cmd = new SqlCommand(Quary, con.Con);
                SDR = cmd.ExecuteReader();

                while (SDR.Read())
                {
                    Commission OBJBS = new Commission();
                    OBJBS.CommisionId = SDR.GetInt32(0);
                    OBJBS.CommissionAmount = SDR.GetDouble(1);           
                    OBJBS.TaxOnCommission = SDR.GetDouble(2);
                    OBJBS.EntryNo = SDR.GetInt32(3);
                    OBJBS.BalanceStatementId = SDR.GetInt32(4);
                    OBJBS.OrgId = SDR.GetInt32(5);
                    CommissionList.Add(OBJBS);
                }

            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return (CommissionList);
        }
        public static int Dell(int ID)
        {
            int R = 0;
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            try
            {
                string Query = "Delete FROM  ACCommission where OrgId=" + ID;
                cmd = new SqlCommand(Query, Con);
                R = cmd.ExecuteNonQuery();
            }
            catch (System.Exception e)
            { e.ToString(); }

            finally
            {
                Con.Close();
            }
            return R;
        }

    }

}