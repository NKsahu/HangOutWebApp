using System;
using System.Data.SqlClient;
namespace HangOut.Models
{
    public class Customer
    {
        public Int64 CID { get; set; }
        public int OrgId { get; set; }
        public  DateTime JoinDate {get;set;}

        public int save()
        {
            DBCon con = new DBCon();
            SqlCommand cmd = new SqlCommand();
            try
            {
               
                    cmd = new SqlCommand("insert into MyCustomer values(@CID,@OrgId,@JoinDate)", con.Con);
                    cmd.Parameters.AddWithValue("@CID", this.CID);
                    cmd.Parameters.AddWithValue("@OrgId", this.OrgId);
                    cmd.Parameters.AddWithValue("@JoinDate", this.JoinDate);
                   cmd.ExecuteNonQuery();
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
            return this.OrgId;
        }
        public static bool IsJoined(Int64 CID,int OrgId)
        {
            string Query = "SELECT CID,OrgId from MyCustomer where CID=" +CID.ToString() + " and OrgId="+OrgId.ToString();
            bool joindSts = false;
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            DBCon Obj = new DBCon();
            try
            {
                cmd = new SqlCommand(Query, Obj.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    joindSts = true;

                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); SDR.Close(); Obj.Con.Close(); Obj.Con.Dispose(); Obj.Con = null; }
            return joindSts;
        }
        public static int MyCustomerCnt(int OrgId)
        {
            string Query = "SELECT COUNT(CID) from MyCustomer where  OrgId=" + OrgId.ToString();
            int Count = 0;
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            DBCon Obj = new DBCon();
            try
            {
                cmd = new SqlCommand(Query, Obj.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    Count = SDR.GetInt32(0);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); SDR.Close(); Obj.Con.Close(); Obj.Con.Dispose(); Obj.Con = null; }
            return Count;
        }
    }
}