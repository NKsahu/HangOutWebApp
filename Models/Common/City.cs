using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace HangOut.Models.Common
{
    public class City
    {
      public  int CityId { get; set; }
      public  string Name { get; set; }
     public   int StateId { get; set; }
        public int Type { get; set; }//{0 city/district  1 means tehsil/TALUKA 
        
        public List<City> GetAllByState(int StateId)
        {
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlDataReader SDR = null;
            List<City> ListTmp = new List<City>();
            System.Data.SqlClient.SqlConnection Con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            try
            {
                string Query = "SELECT * FROM City where StateId="+StateId.ToString()+" ORDER BY Name ASC";
                cmd = new System.Data.SqlClient.SqlCommand(Query, Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    City ObjTmp = new City
                    {
                        CityId = int.Parse(SDR["CityId"].ToString()),
                        Name = SDR["Name"].ToString(),
                        StateId =int.Parse(SDR["StateId"].ToString()),
                        Type = int.Parse(SDR["Type"].ToString())
                };
                    ListTmp.Add(ObjTmp);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); SDR.Close(); Con.Close(); Con.Dispose(); Con = null; }
            return (ListTmp);
        }

        public City GetOne(int Cityid)
        {
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlDataReader SDR = null;
            City ObjTmp = new City();
            System.Data.SqlClient.SqlConnection Con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            try
            {
                string Query = "SELECT TOP 1 * FROM City where CityId=" + Cityid.ToString();
                cmd = new System.Data.SqlClient.SqlCommand(Query, Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {

                 ObjTmp.CityId = int.Parse(SDR["CityId"].ToString());
                 ObjTmp.Name = SDR["Name"].ToString();
                 ObjTmp.StateId = int.Parse(SDR["StateId"].ToString());
                    ObjTmp.Type = int.Parse(SDR["Type"].ToString());
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); SDR.Close(); Con.Close(); Con.Dispose(); Con = null; }
            return (ObjTmp);
        }
        public int save()
        {
            int Row = 0;
            System.Data.SqlClient.SqlConnection Con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            try
            {
                Con.Open();
                SqlCommand cmd = null;
                string Query = "";
                if (this.CityId == 0)
                    Query = "insert into City values (@Name,@StateId,@Type); SELECT SCOPE_IDENTITY(); ";
                else
                    Query = "Update   City set Name=@Name,StateId=@StateId,Type=@Type where CityId=@CityId";
                cmd = new SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("CityId", CityId);
                cmd.Parameters.AddWithValue("Name", Name);
                cmd.Parameters.AddWithValue("StateId", StateId);
                cmd.Parameters.AddWithValue("@Type", Type);
                if (this.CityId == 0) { 
                    Row = System.Convert.ToInt32(cmd.ExecuteScalar());
                this.CityId = Row;
                }
                else if (cmd.ExecuteNonQuery() > 0)
                {
                    Row = this.CityId;
                }

            }
            catch (Exception e) { e.ToString(); }
            finally { Con.Close(); }
            return Row;
        }
        public static int Dell(int ID)
        {
            int R = 0;
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            try
            {
                string Query = "Delete FROM  City where CityId=" + ID;
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