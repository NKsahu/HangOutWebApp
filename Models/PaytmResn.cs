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

      public  int id { get; set; } // 0 for insert id
      public Int64 OID { get; set; } // oid primary key
        public string OIDkey { get; set; }
        public string TxnId { get; set; }
        public int TxnSts { get; set; } // {0: failer ,1 means success}
        public DateTime TxtDate { get; set; }
        public Int64 CID { get; set; }// customer id
        public string PaytmResp { get; set; }
        public string PaidAmount { get; set; }
        public PaytmResn()
        {
            id = 0;
            TxtDate = DateTime.Now;
            TxnId = "";
            TxnSts = 1;

        }
        public int save()
        {
            DBCon con = new DBCon();
            SqlCommand cmd = new SqlCommand();
            try
            {
                if (this.id == 0)
                {
                    cmd = new SqlCommand("insert into PaytmTxn values(@OID,@OIDkey,@TxnId,@TxnSts,@TxtDate,@CID,@PaytmResp,@PaidAmount); SELECT SCOPE_IDENTITY();", con.Con);
                }
                else
                {
                    cmd = new SqlCommand("update PaytmTxn set TxnId=@TxnId,TxnSts=@TxnSts,TxtDate=@TxtDate,CID=@CID,PaytmResp=@PaytmResp, OIDkey=@OIDkey,PaidAmount=@PaidAmount where ID=@ID ", con.Con);
                    cmd.Parameters.AddWithValue("@ID", this.id);
                }
                cmd.Parameters.AddWithValue("@OIDkey", this.OIDkey);
                cmd.Parameters.AddWithValue("@OID", this.OID);
                cmd.Parameters.AddWithValue("@TxnId", this.TxnId);
                cmd.Parameters.AddWithValue("@TxnSts", this.TxnSts);
                cmd.Parameters.AddWithValue("@TxtDate", this.TxtDate);
                cmd.Parameters.AddWithValue("@CID", this.CID);
                cmd.Parameters.AddWithValue("@PaytmResp", this.PaytmResp);
                cmd.Parameters.AddWithValue("@PaidAmount", this.PaidAmount);
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
                    int index = -1;
                    PaytmResn hG_Ticket = new PaytmResn();
                    hG_Ticket.id = sqlDataReader.GetInt32(++index);
                    hG_Ticket.OID = sqlDataReader.GetInt64(++index);
                    hG_Ticket.OIDkey = sqlDataReader.GetString(++index);
                    hG_Ticket.TxnId = sqlDataReader.GetString(++index);
                    hG_Ticket.TxnSts = sqlDataReader.GetInt32(++index);
                    hG_Ticket.TxtDate = sqlDataReader.GetDateTime(++index);
                    hG_Ticket.CID = sqlDataReader.GetInt64(++index);
                    hG_Ticket.PaytmResp = sqlDataReader.GetString(++index);
                    hG_Ticket.PaidAmount = sqlDataReader.GetString(++index);
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

        public static PaytmResn Getone(Int64 OID)
        {

            PaytmResn Temp = new PaytmResn();
            DBCon con = new DBCon();
            SqlCommand cmd = new SqlCommand();
            string query = "select * from PaytmTxn where OID=@OID";
            try
            {
                cmd = new SqlCommand(query, con.Con);
                cmd.Parameters.AddWithValue("@OID", OID);
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    int index = -1;
                    PaytmResn hG_Ticket = new PaytmResn();
                    hG_Ticket.id = sqlDataReader.GetInt32(++index);
                    hG_Ticket.OID = sqlDataReader.GetInt64(++index);
                    hG_Ticket.OIDkey = sqlDataReader.GetString(++index);
                    hG_Ticket.TxnId = sqlDataReader.GetString(++index);
                    hG_Ticket.TxnSts = sqlDataReader.GetInt32(++index);
                    hG_Ticket.TxtDate = sqlDataReader.GetDateTime(++index);
                    hG_Ticket.CID = sqlDataReader.GetInt64(++index);
                    hG_Ticket.PaytmResp = sqlDataReader.GetString(++index);
                    hG_Ticket.PaidAmount = sqlDataReader.GetString(++index);
                    Temp =hG_Ticket;
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