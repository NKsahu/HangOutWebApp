﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HangOut.Models
{

    public class HG_OrderItem
    {
        public System.Int64 OIID { get; set; }
        public System.Int64 FID { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
        public string Qty { get; set; }
        public System.Int64 OID { get; set; }
        public bool Deleted { get; set; }
        public int Status { get; set; }//{ 0: itemplaceByUser,1-Item ReadybyMess,2:ItemCollecteByAdmin
        public System.DateTime OrderDate { get; set; }
        public int UpdatedBy { get; set; }
        public System.DateTime UpdationDate { get; set; }
        public HG_OrderItem()
        {
            this.Qty = "0.00";
            this.OrderDate = System.DateTime.Now;
            this.UpdatedBy = 0;
            this.UpdationDate = System.DateTime.Now;
           
        }

        public System.Int64 Save()
        {
            System.Data.SqlClient.SqlCommand cmd = null;
            DBCon Obj = new DBCon();
            try
            {
                if (this.OIID == 0)
                    cmd = new System.Data.SqlClient.SqlCommand("INSERT INTO ORDERITEM (FID,Price,Count,Qty,OID,Deleted,MessId,OrderDate,TifinRackId,UpdatedBy,UpdationDate,TifinID) VALUES (@FID,@Price,@Count,@Qty,@OID,@Deleted,@MessId,@OrderDate,@TifinRackId,@UpdatedBy,@UpdationDate,@TifinID);select SCOPE_IDENTITY();", Obj.Con);
                else
                {
                    cmd = new System.Data.SqlClient.SqlCommand("UPDATE ORDERITEM SET FID=@FID,Price=@Price,Count=@Count,Qty=@Qty,OID=@OID,Deleted=@Deleted,MessId=@MessId,Status=@Status,TifinRackId=@TifinRackId,UpdatedBy=@UpdatedBy,UpdationDate=@UpdationDate,TifinID=@TifinID where OIID=@OIID", Obj.Con);
                    cmd.Parameters.AddWithValue("@OIID", this.OIID);
                }
                cmd.Parameters.AddWithValue("@FID", this.FID);
                cmd.Parameters.AddWithValue("@Price", this.Price);
                cmd.Parameters.AddWithValue("@Count", this.Count);
                cmd.Parameters.AddWithValue("@Qty", this.Qty);
                cmd.Parameters.AddWithValue("@OID", this.OID);
                cmd.Parameters.AddWithValue("@Deleted", this.Deleted);
                cmd.Parameters.AddWithValue("@Status", this.Status);
                cmd.Parameters.AddWithValue("@OrderDate", this.OrderDate);
                cmd.Parameters.AddWithValue("@UpdatedBy", this.UpdatedBy);
                cmd.Parameters.AddWithValue("@UpdationDate", this.UpdationDate);
                if (this.OIID == 0)
                {
                    this.OIID = System.Convert.ToInt64(cmd.ExecuteScalar());
                    if (this.OIID > 0) { }
                }
                else
                {
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                    }
                }
            }
            catch (System.Exception e) { this.OIID = 0; e.ToString(); }
            finally { cmd.Dispose(); Obj.Con.Close(); Obj.Con.Dispose(); Obj.Con = null; }
            return this.OIID;
        }

        public System.Collections.Generic.List<HG_OrderItem> GetAll()
        {
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlDataReader SDR = null;
            System.Collections.Generic.List<HG_OrderItem> ListTmp = new System.Collections.Generic.List<HG_OrderItem>();
            DBCon Obj = new DBCon();
            try
            {
                string Query = "SELECT * FROM ORDERITEM WHERE Deleted=0 ORDER BY OIID DESC";
                cmd = new System.Data.SqlClient.SqlCommand(Query, Obj.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    HG_OrderItem ObjTmp = new HG_OrderItem();
                    ObjTmp.OIID = SDR.GetInt64(0);
                    ObjTmp.FID = SDR.GetInt64(1);
                    ObjTmp.Price = SDR.GetDouble(2);
                    ObjTmp.Count = SDR.GetInt32(3);
                    ObjTmp.Qty = SDR.GetString(4);
                    ObjTmp.OID = SDR.GetInt64(5);
                    ObjTmp.Status = SDR.IsDBNull(8) ? 0 : SDR.GetInt32(8);
                    ObjTmp.OrderDate = SDR.IsDBNull(9) ? System.DateTime.Now : SDR.GetDateTime(9);
                    ObjTmp.UpdatedBy = SDR.IsDBNull(11) ? 0 : SDR.GetInt32(11);
                    ObjTmp.UpdationDate = SDR.IsDBNull(12) ? System.DateTime.Now : SDR.GetDateTime(12);
                    ListTmp.Add(ObjTmp);
                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { cmd.Dispose(); SDR.Close(); Obj.Con.Close(); Obj.Con.Dispose(); Obj.Con = null; }
            return (ListTmp);
        }
    }
}
