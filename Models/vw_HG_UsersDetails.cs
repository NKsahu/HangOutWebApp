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
        public string EMail { get; set; }
        public string UPhoto { get; set; }
        public int EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool Status { get; set; }
       public vw_HG_UsersDetails()
        {
            OrgID = 0;
           UPhoto = "";
        }
        public List<vw_HG_UsersDetails> GetAll()
        {
            SqlConnection Con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
             
            List<vw_HG_UsersDetails> listOfuser = new List<vw_HG_UsersDetails>();
            string Query = "SELECT * FROM HG_UsersDetails WHERE Status=1 ;";
          SqlCommand  cmd = new  SqlCommand(Query, Con);
            try
            {
               
               
               SqlDataReader SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    vw_HG_UsersDetails ObjTmp = new vw_HG_UsersDetails();
                    
                    ObjTmp.UserCode = SDR.GetInt32(0);
                    ObjTmp.OrgID = SDR.GetInt32(1);
                    ObjTmp.UserType = SDR.GetString(2);
                    ObjTmp.UserName = SDR.GetString(3);
                    ObjTmp.UserId = SDR.GetString(4);
                    ObjTmp.Password = SDR.GetString(5);
                    ObjTmp.EMail = SDR.GetString(6);
                    listOfuser.Add(ObjTmp);
                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { cmd.Dispose();Con.Close(); }
            return listOfuser;
        }
        //this is get One Function
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
                    ObjTmp.EMail = SDR.GetString(6);

                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { cmd.Dispose(); SDR.Close(); Con.Close(); Con.Dispose(); Con = null; }
            return (ObjTmp);
        }
        public vw_HG_UsersDetails GetSingleByUserId(int UserCode)
        {
            SqlConnection Con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            vw_HG_UsersDetails ObjTmp = new vw_HG_UsersDetails();
            try
            {
                string Query = "SELECT * FROM HG_UsersDetails where UserCode=" + UserCode.ToString()+" and Status=1 ;";
                cmd = new System.Data.SqlClient.SqlCommand(Query, Con);
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
                    ObjTmp.EMail = SDR.GetString(6);

                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { cmd.Dispose(); SDR.Close(); Con.Close(); Con.Dispose(); Con = null; }
            return (ObjTmp);
        }
        public int save()
        {
            int R = 0;
            SqlConnection Con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            try
            {
                Con.Open();
                SqlCommand cmd = null;
                string Quary = "";
                if (this.UserCode == 0)
                
                    Quary = "Insert into HG_UsersDetails values(@OrgID,@UserType,@UserName,@UserId,@Password,@EMail,@UPhoto,@EntryBy,@EntryDate,@UpdateDate,@status);SELECT SCOPE_IDENTITY(); ";
                     
                 
                    Quary = "Update HG_UsersDetails set OrgID=@OrgID,UserType=@UserType,UserName=@UserName,UserId=@UserId,Password=@Password,EMail=@EMail,UPhoto=@Uphoto,EntryBy=@EntryBy,EntryDate=@EntryDate,UpdateDate=@UpdateDate,status=@status where UserCode=@UserCode;";

                cmd = new SqlCommand(Quary, Con);
                cmd.Parameters.AddWithValue("@UserCode", this.UserCode);
                cmd.Parameters.AddWithValue("@OrgID", this.OrgID);
                cmd.Parameters.AddWithValue("@UserType", this.UserType);
                cmd.Parameters.AddWithValue("@UserName", this.UserName);
                cmd.Parameters.AddWithValue("@UserId", this.UserId);
                cmd.Parameters.AddWithValue("@Password", this.Password);
                cmd.Parameters.AddWithValue("@EMail", this.EMail);
                cmd.Parameters.AddWithValue("@UPhoto", this.UPhoto);
                cmd.Parameters.AddWithValue("@EntryBy", this.EntryBy);
                cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@Status", this.Status);
               
                if (this.UserCode == 0)
                {
                    R = System.Convert.ToInt32(cmd.ExecuteScalar());
                   
                }
                else
                {
                   if(cmd.ExecuteNonQuery() > 0)
                    {
                        R = this.UserCode;
                    }
                }
               
            }
            catch (System.Exception e){ e.ToString(); }

            finally
            {
                Con.Close(); Con.Dispose(); Con = null;
            }
            return R;
        }
    }
}