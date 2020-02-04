using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models.Common
{
    public class Video
    {
        public int ID { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Discription { get; set; }
        public string Link { get; set; }
        public int Languange { get; set; }// { 1: Hindi , 2: English ,3:Urdu
        public int SerialNumber { get; set; }
        public bool IsImp { get; set; }
        public bool Restaurant { get; set; }
        public bool Theater { get; set; }
        public bool Prepaid { get; set; }
        public bool Postpaid { get; set; }

        public int Save()
        {
            int Row = 0;
            DBCon OBJCon = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Query = "";
                if (this.ID == 0)
                {
                    Query = "Insert into  Video  values( @CategoryId  ,@Title ,@Discription,@Link,@Languange,@SerialNumber,@IsImp,@Restaurant,@Theater,@Prepaid,@Postpaid); SELECT SCOPE_IDENTITY();";


                }
                else
                {
                    Query = "update  Video set   CategoryId =@CategoryId ,Title=@Title,Discription=@Discription,Link=@Link,Languange=@Languange,SerialNumber=@SerialNumber,IsImp=@IsImp,Restaurant=@Restaurant,Theater=@Theater,Prepaid=@Prepaid,Postpaid=@Postpaid where ID=@ID";

                }
                cmd = new SqlCommand(Query, OBJCon.Con);
                cmd.Parameters.AddWithValue("@ID", this.ID);
                cmd.Parameters.AddWithValue("@CategoryId", this.CategoryId);
                cmd.Parameters.AddWithValue("@Title", this.Title);
                cmd.Parameters.AddWithValue("@Discription", this.Discription);
                cmd.Parameters.AddWithValue("@Link", this.Link);
                cmd.Parameters.AddWithValue("@Languange", this.Languange);
                cmd.Parameters.AddWithValue("@SerialNumber", this.SerialNumber);
                cmd.Parameters.AddWithValue("@IsImp", this.IsImp);
                cmd.Parameters.AddWithValue("@Restaurant", this.Restaurant);
                cmd.Parameters.AddWithValue("@Theater", this.Theater);
                cmd.Parameters.AddWithValue("@Prepaid", this.Prepaid);
                cmd.Parameters.AddWithValue("@Postpaid", this.Postpaid);
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
            finally { cmd.Dispose(); OBJCon.Con.Close(); }
            return Row;
        }
        public static List<Video>GetAll(int CatId)
        {
            DBCon OBJCon = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<Video> ListTmp = new List<Video>();

            try
            {
                string Query = "SELECT * FROM  Video where CategoryId="+ CatId;
                cmd = new SqlCommand(Query, OBJCon.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    Video ObjTmp = new Video();
                    ObjTmp. ID = SDR.GetInt32(0);
                    ObjTmp.CategoryId = SDR.GetInt32(1);
                    ObjTmp.Title = SDR.GetString(2);
                    ObjTmp.Discription = SDR.GetString(3);
                    ObjTmp.Link = SDR.GetString(4);
                    ObjTmp.Languange = SDR.GetInt32(5);
                    ObjTmp.SerialNumber = SDR.GetInt32(6);
                    ObjTmp.IsImp = SDR.GetBoolean(7);
                    ObjTmp.Restaurant = SDR.GetBoolean(8);
                    ObjTmp.Theater = SDR.GetBoolean(9);
                    ObjTmp.Prepaid = SDR.GetBoolean(10);
                    ObjTmp.Postpaid = SDR.GetBoolean(11);

                    ListTmp.Add(ObjTmp);
                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { cmd.Dispose(); ; OBJCon.Con.Close(); }

            return (ListTmp);
        }
        public static Video GetOne(int ID)
        {
            DBCon OBJCON = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            Video ObjTmp = new Video();

            try
            {
                string Query = "SELECT * FROM  Video where  ID=@ID";
                cmd = new SqlCommand(Query, OBJCON.Con);
                cmd.Parameters.AddWithValue("@ID",ID);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    ObjTmp.ID = SDR.GetInt32(0);
                    ObjTmp.CategoryId = SDR.GetInt32(1);
                    ObjTmp.Title = SDR.GetString(2);
                    ObjTmp.Discription = SDR.GetString(3);
                    ObjTmp.Link = SDR.GetString(4);
                    ObjTmp.Languange = SDR.GetInt32(5);
                    ObjTmp.SerialNumber = SDR.GetInt32(6);
                    ObjTmp.IsImp = SDR.GetBoolean(7);
                    ObjTmp.IsImp = SDR.GetBoolean(7);
                    ObjTmp.Restaurant = SDR.GetBoolean(8);
                    ObjTmp.Theater = SDR.GetBoolean(9);
                    ObjTmp.Prepaid = SDR.GetBoolean(10);
                    ObjTmp.Postpaid = SDR.GetBoolean(11);
                }
            }
            catch (System.Exception e)
            { e.ToString(); }

            finally {cmd.Dispose(); OBJCON.Con.Close(); }

            return (ObjTmp);
        }

        public static List<VideoLangu> Languages()
        {
            List<VideoLangu> videoLangus = new List<VideoLangu>();
            videoLangus.Add(new VideoLangu { id=1,Name="Hindi"});
            videoLangus.Add(new VideoLangu { id = 2, Name = "English" });
            videoLangus.Add(new VideoLangu { id = 3, Name = "Urdu" });
            return videoLangus;
        }
    }
}
public class VideoLangu
{
   public  int id { get; set; }
    public string Name { get; set; }
}