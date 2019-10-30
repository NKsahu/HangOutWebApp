using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models
{
    public class HG_Category
    {
        
        public int CategoryID { get; set; }
        public int OrgID { get; set; }
        public string Category { get; set; }
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
                if (this.CategoryID == 0)
                {
                    Query = "Insert into  HG_Category  values( @OrgID ,@Category ,@EntryBy,@EntryDate,@UpdateDate,@Status);";
                    cmd = new SqlCommand(Query, Con);
                    cmd.Parameters.AddWithValue("@EntryBy", HttpContext.Current.Session["ID"]);
                    cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);

                }
                else
                {

                    Query = "update  HG_Category set   OrgID =@OrgID ,Category=@Category,EntryBy=@EntryBy,EntryDate=@EntryDate,UpdateDate=@UpdateDate,Status=@Status where CategoryID=@CategoryID";
                    cmd = new SqlCommand(Query, Con);
                    cmd.Parameters.AddWithValue("@CategoryID", this.CategoryID);
                    cmd.Parameters.AddWithValue("@EntryBy", HttpContext.Current.Session["ID"]);
                    cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                }

                
                cmd.Parameters.AddWithValue("@OrgID", this.OrgID);
                cmd.Parameters.AddWithValue("@Category", this.Category);
                cmd.Parameters.AddWithValue("@Status", this.Status);
                Row = cmd.ExecuteNonQuery();
            }
            catch (Exception e) { e.ToString(); }
            finally { Con.Close(); }
            return Row;
        }

        public List<HG_Category> GetAll()
        {
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<HG_Category> ListTmp = new List<HG_Category>();

            try
            {
                string Query = "SELECT * FROM  HG_Category ORDER BY CategoryID DESC";
                cmd = new SqlCommand(Query, Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    HG_Category ObjTmp = new HG_Category();
                    ObjTmp.CategoryID = SDR.GetInt32(0);
                    ObjTmp.OrgID = SDR.GetInt32(1);
                    ObjTmp.Category = SDR.GetString(2);
                    ObjTmp.Status = SDR.GetBoolean(6);
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