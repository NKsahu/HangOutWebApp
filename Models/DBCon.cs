using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HangOut.Models
{
    public class DBCon
    {
        public System.Data.SqlClient.SqlConnection Con;
        public DBCon()
        {
            Con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
        }
        public void Close()
        {
            if(this.Con.State== System.Data.ConnectionState.Open)
            {
                this.Con.Close();
            }
        }
    }
}