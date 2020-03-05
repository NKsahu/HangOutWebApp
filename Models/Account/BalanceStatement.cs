using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
namespace HangOut.Models.Account
{
    public class BalanceStatement
    {
        public int BID { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public string Narration { get; set; }
        public long OrderId { get; set; }
        public int OrgId { get; set; }
        public double CRAmount { get; set; }
        public int EntryNo { get; set; }
        public double Balance { get; set; }
        public bool isCash { get; set; }
        public double TaxOnCash { get; set; }
        public double TaxOnOnline { get; set; }


        public int Save()
        {
            int Row = 0;
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            if (this.isCash == false)
            {
                try
            {
               
                    string Quary = "";
                    if (this.BID == 0)
                    {
                        Quary = "Insert Into ACBalanceStatement Values (@Date,@Amount,@Narration,@OrderId,@OrgId,@CRAmount,@EntryNo,@Balance,@TaxOnCash,@TaxOnOnline);SELECT SCOPE_IDENTITY();";
                    }
                    else
                    {
                        Quary = "Update ACBalanceStatement Set Date=@Date,Amount=@Amount,Narration=@Narration,OrderId=@OrderId,OrgId=@OrgId,CRAmount=@CRAmount,EntryNo=@EntryNo,Balance=@Balance,TaxOnCash=@TaxOnCash,TaxOnOnline=@TaxOnOnline where ID=@ID";
                    }
                    cmd = new SqlCommand(Quary, con.Con);
                    cmd.Parameters.AddWithValue("@BID", this.BID);
                    cmd.Parameters.AddWithValue("@Date", this.Date);
                    cmd.Parameters.AddWithValue("@Amount", this.Amount);
                    cmd.Parameters.AddWithValue("@Narration", this.Narration);
                    cmd.Parameters.AddWithValue("@OrderId", this.OrderId);
                    cmd.Parameters.AddWithValue("@OrgId", this.OrgId);
                    cmd.Parameters.AddWithValue("@CRAmount", this.CRAmount);
                    cmd.Parameters.AddWithValue("@EntryNo", this.EntryNo);
                    cmd.Parameters.AddWithValue("@Balance", this.Balance);
                    cmd.Parameters.AddWithValue("@TaxOnCash", this.TaxOnCash);
                    cmd.Parameters.AddWithValue("@TaxOnOnline", this.TaxOnOnline);
                    if (this.BID == 0)
                    {
                        Row = Convert.ToInt32(cmd.ExecuteScalar());
                        this.BID = Row;
                    }
                    else
                    {
                        Row = cmd.ExecuteNonQuery();
                        //this.CategoryID = Row;
                    }



                }
                catch (Exception e) { e.ToString(); }
                finally { cmd.Dispose(); con.Con.Close(); }
                BalanceStatement Obj = new BalanceStatement();
                HG_Orders ord = new HG_Orders().GetOne(OrderId);
                Ledger LedgerDetails = Ledger.GetAllList().Where(x => x.DebtorType == 1
                        && x.OrgId == OrgId).FirstOrDefault();



                if (ord.PaymentStatus == 1 || ord.PaymentStatus == 2)
                {
                    double amt = (Amount * LedgerDetails.MarginOnCash) / 100;
                    Obj.CRAmount = (amt) +((amt * LedgerDetails.TaxOnAboveMargin) / 100);
                    Obj.TaxOnCash = LedgerDetails.TaxOnAboveMargin;
                }
                else if (ord.PaymentStatus == 3)
                {
                    double amt = (Amount * LedgerDetails.MarginOnline) / 100;
                    Obj.CRAmount = (amt) + ((amt * LedgerDetails.TaxOnAboveMarginOnline) / 100);
                    Obj.TaxOnOnline = LedgerDetails.TaxOnAboveMarginOnline;
                }
                Obj.Date = Date;
               
               // Obj.Amount = Amount;
                BalanceStatement TotalBalance = BalanceStatement.GetAllForBalanceCalculation(OrgId).Last();
                Obj.Balance = TotalBalance.Balance - Obj.CRAmount;
                Obj.OrgId = OrgId;
                Obj.OrderId = OrderId;
                Obj.Narration = "Commission of Order No." + OrderId;
                Obj.SaveCRValue();
            }
           
            else
            {
                BalanceStatement Obj = new BalanceStatement();
                HG_Orders ord = new HG_Orders().GetOne(OrderId);
                Ledger LedgerDetails = Ledger.GetAllList().Where(x => x.DebtorType == 1
                        && x.OrgId == OrgId).FirstOrDefault();

                if (ord.PaymentStatus == 1 || ord.PaymentStatus == 2)
                {
                    double amt = (Amount * LedgerDetails.MarginOnCash) / 100;
                    Obj.CRAmount = (amt) + ((amt * LedgerDetails.TaxOnAboveMargin) / 100);
                    Obj.TaxOnCash = LedgerDetails.TaxOnAboveMargin;
                }
                else if (ord.PaymentStatus == 3)
                {
                    double amt = (Amount * LedgerDetails.MarginOnline) / 100;
                    Obj.CRAmount = (amt) + ((amt * LedgerDetails.TaxOnAboveMarginOnline) / 100);
                    Obj.TaxOnOnline = LedgerDetails.TaxOnAboveMarginOnline;
                }

                Obj.Date = Date;
                //Obj.Amount = Amount;
                BalanceStatement TotalBalance = BalanceStatement.GetAllForBalanceCalculation(OrgId).Last();
                Obj.Balance = TotalBalance.Balance - Obj.CRAmount;
                Obj.OrgId = OrgId;
                Obj.OrderId = OrderId;
                Obj.Narration = "Commission of Order No." + OrderId;
                Obj.SaveCRValue();
            }
           
            return Row;

        }

        public int SaveCRValue()
        {
            int Row = 0;
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Quary = "";
            
                Quary = "Insert Into ACBalanceStatement Values (@Date,@Amount,@Narration,@OrderId,@OrgId,@CRAmount,@EntryNo,@Balance,@TaxOnCash,@TaxOnOnline);SELECT SCOPE_IDENTITY();";
                
              
                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@BID", this.BID);
                cmd.Parameters.AddWithValue("@Date", this.Date);
                cmd.Parameters.AddWithValue("@Amount", this.Amount);
                cmd.Parameters.AddWithValue("@Narration", this.Narration);
                cmd.Parameters.AddWithValue("@OrderId", this.OrderId);
                cmd.Parameters.AddWithValue("@OrgId", this.OrgId);
                cmd.Parameters.AddWithValue("@CRAmount", this.CRAmount);
                cmd.Parameters.AddWithValue("@EntryNo", this.EntryNo);
                cmd.Parameters.AddWithValue("@Balance", this.Balance);
                cmd.Parameters.AddWithValue("@TaxOnCash", this.TaxOnCash);
                cmd.Parameters.AddWithValue("@TaxOnOnline", this.TaxOnOnline);

                Row = Convert.ToInt32(cmd.ExecuteScalar());
                 this.BID = Row;


           
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return Row;

        }
        public int SaveOpeningValue()
        {
            int Row = 0;
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Quary = "";

                Quary = "Insert Into ACBalanceStatement Values (@Date,@Amount,@Narration,@OrderId,@OrgId,@CRAmount,@EntryNo,@Balance,@TaxOnCash,@TaxOnOnline);SELECT SCOPE_IDENTITY();";


                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@BID", this.BID);
                cmd.Parameters.AddWithValue("@Date", this.Date);
                cmd.Parameters.AddWithValue("@Amount", this.Amount);
                cmd.Parameters.AddWithValue("@Narration", this.Narration);
                cmd.Parameters.AddWithValue("@OrderId", this.OrderId);
                cmd.Parameters.AddWithValue("@OrgId", this.OrgId);
                cmd.Parameters.AddWithValue("@CRAmount", this.CRAmount);
                cmd.Parameters.AddWithValue("@EntryNo", this.EntryNo);
                cmd.Parameters.AddWithValue("@Balance", this.Balance);
                cmd.Parameters.AddWithValue("@TaxOnCash", this.TaxOnCash);
                cmd.Parameters.AddWithValue("@TaxOnOnline", this.TaxOnOnline);

                Row = Convert.ToInt32(cmd.ExecuteScalar());
                this.BID = Row;



            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return Row;

        }
        public static List<BalanceStatement> GetAllForBalanceCalculation(int OrgId)
        {
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<BalanceStatement> BalanceStatementList = new List<BalanceStatement>();
          // BalanceStatement OBJBS = new BalanceStatement();
            try
            {
                string Quary = "Select * from ACBalanceStatement where OrgId=@OrgId ORDER BY Date ASC";
                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@OrgId", OrgId);
                SDR = cmd.ExecuteReader();

                while (SDR.Read())
                {
                    BalanceStatement OBJBS = new BalanceStatement();
                    OBJBS.BID = SDR.GetInt32(0);
                    OBJBS.Date = SDR.GetDateTime(1);
                    OBJBS.Amount = SDR.GetDouble(2);
                    OBJBS.Narration = SDR.GetString(3);
                    OBJBS.OrderId = SDR.GetInt32(4);
                    OBJBS.OrgId = SDR.GetInt32(5);
                    OBJBS.CRAmount = SDR.GetDouble(6);
                    OBJBS.EntryNo = SDR.GetInt32(7);
                    OBJBS.Balance = SDR.GetDouble(8);
                    OBJBS.TaxOnCash = SDR.GetDouble(9);
                    OBJBS.TaxOnOnline = SDR.GetDouble(10);
                    BalanceStatementList.Add(OBJBS);
                }

            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return (BalanceStatementList);
        }
        public static List<BalanceStatement> GetAllList()
        {
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<BalanceStatement> BalanceStatementList = new List<BalanceStatement>();
          
            try
            {
                string Quary = "Select * from ACBalanceStatement ORDER BY Date ASC";
                cmd = new SqlCommand(Quary, con.Con);
                SDR = cmd.ExecuteReader();

                while (SDR.Read())
                {
                    BalanceStatement OBJBS = new BalanceStatement();
                    OBJBS.BID = SDR.GetInt32(0);
                    OBJBS.Date = SDR.GetDateTime(1);
                    OBJBS.Amount = SDR.GetDouble(2);
                    OBJBS.Narration = SDR.GetString(3);
                    OBJBS.OrderId = SDR.GetInt32(4);
                    OBJBS.OrgId = SDR.GetInt32(5);
                    OBJBS.CRAmount = SDR.GetDouble(6);
                    OBJBS.EntryNo = SDR.GetInt32(7);
                    OBJBS.Balance = SDR.GetDouble(8);
                    OBJBS.TaxOnCash = SDR.GetDouble(9);
                    OBJBS.TaxOnOnline = SDR.GetDouble(10);
                    BalanceStatementList.Add(OBJBS);
                }

            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return (BalanceStatementList);
        }
        public static List<BalanceStatement> GetByOrgId(int OrgId)
        {
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<BalanceStatement> BalanceStatementList = new List<BalanceStatement>();
            try
            {
                string Quary = "Select * from ACBalanceStatement where OrgId=" + OrgId+ " ORDER BY Date ASC"; 
                cmd = new SqlCommand(Quary, con.Con);
                SDR = cmd.ExecuteReader();

                while (SDR.Read())
                {
                    BalanceStatement OBJBS = new BalanceStatement();
                    OBJBS.BID = SDR.GetInt32(0);
                    OBJBS.Date = SDR.GetDateTime(1);
                    OBJBS.Amount = SDR.GetDouble(2);
                    OBJBS.Narration = SDR.GetString(3);
                    OBJBS.OrderId = SDR.GetInt32(4);
                    OBJBS.OrgId = SDR.GetInt32(5);
                    OBJBS.CRAmount = SDR.GetDouble(6);
                    OBJBS.EntryNo = SDR.GetInt32(7);
                    OBJBS.Balance = SDR.GetDouble(8);
                    BalanceStatementList.Add(OBJBS);
                }

            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return (BalanceStatementList);
        }
        public static int Dell(int ID)
        {
            int R = 0;
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            try
            {
                string Query = "Delete FROM  ACBalanceStatement where OrgId=" + ID;
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
    }

}  