using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models
{
    public class PendingPrints
    {   
        public int ID { get; set; }
        public Int64 OID { get; set; }
        public int OrgId { get; set; }
        public int InvoiceNoCopy { get; set; }
        public int KotNoOfCopy { get; set; }
        public DateTime Createdate { get; set; }
        public int TicketNo { get; set; }
        public Int64 Save()
        {
            int ROW = 0;
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Query = "";
                if(this.ID==0)
                {
                    Query = "INSERT INTO pendingprint values (@OID,@OrgId,@InvoiceNoCopy,@KotNoOfCopy,@Createdate,@TicketNo)";
                    cmd = new SqlCommand(Query, dBCon.Con);
                    cmd.Parameters.AddWithValue("Createdate", DateTime.Now.Date);
                    cmd.Parameters.AddWithValue("OID", this.OID);
                    cmd.Parameters.AddWithValue("OrgId", this.OrgId);
                    cmd.Parameters.AddWithValue("TicketNo", this.TicketNo);
                }
                else
                {
                    Query = "Update pendingprint set InvoiceNoCopy=@InvoiceNoCopy,KotNoOfCopy=@KotNoOfCopy where  ID=@ID";
                    cmd.Parameters.AddWithValue("ID", this.ID);
                }
                cmd.Parameters.AddWithValue("InvoiceNoCopy", this.InvoiceNoCopy);
                cmd.Parameters.AddWithValue("KotNoOfCopy", this.KotNoOfCopy);
                if (this.ID == 0)
                {
                    ROW = Convert.ToInt32(cmd.ExecuteScalar());
                    this.ID = ROW;
                }
                else
                {
                    ROW = cmd.ExecuteNonQuery();
                    //this.CategoryID = Row;
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); dBCon.Con.Close(); }
            return ROW;
        }
        public static void SaveKotPrint(HG_Orders order, int NoOfCopy,int TicketNo=0,List<HG_OrderItem> hG_OrderItems=null)
        {
            if (TicketNo == 0)
            {
                var tickets = hG_OrderItems.GroupBy(x => x.TickedNo);
                foreach(var ticket in tickets)
                {
                    PendingPrints pendingPrints = new PendingPrints();
                    pendingPrints.OID = order.OID;
                    pendingPrints.OrgId = order.OrgId;
                    pendingPrints.KotNoOfCopy = NoOfCopy;
                    pendingPrints.InvoiceNoCopy = 0;
                    pendingPrints.TicketNo = ticket.Key;
                    try
                    {
                        pendingPrints.Save();
                    }
                    catch (Exception e)
                    {
                    }
                }
                
            }
            else
            {
                PendingPrints pendingPrints = new PendingPrints();
                pendingPrints.OID = order.OID;
                pendingPrints.OrgId = order.OrgId;
                pendingPrints.KotNoOfCopy = NoOfCopy;
                pendingPrints.InvoiceNoCopy = 0;
                pendingPrints.TicketNo = TicketNo;
                try
                {
                    pendingPrints.Save();
                }
                catch (Exception e)
                {

                }
            }
            

        }
        //if (ObjOrg.OrderDisplay == 2 && AppType != 3 && ObjOrg.PrinttingType == 2 && (PaymtSts > 0 ||ObjOrg.PaymentType==2))
               // {
               //     PendingPrints.SaveKotPrint(ObjOrders, ObjOrg.Copy, Ticketno);
               // }
    public static void SaveInvoicePrint(HG_Orders order, int NoOfCopy)
        {
            PendingPrints pendingPrints = new PendingPrints();
            pendingPrints.OID = order.OID;
            pendingPrints.OrgId = order.OrgId;
            pendingPrints.InvoiceNoCopy = NoOfCopy;
            pendingPrints.KotNoOfCopy = 0;
            pendingPrints.TicketNo = 0;
            try
            {
                pendingPrints.Save();
            }
            catch (Exception e)
            {

            }
        }
        public static List<PendingPrints>GetAll()
        {
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<PendingPrints> listpending = new List<PendingPrints>();
            try
            {
                string Quary = "Select * from pendingprint ";
                cmd = new SqlCommand(Quary, dBCon.Con);
                SDR = cmd.ExecuteReader();

                while (SDR.Read())
                {
                    PendingPrints OBJPENDING = new PendingPrints();
                    OBJPENDING.ID = SDR.GetInt32(0);
                    OBJPENDING.OID = SDR.GetInt64(1);
                    OBJPENDING.OrgId = SDR.GetInt32(2);
                    OBJPENDING.InvoiceNoCopy = SDR.GetInt32(3);
                    OBJPENDING.KotNoOfCopy = SDR.GetInt32(4);
                    OBJPENDING.Createdate = SDR.GetDateTime(5);
                    OBJPENDING.TicketNo = SDR.GetInt32(6);
                    listpending.Add(OBJPENDING);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); dBCon.Con.Close(); }
            return (listpending);
        }

    }
}