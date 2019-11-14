using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models
{
    public class HG_UserTypes
    {
        public int UTID { get; set; }
        public string UserType { get; set; }
        [Display(Name = "Short Name")]
        public string UserTypeName { get; set; }
        public int EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool Status { get; set; }

       



        public int Save()
        {
            int Row = 0;


            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            try
            {
                Con.Open();
                SqlCommand cmd = null;
                string Query = "";
                if (this.UTID == 0)
                {
                    Query = "Insert into  HG_UserTypes  values(@UserType,@UserTypeName,@EntryBy,@EntryDate,@UpdateDate,@Status);";
                    cmd = new SqlCommand(Query, Con);
                    cmd.Parameters.AddWithValue("@EntryBy", this.EntryBy);
                    cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                     
                }
                else
                {

                    Query = "update  HG_UserTypes set UserType=@UserType,UserTypeName=@UserTypeName,EntryBy=@EntryBy,EntryDate=@EntryDate,UpdateDate=@UpdateDate,Status=@Status where UTID=@UTID";
                    cmd = new SqlCommand(Query, Con);
                    cmd.Parameters.AddWithValue("@UTID", this.UTID);
                    cmd.Parameters.AddWithValue("@EntryBy", this.EntryBy);
                    cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                }

                cmd.Parameters.AddWithValue("@UserType", this.UserType);
                cmd.Parameters.AddWithValue("@UserTypeName ", this.UserTypeName);
                cmd.Parameters.AddWithValue("@Status", this.Status);
                Row = cmd.ExecuteNonQuery();
            }
            catch (Exception e) { e.ToString(); }
            finally { Con.Close(); }
            return Row;
        }

        public List<HG_UserTypes> GetAll()
        {
            var CurrOrgID = HttpContext.Current.Request.Cookies["UserInfo"];
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<HG_UserTypes> ListTmp = new List<HG_UserTypes>();
            string Query = "SELECT * FROM  HG_UserTypes   ORDER BY UTID DESC";
            if (CurrOrgID!=null && int.Parse(CurrOrgID["OrgId"]) >0)
            {
                  Query = "SELECT * FROM  HG_UserTypes where UserType!='SA' And UserType!='A' ORDER BY UTID DESC";
            }
            try
            {
             
                cmd = new SqlCommand(Query, Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    HG_UserTypes ObjTmp = new HG_UserTypes();
                    ObjTmp.UTID = SDR.GetInt32(0);
                    ObjTmp.UserType= SDR.GetString(1);
                    ObjTmp.UserTypeName = SDR.GetString(2);
                    ObjTmp.EntryBy = SDR.GetInt32(3);
                    ObjTmp. EntryDate = SDR.GetDateTime(4);
                    ObjTmp.UpdateDate = SDR.GetDateTime(5);
                    ObjTmp.Status = SDR.GetBoolean(6);
                    ListTmp.Add(ObjTmp);
                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { Con.Close(); }

            return (ListTmp);
        }
        public HG_UserTypes GetOne(int UTID)
        {
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            HG_UserTypes ObjTmp = new HG_UserTypes();

            try
            {
                string Query = "SELECT * FROM  HG_UserTypes where UTID=@UTID";
                cmd = new SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("@UTID", UTID);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    ObjTmp.UTID = SDR.GetInt32(0);
                    ObjTmp.UserType = SDR.GetString(1);
                    ObjTmp.UserTypeName = SDR.GetString(2);
                    ObjTmp.Status = SDR.GetBoolean(3);
                }
            }
            catch (System.Exception e)
            { e.ToString(); }

            finally { Con.Close(); }

            return (ObjTmp);
        }

        public int Dell(int ID)
        {
            int R = 0;
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            try
            {
                string Query = "Delete FROM  HG_UserTypes where UTID=" + ID;
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