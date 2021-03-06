﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace HangOut.Models
{
    public class HG_Ticket
    {
        [Column("TID")]
        public Int64 TicketId { get; set; }
        public int TicketNo { get; set; }
        public int OrgId { get; set; }
        [Column("OrderId")]
        public Int64 OID { get; set; }// order id
        [Column("CreateDate")]
        public DateTime CreationDate { get; set; }
        public double DeliveryCharge { get; set; }
        public HG_Ticket()
        {
            CreationDate = DateTime.Now;
        }
        public int save()
        {
            DBCon con = new DBCon();
            SqlCommand cmd = new SqlCommand();
            try
            {
                if (this.TicketId == 0)
                {
                    cmd = new SqlCommand("insert into HG_Ticket values(@TicketNo,@OrgId,@OrderId,@CreateDate,@DeliveryCharge)", con.Con);
                    cmd.Parameters.AddWithValue("@TicketNo", this.TicketNo);
                    cmd.Parameters.AddWithValue("@OrgId", this.OrgId);
                    cmd.Parameters.AddWithValue("@OrderId", this.OID);
                    cmd.Parameters.AddWithValue("@CreateDate", this.CreationDate.Date);
                }
                else
                {
                    cmd = new SqlCommand("UPDATE  HG_Ticket set DeliveryCharge=@DeliveryCharge where TID=@TID", con.Con);
                    cmd.Parameters.AddWithValue("@TID", this.TicketId);
                }
                cmd.Parameters.AddWithValue("@DeliveryCharge", this.DeliveryCharge);
                this.TicketId = Convert.ToInt64(cmd.ExecuteNonQuery());
            }
            catch(Exception e)
            {
                e.Message.ToString();
                return 0;
            }
            finally
            {
                cmd.Dispose(); con.Con.Close();
            }
            return this.TicketNo;
        }
        public List<HG_Ticket> GetAll(int OrgId,DateTime ? onDate =null)
        {
            if (!onDate.HasValue)
            {
                onDate = DateTime.Now;
            }
            var CurrOrgID = HttpContext.Current.Request.Cookies["UserInfo"];
            List<HG_Ticket> listtemp = new List<HG_Ticket>();
            DBCon con = new DBCon();
            SqlCommand cmd = new SqlCommand();
            string query = "select * from HG_Ticket where OrgId ="+OrgId.ToString()+ " and CreateDate='"+ onDate.Value.Date.ToString("MM/dd/yyyy")+"'";
            try
            {
                cmd = new SqlCommand(query, con.Con);
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    HG_Ticket hG_Ticket = new HG_Ticket();
                    hG_Ticket.TicketId = sqlDataReader.GetInt64(0);
                    hG_Ticket.TicketNo = sqlDataReader.GetInt32(1);
                    hG_Ticket.OrgId = sqlDataReader.GetInt32(2);
                    hG_Ticket.OID = sqlDataReader.GetInt64(3);
                    hG_Ticket.CreationDate = sqlDataReader.GetDateTime(4);
                    hG_Ticket.DeliveryCharge = sqlDataReader.GetDouble(5);
                    listtemp.Add(hG_Ticket);
                }

            }
            catch(Exception e)
            {
                e.ToString();
            }
            finally
            {
                con.Con.Close(); con.Con.Dispose(); cmd.Dispose();
            }

            return listtemp;
        }
        public int TicketCnt(int OrgID,DateTime ?  onDate = null)
        {
            int TicketOnDate = 0;
            if (!onDate.HasValue)
            {
                onDate = DateTime.Now;
            }
            DBCon con = new DBCon();
            SqlCommand cmd = new SqlCommand();
            //string query = "select * from HG_Ticket where OrgId =" + OrgId.ToString() + " and CreateDate='" + onDate.Value.Date.ToString("MM/dd/yyyy") + "'";
            try
            {
                cmd = new SqlCommand("TicketCntOnDate", con.Con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrgID", OrgID);
                cmd.Parameters.AddWithValue("@Ondate", onDate.Value.Date.ToString("MM/dd/yyyy"));
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    TicketOnDate = sqlDataReader.GetInt32(0);
                    break;
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                con.Con.Close(); con.Con.Dispose(); cmd.Dispose();
            }

            return TicketOnDate;
        }
        public static List<HG_Ticket> GetByOID(Int64 OID)
        {
            List<HG_Ticket> listtemp = new List<HG_Ticket>();
            DBCon con = new DBCon();
            SqlCommand cmd = new SqlCommand();
            string query = "select * from HG_Ticket where OrderId ="+OID.ToString();
            try
            {
                cmd = new SqlCommand(query, con.Con);
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    HG_Ticket hG_Ticket = new HG_Ticket();
                    hG_Ticket.TicketId = sqlDataReader.GetInt64(0);
                    hG_Ticket.TicketNo = sqlDataReader.GetInt32(1);
                    hG_Ticket.OrgId = sqlDataReader.GetInt32(2);
                    hG_Ticket.OID = sqlDataReader.GetInt64(3);
                    hG_Ticket.CreationDate = sqlDataReader.GetDateTime(4);
                    hG_Ticket.DeliveryCharge = sqlDataReader.GetDouble(5);
                    listtemp.Add(hG_Ticket);
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                con.Con.Close(); con.Con.Dispose(); cmd.Dispose();
            }
            return listtemp;
        }

    }
}