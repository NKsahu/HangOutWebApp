using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using HangOut.Models.Common;
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
        public string Type { get; set; } // "1" means Table "2" Sheat //3 Take Away
        public  DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public int Status { get; set; }// {"1":free,"2":"BOOKED call for bill",3:"PROGRESS"}
        public int Otp { get; set; }
        public int OMID { get; set; } //order menu id
        public string QrCode { get; set; } //QrCode
        public HG_Tables_or_Sheat()
        {
            Status = 1;
            OMID = 0;
            CreateDate = DateTime.Now;
            QrCode = "";
        }
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
                {
                    Query = "Insert Into HG_Tables_or_Sheat values(@OrgId,@Table_or_SheetName,@Floor_or_ScreenId,@FloorSide_or_RowNoID,@Type,@CreateDate,@CreateBy,@Status,@Otp,@OMID,@QrCode);SELECT SCOPE_IDENTITY();";
                    this.Otp = OTPGeneretion.Generate();
                }
                else
                Query = "Update HG_Tables_or_Sheat  set OrgId=@OrgId,Table_or_SheetName =@Table_or_SheetName,Floor_or_ScreenId =@Floor_or_ScreenId,FloorSide_or_RowNoID=@FloorSide_or_RowNoID,Type=@Type,CreateDate=@CreateDate,CreateBy=@CreateBy,Status=@Status,Otp=@Otp,OMID=@OMID,QrCode=@QrCode Where Table_or_RowID=@Table_or_RowID;";
                cmd = new SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("@Table_or_RowID", this.Table_or_RowID);
                cmd.Parameters.AddWithValue("@OrgId", this. OrgId);
                cmd.Parameters.AddWithValue("@Table_or_SheetName", this. Table_or_SheetName);
                cmd.Parameters.AddWithValue("@Floor_or_ScreenId", this.Floor_or_ScreenId);
                cmd.Parameters.AddWithValue("@FloorSide_or_RowNoID", this.FloorSide_or_RowNoID);
                cmd.Parameters.AddWithValue("@Type", this.Type);
                cmd.Parameters.AddWithValue("@CreateDate",this.CreateDate);
                cmd.Parameters.AddWithValue("@CreateBy", this.CreateBy);
                cmd.Parameters.AddWithValue("@Status", this.Status);
                cmd.Parameters.AddWithValue("@Otp", this.Otp);
                cmd.Parameters.AddWithValue("@OMID", this.OMID);
                cmd.Parameters.AddWithValue("@QrCode", this.QrCode);
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
        public List<HG_Tables_or_Sheat> GetAll(int Type,int OrgId=0)
        {
            var CurrOrgID = HttpContext.Current.Request.Cookies["UserInfo"];
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR  = null;
            List<HG_Tables_or_Sheat> listTemp = new List<HG_Tables_or_Sheat>();
            string Query = "SELECT * FROM HG_Tables_or_Sheat where  Type=" + Type.ToString()+" ORDER BY Table_or_RowID DESC";
            if (OrgId > 0)
            {
                Query = "SELECT * FROM HG_Tables_or_Sheat where OrgId=" + OrgId.ToString()+ " and Type=" + Type.ToString() + " ORDER BY Table_or_RowID DESC";
            }
            else if (CurrOrgID != null && int.Parse(CurrOrgID["OrgId"]) > 0)
            {
                Query = "SELECT * FROM HG_Tables_or_Sheat where OrgId="+CurrOrgID["OrgId"]+" and Type=" + Type.ToString() + " ORDER BY Table_or_RowID DESC";
            }
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
                    ObjTemp.Status = SDR.IsDBNull(8) ? 1: SDR.GetInt32(8);
                    ObjTemp.Otp = SDR.IsDBNull(9) ? 1000 : SDR.GetInt32(9);
                    ObjTemp.OMID = SDR.IsDBNull(10) ? 0 : SDR.GetInt32(10);
                    ObjTemp.QrCode = SDR.IsDBNull(11) ? "0": SDR.GetString(11);
                    listTemp.Add(ObjTemp);
                }

            }
            catch (System.Exception e) { e.ToString(); }
            finally { Con.Close(); }

            return (listTemp);
        }
        public HG_Tables_or_Sheat GetOne(Int64 Table_or_RowID=0,string QrOcde="")
        {
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            HG_Tables_or_Sheat ObjTemp = new HG_Tables_or_Sheat();

            try
            {
                string Query = "";
                if (Table_or_RowID > 0)
                {
                    Query = "SELECT * FROM  HG_Tables_or_Sheat where Table_or_RowID="+Table_or_RowID.ToString();
                }
                else
                {
                    Query = "SELECT * FROM  HG_Tables_or_Sheat where QrCode='" + QrOcde+"'";
                }
                cmd = new SqlCommand(Query, Con);
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
                    ObjTemp.Status = SDR.IsDBNull(8) ? 1: SDR.GetInt32(8);
                    ObjTemp.Otp = SDR.IsDBNull(9) ? 1000 : SDR.GetInt32(9);
                    ObjTemp.OMID = SDR.IsDBNull(10) ? 0 : SDR.GetInt32(10);
                    ObjTemp.QrCode = SDR.IsDBNull(11) ? "0" : SDR.GetString(11);


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
                    ObjTemp.Status = SDR.IsDBNull(8) ? 1 : SDR.GetInt32(8);
                    ObjTemp.Otp = SDR.IsDBNull(9) ? 1000 : SDR.GetInt32(9);
                    ObjTemp.OMID = SDR.IsDBNull(10) ? 0 : SDR.GetInt32(10);
                    ObjTemp.QrCode = SDR.IsDBNull(11) ? "0" : SDR.GetString(11);
                    TempList.Add(ObjTemp);
                }
            }
            catch (System.Exception e)
            { e.ToString(); }

            finally { Con.Close(); }

            return (TempList);
        }

        public static int Dell(Int64 ID)
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
        public List<HG_Tables_or_Sheat> GetAllWithTakeAwya(int Type, int OrgId = 0)
        {
            var CurrOrgID = HttpContext.Current.Request.Cookies["UserInfo"];
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<HG_Tables_or_Sheat> listTemp = new List<HG_Tables_or_Sheat>();
            string Query = "SELECT * FROM HG_Tables_or_Sheat  ORDER BY Table_or_RowID DESC";
            if (OrgId > 0)
            {
                Query = "SELECT * FROM HG_Tables_or_Sheat where OrgId=" + OrgId.ToString() + " and ( Type=" + Type.ToString() + " or Type=3 ) ORDER BY Table_or_RowID DESC";
            }
            else if (CurrOrgID != null && int.Parse(CurrOrgID["OrgId"]) > 0)
            {
                Query = "SELECT * FROM HG_Tables_or_Sheat where OrgId=" + CurrOrgID["OrgId"] + " and ( Type=" + Type.ToString() + " or Type=3) ORDER BY Table_or_RowID DESC";
            }
            try
            {
                cmd = new SqlCommand(Query, Con);
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
                    ObjTemp.Status = SDR.IsDBNull(8) ? 1 : SDR.GetInt32(8);
                    ObjTemp.Otp = SDR.IsDBNull(9) ? 1000 : SDR.GetInt32(9);
                    ObjTemp.OMID = SDR.IsDBNull(10) ? 0 : SDR.GetInt32(10);
                    ObjTemp.QrCode = SDR.IsDBNull(11) ? "0" : SDR.GetString(11);
                    listTemp.Add(ObjTemp);
                }

            }
            catch (System.Exception e) { e.ToString(); }
            finally { Con.Close(); }

            return (listTemp);
        }

        public static string Seating(Int64 SeatId)
        {
            HG_Tables_or_Sheat Objseating = new HG_Tables_or_Sheat().GetOne(SeatId);
            string Seating = "";
            if (Objseating.Table_or_RowID > 0)
            {
                HG_Floor_or_ScreenMaster floor_Or_ScreenMaster = new HG_Floor_or_ScreenMaster().GetOne(Objseating.Floor_or_ScreenId);
                if (floor_Or_ScreenMaster.Floor_or_ScreenID > 0)
                {
                    Seating = floor_Or_ScreenMaster.Name + " ";
                }
                HG_FloorSide_or_RowName side_Or_RowName = new HG_FloorSide_or_RowName().GetOne(Objseating.FloorSide_or_RowNoID);
                if (side_Or_RowName.ID > 0)
                {
                    Seating += side_Or_RowName.FloorSide_or_RowName +" ";
                }
                Seating += Objseating.Table_or_SheetName;
            }
            return Seating;
        }
    }
}