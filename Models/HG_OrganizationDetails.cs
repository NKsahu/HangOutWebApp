using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models
{
    public class HG_OrganizationDetails
    {
        
        public int OrgID { get; set; }
        public string OrgTypes{ get; set; }//1 mensa restuarant 2 means threater
        public string HeadName{ get; set; }
        public string Name{ get; set; }
        public string Address{ get; set; }// address 1
        public string City{ get; set; }
        public string State{ get; set; }
        public string PinCode{ get; set; }
        public string Phone{ get; set; }
        public string Cell { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
        public string Logo { get; set; }
        public DateTime DOR{ get; set; }
        public DateTime DOE { get; set; }
        public string GSTNO { get; set; }//license1
        public string PANNO { get; set; }
        public string BankName{ get; set; }
        public string ACNO{ get; set; }
        public string AcType { get; set; }
        public int EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool Status { get; set; } 
        public int PaymentType { get; set; }// {'1':'prepaid','2':'postpaid'}
        public string IvoiceHeading { get; set; }
        public string AddressLin2 { get; set; }
        public string AddressLine3 { get; set; }
        public string Licence2 { get; set; }
        public string License3 { get; set; }
        public string PrintRemark { get; set; }
        public bool CustomerOrdering { get; set; }// true means enable ordering else  Ordering not Allowed
        public string InvoiceTitle { get; set; }
        public string invoicePhone { get; set; }
        public int DistrictId { get; set; }
        public HG_OrganizationDetails()
        {
            EntryDate = DateTime.Now;
            UpdateDate = DateTime.Now;
            EntryBy = 0;
            PaymentType = 1;
            IvoiceHeading = "";
            Address= "";
            AddressLin2 = "";
            AddressLine3 = "";
            Status = true;
            this.Logo = "";
        }
        public int Save()
        {
            int Row = 0;
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            try
            {
                Con.Open();
                SqlCommand cmd = null;
                string Query = "";
                if (this.OrgID  == 0)
                {
                    Query = "Insert into  HG_OrganizationDetails  values(@OrgTypes,@HeadName,@Name,@Address,@City,@State,@PinCode,@Phone,@Cell,@Email,@WebSite,@Logo,@DOR,@DOE,@GSTNO,@PANNO,@BankName,@ACNO,@AcType,@EntryBy,@EntryDate,@UpdateDate,@Status,@PaymentType,@InvoiceHeading,@AddressLine2,@AddressLin3,@License2,@License3,@PrintRemark,@CustomerOrdering,@InvoiceTitle,@InvoicePhone,@DistrictId);";
                    cmd = new SqlCommand(Query, Con);
                    cmd.Parameters.AddWithValue("@EntryBy",int.Parse(HttpContext.Current.Request.Cookies["UserInfo"]["UserCode"]));
                    cmd.Parameters.AddWithValue("@EntryDate",System.DateTime.Now);
                    cmd.Parameters.AddWithValue("@OrgTypes", this.OrgTypes);
                }
                else
                {
                    Query = "update  HG_OrganizationDetails set HeadName =@HeadName,Name=@Name,Address=@Address,City=@City,State=@State,PinCode=@PinCode,Phone=@Phone,Cell=@Cell,Email=@Email,WebSite=@WebSite,Logo=@Logo,DOR=@DOR,DOE=@DOE,GSTNO=@GSTNO,PANNO=@PANNO,BankName=@BankName,ACNO=@ACNO,AcType=@AcType,UpdateDate=@UpdateDate,Status=@Status,PaymentType=@PaymentType,InvoiceHeading=@InvoiceHeading,AddressLine2=@AddressLine2,AddressLin3=@AddressLin3,License2=@License2,License3=@License3,PrintRemark=@PrintRemark,CustomerOrdering=@CustomerOrdering,InvoiceTitle=@InvoiceTitle,InvoicePhone=@InvoicePhone,DistrictId=@DistrictId where OrgID =@OrgID ";
                    cmd = new SqlCommand(Query, Con);
                    cmd.Parameters.AddWithValue("@OrgID ", this.OrgID );
                }
                cmd.Parameters.AddWithValue("@UpdateDate", System.DateTime.Now);
                cmd.Parameters.AddWithValue("@HeadName", this.HeadName);
                cmd.Parameters.AddWithValue("@Name", this.Name);
                cmd.Parameters.AddWithValue("@Address", this.Address);
                cmd.Parameters.AddWithValue("@City", this.City);
                cmd.Parameters.AddWithValue("@State ", this.State);
                cmd.Parameters.AddWithValue("@PinCode ", this.PinCode);
                cmd.Parameters.AddWithValue("@Phone ", this.Phone);
                cmd.Parameters.AddWithValue("@Cell ", this.Cell);
                cmd.Parameters.AddWithValue("@Email ", this.Email);
                cmd.Parameters.AddWithValue("@WebSite ", this.WebSite);
                cmd.Parameters.AddWithValue("@Logo ", this.Logo);
                cmd.Parameters.AddWithValue("@DOR ", System.DateTime.Now);
                cmd.Parameters.AddWithValue("@DOE", System.DateTime.Now);
                cmd.Parameters.AddWithValue("@GSTNO", this.GSTNO);
                cmd.Parameters.AddWithValue("@PANNO", this.PANNO);
                cmd.Parameters.AddWithValue("@BankName", this.BankName);
                cmd.Parameters.AddWithValue("@ACNO", this.ACNO);
                cmd.Parameters.AddWithValue("@AcType", this.AcType);
                cmd.Parameters.AddWithValue("@Status", this.Status);
                cmd.Parameters.AddWithValue("@PaymentType", this.PaymentType);
                cmd.Parameters.AddWithValue("@InvoiceHeading", this.IvoiceHeading);
                cmd.Parameters.AddWithValue("@AddressLine2", this.AddressLin2);
                cmd.Parameters.AddWithValue("@AddressLin3", this.AddressLine3);
                cmd.Parameters.AddWithValue("@License2", this.Licence2);
                cmd.Parameters.AddWithValue("@License3", this.License3);
                cmd.Parameters.AddWithValue("@CustomerOrdering", this.CustomerOrdering);
                cmd.Parameters.AddWithValue("@PrintRemark", this.PrintRemark);
                cmd.Parameters.AddWithValue("@InvoiceTitle", this.InvoiceTitle);
                cmd.Parameters.AddWithValue("@InvoicePhone", this.invoicePhone);
                cmd.Parameters.AddWithValue("@DistrictId", this.DistrictId);
                Row = cmd.ExecuteNonQuery();
                this.OrgID  = Row;
            }
            catch (Exception e) { e.ToString(); }
            finally { Con.Close(); }
            return Row;
        }
        public List<HG_OrganizationDetails> GetAll(int Orgid=0)
        {
            var CurrOrgID = HttpContext.Current.Request.Cookies["UserInfo"];
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<HG_OrganizationDetails> ListTmp = new List<HG_OrganizationDetails>();
            string Query = "SELECT * FROM  HG_OrganizationDetails ORDER BY OrgID  DESC";
            if (OrgID > 0)
            {
                Query = "SELECT * FROM  HG_OrganizationDetails where OrgID="+OrgID.ToString()+"  ORDER BY OrgID  DESC";
            }else if(CurrOrgID!=null && int.Parse(CurrOrgID["OrgId"])>0)
            {
                Query = "SELECT * FROM  HG_OrganizationDetails where OrgID=" + CurrOrgID["OrgId"] + "  ORDER BY OrgID  DESC";
            }
            try
            {
                cmd = new SqlCommand(Query, Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    HG_OrganizationDetails ObjTmp = new HG_OrganizationDetails();
                    ObjTmp.OrgID  = SDR.GetInt32(0);
                    ObjTmp.OrgTypes = SDR.GetString(1); 
                    ObjTmp.HeadName = SDR.GetString(2); 
                    ObjTmp.Name = SDR.GetString(3); 
                    ObjTmp.Address = SDR.GetString(4); 
                    ObjTmp.City = SDR.GetString(5); 
                    ObjTmp.State = SDR.GetString(6); 
                    ObjTmp.PinCode = SDR.GetString(7); 
                    ObjTmp.Phone = SDR.GetString(8); 
                    ObjTmp.Cell = SDR.GetString(9); 
                    ObjTmp.Email = SDR.GetString(10); 
                    ObjTmp.WebSite = SDR.GetString(11); 
                    ObjTmp.Logo = SDR.GetString(12); 
                    ObjTmp.GSTNO = SDR.GetString(15); 
                    ObjTmp.PANNO = SDR.GetString(16); 
                    ObjTmp.BankName = SDR.GetString(17); 
                    ObjTmp.ACNO = SDR.GetString(18); 
                    ObjTmp.AcType = SDR.GetString(19); 
                    ObjTmp.Status = SDR.GetBoolean(23);
                    ObjTmp.PaymentType =SDR.IsDBNull(24)?1: SDR.GetInt32(24);
                    ObjTmp.IvoiceHeading= SDR.IsDBNull(25) ? "  " : SDR.GetString(25);
                    ObjTmp.AddressLin2 = SDR.IsDBNull(26) ? "  " : SDR.GetString(26);
                    ObjTmp.AddressLine3 = SDR.IsDBNull(27) ? "  " : SDR.GetString(27);
                    ObjTmp.Licence2 = SDR.IsDBNull(28) ? "  " : SDR.GetString(28);
                    ObjTmp.License3 = SDR.IsDBNull(29) ? "  " : SDR.GetString(29);
                    ObjTmp.PrintRemark = SDR.GetString(30);
                    ObjTmp.CustomerOrdering = SDR.GetBoolean(31);
                    ObjTmp.InvoiceTitle = SDR.GetString(32);
                    ObjTmp.invoicePhone = SDR.GetString(33);
                    ObjTmp.DistrictId = SDR.GetInt32(34);
                    ListTmp.Add(ObjTmp);
                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { Con.Close(); }

            return (ListTmp);
        }
        public HG_OrganizationDetails GetOne(int OrgID )
        {
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            HG_OrganizationDetails ObjTmp = new HG_OrganizationDetails();

            try
            {
                string Query = "SELECT * FROM  HG_OrganizationDetails where OrgID =@OrgID ";
                cmd = new SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("@OrgID ", OrgID );
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    ObjTmp.OrgID = SDR.GetInt32(0);
                    ObjTmp.OrgTypes = SDR.GetString(1);
                    ObjTmp.HeadName = SDR.GetString(2);
                    ObjTmp.Name = SDR.GetString(3);
                    ObjTmp.Address = SDR.GetString(4);
                    ObjTmp.City = SDR.GetString(5);
                    ObjTmp.State = SDR.GetString(6);
                    ObjTmp.PinCode = SDR.GetString(7);
                    ObjTmp.Phone = SDR.GetString(8);
                    ObjTmp.Cell = SDR.GetString(9);
                    ObjTmp.Email = SDR.GetString(10);
                    ObjTmp.WebSite = SDR.GetString(11);
                    ObjTmp.Logo = SDR.GetString(12);
                    ObjTmp.GSTNO = SDR.GetString(15);
                    ObjTmp.PANNO = SDR.GetString(16);
                    ObjTmp.BankName = SDR.GetString(17);
                    ObjTmp.ACNO = SDR.GetString(18);
                    ObjTmp.AcType = SDR.GetString(19);
                    ObjTmp.Status = SDR.GetBoolean(23);
                    ObjTmp.PaymentType = SDR.IsDBNull(24) ? 1 : SDR.GetInt32(24);
                    ObjTmp.IvoiceHeading = SDR.IsDBNull(25) ? " " : SDR.GetString(25);
                    ObjTmp.AddressLin2 = SDR.IsDBNull(26) ? " " : SDR.GetString(26);
                    ObjTmp.AddressLine3 = SDR.IsDBNull(27) ? " " : SDR.GetString(27);
                    ObjTmp.Licence2 = SDR.IsDBNull(28) ? " " : SDR.GetString(28);
                    ObjTmp.License3 = SDR.IsDBNull(29) ? " " : SDR.GetString(29);
                    ObjTmp.PrintRemark = SDR.GetString(30);
                    ObjTmp.CustomerOrdering = SDR.GetBoolean(31);
                    ObjTmp.InvoiceTitle = SDR.GetString(32);
                    ObjTmp.invoicePhone = SDR.GetString(33);
                    ObjTmp.DistrictId = SDR.GetInt32(34);
                }
            }
            catch (System.Exception e)
            { e.ToString(); }

            finally { Con.Close(); }

            return (ObjTmp);
        }
        public int Dell(int ID)
        {
            int R = 0;
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            try
            {
                string Query = "Delete FROM  HG_OrganizationDetails where OrgID =" + ID;
                cmd = new SqlCommand(Query, Con);
                R = cmd.ExecuteNonQuery();
            }
            catch (System.Exception e)
            { e.ToString(); }

            finally
            {
                Con.Close();
            }
            return R;
        }
        public static int MyCustomerCnt(int OrgId)
        {
            string Query = "SELECT COUNT(Distinct(CID)) FROM HG_Orders where orgid ="+OrgId+" and CID in(select UserCode from HG_UsersDetails where UserType = 'CUST') ";
            int Count = 0;
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            DBCon Obj = new DBCon();
            try
            {
                cmd = new SqlCommand(Query, Obj.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    Count = SDR.GetInt32(0);

                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); SDR.Close(); Obj.Con.Close(); Obj.Con.Dispose(); Obj.Con = null; }
            return Count;
        }
    }
}