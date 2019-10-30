using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models
{
    public class vw_HG_UsersDetails
    {
        public int UserCode { get; set; }
        public int OrgID { get; set; }
        public string UserType { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Status { get; set; }


        public vw_HG_UsersDetails Checkvw_HG_UsersDetails()
        {
            SqlConnection Con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            vw_HG_UsersDetails ObjTmp = new vw_HG_UsersDetails();
            try
            {
                string Query = "SELECT * FROM vw_HG_UsersDetails WHERE UserName = @UserName AND Password = @Password and Status=1 and Convert(Date,DOE) >=Convert(Date,GetDate());";
                cmd = new System.Data.SqlClient.SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("@UserName", this.UserName);
                cmd.Parameters.AddWithValue("@Password", this.Password);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    ObjTmp.UserCode = SDR.GetInt32(0);
                    ObjTmp.OrgID = SDR.GetInt32(1);
                    ObjTmp.UserType = SDR.GetString(2);
                    ObjTmp.UserName = SDR.GetString(3);
                    ObjTmp.Password = SDR.GetString(4);
                    ObjTmp.Status = SDR.GetBoolean(5);
                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { cmd.Dispose(); SDR.Close(); Con.Close(); Con.Dispose(); Con = null; }
            return (ObjTmp);
        }
    }
}