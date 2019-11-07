using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HangOut.Models.Common
{
    public class State
    {
        public int StateId { get; set; }
        public string Name { get; set; }
    

    public List<State> GetAll()
    {
        System.Data.SqlClient.SqlCommand cmd = null;
        System.Data.SqlClient.SqlDataReader SDR = null;
        List<State> ListTmp = new List<State>();
        System.Data.SqlClient.SqlConnection Con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
        Con.Open();
        try
        {
            string Query = "SELECT * FROM State ORDER BY Name ASC";
            cmd = new System.Data.SqlClient.SqlCommand(Query, Con);
            SDR = cmd.ExecuteReader();
            while (SDR.Read())
            {
                    State ObjTmp   = new State
                {
                    StateId = int.Parse(SDR["StateId"].ToString()),
                    Name = SDR["Name"].ToString(),
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