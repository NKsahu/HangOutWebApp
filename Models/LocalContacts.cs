using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models
{
    public class LocalContacts
    {
        public int ContctID { get; set; }
        public string MobileNo { get; set; }
        public string Cust_Name { get; set; }
        public int OrgId { get; set; }
        public int Save()
        {
            int Row = 0;
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Quary = "";
                if (this.ContctID == 0)
                {
                    Quary = "Insert Into LocalContacts Values (@MobileNo,@Cust_Name,@OrgId);SELECT SCOPE_IDENTITY();";
                }
                else
                {
                    Quary = "Update LocalContacts Set MobileNo=@MobileNo,Cust_Name=@Cust_Name where ContctID=@ContctID";
                }
                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@ContctID", this.ContctID);
                cmd.Parameters.AddWithValue("@MobileNo", this.MobileNo);
                cmd.Parameters.AddWithValue("@Cust_Name", this.Cust_Name);
                if (this.ContctID == 0)
                {
                    cmd.Parameters.AddWithValue("@OrgId", this.OrgId);
                    Row = Convert.ToInt32(cmd.ExecuteScalar());
                    this.ContctID = Row;
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
        public static List<LocalContacts> GetAll()
        {
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<LocalContacts> listUnit = new List<LocalContacts>();
            try
            {
                string Quary = "Select * from LocalContacts ORDER BY ContctID DESC";
                cmd = new SqlCommand(Quary, con.Con);
                SDR = cmd.ExecuteReader();

                while (SDR.Read())
                {
                    LocalContacts OBJINT = new LocalContacts();
                    OBJINT.ContctID = SDR.GetInt32(0);
                    OBJINT.MobileNo = SDR.GetString(1);
                    OBJINT.Cust_Name = SDR.GetString(2);
                    listUnit.Add(OBJINT);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return (listUnit);
        }
        public static LocalContacts GetOne(int ID=0,string Mobile=null)
        {
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            var CurrOrgObj = HttpContext.Current.Request.Cookies["UserInfo"];
            LocalContacts ObjTmp = new LocalContacts();
            try
            {
                string Query = "SELECT TOP 1 * FROM  LocalContacts";
                if (ID > 0)
                {
                    Query+=" where ContctID=" + ID;
                }
                else if (Mobile != null)
                {

                    Query += " where MobileNo=" + Mobile;
                }
                if (CurrOrgObj != null &&ID==0)
                {
                    Query += " and OrgId=" + CurrOrgObj["OrgId"];
                }
                cmd = new SqlCommand(Query, Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    ObjTmp.ContctID = SDR.GetInt32(0);
                    ObjTmp.MobileNo = SDR.GetString(1);
                    ObjTmp.Cust_Name = SDR.GetString(2);
                }
            }
            catch (System.Exception e)
            { e.ToString(); }

            finally { cmd.Dispose(); Con.Close(); }

            return (ObjTmp);
        }
    }
}