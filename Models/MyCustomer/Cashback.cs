﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models.MyCustomer
{
    public class Cashback
    {
        public int CashBkId { get; set; }
        public int OrgID { get; set; }
        public DateTime StartDate { get; set; }
        public int ValidTill { get; set; }// 1 unspecify , 2 specify ValidTillDate 
        public DateTime ValidTillDate { get; set; }

        public int CashBkType { get; set; }  // 1 :Percentage

        public double Percentage { get; set; }
        public double MaxAmt { get; set; }

        public int MinBilAmtType { get; set; }//1 dynamic , 2 manual amt
        public double BilAmt { get; set; }

        public Cashback()
        {
            StartDate = DateTime.Now;
            ValidTillDate= DateTime.Now;
        }

        public int Save()
        {
            int R = 0;
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Query = "";
                if (this.CashBkId == 0)
                {
                    Query = "Insert into  HG_Items  values(@StartDate,@ValidTill,@ValidTillDate,@CashBkType,@MaxAmt,@MinBilAmtType,@BilAmt); SELECT SCOPE_IDENTITY();";
                    cmd = new SqlCommand(Query, dBCon.Con);
                    cmd.Parameters.AddWithValue("@OrgID", this.OrgID);
                }
                else
                {
                    Query = "update  HG_Items set StartDate=@StartDate,ValidTill=@ValidTill,ValidTillDate=@ValidTillDate,CashBkType=@CashBkType,MaxAmt=@MaxAmt,MinBilAmtType=@MinBilAmtType,BilAmt=@BilAmt where CashBkId=@CashBkId";
                    cmd = new SqlCommand(Query, dBCon.Con);
                }
                cmd.Parameters.AddWithValue("@CashBkId", this.CashBkId);
                cmd.Parameters.AddWithValue("@StartDate", this.StartDate);
                cmd.Parameters.AddWithValue("@ValidTill", this.ValidTill);
                cmd.Parameters.AddWithValue("@ValidTillDate", this.ValidTillDate);
                cmd.Parameters.AddWithValue("@CashBkType", this.CashBkType);
                cmd.Parameters.AddWithValue("@MaxAmt ", this.MaxAmt);
                cmd.Parameters.AddWithValue("@MinBilAmtType ", this.MinBilAmtType);
                cmd.Parameters.AddWithValue("@BilAmt", this.BilAmt);
                if (this.CashBkId == 0)
                {
                    R = Convert.ToInt32(cmd.ExecuteScalar());
                    this.CashBkId = R;

                }
                else
                {
                    R = cmd.ExecuteNonQuery();
                    if (R > 0)
                        R = this.CashBkId;
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

        public static List<Cashback> GetAll(int OrgId)
        {
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<Cashback> ListTmp = new List<Cashback>();
            string Query = "SELECT * FROM  CashBack where OrgID=" + OrgId.ToString();
            try
            {
                cmd = new SqlCommand(Query, dBCon.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    int index = 0;
                    Cashback ObjTmp = new Cashback();
                    ObjTmp.CashBkId = SDR.GetInt32(index++);
                    ObjTmp.OrgID = SDR.GetInt32(index++);
                    ObjTmp.StartDate = SDR.GetDateTime(index++);
                    ObjTmp.ValidTill = SDR.GetInt32(index++);
                    ObjTmp.ValidTillDate = SDR.GetDateTime(index++);
                    ObjTmp.CashBkType = SDR.GetInt32(index++);
                    ObjTmp.Percentage = SDR.GetDouble(index++);
                    ObjTmp.MaxAmt = SDR.GetDouble(index++);
                    ObjTmp.MinBilAmtType = SDR.GetInt32(index++);
                    ObjTmp.MinBilAmtType = SDR.GetInt32(index++);
                    ObjTmp.BilAmt = SDR.GetDouble(index++);
                    ListTmp.Add(ObjTmp);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { dBCon.Close(); }

            return (ListTmp);
        }
        public static Cashback Getone(int CashBkId)
        {
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            Cashback Tmp = new Cashback();
            string Query = "SELECT * FROM  CashBack where CashBkId=" + CashBkId.ToString();
            try
            {
                cmd = new SqlCommand(Query, dBCon.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    int index = 0;
                    Cashback ObjTmp = new Cashback();
                    ObjTmp.CashBkId = SDR.GetInt32(index++);
                    ObjTmp.OrgID = SDR.GetInt32(index++);
                    ObjTmp.StartDate = SDR.GetDateTime(index++);
                    ObjTmp.ValidTill = SDR.GetInt32(index++);
                    ObjTmp.ValidTillDate = SDR.GetDateTime(index++);
                    ObjTmp.CashBkType = SDR.GetInt32(index++);
                    ObjTmp.Percentage = SDR.GetDouble(index++);
                    ObjTmp.MaxAmt = SDR.GetDouble(index++);
                    ObjTmp.MinBilAmtType = SDR.GetInt32(index++);
                    ObjTmp.MinBilAmtType = SDR.GetInt32(index++);
                    ObjTmp.BilAmt = SDR.GetDouble(index++);
                    Tmp = ObjTmp;
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { dBCon.Close(); }

            return (Tmp);
        }
    }
    
}