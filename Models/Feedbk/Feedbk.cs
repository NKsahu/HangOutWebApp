using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models.Feedbk
{
    public class Feedbk
    {
       public int  FeedBkId { get; set; }
       public int OrgId { get; set; }
       public Int64 OrderId { get; set; }
       public  int FeedbkFormId { get; set; }
       public DateTime CreateOn { get; set; }
        public int save()
        {
            int R = 0;
            DBCon con = new DBCon();
            SqlCommand cmd = null;

            try
            {
                string Quary = "";
                if(FeedBkId ==0)
                {
                    Quary = "Insert into FeedBk values(@OrgId,@OrderId,@FeedbkFormId,@CreateOn);select SCOPE_IDENTITY();";
                    cmd = new SqlCommand(Quary, con.Con);
                }
                else
                {
                    Quary = "Update FeedBk Set OrgId=@OrgId,OrderId=@OrderId,FeedbkFormId=@FeedbkFormId where FeedBkId=@FeedBkId";
                    cmd = new SqlCommand(Quary, con.Con);
                    cmd.Parameters.AddWithValue("@FeedBkId", this.FeedBkId);
                }
                cmd.Parameters.AddWithValue("@OrgId", this.OrgId);
                cmd.Parameters.AddWithValue("@OrderId", this.OrderId);
                cmd.Parameters.AddWithValue("@FeedbkFormId", this.FeedbkFormId);
                if (this.FeedBkId == 0)
                {
                    cmd.Parameters.AddWithValue("@CreateOn", DateTime.Now);
                    R = Convert.ToInt32(cmd.ExecuteScalar());
                    this.FeedBkId = R;
                }
                else
                {
                    R = cmd.ExecuteNonQuery();
                    
                }

            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return R;
        }
        public static List<Feedbk>GetAll(int OrgId,DateTime Fdate,DateTime Tdate)
        {
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<Feedbk> listfeedbk = new List<Feedbk>();
            try
            {
                string Quary = "Select * From FeedBk where OrgId="+OrgId;
                if (Fdate != null && Tdate != null)
                {
                    Quary += "and CreateOn between '" + Fdate.ToString("MM/dd/yyyy") + "' and '" + Tdate.ToString("MM/dd/yyyy HH:mm:ss") + "";
                }
                cmd = new SqlCommand(Quary, dBCon.Con);
                SDR = cmd.ExecuteReader();
                while(SDR.Read())
                {
                Feedbk OBJfeedbk = new Feedbk();
                OBJfeedbk.FeedBkId = SDR.GetInt32(0);
                OBJfeedbk.OrgId = SDR.GetInt32(1);
                OBJfeedbk.OrderId = SDR.GetInt64(2);
                OBJfeedbk.FeedbkFormId = SDR.GetInt32(3);
                OBJfeedbk.CreateOn = SDR.GetDateTime(4);
                listfeedbk.Add(OBJfeedbk);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally {  cmd.Dispose(); ; dBCon.Con.Close(); }
            return (listfeedbk);
        }
        public static Feedbk GetOne(Int64 OID)
        {
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            Feedbk feedbk = new Feedbk();
            try
            {
                string Quary = "Select TOP 1 * From FeedBk where OrderId=" + OID;
                cmd = new SqlCommand(Quary, dBCon.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    feedbk.FeedBkId = SDR.GetInt32(0);
                    feedbk.OrgId = SDR.GetInt32(1);
                    feedbk.OrderId = SDR.GetInt64(2);
                    feedbk.FeedbkFormId = SDR.GetInt32(3);
                    feedbk.CreateOn = SDR.GetDateTime(4);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); ; dBCon.Con.Close(); }

            return (feedbk);

        }
    }
}