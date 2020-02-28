using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models
{
    public class OrdDiscntChrge
    {
      public int   ID { get; set; }
      public string Title { get; set; }
      public Int64 OID { get; set; }
      public int Type { get; set; }// 1 discount, 2 charge
      public double Amt { get; set; }
      public double Tax { get; set; }
      public string Remark { get; set; }
      public DateTime Datetime { get; set; }
     public Int64 SeatingId { get; set; }
        public int SeatingOtp { get; set; }

    public OrdDiscntChrge()
        {
            Amt = 0.00;
            Tax = 0.00;
        }

        public int Save()
        {
            int Row = 0;
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Quary = "";
                if (this.ID == 0)
                {
                    Quary = "Insert Into OrdDiscntChrge Values (@Title,@OID,@Type,@Amt,@Tax,@Remark,@Datetime);SELECT SCOPE_IDENTITY();";
                }
                else
                {
                    Quary = "Update OrdDiscntChrge Set Title=@Title,OID=@OID,Type=@Type,Amt=@Amt,Tax=@Tax,Remark=@Remark,Datetime=@Datetime where ID=@ID";
                }
                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@ID", this.ID);
                cmd.Parameters.AddWithValue("@Title", this.Title);
                cmd.Parameters.AddWithValue("@Type", this.Type);
                cmd.Parameters.AddWithValue("@Amt", this.Amt);
                cmd.Parameters.AddWithValue("@Tax", this.Tax);
                cmd.Parameters.AddWithValue("@Remark", this.Remark);
                cmd.Parameters.AddWithValue("@Datetime", DateTime.Now);
                
                if (this.ID == 0)
                {
                    Row = Convert.ToInt32(cmd.ExecuteScalar());
                    this.ID = Row;
                }
                else
                {
                    Row = cmd.ExecuteNonQuery();
                    //this.CategoryID = Row;
                }

            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return Row;

        }
        
        public static OrdDiscntChrge GetOne(int ID )
        {
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            OrdDiscntChrge ObjTmp = new OrdDiscntChrge();
            try
            {
                string Query = "SELECT   * FROM  OrdDiscntChrge";
                
                cmd = new SqlCommand(Query, Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    ObjTmp. ID = SDR.GetInt32(0);
                    ObjTmp.Title = SDR.GetString(1);
                    ObjTmp.Type = SDR.GetInt32(2);
                    ObjTmp.Amt = SDR.GetInt32(3);
                    ObjTmp.Tax = SDR.GetInt32(4);
                    ObjTmp.Remark = SDR.GetString(5);
                    ObjTmp.Datetime = SDR.GetDateTime(6);
                }
            }
            catch (System.Exception e)
            { e.ToString(); }

            finally { cmd.Dispose(); Con.Close(); }

            return (ObjTmp);
        }
    }
    public class DiscntCharge
    {
        public static List<OrdDiscntChrge> ListDiscntChrge { get; set; }
    }
}