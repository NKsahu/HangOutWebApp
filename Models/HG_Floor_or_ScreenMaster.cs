using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace HangOut.Models
{
    public class HG_Floor_or_ScreenMaster
    {
        public int Floor_or_ScreenID { get; set; }
        public string Name { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatonDate { get; set; }
        public int UpdatedBy{ get; set; }
        public int UpdateionDate { get; set; }
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
                if (this.Floor_or_ScreenID ==0)
                
                    Query = "Insert into  HG_Floor_or_ScreenMaster  values(@Name,@CreatedBy,@CreationDate,@UpdatedBy,@UpdateionDate,@Type,@OrgID); SELECT SCOPE_IDENTITY();";
                    
                 else
                 
                  Query = "update  HG_Floor_or_ScreenMaster set  Name=@Name,CreatedBy=@CreatedBy,CreationDate=@CreationDate,UpdatedBy=@UpdatedBy,UpdateionDate=@UpdateionDate,Type=@Type,OrgID=@OrgID where Floor_or_ScreenID=@Floor_or_ScreenID";
                 
                cmd = new SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("@Floor_or_ScreenID", this.Floor_or_ScreenID);
                cmd.Parameters.AddWithValue("@Name", this.Name);
                cmd.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
                cmd.Parameters.AddWithValue("@CreationDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@UpdatedBy", this.UpdatedBy);
                cmd.Parameters.AddWithValue("@UpdateionDate", this.UpdateionDate);
                cmd.Parameters.AddWithValue("@Type", this.Type);
                cmd.Parameters.AddWithValue("@OrgID", this.OrgID);
                 if(this.Floor_or_ScreenID ==0)
                Row =System.Convert.ToInt32(cmd.ExecuteScalar()) ;
                 else if(cmd.ExecuteNonQuery()>0)
                {
                    Row = this.Floor_or_ScreenID;
                }

            }
             catch (Exception e) { e.ToString(); }
            finally { Con.Close(); }
            return Row;
        }
        public List<HG_Floor_or_ScreenMaster>GetAll(int Type)
        {
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<HG_Floor_or_ScreenMaster> ListTmp = new List<HG_Floor_or_ScreenMaster>();

            try
            {
                string Query = "SELECT * FROM  HG_Floor_or_ScreenMaster where Type="+Type.ToString()+" ORDER BY Floor_or_ScreenID DESC";
                cmd = new SqlCommand(Query, Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    HG_Floor_or_ScreenMaster ObjTmp = new HG_Floor_or_ScreenMaster();
                    ObjTmp.Floor_or_ScreenID = SDR.GetInt32(0);
                    ObjTmp.Name = SDR.GetString(1);
                    ObjTmp.CreatedBy = SDR.GetInt32(2);
                    ObjTmp.CreatonDate = SDR.GetDateTime(3);
                    ObjTmp.UpdatedBy = SDR.GetInt32(4);
                    ObjTmp.UpdateionDate = SDR.GetInt32(5);
                    ObjTmp.Type = SDR.GetString(6);
                    ObjTmp.OrgID = SDR.GetInt32(7);
                    

                    ListTmp.Add(ObjTmp);
                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { Con.Close(); }

            return (ListTmp);
        }
        public HG_Floor_or_ScreenMaster GetOne(int Floor_or_ScreenID)
        {
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            HG_Floor_or_ScreenMaster ObjTmp = new HG_Floor_or_ScreenMaster();

            try
            {
                string Query = "SELECT * FROM  HG_Floor_or_ScreenMaster where Floor_or_ScreenID=@Floor_or_ScreenID";
                cmd = new SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("@Floor_or_ScreenID", Floor_or_ScreenID);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    ObjTmp.Floor_or_ScreenID = SDR.GetInt32(0);
                    ObjTmp.Name = SDR.GetString(1);
                    ObjTmp.CreatedBy = SDR.GetInt32(2);
                    ObjTmp.CreatonDate = SDR.GetDateTime(3);
                    ObjTmp.UpdatedBy = SDR.GetInt32(4);
                    ObjTmp.UpdateionDate = SDR.GetInt32(5);
                    ObjTmp.Type = SDR.GetString(6);
                    ObjTmp.OrgID = SDR.GetInt32(7);
                }
            }
            catch (System.Exception e)
            { e.ToString(); }

            finally { Con.Close(); }

            return (ObjTmp);
        }

    }


}