﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models
{
    public class OrdDiscntChrge
    {
      public int   ID { get; set; }
      public string Title { get; set; }
      public Int64 OID { get; set; }
      public int Type { get; set; }// 1 discount, 2 charge
      public double Amt { get; set; }
      public double Tax { get; set; }
      public string Remark { get; set; }
      public DateTime Datetime { get; set; }
     public Int64 SeatingId { get; set; }
        public int SeatingOtp { get; set; }

    public OrdDiscntChrge()
        {
            Amt = 0.00;
            Tax = 0.00;
        }

        public int Save()
        {
            int Row = 0;
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Quary = "";
                if (this.ID == 0)
                {
                    Quary = "Insert Into OrdDiscntChrge Values (@Title,@OID,@Type,@Amt,@Tax,@Remark,@Datetime);SELECT SCOPE_IDENTITY();";
                    cmd = new SqlCommand(Quary, con.Con);
                    cmd.Parameters.AddWithValue("@Datetime", DateTime.Now);
                }
                else
                {
                    Quary = "Update OrdDiscntChrge Set Title=@Title,OID=@OID,Type=@Type,Amt=@Amt,Tax=@Tax,Remark=@Remark where ID=@ID";
                    cmd = new SqlCommand(Quary, con.Con);
                    cmd.Parameters.AddWithValue("@ID", this.ID);
                }
                cmd.Parameters.AddWithValue("@Title", this.Title);
                cmd.Parameters.AddWithValue("@Type", this.Type);
                cmd.Parameters.AddWithValue("@Amt", this.Amt);
                cmd.Parameters.AddWithValue("@Tax", this.Tax);
                cmd.Parameters.AddWithValue("@Remark", this.Remark);
                if (this.ID == 0)
                {
                    Row = Convert.ToInt32(cmd.ExecuteScalar());
                    this.ID = Row;
                }
                else
                {
                    Row = cmd.ExecuteNonQuery();
                    //this.CategoryID = Row;
                }

            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return Row;

        }
        
        public static OrdDiscntChrge GetAll(int ID )
        {
            DBCon Con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            OrdDiscntChrge ObjTmp = new OrdDiscntChrge();
            try
            {
                string Query = "SELECT  * FROM  OrdDiscntChrge";
                
                cmd = new SqlCommand(Query, Con.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    ObjTmp. ID = SDR.GetInt32(0);
                    ObjTmp.Title = SDR.GetString(1);
                    ObjTmp.Type = SDR.GetInt32(2);
                    ObjTmp.Amt = SDR.GetInt32(3);
                    ObjTmp.Tax = SDR.GetInt32(4);
                    ObjTmp.Remark = SDR.GetString(5);
                    ObjTmp.Datetime = SDR.GetDateTime(6);
                }
            }
            catch (System.Exception e)
            { e.ToString(); }

            finally { cmd.Dispose(); Con.Con.Close();Con = null; }

            return (ObjTmp);
        }
        public static void RemoveDiscntCharge(Int64 SeatingId,int Otp)
        {
            List<OrdDiscntChrge> discntCharges = DiscntCharge.ListDiscntChrge.FindAll(x => x.SeatingId == SeatingId && x.SeatingOtp == Otp);
            string DisntChargeIDs = "";
            for(int i=0;i<discntCharges.Count;i++)
            {
                discntCharges[i].Save();
                if (i == 0)
                {
                    DisntChargeIDs += discntCharges[i].ID.ToString();
                }
                else
                {
                    DisntChargeIDs +=","+ discntCharges[i].ID.ToString();
                }
                
            }
            if (DisntChargeIDs != "")
            {
                HG_Orders hG_Orders = new HG_Orders().GetOne(discntCharges[0].OID);
                if (hG_Orders.OID > 0)
                {
                    if(hG_Orders.DisntChargeIDs!=""&& hG_Orders.DisntChargeIDs != "0")
                    {
                        hG_Orders.DisntChargeIDs = hG_Orders.DisntChargeIDs+","+ DisntChargeIDs;
                    }
                    else
                    {
                        hG_Orders.DisntChargeIDs = DisntChargeIDs;
                    }
                   
                    hG_Orders.Save();
                }
            }
            if (discntCharges.Count > 0)
            {
                DiscntCharge.ListDiscntChrge.RemoveAll(x => x.SeatingId == SeatingId && x.SeatingOtp == Otp);
            }
        }
    }
    public class DiscntCharge
    {
        public static List<OrdDiscntChrge> ListDiscntChrge { get; set; }
    }
   
}