using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models.Common
{
    public class VideoMark
    {
        public int ID { get; set; }
        public int VideoID { get; set; }
        public int CID { get; set; }
        public DateTime CreateDate { get; set; }
         public int Save()
        {
            int Row = 0;
            DBCon Con = new DBCon();
            SqlCommand cmd = null;
           
            try
            {
                string Quary = "";
                if(this.ID==0)
                {
                    Quary = "Insert Into VideoMark values(@VideoID,@CID,@CreateDate); SELECT SCOPE_IDENTITY();";
                }else
                {
                    Quary = "Update VideoMark Set VideoID=@VideoID,CID=@CID,CreateDate=@CreateDate Where ID=@ID";
                }
                cmd = new SqlCommand(Quary,Con.Con);
                cmd.Parameters.AddWithValue("@ID", this.ID);
                cmd.Parameters.AddWithValue("@VideoID", this.VideoID);
                cmd.Parameters.AddWithValue("@CID", this.CID);
                cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                if (this.ID == 0)
                {
                    Row = Convert.ToInt32(cmd.ExecuteScalar());
                    this.ID = Row;
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
        public static List<VideoMark> GetAll(int CID)
        {
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<VideoMark> listvideo = new List<VideoMark>();
            try 
            {
                string Query = "Select * From VideoMark where CID="+CID;
                cmd = new SqlCommand(Query, con.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    VideoMark ObjTmp = new VideoMark();
                    ObjTmp.VideoID = SDR.GetInt32(1);
                    ObjTmp.CID = SDR.GetInt32(2);
                    listvideo.Add(ObjTmp);
                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { cmd.Dispose(); ; con.Con.Close(); }

            return (listvideo);
        }
    }
}