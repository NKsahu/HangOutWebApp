using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace HangOut.Models
{
    public class HG_Ticket
    {
        public Int64 TicketId { get; set; }
        public int TicketNo { get; set; }
        public int OrgId { get; set; }
        public Int64 OID { get; set; }// order id
        public DateTime CreationDate { get; set; }

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
                cmd = new SqlCommand("insert into HG_Ticket values(@TicketNo,@OrgId,@OrderId,@CreateDate)", con.Con);
                cmd.Parameters.AddWithValue("@TicketNo", this.TicketNo);
                cmd.Parameters.AddWithValue("@OrgId", this.OrgId);
                cmd.Parameters.AddWithValue("@OrderId", this.OID);
                cmd.Parameters.AddWithValue("@CreateDate", this.CreationDate.Date);
                this.TicketId = System.Convert.ToInt32(cmd.ExecuteScalar());
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
        public List<HG_Ticket> GetAll(DateTime ? onDate =null)
        {
            if (!onDate.HasValue)
            {
                onDate = DateTime.Now;
            }
            var CurrOrgID = HttpContext.Current.Request.Cookies["UserInfo"];
            List<HG_Ticket> listtemp = new List<HG_Ticket>();
            DBCon con = new DBCon();
            SqlCommand cmd = new SqlCommand();
            string query = "select * from HG_Ticket where OrgId ="+ CurrOrgID["OrgId"]+ " and CreateDate="+ onDate.ToString("yyyy/MM/dd")+"";
            try
            {
                cmd=new SqlCommand("")

            }catch(Exception e)
            {
                e.ToString();
                
            }
            finally
            {

            }

            return listtemp;
        }

    }
}