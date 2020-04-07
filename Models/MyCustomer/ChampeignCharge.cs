using HangOut.Models.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace HangOut.Models.MyCustomer
{
    public class ChampeignCharge
    {

        public int ChargeId { get; set; }
        public double  ChargeAmt { get; set; }
     public Int64  OID { get; set; }
     public int CashBkId { get; set; }
      public DateTime  CreateDate {get;set;}
     public int  OrgId {get;set;}


        public int Save()
        {
            int R = 0;
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Query = "";
                if (this.ChargeId == 0)
                {
                    Query = "Insert into  ChampeignCharge  values(@ChargeAmt,@OID,@CashBkId,@CreateDate,@OrgId); SELECT SCOPE_IDENTITY();";
                    cmd = new SqlCommand(Query, dBCon.Con);
                    cmd.Parameters.AddWithValue("@ChargeAmt", this.ChargeAmt);
                    cmd.Parameters.AddWithValue("@OID", this.OID);
                    cmd.Parameters.AddWithValue("@CashBkId", this.CashBkId);
                    cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@OrgId", this.OrgId);
                }
                
                
                    R = Convert.ToInt32(cmd.ExecuteScalar());
                    this.ChargeId = R;
            }
            catch (Exception e) { e.ToString(); }
            finally
            {
                dBCon.Close();
                if (cmd != null) cmd.Dispose();
            }
            return R;
        }

        public static List<ChampeignCharge> GetAll(int CID)
        {
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<ChampeignCharge> ListTmp = new List<ChampeignCharge>();
            string Query = "SELECT * FROM  CustWallet where CID=" + CID.ToString();
            try
            {
                cmd = new SqlCommand(Query, dBCon.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    int index = 0;
                    ChampeignCharge ObjTmp = new ChampeignCharge();
                    ObjTmp.ChargeId = SDR.GetInt32(index++);
                    ObjTmp.ChargeAmt = SDR.GetDouble(index++);
                    ObjTmp.OID = SDR.GetInt64(index++);
                    ObjTmp.CashBkId = SDR.GetInt32(index++);
                    ObjTmp.CreateDate = SDR.GetDateTime(index++);
                    ObjTmp.OrgId = SDR.GetInt32(index++);
                    ListTmp.Add(ObjTmp);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { dBCon.Close(); }

            return (ListTmp);
        }

        public static int ChargeCamp(Int64 OID,int CBID,int OrgId)
        {
            if (CBID > 0)
            {
                var chargeAmt = new Settings().GetOne("CBCHARGE");
                ChampeignCharge champeignCharge = new ChampeignCharge();
                champeignCharge.ChargeAmt = double.Parse(chargeAmt.KeyValue);
                champeignCharge.CashBkId = CBID;
                champeignCharge.OID = OID;
                champeignCharge.OrgId = OrgId;
                champeignCharge.CreateDate = DateTime.Now;
                champeignCharge.Save();
                return champeignCharge.ChargeId;
            }
            return 0;
            
        }
    }
}