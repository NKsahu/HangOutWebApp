﻿using HangOut.Models.DynamicList;
using System;
using System.Collections.Generic;
using System.Web;

namespace HangOut.Models
{

    public class HG_OrderItem
    {
        public Int64 OIID { get; set; }
        public Int64 FID { get; set; }//FID mean food ID
        public double Price { get; set; }// single item*count = price TotalWithTax
        public int Count { get; set; }
        public string IsAddon { get; set; } //{ 0: no,1:yes==aplly addons and multiservings Or Parcel Applied } //Qty replace with IsAddon Item
        public Int64 OID { get; set; }
        public bool Deleted { get; set; }
        public int Status { get; set; }//"1":Order Placed,"2":Processing,3:"Completed" ,"4" :"Cancelled"
        public DateTime OrderDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdationDate { get; set; }
        public int TickedNo { get; set; }
        public int ChefSeenBy { get; set; }// item complete or cancel by
        public int OrgId { get; set; }
        public Int64 OrdById { get; set; }// Item order by id
        public double TaxInItm { get; set; }//single item Tax*Count  =Tax
        public double CostPrice { get; set; }// single cost price= CostPrice*count without tax

        public string ItemNam { get; set; }
        public HG_OrderItem()
        {
            this.IsAddon = "0";
            this.UpdationDate = System.DateTime.Now;
            this.TickedNo = 0;
            this.ChefSeenBy = 0;
        }

        public Int64 Save()
        {
            Int64 Row = 0;
            System.Data.SqlClient.SqlCommand cmd = null;
            DBCon Obj = new DBCon();
            try
            {
                if (this.OIID == 0) {
                    cmd = new System.Data.SqlClient.SqlCommand("INSERT INTO HG_ORDERITEM (FID,Price,Count,Qty,OID,Deleted,Status,OrderDate,UpdatedBy,UpdationDate,TickedNo,ChefSeenBy,OrgId,OrdById,TaxInItm,CostPrice) VALUES (@FID,@Price,@Count,@Qty,@OID,@Deleted,@Status,@OrderDate,@UpdatedBy,@UpdationDate,@TickedNo,@ChefSeenBy,@OrgId,@OrdById,@TaxInItm,@CostPrice);select SCOPE_IDENTITY();", Obj.Con);
                    cmd.Parameters.AddWithValue("@OrdById", this.OrdById);
                    cmd.Parameters.AddWithValue("@TaxInItm", this.TaxInItm);
                }
                else
                {
                    cmd = new System.Data.SqlClient.SqlCommand("UPDATE HG_ORDERITEM SET FID=@FID,Price=@Price,Count=@Count,Qty=@Qty,OID=@OID,Deleted=@Deleted,Status=@Status,UpdatedBy=@UpdatedBy,UpdationDate=@UpdationDate,TickedNo=@TickedNo,ChefSeenBy=@ChefSeenBy,OrgId=@OrgId,CostPrice=@CostPrice where OIID=@OIID", Obj.Con);
                    cmd.Parameters.AddWithValue("@OIID", this.OIID);
                }
                cmd.Parameters.AddWithValue("@FID", this.FID);
                cmd.Parameters.AddWithValue("@Price", this.Price);
                cmd.Parameters.AddWithValue("@Count", this.Count);
                cmd.Parameters.AddWithValue("@Qty", this.IsAddon);
                cmd.Parameters.AddWithValue("@OID", this.OID);
                cmd.Parameters.AddWithValue("@Deleted", this.Deleted);
                cmd.Parameters.AddWithValue("@Status", this.Status);
                cmd.Parameters.AddWithValue("@OrderDate", this.OrderDate);
                cmd.Parameters.AddWithValue("@UpdatedBy", this.UpdatedBy);
                cmd.Parameters.AddWithValue("@UpdationDate", this.UpdationDate);
                cmd.Parameters.AddWithValue("@TickedNo", this.TickedNo);
                cmd.Parameters.AddWithValue("@ChefSeenBy", this.ChefSeenBy);
                cmd.Parameters.AddWithValue("@OrgId", this.OrgId);
                cmd.Parameters.AddWithValue("@CostPrice", this.CostPrice);
                if (this.OIID == 0)
                {
                    this.OIID = System.Convert.ToInt64(cmd.ExecuteScalar());
                    Row = this.OIID;
                }
                else
                {
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        Row = this.OIID;
                    }
                }
            }
            catch (Exception e) { this.OIID = 0; e.ToString(); }
            finally { cmd.Dispose(); Obj.Con.Close(); Obj.Con.Dispose(); Obj.Con = null; }
            return Row;
        }

