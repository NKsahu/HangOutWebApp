using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models
{
    public class HG_FloorSide_or_RowName
    {
        public int  ID { get; set; }
        public string FloorSide_or_RowName { get; set; }
        public int RowSize { get; set; }
        public string Type { get; set; }
        public int OrgID { get; set; }
        

        public int save()
        {
            int Row = 0;
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            try
            {
                Con.Open();
                SqlCommand cmd = null;
                string Query = "";
                if (this. ID == 0)

                    Query = "Insert into  HG_FloorSide_or_RowName  values(@FloorSide_or_RowName,@RowSize,@Type,@OrgID); SELECT SCOPE_IDENTITY();";

                else

                    Query = "update  HG_FloorSide_or_RowName set  FloorSide_or_RowName=@FloorSide_or_RowName,RowSize=@RowSize,Type=@Type,OrgID=@OrgID where ID=@ID";

                cmd = new SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("@Floor_or_ScreenID", this. ID);
                cmd.Parameters.AddWithValue("@FloorSide_or_RowName", this.FloorSide_or_RowName);
                cmd.Parameters.AddWithValue("@RowSize", this.RowSize);
                cmd.Parameters.AddWithValue("@Type", this.Type);
                cmd.Parameters.AddWithValue("@OrgID", this.OrgID);
                if (this. ID == 0)
                    Row = System.Convert.ToInt32(cmd.ExecuteScalar());
                else if (cmd.ExecuteNonQuery() > 0)
                {
                    Row = this. ID;
                }

            }
            catch (Exception e) { e.ToString(); }
            finally { Con.Close(); }
            return Row;
        }
        public List<HG_FloorSide_or_RowName> GetAll()
        {
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<HG_FloorSide_or_RowName> ListTmp = new List<HG_FloorSide_or_RowName>();

            try
            {
                string Query = "SELECT * FROM  HG_FloorSide_or_RowName ORDER BY Floor_or_ScreenID DESC";
                cmd = new SqlCommand(Query, Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    HG_FloorSide_or_RowName ObjTmp = new HG_FloorSide_or_RowName();
                    ObjTmp. ID = SDR.GetInt32(0);
                    ObjTmp.FloorSide_or_RowName = SDR.GetString(1);
                    ObjTmp.RowSize = SDR.GetInt32(2);
                    ObjTmp.Type = SDR.GetString(3);
                    ObjTmp.OrgID = SDR.GetInt32(4);
                    


                    ListTmp.Add(ObjTmp);
                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { Con.Close(); }

            return (ListTmp);
        }
        public HG_FloorSide_or_RowName GetOne(int Floor_or_ScreenID)
        {
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            HG_FloorSide_or_RowName ObjTmp = new HG_FloorSide_or_RowName();

            try
            {
                string Query = "SELECT * FROM  HG_FloorSide_or_RowName where RantalID=@RantalID";
                cmd = new SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("@Floor_or_ScreenID", Floor_or_ScreenID);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    ObjTmp.ID = SDR.GetInt32(0);
                    ObjTmp.FloorSide_or_RowName = SDR.GetString(1);
                    ObjTmp.RowSize = SDR.GetInt32(2);
                    ObjTmp.Type = SDR.GetString(3);
                    ObjTmp.OrgID = SDR.GetInt32(4);
                }
            }
            catch (System.Exception e)
            { e.ToString(); }

            finally { Con.Close(); }

            return (ObjTmp);
        }

    }
}