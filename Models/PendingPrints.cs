using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models
{
    public class PendingPrints
    {
        public Int64 OID { get; set; }
        public int OrgId { get; set; }
        public int InvoiceNoCopy { get; set; }
        public int KotNoOfCopy { get; set; }
        public DateTime Createdate { get; set; }
        public Int64 Save()
        {
            Int64 ROW = 0;
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Query = "";
                if(this.OID==0)
                {
                    Query = "INSERT INTO PendingPrints values (@OID,@OrgId,@InvoiceNoCopy,@KotNoOfCopy,@Createdate)";
                    cmd = new SqlCommand(Query, dBCon.Con);
                    cmd.Parameters.AddWithValue("", this.Createdate);
                }
                else
                {
                    Query = "Update PendingPrints set InvoiceNoCopy=@InvoiceNoCopy,KotNoOfCopy=@KotNoOfCopy where OID=@OID";
                }
                
                cmd.Parameters.AddWithValue("",this.OID);
                cmd.Parameters.AddWithValue("",this.OrgId);
                cmd.Parameters.AddWithValue("",this.InvoiceNoCopy);
                cmd.Parameters.AddWithValue("",this.KotNoOfCopy);
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
                    OBJPENDING.OID = SDR.GetInt64(0);
                    OBJPENDING.OrgId = SDR.GetInt32(1);
                    OBJPENDING.InvoiceNoCopy = SDR.GetInt32(2);
                    OBJPENDING.KotNoOfCopy = SDR.GetInt32(3);
                    OBJPENDING.Createdate = SDR.GetDateTime(4);
                    listpending.Add(OBJPENDING);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); dBCon.Con.Close(); }
            return (listpending);
        }

    }
}