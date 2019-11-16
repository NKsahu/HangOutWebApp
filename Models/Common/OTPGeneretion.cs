using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models.Common
{
    public class OTPGeneretion
    {
        public int ID { get; set; }
        public string MobileNO { get; set; }
        public string OTP { get; set; }
        public DateTime Creation_Date { get; set; }
       public OTPGeneretion()
        {
            Creation_Date = DateTime.Now;
        }
        public int save()
        {
            int Row = 0;
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            try
            {
                Con.Open();
                SqlCommand cmd = null;
                string Query = "";
                if(this.ID>0)
                 
                    Query = "INSERT INTO OTPGeneretion Values (@MobileNo,@OTP,@Creation_Date); SELECT SCOPE_IDENTITY();";
                cmd = new SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("@ID",this.ID);
                cmd.Parameters.AddWithValue("@MobileNO", this.MobileNO);
                cmd.Parameters.AddWithValue("@OTP", this.OTP);
                cmd.Parameters.AddWithValue("@Creation_Date", this.Creation_Date);
                if (this.ID == 0)
                {
                    Row = System.Convert.ToInt32(cmd.ExecuteScalar());
                    this.ID = Row;

                }
                else
                {
                    Row = cmd.ExecuteNonQuery();
                    if (Row > 0)
                        Row = this.ID;
                }

            }
            catch (Exception e) { e.ToString(); }
            finally { Con.Close(); }
            return Row;
        }
        public OTPGeneretion GetOne(string MobileNO , string Password)
        {
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            OTPGeneretion OTPTemp = new OTPGeneretion();
            try
            {
                string Query = "SELECT * FROM  OTPGeneretion  where  MobileNO=" + MobileNO+" and Password ="+Password+ "";
                cmd = new SqlCommand(Query , Con);
                SDR = cmd.ExecuteReader();
                while(SDR.Read())
                {
                    OTPTemp.ID = SDR.GetInt32(0);
                    OTPTemp.MobileNO = SDR.GetString(1);
                    OTPTemp.OTP = SDR.GetString(2);
                    OTPTemp.Creation_Date = SDR.GetDateTime(3);
                }

            }
            catch (System.Exception e)
            { e.ToString(); }

            finally { Con.Close(); }

            return (OTPTemp);
        }
        public int Dell(int ID)
        {
            int R = 0;
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            try
            {
                string Query = "Delete FROM  OTPGeneretion where ID=" + ID;
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