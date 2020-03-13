using System;
using System.Collections.Generic;
using System.Data.SqlClient;
namespace HangOut.Models.POS
{
    public class ServingSize
    {
        public int ServingId { get; set; }
        public string Name { get; set; }
        public int OrgId { get; set; }
        public int Save()
        {
            int Row = 0;
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Quary = "";
                if (this.ServingId == 0)
                {
                    Quary = "Insert Into HG_ServingSize Values (@Name,@OrgId);SELECT SCOPE_IDENTITY();";
                }
                else
                {
                    Quary = "Update HG_ServingSize Set Name=@Name,OrgId=@OrgId where ServingId=@ServingId";
                }
                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@ServingId", this.ServingId);
                cmd.Parameters.AddWithValue("@Name", this.Name);
                cmd.Parameters.AddWithValue("@OrgId", this.OrgId);
                if (this.ServingId == 0)
                {
                    Row = Convert.ToInt32(cmd.ExecuteScalar());
                    this.ServingId = Row;
                }
                else
                {
                    Row = cmd.ExecuteNonQuery();
                }

            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return Row;

        }
        public static List<ServingSize> GetAll(int OrgId)
        {
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<ServingSize> listOrgId = new List<ServingSize>();
            try
            {
                string Quary = "Select * from HG_ServingSize where OrgId=" + OrgId;
                cmd = new SqlCommand(Quary, con.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    ServingSize OBJINT = new ServingSize();
                    OBJINT.ServingId = SDR.GetInt32(0);
                    OBJINT.Name = SDR.GetString(1);
                    OBJINT.OrgId = SDR.GetInt32(2);
                    listOrgId.Add(OBJINT);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return (listOrgId);
        }
        public ServingSize GetOne(int ID)
        {
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            ServingSize ObjTmp = new ServingSize();

            try
            {
                string Query = "SELECT * FROM  HG_ServingSize where ServingId=" + ID;
                cmd = new SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("@ServingId", ID);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    ObjTmp.ServingId = SDR.GetInt32(0);
                    ObjTmp.Name = SDR.GetString(1);
                    ObjTmp.OrgId = SDR.GetInt32(2);
                }
            }
            catch (Exception e)
            { e.ToString(); }

            finally { Con.Close(); }

            return (ObjTmp);
        }
    }
}