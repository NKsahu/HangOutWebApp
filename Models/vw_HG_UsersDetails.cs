using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HangOut.Models
{
    public class vw_HG_UsersDetails
    {
        public int UserCode { get; set; } // Every User Uniqe Id Auto generated
        public int OrgID { get; set; }
        public string UserType { get; set; }
        [Display(Name = "Name")]
        public string UserName { get; set; }// only for dispaly
        [Display(Name ="Mobile No")]
        public string UserId { get; set; }// used for Login as Authentication
        public string Password { get; set; }
        public string EMail { get; set; }
        public string UPhoto { get; set; }
        public int EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool Status { get; set; }
        public bool CurrentStatus { get; set; }
        public string orgType { get; set; }// only for check OrgType not included in table column
        public int JoinByOrg { get; set; }// join from which org
        public bool IsHeadChef { get; set; }
        public bool TickUntick { get; set; }//{0 unticked, 1 Ticked}
        public int RateNow { get; set; }//{0:ask for rating,1: rated in play store
       public vw_HG_UsersDetails()
        {
            Status = true;
            OrgID = 0;
           UPhoto = "";
            EMail = "";
            UserName = "";
            CurrentStatus = true;
            TickUntick = true;
            RateNow = 0;
        }
        public int save()
        {
            int R = 0;
            DBCon dBCon = new DBCon();
            try
            {
                SqlCommand cmd = null;
                string Quary = "";
                if (this.UserCode == 0)
                {
                    Quary = "Insert into HG_UsersDetails values(@OrgID,@UserType,@UserName,@UserId,@Password,@EMail,@UPhoto,@EntryBy,@EntryDate,@UpdateDate,@status,@CurrentStatus,@JoinByOrg,@IsHeadChef,@TickUntick,@RateNow);SELECT SCOPE_IDENTITY(); ";
                    cmd = new SqlCommand(Quary, dBCon.Con);
                    cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@EntryBy", this.EntryBy);
                }
                else
                {
                    Quary = "Update HG_UsersDetails set OrgID=@OrgID,UserType=@UserType,UserName=@UserName,UserId=@UserId,Password=@Password,EMail=@EMail,UPhoto=@Uphoto,UpdateDate=@UpdateDate,status=@status,CurrentStatus=@CurrentStatus,JoinByOrg=@JoinByOrg,IsHeadChef=@IsHeadChef,TickUntick=@TickUntick,RateNow=@RateNow where UserCode=@UserCode;";
                    cmd = new SqlCommand(Quary, dBCon.Con);
                    cmd.Parameters.AddWithValue("@UserCode", this.UserCode);
                }
                cmd.Parameters.AddWithValue("@OrgID", this.OrgID);
                cmd.Parameters.AddWithValue("@UserType", this.UserType);
                cmd.Parameters.AddWithValue("@UserName", this.UserName);
                cmd.Parameters.AddWithValue("@UserId", this.UserId);
                cmd.Parameters.AddWithValue("@Password", this.Password);
                cmd.Parameters.AddWithValue("@EMail", this.EMail);
                cmd.Parameters.AddWithValue("@UPhoto", this.UPhoto);
                cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@Status", this.Status);
                cmd.Parameters.AddWithValue("@CurrentStatus", this.CurrentStatus);
                cmd.Parameters.AddWithValue("@JoinByOrg", this.JoinByOrg);
                cmd.Parameters.AddWithValue("@IsHeadChef", this.IsHeadChef);
                cmd.Parameters.AddWithValue("@TickUntick", this.TickUntick);
                cmd.Parameters.AddWithValue("@RateNow", this.RateNow);
                if (this.UserCode == 0)
                {
                    R = Convert.ToInt32(cmd.ExecuteScalar());
                    this.UserCode = R;
                }
                else
                {
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        R = this.UserCode;
                    }
                }
            }
            catch (Exception e) { e.ToString(); }

            finally
            {
                dBCon.Con.Close(); dBCon.Con.Dispose(); ;
            }
            return R;
        }
        public List<vw_HG_UsersDetails> GetAll(string Type="",int OrgId=0)
        {
            var CurrOrgID = HttpContext.Current.Request.Cookies["UserInfo"];
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
             
            List<vw_HG_UsersDetails> listOfuser = new List<vw_HG_UsersDetails>();
            string Query = " SELECT * FROM HG_UsersDetails WHERE  UserType!='CUST' ;";
            if (Type != "")
            {
                Query = "SELECT * FROM HG_UsersDetails WHERE UserType='"+Type+"' ;";
            }
            else if (OrgId > 0)
            {
                Query = "SELECT * FROM HG_UsersDetails WHERE OrgID=" + OrgId.ToString()+ ";";
            }
            else if (CurrOrgID != null && int.Parse(CurrOrgID["OrgId"]) > 0)
            {
                Query = "SELECT * FROM HG_UsersDetails WHERE OrgID=" + CurrOrgID["OrgId"] + ";";
            }
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
                    ObjTmp.EMail =SDR.IsDBNull(6)?"": SDR.GetString(6);
                    ObjTmp.UPhoto = SDR.IsDBNull(7) ? "" : SDR.GetString(7);
                    ObjTmp.EntryBy = SDR.GetInt32(8);
                    ObjTmp.EntryDate = SDR.GetDateTime(9);
                    ObjTmp.UpdateDate = SDR.GetDateTime(10);
                    ObjTmp.Status = SDR.GetBoolean(11);
                    ObjTmp.CurrentStatus = SDR.IsDBNull(12) ? false : SDR.GetBoolean(12);
                    ObjTmp.JoinByOrg = SDR.GetInt32(13);
                    ObjTmp.IsHeadChef = SDR.GetBoolean(14);
                    ObjTmp.TickUntick = SDR.GetBoolean(15);
                    ObjTmp.RateNow = SDR.GetInt32(16);
                    listOfuser.Add(ObjTmp);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose();Con.Close(); }
            return listOfuser; 
        }
        //this is get One Function
        public vw_HG_UsersDetails Checkvw_HG_UsersDetails()
        {
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            vw_HG_UsersDetails ObjTmp = null;
            try
            {
                string Query = "SELECT TOP 1 * FROM HG_UsersDetails WHERE UserID = @UserID AND Password = @Password COLLATE Latin1_General_CS_AS;";
                cmd = new System.Data.SqlClient.SqlCommand(Query, dBCon.Con);
                cmd.Parameters.AddWithValue("@UserID", UserId);
                cmd.Parameters.AddWithValue("@Password", Password);
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
                    ObjTmp.UPhoto = SDR.IsDBNull(7) ? "" : SDR.GetString(7);
                    ObjTmp.EntryBy = SDR.GetInt32(8);
                    ObjTmp.EntryDate = SDR.GetDateTime(9);
                    ObjTmp.UpdateDate = SDR.GetDateTime(10);
                    ObjTmp.Status = SDR.GetBoolean(11);
                    ObjTmp.CurrentStatus = SDR.IsDBNull(12) ? false : SDR.GetBoolean(12);
                    ObjTmp.JoinByOrg = SDR.GetInt32(13);
                    ObjTmp.IsHeadChef = SDR.GetBoolean(14);
                    ObjTmp.TickUntick = SDR.GetBoolean(15);
                    ObjTmp.RateNow = SDR.GetInt32(16);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); SDR.Close(); dBCon.Con.Close(); dBCon.Con.Dispose();SDR.Close(); }
            return (ObjTmp);
        }


        public vw_HG_UsersDetails GetSingleByUserId(int UserCode=0,string UserLogin= null)
        {
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            vw_HG_UsersDetails ObjTmp = new vw_HG_UsersDetails();
            string Query = "";
            try
            {
                if (UserCode > 0)
                {
                    Query = "SELECT TOP 1 * FROM HG_UsersDetails where UserCode=" + UserCode.ToString() + "";
                }else if (UserLogin != null)
                {
                    Query= "SELECT TOP 1 * FROM HG_UsersDetails where UserId='" + UserLogin + "'";
                }
                cmd = new SqlCommand(Query, Con);
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
                    ObjTmp.UPhoto = SDR.IsDBNull(7) ? "" : SDR.GetString(7);
                    ObjTmp.EntryBy = SDR.GetInt32(8);
                    ObjTmp.EntryDate = SDR.GetDateTime(9);
                    ObjTmp.UpdateDate = SDR.GetDateTime(10);
                    ObjTmp.Status = SDR.GetBoolean(11);
                    ObjTmp.CurrentStatus = SDR.IsDBNull(12) ? false : SDR.GetBoolean(12);
                    ObjTmp.JoinByOrg = SDR.GetInt32(13);
                    ObjTmp.IsHeadChef = SDR.GetBoolean(14);
                    ObjTmp.TickUntick = SDR.GetBoolean(15);
                    ObjTmp.RateNow = SDR.GetInt32(16);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); SDR.Close(); Con.Close(); Con.Dispose(); Con = null; }
            return (ObjTmp);
        }
        
       
       public vw_HG_UsersDetails MobileAlreadyExist(string UserLogin)
        {
            vw_HG_UsersDetails ObjTmp = new vw_HG_UsersDetails();
            ObjTmp = new vw_HG_UsersDetails().GetSingleByUserId(UserLogin: UserLogin);
            return (ObjTmp);
        }
    }
}