        public List<HG_OrderItem> GetAll(Int64 OID = 0)
        {
            var CurrOrgID = HttpContext.Current.Request.Cookies["UserInfo"];
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlDataReader SDR = null;
            List<HG_OrderItem> ListTmp = new List<HG_OrderItem>();
            DBCon Obj = new DBCon();
            try
            {
                string Query = "SELECT * FROM HG_ORDERITEM";
                if (OID > 0)
                {
                    Query = "SELECT * FROM HG_ORDERITEM WHERE OID=" + OID.ToString() + " and Deleted=0 ";
                }
                else if (int.Parse(CurrOrgID["OrgId"]) > 0)
                {
                    Query = "SELECT * FROM HG_ORDERITEM WHERE OrgId=" + CurrOrgID["OrgId"] + " and Deleted=0 ";
                }
                cmd = new System.Data.SqlClient.SqlCommand(Query, Obj.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    HG_OrderItem ObjTmp = new HG_OrderItem();
                    ObjTmp.OIID = SDR.GetInt64(0);
                    ObjTmp.FID = SDR.GetInt64(1);
                    ObjTmp.Price = SDR.GetDouble(2);
                    ObjTmp.Count = SDR.GetInt32(3);
                    ObjTmp.IsAddon = SDR.GetString(4);
                    ObjTmp.OID = SDR.GetInt64(5);
                    ObjTmp.Status = SDR.GetInt32(7);
                    ObjTmp.OrderDate = SDR.GetDateTime(8);
                    ObjTmp.UpdatedBy = SDR.GetInt32(9);
                    ObjTmp.UpdationDate = SDR.GetDateTime(10);
                    ObjTmp.TickedNo = SDR.GetInt32(11);
                    ObjTmp.ChefSeenBy = SDR.GetInt32(12);
                    ObjTmp.OrgId = SDR.GetInt32(13);
                    ObjTmp.OrdById = SDR.GetInt64(14);
                    ObjTmp.TaxInItm = SDR.GetDouble(15);
                    ObjTmp.CostPrice = SDR.GetDouble(16);
                    ListTmp.Add(ObjTmp);
                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { cmd.Dispose(); SDR.Close(); Obj.Con.Close(); Obj.Con.Dispose(); Obj.Con = null; }
            return (ListTmp);
        }
        public List<HG_OrderItem> GetAllByOrg(int OrgId, int ChefId = 0, int ItemStatus = 0, bool TodayOnly = false, string Status = null)
        {
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlDataReader SDR = null;
            List<HG_OrderItem> ListTmp = new List<HG_OrderItem>();
            DBCon Obj = new DBCon();
            try
            {
                string Query = "";
                if (OrgId > 0)
                {
                    Query = "SELECT * FROM HG_ORDERITEM WHERE OrgId=" + OrgId.ToString() + "";
                }
                else if (OrgId <= 0)//super admin && postpaid-admin
                {
                    Query = "SELECT * FROM HG_ORDERITEM WHERE OrgId>0";
                }
                if (ChefId > 0)
                {
                    Query += " and (ChefSeenBy=" + ChefId.ToString() + " or ChefSeenBy=0)";
                }
                if (ItemStatus > 0)
                {
                    Query += " and Status=" + ItemStatus.ToString();
                }
                if (TodayOnly)
                {
                    var Formdate = DateTime.Now;
                    var theDate = new DateTime(Formdate.Year, Formdate.Month, Formdate.Day, 23, 59, 00);
                    Query = "SELECT * FROM HG_ORDERITEM WHERE OrderDate between '" + Formdate.ToString("MM/dd/yyyy") + "' and '" + theDate.ToString("MM/dd/yyyy HH:mm:ss") + "' and OrgId=" + OrgId.ToString() + "";
                }
                if (Status != null && ItemStatus == 0)
                {
                    Query += " and " + Status;
                }
                cmd = new System.Data.SqlClient.SqlCommand(Query, Obj.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    HG_OrderItem ObjTmp = new HG_OrderItem();
                    ObjTmp.OIID = SDR.GetInt64(0);
                    ObjTmp.FID = SDR.GetInt64(1);
                    ObjTmp.Price = SDR.GetDouble(2);
                    ObjTmp.Count = SDR.GetInt32(3);
                    ObjTmp.IsAddon = SDR.GetString(4);
                    ObjTmp.OID = SDR.GetInt64(5);
                    ObjTmp.Status = SDR.GetInt32(7);
                    ObjTmp.OrderDate = SDR.GetDateTime(8);
                    ObjTmp.UpdatedBy = SDR.GetInt32(9);
                    ObjTmp.UpdationDate = SDR.GetDateTime(10);
                    ObjTmp.TickedNo = SDR.GetInt32(11);
                    ObjTmp.ChefSeenBy = SDR.GetInt32(12);
                    ObjTmp.OrgId = SDR.GetInt32(13);
                    ObjTmp.OrdById = SDR.GetInt64(14);
                    ObjTmp.TaxInItm = SDR.GetDouble(15);
                    ObjTmp.CostPrice = SDR.GetDouble(16);
                    ListTmp.Add(ObjTmp);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); SDR.Close(); Obj.Con.Close(); Obj.Con.Dispose(); Obj.Con = null; }
            return (ListTmp);
        }
        public HG_OrderItem GetOne(Int64 OIID = 0)
        {
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlDataReader SDR = null;
            HG_OrderItem ObjTmp = new HG_OrderItem();
            DBCon Obj = new DBCon();
            try
            {
                string Query = "SELECT * FROM HG_ORDERITEM WHERE OIID=" + OIID.ToString() + " and Deleted=0 ";
                //if (TicketNo > 0)
                //{
                //    Query = "SELECT * FROM HG_ORDERITEM WHERE TickedNo=" + TicketNo.ToString() + " and Deleted=0 ";
                //}
                cmd = new System.Data.SqlClient.SqlCommand(Query, Obj.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    ObjTmp = new HG_OrderItem();
                    ObjTmp.OIID = SDR.GetInt64(0);
                    ObjTmp.FID = SDR.GetInt64(1);
                    ObjTmp.Price = SDR.GetDouble(2);
                    ObjTmp.Count = SDR.GetInt32(3);
                    ObjTmp.IsAddon = SDR.GetString(4);
                    ObjTmp.OID = SDR.GetInt64(5);
                    ObjTmp.Status = SDR.GetInt32(7);
                    ObjTmp.OrderDate = SDR.GetDateTime(8);
                    ObjTmp.UpdatedBy = SDR.GetInt32(9);
                    ObjTmp.UpdationDate = SDR.GetDateTime(10);
                    ObjTmp.TickedNo = SDR.GetInt32(11);
                    ObjTmp.ChefSeenBy = SDR.GetInt32(12);
                    ObjTmp.OrgId = SDR.GetInt32(13);
                    ObjTmp.OrdById = SDR.GetInt64(14);
                    ObjTmp.TaxInItm = SDR.GetDouble(15);
                    ObjTmp.CostPrice = SDR.GetDouble(16);

                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); SDR.Close(); Obj.Con.Close(); Obj.Con.Dispose(); Obj.Con = null; }
            return (ObjTmp);
        }
        public static HG_OrderItem PaybleAmt(Int64 OID)
        {
            HG_OrderItem objOrdItm = new HG_OrderItem();
            var OrderItms = new HG_OrderItem().GetAll(OID);
            for (int i = 0; i < OrderItms.Count; i++)
            {
                objOrdItm.CostPrice += OrderItms[i].Count * OrderItms[i].CostPrice;
                objOrdItm.TaxInItm += OrgType.TotalTax(OrderItms[i].CostPrice, OrderItms[i].TaxInItm, OrderItms[i].Count);
                objOrdItm.Price += OrderItms[i].Count * OrderItms[i].Price;
            }
            return objOrdItm;
        }
        public static HG_OrderItem ActualAmtToPay(Int64 OID)
        {
            HG_OrderItem objOrdItm = new HG_OrderItem();
            var OrderItms = new HG_OrderItem().GetAll(OID);
            OrderItms = OrderItms.FindAll(x => x.Status != 4);
            for (int i = 0; i < OrderItms.Count; i++)
            {
                objOrdItm.CostPrice += OrderItms[i].Count * OrderItms[i].CostPrice;
                objOrdItm.TaxInItm += OrgType.TotalTax(OrderItms[i].CostPrice, OrderItms[i].TaxInItm, OrderItms[i].Count);
                objOrdItm.Price += OrderItms[i].Count * OrderItms[i].Price;
            }
            return objOrdItm;
        }
        public static int UnseenOrd(int OrgId)
        {
            string Query = "SELECT distinct TickedNo,OID FROM HG_OrderItem WHERE  (Status=1 or Status=2) and OID In(select OID from HG_Orders where  Status='1' ";
            int Count = 0;
            HG_OrganizationDetails ObjOrg = new HG_OrganizationDetails().GetOne(OrgId);
            if (OrgId == 0 || (ObjOrg.OrgID > 0 && ObjOrg.PaymentType == 1))
            {
                Query = "SELECT distinct TickedNo,OID FROM HG_OrderItem WHERE  (Status=1 or Status=2) and OID In(select OID from HG_Orders where  Status='1' and PaymentStatus!= 0 ";
            }
            if (OrgId > 0)
            {
                Query += "and OrgId=" + OrgId.ToString() + " )";
            }
            else
            {
                Query += ")";
            }

            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlDataReader SDR = null;
            HG_OrderItem ObjTmp = new HG_OrderItem();
            DBCon Obj = new DBCon();
            try
            {
                cmd = new System.Data.SqlClient.SqlCommand(Query, Obj.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    Count = Count + 1;

                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { cmd.Dispose(); SDR.Close(); Obj.Con.Close(); Obj.Con.Dispose(); Obj.Con = null; }
            return Count;
        }
        public static double GetTaxbleAmt (double Amt, double Tax){
          double  TaxableAmt = (Amt * Tax) / 100;
            return TaxableAmt;
        }
    }
}
