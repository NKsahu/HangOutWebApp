using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models
{
    public class TakeAway
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OrgId { get; set; }
        public int Type { get; set; } //{'0','new take away',1:'new delivery'}
        public TakeAway()
        {
            Id = 0;
            OrgId = 0;
            Type = 0;
        }
        public List<TakeAway> GetAll(int Type)
        {
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlDataReader SDR = null;
            List<TakeAway> ListTmp = new List<TakeAway>();
            System.Data.SqlClient.SqlConnection Con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            try
            {
                string Query = "SELECT * FROM TakeAway where Type=" + Type.ToString() + " ORDER BY Name ASC";
                cmd = new System.Data.SqlClient.SqlCommand(Query, Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    TakeAway ObjTmp = new TakeAway
                    {
                        Id = int.Parse(SDR["ID"].ToString()),
                        Name = SDR["Name"].ToString(),
                        OrgId = int.Parse(SDR["OrgID"].ToString()),
                        Type = int.Parse(SDR["Type"].ToString())
                    };
                    ListTmp.Add(ObjTmp);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); SDR.Close(); Con.Close(); Con.Dispose(); Con = null; }
            return (ListTmp);
        }

        public TakeAway GetOne(int id)
        {
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlDataReader SDR = null;
            TakeAway ObjTmp = new TakeAway();
            System.Data.SqlClient.SqlConnection Con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            try
            {
                string Query = "SELECT TOP 1 * FROM TakeAway where ID=" + id.ToString();
                cmd = new System.Data.SqlClient.SqlCommand(Query, Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {

                    Id = int.Parse(SDR["ID"].ToString());
                    Name = SDR["Name"].ToString();
                    OrgId = int.Parse(SDR["OrgID"].ToString());
                    Type = int.Parse(SDR["Type"].ToString());

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
                if (this.Id == 0)
                    Query = "TakeAway into City values (@Name,@OrgID,@Type); SELECT SCOPE_IDENTITY(); ";
                else
                    Query = "TakeAway   City set Name=@Name,OrgID=@OrgID,Type=@Type where ID=@ID";
                cmd = new SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("ID", this.Id);
                cmd.Parameters.AddWithValue("Name", this.Name);
                cmd.Parameters.AddWithValue("OrgID", this.OrgId);
                cmd.Parameters.AddWithValue("Type", this.Type);
                if (this.Id == 0)
                {
                    Row = System.Convert.ToInt32(cmd.ExecuteScalar());
                    this.Id = Row;
                }
                else if (cmd.ExecuteNonQuery() > 0)
                {
                    Row = this.Id;
                }

            }
            catch (Exception e) { e.ToString(); }
            finally { Con.Close(); }
            return Row;
        }

    }
}