using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models.Common
{
    public class VideoCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Video> Videos { get; set; }
        public int Save()
        {
            int Row = 0;
            DBCon Con = new DBCon();
            SqlCommand cmd = null;
           
            try
            {
                string Quary = "";
                if(this.Id==0)
                {
                    Quary = "Insert Into VideoCategory values(@Name); SELECT SCOPE_IDENTITY();";
                }else
                {
                    Quary = "Update VideoCategory Set Name=@Name Where Id=@Id";
                }
                cmd = new SqlCommand(Quary,Con.Con);
                cmd.Parameters.AddWithValue("@Id", this.Id);
                cmd.Parameters.AddWithValue("@Name", this.Name);
                if (this.Id == 0)
                {
                    Row = Convert.ToInt32(cmd.ExecuteScalar());
                    this.Id = Row;
                }
                else
                {
                    Row = cmd.ExecuteNonQuery();
                    //this.CategoryID = Row;
                }

            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); Con.Con.Close(); }
            return Row;
        }
        public static List<VideoCategory>GetAll()
        {
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<VideoCategory> listvideo = new List<VideoCategory>();
            try 
            {
                string Query = "Select *From VideoCategory ORDER BY  Id DESC";
                cmd = new SqlCommand(Query, con.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    VideoCategory ObjTmp = new VideoCategory();
                    ObjTmp.Id = SDR.GetInt32(0);
                    ObjTmp.Name = SDR.GetString(1);
                    listvideo.Add(ObjTmp);
                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { cmd.Dispose(); ; con.Con.Close(); }

            return (listvideo);
        }
        public  VideoCategory  GetOne(int Id)
        {
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            VideoCategory listvideo = new VideoCategory();
            try
            {
                string Query = "SELECT * FROM  Video where  ID=@ID";
                cmd = new SqlCommand(Query, con.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    VideoCategory ObjTmp = new VideoCategory();
                    ObjTmp.Id = SDR.GetInt32(0);
                    ObjTmp.Name = SDR.GetString(1);
                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { cmd.Dispose(); ; con.Con.Close(); }

            return (listvideo);
        }
    }
}