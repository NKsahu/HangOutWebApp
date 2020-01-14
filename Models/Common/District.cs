using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models.Common
{
    public class District
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public List<District> GetAllByStsCity(int StateId,int CityId)
        {
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlDataReader SDR = null;
            List<District> ListTmp = new List<District>();
            System.Data.SqlClient.SqlConnection Con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            try
            {
                string Query = "SELECT * FROM District where StateId=" + StateId.ToString() + " and CityId="+CityId.ToString();
                cmd = new System.Data.SqlClient.SqlCommand(Query, Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    District ObjTmp = new District
                    {
                        Id= int.Parse(SDR["Id"].ToString()),
                        Name = SDR["Name"].ToString(),
                        StateId = int.Parse(SDR["StateId"].ToString()),
                        CityId = int.Parse(SDR["CityId"].ToString()),
                    };
                    ListTmp.Add(ObjTmp);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); SDR.Close(); Con.Close(); Con.Dispose(); Con = null; }
            return (ListTmp);
        }

        public District GetOne(int Id)
        {
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlDataReader SDR = null;
            District ObjTmp = new District();
            System.Data.SqlClient.SqlConnection Con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            try
            {
                string Query = "SELECT TOP 1 * FROM District where Id=" + Id.ToString();
                cmd = new System.Data.SqlClient.SqlCommand(Query, Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {

                    ObjTmp.Id = int.Parse(SDR["Id"].ToString());
                    ObjTmp.Name = SDR["Name"].ToString();
                    ObjTmp.StateId = int.Parse(SDR["StateId"].ToString());
                    ObjTmp.CityId = int.Parse(SDR["CityId"].ToString());
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
                    Query = "insert into District values (@Name,@StateId,@CityId); SELECT SCOPE_IDENTITY(); ";
                else
                    Query = "Update   District set Name=@Name,StateId=@StateId ,CityId=@CityId, where Id=@Id";
                cmd = new SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("Id", this.Id);
                cmd.Parameters.AddWithValue("CityId", CityId);
                cmd.Parameters.AddWithValue("Name", Name);
                cmd.Parameters.AddWithValue("StateId", StateId);
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