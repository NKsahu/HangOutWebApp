using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models
{
    public class PaytmResn
    {

      //  [ID]
      //,[OID]
      //,[OIDKey]
      //,[TxtId]
      //,[TxtSts]
      //,[TxtDate]
      //,[CID]
      //,[PaytmResponse]

      public  int id { get; set; }
      public Int64 OID { get; set; }
        public string OIDkey { get; set; }
        public string TxnId { get; set; }
        public int TxnSts { get; set; }
        public DateTime TxtDate { get; set; }
        public Int64 CID { get; set; }// customer id
        public string PaytmResp { get; set; }
        public PaytmResn()
        {
            id = 0;
            TxtDate = DateTime.Now;

        }
        public int save()
        {
            DBCon con = new DBCon();
            SqlCommand cmd = new SqlCommand();
            try
            {
                if (this.id == 0)
                {
                    cmd = new SqlCommand("insert into PaytmTxn values(@OID,@OIDkey,@TxnId,@TxnSts,@TxtDate,@CID,@PaytmResp); SELECT SCOPE_IDENTITY();", con.Con);
                }
                else
                {
                    cmd = new SqlCommand("update PaytmTxn set TxnId=@TxnId,TxnSts=@TxnSts,TxtDate=@TxtDate,CID=@CID,PaytmResp=@PaytmResp, OIDkey=@OIDkey where ID=@ID ", con.Con);
                    cmd.Parameters.AddWithValue("@ID", this.id);
                }
                cmd.Parameters.AddWithValue("@OIDkey", this.OIDkey);
                cmd.Parameters.AddWithValue("@OID", this.OID);
                cmd.Parameters.AddWithValue("@TxnId", this.TxnId);
                cmd.Parameters.AddWithValue("@TxnSts", this.TxnSts);
                cmd.Parameters.AddWithValue("@TxtDate", this.TxtDate);
                cmd.Parameters.AddWithValue("@CID", this.CID);
                cmd.Parameters.AddWithValue("@PaytmResp", this.PaytmResp);
                if (this.id == 0)
                {
                    this.id = Convert.ToInt32(cmd.ExecuteScalar());
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
            return this.id;
        }
        public static List<PaytmResn> GetAll()
        {

            List<PaytmResn> listtemp = new List<PaytmResn>();
            DBCon con = new DBCon();
            SqlCommand cmd = new SqlCommand();
            string query = "select * from PaytmTxn";
            try
            {
                cmd = new SqlCommand(query, con.Con);
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    int index = 0;
                    PaytmResn hG_Ticket = new PaytmResn();
                    hG_Ticket.id = sqlDataReader.GetInt32(++index);
                    hG_Ticket.OID = sqlDataReader.GetInt32(++index);
                    hG_Ticket.OIDkey = sqlDataReader.GetString(++index);
                    hG_Ticket.TxnId = sqlDataReader.GetString(++index);
                    hG_Ticket.TxnSts = sqlDataReader.GetInt32(++index);
                    hG_Ticket.TxtDate = sqlDataReader.GetDateTime(++index);
                    hG_Ticket.CID = sqlDataReader.GetInt64(++index);
                    hG_Ticket.PaytmResp = sqlDataReader.GetString(++index);
                    listtemp.Add(hG_Ticket);
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

            return listtemp;
        }

        public static PaytmResn Getone(int id)
        {

            PaytmResn Temp = new PaytmResn();
            DBCon con = new DBCon();
            SqlCommand cmd = new SqlCommand();
            string query = "select * from PaytmTxn where ID=@ID";
            try
            {
                cmd = new SqlCommand(query, con.Con);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    int index = 0;
                    PaytmResn hG_Ticket = new PaytmResn();
                    hG_Ticket.id = sqlDataReader.GetInt32(++index);
                    hG_Ticket.OID = sqlDataReader.GetInt32(++index);
                    hG_Ticket.OIDkey = sqlDataReader.GetString(++index);
                    hG_Ticket.TxnId = sqlDataReader.GetString(++index);
                    hG_Ticket.TxnSts = sqlDataReader.GetInt32(++index);
                    hG_Ticket.TxtDate = sqlDataReader.GetDateTime(++index);
                    hG_Ticket.CID = sqlDataReader.GetInt64(++index);
                    hG_Ticket.PaytmResp = sqlDataReader.GetString(++index);
                    Temp=hG_Ticket;
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

    }
}