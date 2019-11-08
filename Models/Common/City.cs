using System;
using System.Collections.Generic;


namespace HangOut.Models.Common
{
    public class City
    {
      public  int CityId { get; set; }
      public  string Name { get; set; }
     public   int StateId { get; set; }
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
                        StateId =int.Parse(SDR["StateId"].ToString())
                    };
                    ListTmp.Add(ObjTmp);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); SDR.Close(); Con.Close(); Con.Dispose(); Con = null; }
            return (ListTmp);
        }
    }
}