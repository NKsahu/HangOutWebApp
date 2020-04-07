using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models.MyCustomer
{
    public class Wallet
    {
        public int  WalletId { get; set; }
        public int CID { get; set; }
       public Int64 OID { get; set; }
        public Int64 CampaiegnId { get; set; }
      public double CashBkAmt { get; set; }
        public DateTime AmtActiveOn { get; set; }
      public bool IsUsed { get; set; }
        public int OrgId { get; set; }
        public int Save()
        {
            int R = 0;
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Query = "";
                if (this.WalletId == 0)
                {
                    Query = "Insert into  CustWallet  values(@CID,@OID,@CampaiegnId,@CashBkAmt,@AmtActiveOn,@IsUsed,@OrgId); SELECT SCOPE_IDENTITY();";
                    cmd = new SqlCommand(Query, dBCon.Con);
                    cmd.Parameters.AddWithValue("@CID", this.CID);
                    cmd.Parameters.AddWithValue("@OID", this.OID);
                   cmd.Parameters.AddWithValue("@CampaiegnId", this.CampaiegnId);
                    cmd.Parameters.AddWithValue("@AmtActiveOn", DateTime.Now);
                    cmd.Parameters.AddWithValue("@OrgId", this.OrgId);
                }
                else
                {
                    Query = "update  CustWallet set CashBkAmt=@CashBkAmt,IsUsed=@IsUsed where WalletId=@WalletId";
                    cmd = new SqlCommand(Query, dBCon.Con);
                    cmd.Parameters.AddWithValue("@WalletId", this.WalletId);
                }

                cmd.Parameters.AddWithValue("@CashBkAmt", this.CashBkAmt);
                cmd.Parameters.AddWithValue("@IsUsed", this.IsUsed);
                if (this.WalletId == 0)
                {
                    R = Convert.ToInt32(cmd.ExecuteScalar());
                    this.WalletId = R;

                }
                else
                {
                    R = cmd.ExecuteNonQuery();
                    if (R > 0)
                        R = this.WalletId;
                }

            }
            catch (Exception e) { e.ToString(); }
            finally
            {
                dBCon.Close();
                if (cmd != null) cmd.Dispose();
            }
            return R;
        }

        public static List<Wallet> GetAll(int CID)
        {
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<Wallet> ListTmp = new List<Wallet>();
            string Query = "SELECT * FROM  CustWallet where CID=" + CID.ToString();
            try
            {
                cmd = new SqlCommand(Query, dBCon.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    int index = 0;
                    Wallet ObjTmp = new Wallet();
                    ObjTmp.WalletId = SDR.GetInt32(index++);
                    ObjTmp.CID = SDR.GetInt32(index++);
                    ObjTmp.OID = SDR.GetInt64(index++);
                    ObjTmp.CampaiegnId = SDR.GetInt64(index++);
                    ObjTmp.CashBkAmt = SDR.GetDouble(index++);
                    ObjTmp.AmtActiveOn = SDR.GetDateTime(index++);
                    ObjTmp.IsUsed = SDR.GetBoolean(index++);
                    ObjTmp.OrgId = SDR.GetInt32(index++);
                    ListTmp.Add(ObjTmp);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { dBCon.Close(); }

            return (ListTmp);
        }
    }
}