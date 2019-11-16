using HangOut.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models.Common
{
    public class Settings
    {
        public int SettingId { get; set; }
        public string KeyName { get; set; }
        public string KeyValue { get; set; }
        public string KeyDiscription { get; set; }
        public System.Int64 Save()
        {
            Int64 Row = 0;
            System.Data.SqlClient.SqlCommand cmd = null;
            DBCon Obj = new DBCon();
            try
            {
                if (this.SettingId == 0)
                    cmd = new System.Data.SqlClient.SqlCommand("INSERT INTO Settings (KeyName,KeyValue,KeyDiscription) VALUES (@KeyName,@KeyValue,@KeyDiscription);select SCOPE_IDENTITY();", Obj.Con);
                else
                {
                    cmd = new System.Data.SqlClient.SqlCommand("UPDATE Settings SET KeyName=@KeyName,KeyValue=@KeyValue,KeyDiscription=@KeyDiscription where SettingId=@SettingId", Obj.Con);
                    cmd.Parameters.AddWithValue("@SettingId", this.SettingId);
                }

                cmd.Parameters.AddWithValue("@KeyName", this.KeyName);
                cmd.Parameters.AddWithValue("@KeyValue", this.KeyValue);
                cmd.Parameters.AddWithValue("@KeyDiscription", this.KeyDiscription);

                if (this.SettingId == 0)
                {
                    this.SettingId = System.Convert.ToInt32(cmd.ExecuteScalar());
                    if (this.SettingId > 0)
                        Row = this.SettingId;
                }
                else
                {
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        Row = this.SettingId;
                    }
                }
            }
            catch (System.Exception e) { Row = 0; e.ToString(); }
            finally { cmd.Dispose(); Obj.Con.Close(); Obj.Con.Dispose(); Obj.Con = null; }
            return Row;
        }

        public List<Settings> GetAll()
        {
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlDataReader SDR = null;
            System.Collections.Generic.List<Settings> ListTmp = new System.Collections.Generic.List<Settings>();
            Settings ObjTmp = null;
            DBCon Obj = new DBCon();
            try
            {
                string Query = "SELECT * FROM Settings ";
                cmd = new System.Data.SqlClient.SqlCommand(Query, Obj.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    ObjTmp = new Settings
                    {
                        SettingId = SDR.GetInt32(0),
                        KeyName = SDR.GetString(1),
                        KeyValue = SDR.GetString(2),
                        KeyDiscription = SDR.GetString(3),
                    };
                    ListTmp.Add(ObjTmp);
                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { cmd.Dispose(); SDR.Close(); Obj.Con.Close(); Obj.Con.Dispose(); Obj.Con = null; }
            return (ListTmp);
        }
        public  Settings GetOne( string keyname)
        {
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            Settings ObjTemp = new Settings();
            try
            {
                string Query = "SELECT * FROM  Settings where KeyName="+keyname;
                cmd = new SqlCommand(Query, Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    ObjTemp.SettingId = SDR.GetInt32(0);
                    ObjTemp.KeyName = SDR.GetString(1);
                    ObjTemp.KeyValue = SDR.GetString(2);
                    ObjTemp.KeyDiscription = SDR.GetString(3);
                    
                }
            }
            catch (System.Exception e)
            { e.ToString(); }

            finally { Con.Close(); }

            return (ObjTemp);

        }
        public Settings GetOne(int SettingId)
        {
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            Settings ObjTemp = new Settings();
            try
            {
                string Query = "SELECT * FROM  Settings where KeyName=" + keyname;
                cmd = new SqlCommand(Query, Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    ObjTemp.SettingId = SDR.GetInt32(0);
                    ObjTemp.KeyName = SDR.GetString(1);
                    ObjTemp.KeyValue = SDR.GetString(2);
                    ObjTemp.KeyDiscription = SDR.GetString(3);

                }
            }
            catch (System.Exception e)
            { e.ToString(); }

            finally { Con.Close(); }

            return (ObjTemp);

        }
    }
}