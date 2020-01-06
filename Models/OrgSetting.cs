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
        public int EnblDeleryChrg { get; set; }//{0  NO,1 YES}
        public double MinOrderAmt {get;set;}
        public double DeliveryCharge { get; set; }
        public int OrdCanlMinTime { get; set; }
        public int ByCash { get; set; }//{1:'NO',2:'YES'}
        public int ByOnline { get; set; }//{1:'NO',2:'YES'}
        public int AcptMinOrd { get; set; }//{0  NO,1 YES}
        public int DeleryChrgType { get; set; }//{0 : 'minimum thresold',1 :'Fixed '}
        public bool ApplyInCustomerApp { get; set; }// {"false": no == true  yes}
        public bool ApplyInCaptainApp { get; set; }// {"false": no == true yes}
        public bool ApplyInAdminPanel { get; set; }// {"false": no == true yes}
        public int save()
        {
            DBCon con = new DBCon();
            SqlCommand cmd = new SqlCommand();
            try
            {
                if (this.id == 0)
                {
                    cmd = new SqlCommand("insert into OrgSettings values(@OrgId,@MinOrdAmt,@DeleveryCharge,@OrdCanMinTime,@ByCash,@ByOnline,@AcptMinOrd,@EnbDeliChrg,@DeliChrgType,@CustomerApp,@CaptainApp,@AdminPanel); SELECT SCOPE_IDENTITY();", con.Con);
                    cmd.Parameters.AddWithValue("@OrgId", this.OrgId);
                }
                else
                {
                    cmd = new SqlCommand("update OrgSettings set MinOrdAmt=@MinOrdAmt,DeleveryCharge=@DeleveryCharge,OrdCanMinTime=@OrdCanMinTime,ByCash=@ByCash,ByOnline=@ByOnline,AcptMinOrd=@AcptMinOrd,EnbDeliChrg=@EnbDeliChrg,DeliChrgType=@DeliChrgType,CustomerApp=@CustomerApp,CaptainApp=@CaptainApp,AdminPanel=@AdminPanel where ID=@ID ", con.Con);
                    cmd.Parameters.AddWithValue("@ID", this.id);
                }
                cmd.Parameters.AddWithValue("@MinOrdAmt", this.MinOrderAmt);
                cmd.Parameters.AddWithValue("@DeleveryCharge", this.DeliveryCharge);
                cmd.Parameters.AddWithValue("@OrdCanMinTime", this.OrdCanlMinTime);
                cmd.Parameters.AddWithValue("@ByCash", this.ByCash);
                cmd.Parameters.AddWithValue("@ByOnline", this.ByOnline);
                cmd.Parameters.AddWithValue("@AcptMinOrd", this.AcptMinOrd);
                cmd.Parameters.AddWithValue("@EnbDeliChrg", this.EnblDeleryChrg);
                cmd.Parameters.AddWithValue("@DeliChrgType", this.DeleryChrgType);
                cmd.Parameters.AddWithValue("@CustomerApp", this.ApplyInCustomerApp);
                cmd.Parameters.AddWithValue("@CaptainApp", this.ApplyInCaptainApp);
                cmd.Parameters.AddWithValue("@AdminPanel", this.ApplyInAdminPanel);
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
                    hG_Ticket.AcptMinOrd = sqlDataReader.GetInt32(++index);
                    hG_Ticket.EnblDeleryChrg = sqlDataReader.GetInt32(++index);
                    hG_Ticket.DeleryChrgType = sqlDataReader.GetInt32(++index);
                    hG_Ticket.ApplyInCustomerApp = sqlDataReader.GetBoolean(++index);
                    hG_Ticket.ApplyInCaptainApp = sqlDataReader.GetBoolean(++index);
                    hG_Ticket.ApplyInAdminPanel = sqlDataReader.GetBoolean(++index);
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