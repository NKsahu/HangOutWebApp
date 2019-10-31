using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HangOut.Models
{
    public class vw_HG_UsersDetails
    {
        public int UserCode { get; set; } // Every User Uniqe Id Auto generated
        public int OrgID { get; set; }
        public string UserType { get; set; }
        public string UserName { get; set; }// only for dispaly
        [Display(Name ="LoginID")]
        public string UserId { get; set; }// used for Login as Authentication
        public string Password { get; set; }
        public bool Status { get; set; }

        public List<vw_HG_UsersDetails> GetAll()
        {
            SqlConnection Con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<vw_HG_UsersDetails> listOfuser = new List<vw_HG_UsersDetails>();
            try
            {
                string Query = "SELECT * FROM vw_HG_UsersDetails WHERE Status=1 ;";
                cmd = new System.Data.SqlClient.SqlCommand(Query, Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    vw_HG_UsersDetails ObjTmp = new vw_HG_UsersDetails();
                    ObjTmp.UserCode = SDR.GetInt32(0);
                    ObjTmp.OrgID = SDR.GetInt32(1);
                    ObjTmp.UserType = SDR.GetString(2);
                    ObjTmp.UserName = SDR.GetString(3);
                    ObjTmp.Password = SDR.GetString(4);
                    ObjTmp.Status = SDR.GetBoolean(5);
                    listOfuser.Add(ObjTmp);
                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { cmd.Dispose(); SDR.Close(); Con.Close(); Con.Dispose(); Con = null; }
            return (listOfuser);
        }
        public vw_HG_UsersDetails Checkvw_HG_UsersDetails()
        {
            SqlConnection Con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            vw_HG_UsersDetails ObjTmp = null;
            try
            {
                string Query = "SELECT * FROM HG_UsersDetails WHERE UserID = @UserID AND Password = @Password and Status=1 ;";
                cmd = new System.Data.SqlClient.SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("@UserID", this.UserId);
                cmd.Parameters.AddWithValue("@Password", this.Password);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    ObjTmp = new vw_HG_UsersDetails();
                    ObjTmp.UserCode = SDR.GetInt32(0);
                    ObjTmp.OrgID = SDR.GetInt32(1);
                    ObjTmp.UserType = SDR.GetString(2);
                    ObjTmp.UserName = SDR.GetString(3);
                    ObjTmp.UserId = SDR.GetString(4);
                    ObjTmp.Password = SDR.GetString(5);

                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { cmd.Dispose(); SDR.Close(); Con.Close(); Con.Dispose(); Con = null; }
            return (ObjTmp);
        }
    }
}