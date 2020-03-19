using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;


namespace HangOut.Models.Account
{
    public class Receipt
    {
        public int ID { get; set; }
        public int LID { get; set; }
        public double Amount { get; set; }
        public string Particular { get; set; }
        public string ReceiptType { get; set; }
        public int CRGroupId { get; set; }
        public int DRGroupId { get; set; }
        public int CRLedgerId { get; set; }
        public int DRLedgerId { get; set; }
        public int BalanceStatementId { get; set; }
        public DateTime Date { get; set; }
        public int OrgId { get; set; }
        public int EntryNo { get; set; }
        public string ReceiptNo { get; set; }
        public string Name { get; set; }
        public double Balance { get; set; }
        public double CRAmount { get; set; }
        public double DrAmount { get; set; }





        public int Save()
        {
            int Row = 0;
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Quary = "";

                Quary = "Insert Into ACReceipt Values (@Particular,@Amount,@CRGroupId,@CRLedgerId,@BalanceStatementId,@Date,@OrgId,@DRLedgerId,@EntryNo,@Balance,@DRGroupId);SELECT SCOPE_IDENTITY();";


                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@ID", this.ID);
                cmd.Parameters.AddWithValue("@Particular", this.Particular);
                cmd.Parameters.AddWithValue("@Amount", this.Amount);
                cmd.Parameters.AddWithValue("@CRGroupId", this.CRGroupId);
                cmd.Parameters.AddWithValue("@CRLedgerId", this.CRLedgerId);
                cmd.Parameters.AddWithValue("@BalanceStatementId", this.BalanceStatementId);
                cmd.Parameters.AddWithValue("@Date", this.Date);
                cmd.Parameters.AddWithValue("@OrgId", this.OrgId);
                cmd.Parameters.AddWithValue("@DRLedgerId", this.DRLedgerId);
                cmd.Parameters.AddWithValue("@EntryNo", this.EntryNo);
                cmd.Parameters.AddWithValue("@Balance", this.Balance);
                cmd.Parameters.AddWithValue("@DRGroupId", this.DRGroupId);

                Row = Convert.ToInt32(cmd.ExecuteScalar());
                this.ID = Row;



            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return Row;

        }
        public static List<Receipt> GetAllList(int OrgId,int ID)
        {
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<Receipt> ReceiptList = new List<Receipt>();

            try
            {
                string Quary = "Select * from ACReceipt ORDER BY Date ASC";

                if(OrgId>0)
                {
                  Quary = "Select * from ACReceipt where OrgId="+OrgId+" ORDER BY Date ASC";
                }
                if (ID > 0)
                {
                    Quary = "Select * from ACReceipt where ID=" + ID + " ORDER BY Date ASC";
                }
                cmd = new SqlCommand(Quary, con.Con);
                SDR = cmd.ExecuteReader();

                while (SDR.Read())
                {
                    Receipt OBJBS = new Receipt();
                    OBJBS.ID = SDR.GetInt32(0);
                    OBJBS.Particular = SDR.GetString(1);
                    OBJBS.Amount = SDR.GetDouble(2);
                    OBJBS.CRGroupId = SDR.GetInt32(3);
                    OBJBS.CRLedgerId = SDR.GetInt32(4);
                    OBJBS.BalanceStatementId = SDR.GetInt32(5);
                    OBJBS.Date = SDR.GetDateTime(6);
                    OBJBS.OrgId = SDR.GetInt32(7);
                    OBJBS.DRLedgerId = SDR.GetInt32(8);
                    OBJBS.EntryNo = SDR.GetInt32(9);
                    OBJBS.Balance = SDR.GetDouble(10);
                    OBJBS.DRGroupId = SDR.GetInt32(11);
                    ReceiptList.Add(OBJBS);
                }

            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return (ReceiptList);
        }

        public static int Dell(int ID)
        {
            int R = 0;
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            try
            {
                string Query = "Delete FROM  ACReceipt where orgId=" + ID;
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


        public static List<Receipt> GetData(int OrgId)
        {
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            Receipt OBJ = new Receipt();
            List<Receipt> ReceiptList = new List<Receipt>();
            try
            {
                List<Receipt> REList = Receipt.GetAllList(OrgId,0).ToList();

             

                for(int i =0; i< REList.Count;i++)
                {
                    OBJ = new Receipt();
                    string LName = Ledger.GetAll().Where(w => w.ID == REList[i].DRLedgerId).Select(s => s.Name).FirstOrDefault();

                    string GName = Group.GetAll().Where(w => w.ID == REList[i].DRGroupId).Select(s => s.Name).FirstOrDefault();

                    OBJ.ReceiptType = "DR";
                    OBJ.Particular = "(L)" + LName + " >>" + " (LG) " + GName;
                    OBJ.DrAmount = REList[i].Amount;
                    ReceiptList.Add(OBJ);

                    OBJ = new Receipt();
                        string LName1 = Ledger.GetAll().Where(w => w.ID == REList[i].CRLedgerId).Select(s => s.Name).FirstOrDefault();

                        string GName1 = Group.GetAll().Where(w => w.ID == REList[i].CRGroupId).Select(s => s.Name).FirstOrDefault();

                        OBJ.ReceiptType = "CR";
                        OBJ.Particular = "(L)" + LName1 + " >>" + " (LG) " + GName1;
                        OBJ.CRAmount = REList[i].Amount;
                        ReceiptList.Add(OBJ);

                       
                    }
                                                       
            }
            catch (Exception e) { e.ToString(); }
           
            return (ReceiptList);
        }
        public static List<Accounts> GetLedgerWiseData(int ID,string  Name)
        {
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            Accounts OBJ = new Accounts();
            List<Accounts> AccountList = new List<Accounts>();
         
            try
            {
                if(Name=="Customer")
                {
                    List<Receipt> GetAllReceipt = Receipt.GetAllList(0, 0).ToList();
                    for (int i = 0; i < GetAllReceipt.Count; i++)
                    {

                        OBJ = new Accounts();
                        if(GetAllReceipt[i].Particular== "Opening Balance")
                        {
                            OBJ.Date = GetAllReceipt[i].Date;
                            OBJ.ReceiptType = "";
                            OBJ.Type = "";
                            OBJ.Narration = GetAllReceipt[i].Particular;
                            OBJ.CRAmount = GetAllReceipt[i].Amount;
                            if (OBJ.CRAmount > 0)
                            {
                                OBJ.Balance = GetAllReceipt[i].Balance + OBJ.CRAmount;
                            }
                            AccountList.Add(OBJ);
                        }
                        else
                        {
                            OBJ.Date = GetAllReceipt[i].Date;
                            OBJ.ReceiptType = "Receipt";
                            OBJ.Type = "R" + GetAllReceipt[i].EntryNo.ToString();
                            OBJ.Narration = GetAllReceipt[i].Particular;
                            OBJ.CRAmount = GetAllReceipt[i].Amount;
                           
                            OBJ.Balance = (GetAllReceipt[i-1].Balance + OBJ.CRAmount)-OBJ.DRAmount;
                            

                            AccountList.Add(OBJ);
                        }
                      
                    }
                }
               else if (Name == "Paytm")
                {
                    List<Receipt> GetAllReceipt = Receipt.GetAllList(0, 0).ToList();
                    for (int i = 0; i < GetAllReceipt.Count; i++)
                    {

                        OBJ = new Accounts();
                        if (GetAllReceipt[i].Particular == "Opening Balance")
                        {
                            OBJ.Date = GetAllReceipt[i].Date;
                            OBJ.ReceiptType = "";
                            OBJ.Type = "";
                            OBJ.Narration = GetAllReceipt[i].Particular;
                            OBJ.DRAmount = GetAllReceipt[i].Amount;
                            if (OBJ.DRAmount > 0)
                            {
                                OBJ.Balance = GetAllReceipt[i].Balance - OBJ.DRAmount;
                            }
                            AccountList.Add(OBJ);
                        }
                        else
                        {
                            OBJ.Date = GetAllReceipt[i].Date;
                            OBJ.ReceiptType = "Receipt";
                            OBJ.Type = "R" + GetAllReceipt[i].EntryNo.ToString();
                            OBJ.Narration = GetAllReceipt[i].Particular;
                            OBJ.DRAmount = GetAllReceipt[i].Amount;                            
                            OBJ.Balance = (GetAllReceipt[i - 1].Balance + OBJ.CRAmount) - OBJ.DRAmount;
                            
                            AccountList.Add(OBJ);
                        }

                    }
                }
                else
                {
                    List<Accounts> REList = Accounts.GetAll().Where(w => w.CRLedgerId == ID || w.DRLedgerId == ID).OrderBy(o => o.Date).ToList();


                    for (int i = 0; i < REList.Count; i++)
                    {

                        OBJ = new Accounts();
                        OBJ.Date = REList[i].Date;
                        OBJ.ReceiptType = REList[i].EntryType;
                        if (REList[i].EntryType == "Receipt")
                        {

                            OBJ.Type = "R" + REList[i].EntryNo.ToString();
                        }
                        if (REList[i].EntryType == "Sale")
                        {
                            OBJ.Type = "S" + REList[i].EntryNo.ToString();
                        }
                        if (REList[i].EntryType == "Journal")
                        {
                            OBJ.Type = "J" + REList[i].EntryNo.ToString();
                        }
                        if (REList[i].EntryType == "Payment")
                        {
                            OBJ.Type = "P" + REList[i].EntryNo.ToString();
                        }
                        OBJ.Narration = REList[i].Narration;
                        OBJ.DRAmount = REList[i].CRAmount;
                        OBJ.CRAmount = REList[i].DRAmount;
                        if (OBJ.CRAmount > 0)
                        {
                            OBJ.Balance = REList[i].Balance + OBJ.CRAmount;
                        }
                        else
                        {
                            OBJ.Balance = REList[i].Balance - OBJ.CRAmount;
                        }

                        AccountList.Add(OBJ);
                    }
                }              
            }
            catch (Exception e) { e.ToString(); }

            return (AccountList);
        }
        

    }
} 