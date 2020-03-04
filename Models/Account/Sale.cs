using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace HangOut.Models.Account
{
    public class Sale
    {
        public int SaleId { get; set; }
        public double SaleAmount { get; set; }
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

                Quary = "Insert Into ACSale Values (@SaleAmount,@EntryNo,@BalanceStatementId,@OrgId);SELECT SCOPE_IDENTITY();";

                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@SaleId", this.SaleId);
                cmd.Parameters.AddWithValue("@SaleAmount", this.SaleAmount);
                cmd.Parameters.AddWithValue("@EntryNo", this.EntryNo);
                cmd.Parameters.AddWithValue("@BalanceStatementId", this.BalanceStatementId);
                cmd.Parameters.AddWithValue("@OrgId", this.OrgId);

                Row = Convert.ToInt32(cmd.ExecuteScalar());
                this.SaleId = Row;
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return Row;

        }
        public static List<Sale> GetAllSales()
        {
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<Sale> SaleList = new List<Sale>();

            try
            {
                string Quary = "Select * from ACSale";
                cmd = new SqlCommand(Quary, con.Con);
                SDR = cmd.ExecuteReader();

                while (SDR.Read())
                {
                    Sale OBJBS = new Sale();
                    OBJBS.SaleId = SDR.GetInt32(0);
                    OBJBS.SaleAmount = SDR.GetDouble(1);
                    OBJBS.EntryNo = SDR.GetInt32(2);
                    OBJBS.BalanceStatementId = SDR.GetInt32(3);
                    OBJBS.OrgId = SDR.GetInt32(4);
                    SaleList.Add(OBJBS);
                }

            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return (SaleList);
        }
        public static int Dell(int ID)
        {
            int R = 0;
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            try
            {
                string Query = "Delete FROM  ACSale where OrgId=" + ID;
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