﻿using System;
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
       public Int64 SeatingId { get; set; }
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
                    Quary = "Insert into FeedBk values(@OrgId,@OrderId,@SeatingId,@FeedbkFormId,@CreateOn);select SCOPE_IDENTITY();";
                }else
                {
                    Quary = "Update FeedBk Set OrgId=@OrgId,OrderId=@OrderId,SeatingId=@SeatingId,FeedbkFormId=@FeedbkFormId,CreateOn=@CreateOn where FeedBkId=@FeedBkId ";
                }
                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@FeedBkId", this.FeedBkId);
                cmd.Parameters.AddWithValue("@OrgId", this.OrgId);
                cmd.Parameters.AddWithValue("@OrderId", this.OrderId);
                cmd.Parameters.AddWithValue("@SeatingId", this.SeatingId);
                cmd.Parameters.AddWithValue("@FeedbkFormId", this.FeedbkFormId);
                cmd.Parameters.AddWithValue("@CreateOn", DateTime.Now);
                if (this.FeedBkId == 0)
                {
                    R = Convert.ToInt32(cmd.ExecuteScalar());
                    this.FeedBkId = R;
                }
                else
                {
                    R = cmd.ExecuteNonQuery();
                    //this.CategoryID = Row;
                }

            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return R;
        }
        public List<Feedbk>GetAll()
        {
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<Feedbk> listfeedbk = new List<Feedbk>();
            try
            {
                string Quary = "Select * From FeedBk ORDER BY FeedBkId  DESC";
                cmd = new SqlCommand(Quary, dBCon.Con);
                SDR = cmd.ExecuteReader();
                while(SDR.Read())
                {

              
                Feedbk OBJfeedbk = new Feedbk();
                OBJfeedbk.FeedBkId = SDR.GetInt32(0);
                OBJfeedbk.OrgId = SDR.GetInt32(1);
                OBJfeedbk.OrderId = SDR.GetInt64(2);
                OBJfeedbk.SeatingId = SDR.GetInt64(3);
                OBJfeedbk.FeedbkFormId = SDR.GetInt32(4);
                OBJfeedbk.CreateOn = SDR.GetDateTime(5);
                listfeedbk.Add(OBJfeedbk);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); ; dBCon.Con.Close(); }

            return (listfeedbk);

        }
    }
}