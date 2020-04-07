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
        public int CashBkId { get; set; }
      public double CashBkAmt { get; set; }// CR
        public DateTime AmtActiveOn { get; set; }
        public int OrgId { get; set; }
        public double DeductedAmt { get; set; }//DR
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
                    Query = "Insert into  CustWallet  values(@CID,@OID,@CashBkId,@CashBkAmt,@AmtActiveOn,@OrgId,@DeductedAmt); SELECT SCOPE_IDENTITY();";
                    cmd = new SqlCommand(Query, dBCon.Con);
                    cmd.Parameters.AddWithValue("@CID", this.CID);
                    cmd.Parameters.AddWithValue("@OID", this.OID);
                   cmd.Parameters.AddWithValue("@CashBkId", this.CashBkId);
                    cmd.Parameters.AddWithValue("@AmtActiveOn", this.AmtActiveOn);
                    cmd.Parameters.AddWithValue("@OrgId", this.OrgId);
                }
                else
                {
                    Query = "update  CustWallet set CashBkAmt=@CashBkAmt,DeductedAmt=@DeductedAmt where WalletId=@WalletId";
                    cmd = new SqlCommand(Query, dBCon.Con);
                    cmd.Parameters.AddWithValue("@WalletId", this.WalletId);
                }

                cmd.Parameters.AddWithValue("@CashBkAmt", this.CashBkAmt);
                cmd.Parameters.AddWithValue("@DeductedAmt", this.DeductedAmt);
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

        public static void AddToWallet(HG_Orders ObjOrder,int AppType=0)
        {


        }
        public static List<Wallet> GetAll(int CID,int OrgId)
        {
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<Wallet> ListTmp = new List<Wallet>();
            string Query = "SELECT * FROM  CustWallet where CID=" + CID.ToString()+ " and OrgId="+OrgId;
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
                    ObjTmp.CashBkId = SDR.GetInt32(index++);
                    ObjTmp.CashBkAmt = SDR.GetDouble(index++);
                    ObjTmp.AmtActiveOn = SDR.GetDateTime(index++);
                    ObjTmp.OrgId = SDR.GetInt32(index++);
                    ListTmp.Add(ObjTmp);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { dBCon.Close(); }

            return (ListTmp);
        }
    }

    public class WalletAmt
    {
        public double CashBkAmt { get; set; }

        public double DeductedAmt { get; set; }
        public static WalletAmt GetUnusedWalletAmt(int CID,int OrgId)
        {
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            WalletAmt ObjTmp = new WalletAmt();
            try
            {
                cmd = new SqlCommand("GetUnusedWalletAmt", dBCon.Con);
                cmd.Parameters.AddWithValue("@CID", CID);
                cmd.Parameters.AddWithValue("@OrgID", OrgId);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    int index = 0;
                    WalletAmt Tmp = new WalletAmt();
                    ObjTmp.CashBkAmt = SDR.GetDouble(index++);
                    ObjTmp.DeductedAmt = SDR.GetDouble(index++);
                    ObjTmp = Tmp;
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { dBCon.Close(); }

            return (ObjTmp);
        }
    }

    public class MyWallet
    {
        public int OrgId { get; set; }

        public double CashBkAmt { get; set; }
        public double deductedAmt { get; set; }
        public string OutLetNamr { get; set; }
        public static List<MyWallet> GetWalletAmt(int CID)
        {
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<MyWallet> ListTmp = new List<MyWallet>();
            try
            {
                cmd = new SqlCommand("MyWallet", dBCon.Con);
                cmd.Parameters.AddWithValue("@CID", CID);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    int index = 0;
                    MyWallet Tmp = new MyWallet();
                    Tmp.OrgId = SDR.GetInt32(index++);
                    Tmp.CashBkAmt = SDR.GetDouble(index++);
                    Tmp.deductedAmt = SDR.GetDouble(index++);
                    Tmp.OutLetNamr = SDR.GetString(index++);
                    ListTmp.Add(Tmp);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { dBCon.Close(); }

            return (ListTmp);
        }
    }
}