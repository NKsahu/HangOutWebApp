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
        public int ByCash { get; set; }//{0,2:'NO',1:'YES'}
        public int ByOnline { get; set; }//{0,2:'NO',1:'YES'}
        public int AcptMinOrd { get; set; }//{0  NO,1 YES}
        public int DeleryChrgType { get; set; }//{0 : 'minimum thresold',1 :'Fixed '}
        public bool ApplyInCustomerApp { get; set; }// {"false": no == true  yes}
        public bool ApplyInCaptainApp { get; set; }// {"false": no == true yes}
        public bool ApplyInAdminPanel { get; set; }// {"false": no == true yes}
        public string ContactHead1 { get; set; }
        public string Contact1 { get; set; }
        public string ContacHead2 { get; set; }
        public string Contact2 { get; set; }
        public int CrxVerification { get; set; }//customer cross verification { 0:"NO",1:"By-Otp",2: "By-Camera"
        public bool CheckBoxStatus { get; set; }
        //====printer
       
        public OrgSetting()
        {
            CheckBoxStatus = true;
        }
        public int save()
        {
            DBCon con = new DBCon();
            SqlCommand cmd = new SqlCommand();
            try
            {
                if (this.id == 0)
                {
                    cmd = new SqlCommand("insert into OrgSettings values(@OrgId,@MinOrdAmt,@DeleveryCharge,@OrdCanMinTime,@ByCash,@ByOnline,@AcptMinOrd,@EnbDeliChrg,@DeliChrgType,@CustomerApp,@CaptainApp,@AdminPanel,@ContactH1,@Contact1,@ContactH2,@Contact2,@CrxVerification,@CheckBoxStatus); SELECT SCOPE_IDENTITY();", con.Con);
                    cmd.Parameters.AddWithValue("@OrgId", this.OrgId);
                }
                else
                {
                    cmd = new SqlCommand("update OrgSettings set MinOrdAmt=@MinOrdAmt,DeleveryCharge=@DeleveryCharge,OrdCanMinTime=@OrdCanMinTime,ByCash=@ByCash,ByOnline=@ByOnline,AcptMinOrd=@AcptMinOrd,EnbDeliChrg=@EnbDeliChrg,DeliChrgType=@DeliChrgType,CustomerApp=@CustomerApp,CaptainApp=@CaptainApp,AdminPanel=@AdminPanel,ContactH1=@ContactH1,Contact1=@Contact1,ContactH2=@ContactH2,Contact2=@Contact2,CrxVerification=@CrxVerification,CheckBoxStatus=@CheckBoxStatus where ID=@ID ", con.Con);
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
                cmd.Parameters.AddWithValue("@ContactH1", this.ContactHead1);
                cmd.Parameters.AddWithValue("@Contact1", this.Contact1);
                cmd.Parameters.AddWithValue("@ContactH2", this.ContacHead2);
                cmd.Parameters.AddWithValue("@Contact2", this.Contact2);
                cmd.Parameters.AddWithValue("@CrxVerification", this.CrxVerification);
                cmd.Parameters.AddWithValue("@CheckBoxStatus", this.CheckBoxStatus);
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
                    hG_Ticket.ContactHead1 = sqlDataReader.GetString(++index);
                    hG_Ticket.Contact1 = sqlDataReader.GetString(++index);
                    hG_Ticket.ContacHead2 = sqlDataReader.GetString(++index);
                    hG_Ticket.Contact2 = sqlDataReader.GetString(++index);
                    hG_Ticket.CrxVerification = sqlDataReader.GetInt32(++index);
                    hG_Ticket.CheckBoxStatus = sqlDataReader.GetBoolean(++index);
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
        public static List<InvoicePtrSetting> GetPtrSetting()
        {
            List<InvoicePtrSetting> invoicePtrSetting = new List<InvoicePtrSetting>();
            invoicePtrSetting.Add(new InvoicePtrSetting { Id = 0, Name = "No" });
            invoicePtrSetting.Add(new InvoicePtrSetting { Id = 1, Name = "Ask" });
            invoicePtrSetting.Add(new InvoicePtrSetting { Id = 2, Name = "Auto" });
            return invoicePtrSetting;
        }
    }
    
}
public class InvoicePtrSetting
{
   public int Id { get; set; }
    public string Name { get; set; }
}