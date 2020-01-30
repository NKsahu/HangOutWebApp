﻿using System;
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
        public int Languange { get; set; }
        public int SerialNumber { get; set; }
        public bool IsImp { get; set; }

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
                    Query = "Insert into  Video  values( @CategoryId  ,@Title ,@Discription,@Link,@Languange,@SerialNumber,@IsImp); SELECT SCOPE_IDENTITY();";


                }
                else
                {
                    Query = "update  Video set   CategoryId =@CategoryId ,Title=@Title,Discription=@Discription,Link=@Link,Languange=@Languange,SerialNumber=@SerialNumber,IsImp=@IsImp where ID=@ID";

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
        public List<Video>GetAll()
        {
            DBCon OBJCon = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<Video> ListTmp = new List<Video>();

            try
            {
                string Query = "SELECT * FROM  Video ORDER BY  ID DESC";
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

                    ListTmp.Add(ObjTmp);
                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { cmd.Dispose(); ; OBJCon.Con.Close(); }

            return (ListTmp);
        }
        public Video GetOne(int ID)
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
                }
            }
            catch (System.Exception e)
            { e.ToString(); }

            finally {cmd.Dispose(); OBJCON.Con.Close(); }

            return (ObjTmp);
        }
    }
}