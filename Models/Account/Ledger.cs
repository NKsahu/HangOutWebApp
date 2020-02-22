using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;


namespace HangOut.Models.Account
{
    public class Ledger
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string MobileNo1 { get; set; }
        public string MobileNo2 { get; set; }
        public int DebtorType { get; set; }
        public int OrgId { get; set; }
        public int State { get; set; }
        public double MarginOnCash { get; set; }
        public int TaxOnAboveMargin { get; set; }
        public double MarginOnline { get; set; }
        public int TaxOnAboveMarginOnline { get; set; }
        public int PaymentFrequency { get; set; }

        public int PaymentDay { get; set; }

        public int CollectionFrequency { get; set; }

        public int CollectionDay { get; set; }

        public DateTime CalculationStartFrom { get; set; }
        public DateTime LisenceRenewalDate { get; set; }
        public int TDSApplicable { get; set; }
        public string Email { get; set; }
        public string Remarks { get; set; }
        public int ParentGroup { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public string BankName { get; set; }
        public string Branch { get; set; }
        public int ManualPaymentDays { get; set; }
        public int ManualCollectionDays { get; set; }


        public Ledger()
        {
          
            ID = 0;
            Name = "";
          
        }




        public int Save()
        {
            int Row = 0;
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Quary = "";
                if (this.ID == 0)
                {
                    Quary = "Insert Into ACLedger Values (@Name,@ShortName,@MobileNo1,@MobileNo2,@DebtorType,@OrgId,@State,@MarginOnCash,@TaxOnAboveMargin,@MarginOnline,@TaxOnAboveMarginOnline,@PaymentFrequency,@PaymentDay,@CollectionFrequency,@CollectionDay,@CalculationStartFrom,@TDSApplicable,@Email,@Remarks,@LisenceRenewalDate,@ParentGroup,@AccountNumber,@IFSCCode,@BankName,@Branch,@ManualPaymentDays,@ManualCollectionDays);SELECT SCOPE_IDENTITY();";
                }
                else
                {
                    Quary = "Update ACLedger Set Name=@Name,ShortName=@ShortName,MobileNo1=@MobileNo1,MobileNo2=@MobileNo2,DebtorType=@DebtorType,OrgId=@OrgId,State=@State,MarginOnCash=@MarginOnCash,TaxOnAboveMargin=@TaxOnAboveMargin,MarginOnline=@MarginOnline,TaxOnAboveMarginOnline=@TaxOnAboveMarginOnline,PaymentFrequency=@PaymentFrequency,PaymentDay=@PaymentDay,CollectionFrequency=@CollectionFrequency,CollectionDay=@CollectionDay,CalculationStartFrom=@CalculationStartFrom,TDSApplicable=@TDSApplicable,Email=@Email,Remarks=@Remarks,@LisenceRenewalDate=LisenceRenewalDate,@AccountNumber=AccountNumber,@IFSCCode=IFSCCode,@BankName=BankName,@Branch=Branch,@ManualPaymentDays=ManualPaymentDays,@ManualCollectionDays=ManualCollectionDays where ID=@ID";
                }
                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@ID", this.ID);
                cmd.Parameters.AddWithValue("@Name", this.Name);
                cmd.Parameters.AddWithValue("@ShortName", this.ShortName);
                cmd.Parameters.AddWithValue("@MobileNo1", this.MobileNo1);
                cmd.Parameters.AddWithValue("@MobileNo2", this.MobileNo2);
                cmd.Parameters.AddWithValue("@DebtorType", this.DebtorType);
                cmd.Parameters.AddWithValue("@OrgId", this.OrgId);
                cmd.Parameters.AddWithValue("@State", this.State);
                cmd.Parameters.AddWithValue("@MarginOnCash", this.MarginOnCash);
                cmd.Parameters.AddWithValue("@TaxOnAboveMargin", this.TaxOnAboveMargin);
                cmd.Parameters.AddWithValue("@MarginOnline", this.MarginOnline);
                cmd.Parameters.AddWithValue("@TaxOnAboveMarginOnline", this.TaxOnAboveMarginOnline);
                cmd.Parameters.AddWithValue("@PaymentFrequency", this.PaymentFrequency);
                cmd.Parameters.AddWithValue("@PaymentDay", this.PaymentDay);
                cmd.Parameters.AddWithValue("@CollectionFrequency", this.CollectionFrequency);
                cmd.Parameters.AddWithValue("@CollectionDay", this.CollectionDay);
                cmd.Parameters.AddWithValue("@CalculationStartFrom", this.CalculationStartFrom);
                cmd.Parameters.AddWithValue("@TDSApplicable", this.TDSApplicable);
                cmd.Parameters.AddWithValue("@Email", this.Email);
                cmd.Parameters.AddWithValue("@Remarks", this.Remarks);
                cmd.Parameters.AddWithValue("@LisenceRenewalDate", this.LisenceRenewalDate);
                cmd.Parameters.AddWithValue("@ParentGroup", this.ParentGroup);
                cmd.Parameters.AddWithValue("@AccountNumber", this.AccountNumber);
                cmd.Parameters.AddWithValue("@IFSCCode", this.IFSCCode);
                cmd.Parameters.AddWithValue("@BankName", this.BankName);
                cmd.Parameters.AddWithValue("@Branch", this.BankName);
                cmd.Parameters.AddWithValue("@ManualPaymentDays", this.ManualPaymentDays);
                cmd.Parameters.AddWithValue("@ManualCollectionDays", this.ManualCollectionDays);

                if (this.ID == 0)
                {
                    Row = Convert.ToInt32(cmd.ExecuteScalar());
                    this.ID = Row;
                }
                else
                {
                    Row = cmd.ExecuteNonQuery();
                    //this.ID = Row;
                }

            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return Row;

        }
        public static List<Ledger> GetAllTheatersList()
        {
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<Ledger> LedgerList = new List<Ledger>();
            try
            {
                string Quary = "Select * from ACLedger where ParentGroup=2 and DebtorType=1 ORDER BY ID DESC";
                cmd = new SqlCommand(Quary, con.Con);
                SDR = cmd.ExecuteReader();

                while (SDR.Read())
                {
                    Ledger OBJLDR = new Ledger();
                    OBJLDR.ID = SDR.GetInt32(0);
                    OBJLDR.Name = SDR.GetString(1);
                    //OBJLDR.ShortName = SDR.GetString(2);
                    //OBJLDR.MobileNo1 = SDR.GetString(3);
                    //OBJLDR.MobileNo2 = SDR.GetString(4);
                    //OBJLDR.DebtorType = SDR.GetInt32(5);
                    //OBJLDR.OrgId = SDR.GetInt32(6);
                    //OBJLDR.State = SDR.GetInt32(7);
                    //OBJLDR.MarginOnCash = SDR.GetDouble(8);
                    //OBJLDR.TaxOnAboveMargin = SDR.GetInt32(9);
                    //OBJLDR.MarginOnline = SDR.GetDouble(10);
                    //OBJLDR.TaxOnAboveMarginOnline = SDR.GetInt32(11);
                    //OBJLDR.PaymentFrequency = SDR.GetInt32(12);
                    //OBJLDR.PaymentDay = SDR.GetInt32(13);
                    //OBJLDR.CollectionFrequency = SDR.GetInt32(14);
                    //OBJLDR.CollectionDay = SDR.GetInt32(15);
                    //OBJLDR.CalculationStartFrom = SDR.GetDateTime(16);
                    //OBJLDR.TDSApplicable = SDR.GetInt32(17);
                    //OBJLDR.Email = SDR.GetString(18);
                    //OBJLDR.Remarks = SDR.GetString(19);
                    //OBJLDR.LisenceRenewalDate = SDR.GetDateTime(20);
                    //OBJLDR.ParentGroup = SDR.GetInt32(21);
                    //OBJLDR.AccountNumber = SDR.GetString(22);
                    //OBJLDR.IFSCCode = SDR.GetString(23);
                    //OBJLDR.BankName = SDR.GetString(24);
                    //OBJLDR.Branch = SDR.GetString(25);

                    LedgerList.Add(OBJLDR);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return (LedgerList);
        }

        public static List<Ledger> GetAll()
        {
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<Ledger> LedgerList = new List<Ledger>();
            try
            {
                string Quary = "Select * from ACLedger where DebtorType=0 ORDER BY ID DESC";
                cmd = new SqlCommand(Quary, con.Con);
                SDR = cmd.ExecuteReader();

                while (SDR.Read())
                {
                    Ledger OBJLDR = new Ledger();
                    OBJLDR.ID = SDR.GetInt32(0);
                    OBJLDR.Name = SDR.GetString(1);
                    OBJLDR.ShortName = SDR.GetString(2);
                    OBJLDR.MobileNo1 = SDR.GetString(3);
                    OBJLDR.MobileNo2 = SDR.GetString(4);
                    OBJLDR.DebtorType = SDR.GetInt32(5);
                    OBJLDR.OrgId = SDR.GetInt32(6);
                    OBJLDR.State = SDR.GetInt32(7);
                    OBJLDR.MarginOnCash = SDR.GetDouble(8);
                    OBJLDR.TaxOnAboveMargin = SDR.GetInt32(9);
                    OBJLDR.MarginOnline = SDR.GetDouble(10);
                    OBJLDR.TaxOnAboveMarginOnline = SDR.GetInt32(11);
                    OBJLDR.PaymentFrequency = SDR.GetInt32(12);
                    OBJLDR.PaymentDay = SDR.GetInt32(13);
                    OBJLDR.CollectionFrequency = SDR.GetInt32(14);
                    OBJLDR.CollectionDay = SDR.GetInt32(15);
                    OBJLDR.CalculationStartFrom = SDR.GetDateTime(16);
                    OBJLDR.TDSApplicable = SDR.GetInt32(17);
                    OBJLDR.Email = SDR.GetString(18);
                    OBJLDR.Remarks = SDR.GetString(19);
                    OBJLDR.LisenceRenewalDate = SDR.GetDateTime(20);
                    OBJLDR.ParentGroup = SDR.GetInt32(21);
                    OBJLDR.AccountNumber = SDR.GetString(22);
                    OBJLDR.IFSCCode = SDR.GetString(23);
                    OBJLDR.BankName = SDR.GetString(24);
                    OBJLDR.Branch = SDR.GetString(25);
                    OBJLDR.ManualPaymentDays = SDR.GetInt32(26);
                    OBJLDR.ManualCollectionDays = SDR.GetInt32(27);

                    LedgerList.Add(OBJLDR);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return (LedgerList);
        }
        public Ledger GetOne(int ID)
        {
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            Ledger OBJLDR = new Ledger();

            try
            {
                string Query = "SELECT * FROM  ACLedger where ID=@ID";
                cmd = new SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("@ID", ID);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                   
                    OBJLDR.ID = SDR.GetInt32(0);
                    OBJLDR.Name = SDR.GetString(1);
                    OBJLDR.ShortName = SDR.GetString(2);
                    OBJLDR.MobileNo1 = SDR.GetString(3);
                    OBJLDR.MobileNo2 = SDR.GetString(4);
                    OBJLDR.DebtorType = SDR.GetInt32(5);
                    OBJLDR.OrgId = SDR.GetInt32(6);
                    OBJLDR.State = SDR.GetInt32(7);
                    OBJLDR.MarginOnCash = SDR.GetDouble(8);
                    OBJLDR.TaxOnAboveMargin = SDR.GetInt32(9);
                    OBJLDR.MarginOnline = SDR.GetDouble(10);
                    OBJLDR.TaxOnAboveMarginOnline = SDR.GetInt32(11);
                    OBJLDR.PaymentFrequency = SDR.GetInt32(12);
                    OBJLDR.PaymentDay = SDR.GetInt32(13);
                    OBJLDR.CollectionFrequency = SDR.GetInt32(14);
                    OBJLDR.CollectionDay = SDR.GetInt32(15);
                    OBJLDR.CalculationStartFrom = SDR.GetDateTime(16);
                    OBJLDR.TDSApplicable = SDR.GetInt32(17);
                    OBJLDR.Email = SDR.GetString(18);
                    OBJLDR.Remarks = SDR.GetString(19);
                    OBJLDR.LisenceRenewalDate = SDR.GetDateTime(20);
                    OBJLDR.ParentGroup = SDR.GetInt32(21);
                    OBJLDR.AccountNumber = SDR.GetString(22);
                    OBJLDR.IFSCCode = SDR.GetString(23);
                    OBJLDR.BankName = SDR.GetString(24);
                    OBJLDR.Branch = SDR.GetString(25);
                    OBJLDR.ManualPaymentDays = SDR.GetInt32(26);
                    OBJLDR.ManualCollectionDays = SDR.GetInt32(27);
                }
            }
            catch (System.Exception e)
            { e.ToString(); }

            finally { cmd.Dispose(); SDR.Close(); Con.Close(); }

            return (OBJLDR);
        }
        public static int Dell(int ID)
        {
            int R = 0;
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            try
            {
                string Query = "Delete FROM  ACLedger where ID=" + ID;
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
        public static List<DebtorType> DTypes()
        {

            List<DebtorType> type = new List<DebtorType>();
            type.Add(new DebtorType { Id = 1, Name = "Organization" });
            type.Add(new DebtorType { Id = 2, Name = "Usual" });
        
            return type;
        }
        public static List<PaymentFrequency> PFrequency()
        {

            List<PaymentFrequency> ptype = new List<PaymentFrequency>();
            ptype.Add(new PaymentFrequency { Id = 1, Name = "Daily" });
            ptype.Add(new PaymentFrequency { Id = 2, Name = "Weekly" });
            ptype.Add(new PaymentFrequency { Id = 3, Name = "Manual" });

            return ptype;
        }
        public static List<PaymentWeekly> WeekDays()
        {

            List<PaymentWeekly> wtype = new List<PaymentWeekly>();
            wtype.Add(new PaymentWeekly { Id = 0, Name = "Sunday" });
            wtype.Add(new PaymentWeekly { Id = 1, Name = "Monday" });
            wtype.Add(new PaymentWeekly { Id = 2, Name = "TuesDay" });
            wtype.Add(new PaymentWeekly { Id = 3, Name = "WednesDay" });
            wtype.Add(new PaymentWeekly { Id = 4, Name = "ThusDay" });
            wtype.Add(new PaymentWeekly { Id = 5, Name = "Friday" });
            wtype.Add(new PaymentWeekly { Id = 6, Name = "SaturDay" });
         

            return wtype;
        }
        public static List<CollectionFrequency> CFrequency()
        {

            List<CollectionFrequency> ptype = new List<CollectionFrequency>();
            ptype.Add(new CollectionFrequency { Id = 1, Name = "Daily" });
            ptype.Add(new CollectionFrequency { Id = 2, Name = "Weekly" });
            ptype.Add(new CollectionFrequency { Id = 3, Name = "Manual" });

            return ptype;
        }
        public static List<CollectionWeekly> CWeekDays()
        {

            List<CollectionWeekly> wtype = new List<CollectionWeekly>();
            wtype.Add(new CollectionWeekly { Id = 0, Name = "Sunday" });
            wtype.Add(new CollectionWeekly { Id = 1, Name = "Monday" });
            wtype.Add(new CollectionWeekly { Id = 2, Name = "TuesDay" });
            wtype.Add(new CollectionWeekly { Id = 3, Name = "WednesDay" });
            wtype.Add(new CollectionWeekly { Id = 4, Name = "ThusDay" });
            wtype.Add(new CollectionWeekly { Id = 5, Name = "Friday" });
            wtype.Add(new CollectionWeekly { Id = 6, Name = "SaturDay" });


            return wtype;
        }
        public static List<TdsApplicable> TDS()
        {

            List<TdsApplicable> ptype = new List<TdsApplicable>();
            ptype.Add(new TdsApplicable { Id = 1, Name = "Yes" });
            ptype.Add(new TdsApplicable { Id = 2, Name = "No" });


            return ptype;
        }
        
    }
}
public class DebtorType
{
    public int Id { get; set; }
    public string Name { get; set; }
}
public class PaymentFrequency
{
    public int Id { get; set; }
    public string Name { get; set; }
}
public class PaymentWeekly
{
    public int Id { get; set; }
    public string Name { get; set; }
}
public class CollectionFrequency
{
    public int Id { get; set; }
    public string Name { get; set; }
}
public class CollectionWeekly
{
    public int Id { get; set; }
    public string Name { get; set; }
}
public class TdsApplicable
{
    public int Id { get; set; }
    public string Name { get; set; }
}
