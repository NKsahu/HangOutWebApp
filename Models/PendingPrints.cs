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
            Int64 ROW = 0;
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Query = "";
                if(this. ID==0)
                {
                    Query = "INSERT INTO PendingPrints values (@OID,@OrgId,@InvoiceNoCopy,@KotNoOfCopy,@Createdate,@TicketNo)";
                    cmd = new SqlCommand(Query, dBCon.Con);
                    cmd.Parameters.AddWithValue("", this.Createdate);
                }
                else
                {
                    Query = "Update PendingPrints set InvoiceNoCopy=@InvoiceNoCopy,KotNoOfCopy=@KotNoOfCopy where  ID=@ ID";
                }
                cmd.Parameters.AddWithValue("ID", this.ID);
                cmd.Parameters.AddWithValue("OID", this.OID);
                cmd.Parameters.AddWithValue("OrgId", this.OrgId);
                cmd.Parameters.AddWithValue("InvoiceNoCopy", this.InvoiceNoCopy);
                cmd.Parameters.AddWithValue("KotNoOfCopy", this.KotNoOfCopy);
                cmd.Parameters.AddWithValue("TicketNo", this.TicketNo);
                if (this.OID == 0)
                {
                    ROW = Convert.ToInt32(cmd.ExecuteScalar());
                    this.OID = ROW;
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
        public static List<PendingPrints>GetAll()
        {
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<PendingPrints> listpending = new List<PendingPrints>();
            try
            {
                string Quary = "Select * from PendingPrints   ORDER BY OrgID  DESC";
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
                    listpending.Add(OBJPENDING);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); dBCon.Con.Close(); }
            return (listpending);
        }

    }
}