using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace HangOut.Models
{
    public class HG_Category
    {
        [Display(Name ="Category Id")]
        public int CategoryID { get; set; }
        [Display(Name ="Organization")]
        public int OrgID { get; set; }
        [Display(Name ="Name")]
        public string Category { get; set; }
        public int EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool Status { get; set; }
        public int CategoryType { get; set; } //1 Item category  ,2 AddonCategory
        

     public   HG_Category()
        {
            EntryBy = 0;
            EntryDate = DateTime.Now;
            Status = true;
        }


        public int Save()
        {
            int Row = 0;
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            try
            {
                Con.Open();
                SqlCommand cmd = null;
                string Query = "";
                if (this.CategoryID == 0)
                {
                    Query = "Insert into  HG_Category  values( @OrgID ,@Category ,@EntryBy,@EntryDate,@UpdateDate,@Status,@CategoryType);";
                    cmd = new SqlCommand(Query, Con);
                    cmd.Parameters.AddWithValue("@EntryBy", this.EntryBy);
                    cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                }
                else
                {
                    Query = "update  HG_Category set   OrgID =@OrgID ,Category=@Category,UpdateDate=@UpdateDate,Status=@Status,CategoryType=@CategoryType where CategoryID=@CategoryID";
                    cmd = new SqlCommand(Query, Con);
                    cmd.Parameters.AddWithValue("@CategoryID", this.CategoryID);
                    cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                }
                cmd.Parameters.AddWithValue("@OrgID", this.OrgID);
                cmd.Parameters.AddWithValue("@Category", this.Category);
                cmd.Parameters.AddWithValue("@Status", this.Status);
                cmd.Parameters.AddWithValue("@CategoryType", this.CategoryType);
                Row = cmd.ExecuteNonQuery();
            }
            catch (Exception e) { e.ToString(); }
            finally { Con.Close(); }
            return Row;
        }

        public List<HG_Category> GetAll(int OrgId=0,int CategoryType=1)
        {
            var CurrOrgID = HttpContext.Current.Request.Cookies["UserInfo"];
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<HG_Category> ListTmp = new List<HG_Category>();
            string Query = "SELECT * FROM  HG_Category where CategoryType="+CategoryType.ToString()+"  ORDER BY CategoryID DESC";
            if (OrgId > 0)
            {
                Query = "SELECT * FROM  HG_Category where OrgID="+OrgId.ToString()+ " and CategoryType=" + CategoryType.ToString() + " ORDER BY CategoryID DESC";
            }
            else if (CurrOrgID != null && int.Parse(CurrOrgID["OrgId"]) > 0)
            {
                Query = "SELECT * FROM  HG_Category where OrgID=" + CurrOrgID["OrgId"] + "and CategoryType=" + CategoryType.ToString() + " ORDER BY CategoryID DESC";

            }
            try
            {
                cmd = new SqlCommand(Query, Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    HG_Category ObjTmp = new HG_Category();
                    ObjTmp.CategoryID = SDR.GetInt32(0);
                    ObjTmp.OrgID = SDR.GetInt32(1);
                    ObjTmp.Category = SDR.GetString(2);
                    ObjTmp.Status = SDR.GetBoolean(6);
                    ObjTmp.CategoryType = SDR.GetInt32(7);
                    ListTmp.Add(ObjTmp);
                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { Con.Close(); }

            return (ListTmp);
        }
        public HG_Category GetOne(int CategoryID)
        {
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            HG_Category ObjTmp = new HG_Category();

            try
            {
                string Query = "SELECT * FROM  HG_Category where CategoryID=@CategoryID";
                cmd = new SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    ObjTmp.CategoryID = SDR.GetInt32(0);
                    ObjTmp.OrgID = SDR.GetInt32(1);
                    ObjTmp.Category = SDR.GetString(2);
                    ObjTmp.Status = SDR.GetBoolean(6);
                    ObjTmp.CategoryType = SDR.GetInt32(7);

                }
            }
            catch (System.Exception e)
            { e.ToString(); }

            finally { Con.Close(); }

            return (ObjTmp);
        }

        public static int Dell(int ID)
        {
            int R = 0;
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            try
            {
                string Query = "Delete FROM  HG_Category where CategoryID=" + ID;
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