using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace HangOut.Models.Common
{
    public class TblData
    {
        public JObject Jobj { get; set; }

        public static JToken GetValues(SqlDataReader SDR, int Position)
        {
            var FType = SDR.GetFieldType(Position);
            try
            {
                if (FType == typeof(Int16))
                {
                    return SDR.GetInt16(Position);
                }
                else if (FType == typeof(Int32))
                {
                    return SDR.GetInt32(Position);
                }
                else if (FType == typeof(Int64))
                {
                    return SDR.GetInt32(Position);
                }
                else if (FType == typeof(Double))
                {
                    return SDR.GetDouble(Position);
                }
                else if (FType == typeof(DateTime))
                {
                    return SDR.GetDateTime(Position);
                }
                else if (FType == typeof(Boolean))
                {
                    return SDR.GetBoolean(Position);
                }
                else
                {
                    return SDR.GetString(Position);
                }

            }
            catch (Exception)
            {

                if (FType == typeof(String))
                {
                    return null;
                }
                else if (FType == typeof(Boolean))
                {
                    return false;
                }
                else
                {
                    return 0;
                }
            }
        }
        public T getVal<T>(string PropName)
        {

            var touple = this.Jobj[PropName].Value<T>();
            return touple;
        }
        public void SetValue(string PropName, dynamic value)
        {
            try
            {
                this.Jobj[PropName] = value;
            }
            catch (Exception e)
            {
                this.Jobj.Add(PropName, value);
            }


        }
        public static List<TblData> GetAll(string Query)
        {

            List<TblData> ListTep = new List<TblData>();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            DBCon conn = new DBCon();
            try
            {

                cmd = new SqlCommand(Query, conn.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    TblData formmdata = new TblData();
                    JObject jObject = new JObject();
                    for (int i = 0; i < SDR.FieldCount; i++)
                    {
                        string fieldName = SDR.GetName(i);
                        var test = SDR.GetValue(i);
                        jObject.Add(fieldName, GetValues(SDR, i));

                    }
                    formmdata.Jobj = jObject;
                    ListTep.Add(formmdata);


                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); SDR.Close(); conn.Close(); }
            return (ListTep);
        }

        public static List<TblData> GetAll(string PName, JObject Params)
        {
            List<TblData> ListTep = new List<TblData>();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            DBCon conn = new DBCon();
            try
            {

                cmd = new SqlCommand(PName, conn.Con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                IList<string> keys = Params.Properties().Select(p => p.Name).ToList();
                for (int i = 0; i < keys.Count; i++)
                {
                    cmd.Parameters.AddWithValue(keys[i], Params[keys[i]].ToString());
                }
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    TblData formmdata = new TblData();
                    JObject jObject = new JObject();
                    for (int i = 0; i < SDR.FieldCount; i++)
                    {
                        string fieldName = SDR.GetName(i);
                        var test = SDR.GetValue(i);
                        jObject.Add(fieldName, GetValues(SDR, i));

                    }
                    formmdata.Jobj = jObject;
                    ListTep.Add(formmdata);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); SDR.Close(); conn.Close(); }
            return (ListTep);


        }
    }
}