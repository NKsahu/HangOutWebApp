using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models.Common
{
    public class OrdNotice
    {
        /****** Script for SelectTopNRows command from SSMS  ******/
      //  SELECT TOP(1000) [id]
      //,[OID]
      //,[Status]
      //  FROM[Hangout].[dbo].[OrdNotifictin]
      public int ID { get; set; }
        public Int64 OID { get; set; }
        public int Status { get; set; }//0 unseen , 1 seen
        public int Type { get; set; }// 0 means Payment done By App-ByCash ,1-All order done by chef,
        public int CID { get; set; }
        public int Orgid { get; set; }
        public int save()
        {
            DBCon con = new DBCon();
            SqlCommand cmd = new SqlCommand();
            try
            {
                if (this.ID == 0)
                {
                    cmd = new SqlCommand("insert into OrdNotifictin values(@OID,@Status,@Type,@CID,@Orgid); SELECT SCOPE_IDENTITY();", con.Con);
                   cmd.Parameters.AddWithValue("@Orgid", this.Orgid);
                }
                else
                {
                    cmd = new SqlCommand("update OrdNotifictin set OID=@OID,Status=@Status,Type=@Type,CID=@CID where ID=@ID ", con.Con);
                    cmd.Parameters.AddWithValue("@id", this.ID);
                }
                cmd.Parameters.AddWithValue("@OID", this.OID);
                cmd.Parameters.AddWithValue("@Status", this.Status);
                cmd.Parameters.AddWithValue("@Type", this.Type);
                cmd.Parameters.AddWithValue("@CID", this.CID);
                if (this.ID == 0)
                {
                    this.ID = Convert.ToInt32(cmd.ExecuteScalar());
                }
                else
                {
                    int i = Convert.ToInt32(cmd.ExecuteNonQuery());
                }

            }
            catch (Exception e)
            {
                e.Message.ToString();
                return 0;
            }
            finally
            {
                cmd.Dispose(); con.Con.Close();
            }
            return this.ID;
        }
        public static List<OrdNotice> GetAll(int Type)
        {

            List<OrdNotice> Temp = new List<OrdNotice>();
            DBCon con = new DBCon();
            SqlCommand cmd = new SqlCommand();
            string query = "select * from OrdNotifictin where Type=@Type and Status=0";
            try
            {
                cmd = new SqlCommand(query, con.Con);
                cmd.Parameters.AddWithValue("@Type", Type);
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    int index = -1;
                    OrdNotice hG_Ticket = new OrdNotice();
                    hG_Ticket.ID = sqlDataReader.GetInt32(++index);
                    hG_Ticket.OID = sqlDataReader.GetInt64(++index);
                    hG_Ticket.Status= sqlDataReader.GetInt32(++index);
                    hG_Ticket.Type = sqlDataReader.GetInt32(++index);
                    hG_Ticket.CID = sqlDataReader.GetInt32(++index);
                    hG_Ticket.Orgid = sqlDataReader.GetInt32(++index);
                    Temp.Add(hG_Ticket);
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                con.Con.Close(); con.Con.Dispose(); cmd.Dispose();
            }

            return Temp;
        }
        public static OrdNotice GetOne(Int64 OID)
        {
               OrdNotice Temp = new OrdNotice();
                DBCon con = new DBCon();
                SqlCommand cmd = new SqlCommand();
                string query = "select * from OrdNotifictin where OID=@OID";
                try
                {
                    cmd = new SqlCommand(query, con.Con);
                    cmd.Parameters.AddWithValue("@OID", OID);
                    SqlDataReader sqlDataReader = cmd.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        int index = -1;
                        OrdNotice hG_Ticket = new OrdNotice();
                        hG_Ticket.ID = sqlDataReader.GetInt32(++index);
                        hG_Ticket.OID = sqlDataReader.GetInt64(++index);
                        hG_Ticket.Status = sqlDataReader.GetInt32(++index);
                        hG_Ticket.Type = sqlDataReader.GetInt32(++index);
                        hG_Ticket.CID = sqlDataReader.GetInt32(++index);
                        hG_Ticket.Orgid = sqlDataReader.GetInt32(++index);
                    Temp = hG_Ticket;
                }

                }
                catch (Exception e)
                {
                    e.ToString();
                }
                finally
                {
                    con.Con.Close(); con.Con.Dispose(); cmd.Dispose();
                }

                return Temp;
           
        }
        public static void ChangeAlertSts(Int64 OID, int Status, int Type)
        {
            OrdNotice obj = OrdNotice.GetOne(OID);
            if (obj!=null&&obj.ID > 0)
            {
                obj.Status = Status;
                obj.Type = Type;

                obj.save();
            }

        }
    }
    
}