using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models
{
    public class OrgSetting
    {
        public int id { get; set; }
        public int OrgId { get; set; }
        public double MinOrderAmt {get;set;}
        public double DeliveryCharge { get; set; }
        public int OrdCanlMinTime { get; set; }
        public int ByCash { get; set; }//{1:'NO',2:'YES'}
        public int ByOnline { get; set; }//{1:'NO',2:'YES'}

        public int save()
        {
            DBCon con = new DBCon();
            SqlCommand cmd = new SqlCommand();
            try
            {
                if (this.id == 0)
                {
                    cmd = new SqlCommand("insert into OrgSettings values(@OrgId,@MinOrdAmt,@DeleveryCharge,@OrdCanMinTime,@ByCash,@ByOnline); SELECT SCOPE_IDENTITY();", con.Con);
                    cmd.Parameters.AddWithValue("@OrgId", this.OrgId
);
                }
                else
                {
                    cmd = new SqlCommand("update OrgSettings set MinOrdAmt=@MinOrdAmt,DeleveryCharge=@DeleveryCharge,OrdCanMinTime=@OrdCanMinTime,ByCash=@ByCash,ByOnline=@ByOnline where ID=@ID ", con.Con);
                    cmd.Parameters.AddWithValue("@ID", this.id);
                }
                cmd.Parameters.AddWithValue("@MinOrdAmt", this.MinOrderAmt);
                cmd.Parameters.AddWithValue("@DeleveryCharge", this.DeliveryCharge);
                cmd.Parameters.AddWithValue("@OrdCanMinTime", this.OrdCanlMinTime);
                cmd.Parameters.AddWithValue("@ByCash", this.ByCash);
                cmd.Parameters.AddWithValue("@ByOnline", this.ByOnline);
                if (this.id == 0)
                {
                    this.id = Convert.ToInt32(cmd.ExecuteScalar());
                }
                else
                {
                    int i = Convert.ToInt32(cmd.ExecuteNonQuery());
                }

            }
            catch (Exception e)
            {
                e.Message.ToString();
                return 0;
            }
            finally
            {
                cmd.Dispose(); con.Con.Close();
            }
            return this.id;
        }

        public static OrgSetting Getone(int OrgId)
        {

            OrgSetting Temp = new OrgSetting();
            DBCon con = new DBCon();
            SqlCommand cmd = new SqlCommand();
            string query = "select * from OrgSettings where OrgId=@OrgId";
            try
            {
                cmd = new SqlCommand(query, con.Con);
                cmd.Parameters.AddWithValue("@OrgId", OrgId);
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    int index = -1;
                    OrgSetting hG_Ticket = new OrgSetting();
                    hG_Ticket.id = sqlDataReader.GetInt32(++index);
                    hG_Ticket.OrgId = sqlDataReader.GetInt32(++index);
                    hG_Ticket.MinOrderAmt = sqlDataReader.GetDouble(++index);
                    hG_Ticket.DeliveryCharge = sqlDataReader.GetDouble(++index);
                    hG_Ticket.OrdCanlMinTime = sqlDataReader.GetInt32(++index);
                    hG_Ticket.ByCash = sqlDataReader.GetInt32(++index);
                    hG_Ticket.ByOnline = sqlDataReader.GetInt32(++index);
                    Temp = hG_Ticket;
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

            return Temp;
        }

    }
}