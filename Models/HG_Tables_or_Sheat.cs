﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HangOut.Models
{
    public class HG_Tables_or_Sheat
    {
        public Int64 Table_or_RowID { get; set; }
        public int  OrgId{ get; set; }
        [Display(Name ="TableOrSheatName")]
        public string Table_or_SheetName { get; set; }
        public int Floor_or_ScreenId { get; set; }
        public int  FloorSide_or_RowNoID{ get; set; }
        public string Type { get; set; } // "1" means restauarant "2" means theater
        public  DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }

        public Int64 save()
        {
            Int64 Row = 0;
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            try
            {
                Con.Open();
                SqlCommand cmd = null;
                string Query = "";
                if (this.Table_or_RowID == 0)
                    Query = "Insert Into HG_Tables_or_Sheat values(@OrgId,@Table_or_SheetName,@Floor_or_ScreenId,@FloorSide_or_RowNoID,@Type,@CreateDate,@CreateBy);SELECT SCOPE_IDENTITY();";
                else
                    Query = "Update HG_Tables_or_Sheat  set OrgId=@OrgId,Table_or_SheetName =@Table_or_SheetName,Floor_or_ScreenId =@Floor_or_ScreenId,FloorSide_or_RowNoID=@FloorSide_or_RowNoID,Type=@Type,CreateDate=@CreateDate,CreateBy=@CreateBy Where Table_or_RowID=@Table_or_RowID;";
                cmd = new SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("@Table_or_RowID", this.Table_or_RowID);
                cmd.Parameters.AddWithValue("@OrgId", this. OrgId);
                cmd.Parameters.AddWithValue("@Table_or_SheetName", this. Table_or_SheetName);
                cmd.Parameters.AddWithValue("@Floor_or_ScreenId", this.Floor_or_ScreenId);
                cmd.Parameters.AddWithValue("@FloorSide_or_RowNoID", this.FloorSide_or_RowNoID);
                cmd.Parameters.AddWithValue("@Type", this.Type);
                cmd.Parameters.AddWithValue("@CreateDate",DateTime.Now);
                cmd.Parameters.AddWithValue("@CreateBy", this.CreateBy);
                if (this.Table_or_RowID == 0)
                {
                    Row = System.Convert.ToInt64(cmd.ExecuteScalar());
                    this.Table_or_RowID = Row;
                }
                else if (cmd.ExecuteNonQuery() > 0)
                {
                    Row = this.Table_or_RowID;
                }

            }
            catch (Exception e) { e.ToString(); }
            finally { Con.Close(); }
            return Row;
        }
        public List<HG_Tables_or_Sheat> GetAll(int Type)
        {

            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR  = null;
            List<HG_Tables_or_Sheat> listTemp = new List<HG_Tables_or_Sheat>();
            string Query = "SELECT * FROM HG_Tables_or_Sheat where Type=" + Type.ToString()+" ORDER BY Table_or_RowID DESC";
            try
            {
                cmd = new SqlCommand(Query,Con);
                SDR  = cmd.ExecuteReader();
                while(SDR .Read())
                {
                    HG_Tables_or_Sheat ObjTemp = new HG_Tables_or_Sheat();
                    ObjTemp.Table_or_RowID = SDR .GetInt64(0);
                    ObjTemp.OrgId = SDR .GetInt32(1);
                    ObjTemp.Table_or_SheetName = SDR .GetString(2);
                    ObjTemp.Floor_or_ScreenId = SDR .GetInt32(3);
                    ObjTemp.FloorSide_or_RowNoID = SDR .GetInt32(4);
                    ObjTemp.Type = SDR.GetString(5);
                    ObjTemp.CreateDate = SDR .GetDateTime(6);
                    ObjTemp.CreateBy = SDR .GetInt32(7);
                    listTemp.Add(ObjTemp);
                }

            }
            catch (System.Exception e) { e.ToString(); }
            finally { Con.Close(); }

            return (listTemp);
        }
        public HG_Tables_or_Sheat GetOne(Int64 Table_or_RowID)
        {
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            HG_Tables_or_Sheat ObjTemp = new HG_Tables_or_Sheat();

            try
            {
                string Query = "SELECT * FROM  HG_Tables_or_Sheat where Table_or_RowID=@Table_or_RowID";
                cmd = new SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("@Table_or_RowID", Table_or_RowID);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    ObjTemp.Table_or_RowID = SDR.GetInt64(0);
                    ObjTemp.OrgId = SDR.GetInt32(1);
                    ObjTemp.Table_or_SheetName = SDR.GetString(2);
                    ObjTemp.Floor_or_ScreenId = SDR.GetInt32(3);
                    ObjTemp.FloorSide_or_RowNoID = SDR.GetInt32(4);
                    ObjTemp.Type = SDR.GetString(5);
                    ObjTemp.CreateDate = SDR.GetDateTime(6);
                    ObjTemp.CreateBy = SDR.GetInt32(7);
                    
                }
            }
            catch (System.Exception e)
            { e.ToString(); }

            finally { Con.Close(); }

            return (ObjTemp);
        }
        public List<HG_Tables_or_Sheat> GetAllByOID(int Orgid)
        {
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<HG_Tables_or_Sheat> TempList = new List<HG_Tables_or_Sheat>();
            try
            {
                string Query = "SELECT * FROM  HG_Tables_or_Sheat where OrgId=@OrgId";
                cmd = new SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("@OrgId", OrgId);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    HG_Tables_or_Sheat ObjTemp = new HG_Tables_or_Sheat();
                    ObjTemp.Table_or_RowID = SDR.GetInt64(0);
                    ObjTemp.OrgId = SDR.GetInt32(1);
                    ObjTemp.Table_or_SheetName = SDR.GetString(2);
                    ObjTemp.Floor_or_ScreenId = SDR.GetInt32(3);
                    ObjTemp.FloorSide_or_RowNoID = SDR.GetInt32(4);
                    ObjTemp.Type = SDR.GetString(5);
                    ObjTemp.CreateDate = SDR.GetDateTime(6);
                    ObjTemp.CreateBy = SDR.GetInt32(7);
                    TempList.Add(ObjTemp);
                }
            }
            catch (System.Exception e)
            { e.ToString(); }

            finally { Con.Close(); }

            return (TempList);
        }

        public int Dell(int ID)
        {
            int R = 0;
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            try
            {
                string Query = "Delete FROM  HG_Tables_or_Sheat where Table_or_RowID =" + ID;
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