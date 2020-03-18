using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace HangOut.Models.Account
{
    public class JournalEntry
    {
        public string Entrytype { get; set; }
        public int AID { get; set; }
        public int EntryNo { get; set; }
        public int JDID { get; set; }
        public double Debit { get; set; }
        public double Balance { get; set; }
        public double DRAmount { get; set; }
        public double CRAmount { get; set; }
        public double Credit { get; set; }
        public DateTime Date { get; set; }
        public int Account { get; set; }
        public int Type { get; set; }
        public string Narration { get; set; }
        public double Amount { get; set; }
        public int GroupId { get; set; }
        public int JournalEntryId { get; set; }
        public int DRLedgerId { get; set; }
        public int CRLedgerId { get; set; }
        public double JEDAmount { get; set; }
        public int OrderId { get; set; }
        public List<JournalEntry> AddOnList { get; set; }
        public int ReceiptID { get; set; }



        public int Save(List<JournalEntry> jdobj)
        {
            int Row = 0;
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Quary = "";
                if (this.AID == 0)
                {
                    Quary = "Insert Into ACJournalEntry Values (@Date,@Amount,@Narration,@GroupId,@OrderId);SELECT SCOPE_IDENTITY();";
                }
                else
                {
                    Quary = "Update ACJournalEntry Set Date=@Date,Amount=@Amount,Narration=@Narration,GroupId=@GroupId,OrderId=@OrderId where ID=@ID";
                }
                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@ID", this.AID);
                cmd.Parameters.AddWithValue("@Date", this.Date);
                cmd.Parameters.AddWithValue("@Amount", this.Amount);
                cmd.Parameters.AddWithValue("@Narration", this.Narration);
                cmd.Parameters.AddWithValue("@GroupId", this.GroupId);
                cmd.Parameters.AddWithValue("@OrderId", this.OrderId);
                if (this.AID == 0)
                {
                    Row = Convert.ToInt32(cmd.ExecuteScalar());
                    this.AID = Row;
                }
                else
                {
                    Row = cmd.ExecuteNonQuery();
                    //this.CategoryID = Row;
                }

                JournalEntry jdObj = new JournalEntry();
                foreach(var item in jdobj)
                {
                    jdObj = new JournalEntry();
                    jdObj.JournalEntryId = Row;
                    jdObj.CRLedgerId = item.CRLedgerId;
                    jdObj.GroupId = item.GroupId;
                    jdObj.JEDAmount = item.JEDAmount;
                    jdObj.Date = Date;
                    jdObj.JDSave();
                }
               
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return Row;

        }
        public int SaveGeneral()
        {
            int Row = 0;
            DBCon con = new DBCon();
            SqlCommand cmd = null;

            Accounts ACDetails = Accounts.GetAll().Where(w => (w.ACRLedgerId == CRLedgerId
            || w.ACRLedgerId == DRLedgerId) && w.EntryType == Entrytype).LastOrDefault();

            if(ACDetails!=null)
            {
                EntryNo = ACDetails.EntryNo + 1;
            }
            else
            {
                EntryNo = EntryNo+1;
            }
            Ledger LedgerDetails = Ledger.GetAll().Where(w => w.ID == CRLedgerId
                       || w.ID == DRLedgerId).FirstOrDefault();


            try
            {
                string Quary = "";
                if (this.AID == 0)
                {
                    Quary = "Insert Into ACAccount Values (@Date,@DRAmount,@CRAmount,@Narration,@Balance,@CRGroupId,@OrgId,@DRGroupId,@CRLedgerId,@DRLedgerId,@EntryNo,@EntryType,@ReceiptID);SELECT SCOPE_IDENTITY();";
                }
                else
                {
                    Quary = "Update ACAccount Set Date=@Date,DRAmount=@DRAmount,CRAmount=@CRAmount,Narration=@Narration,Balance=@Balance,CRGroupId=@CRGroupId,OrgId=@OrgId,DRGroupId=@DRGroupId,CRLedgerId=@CRLedgerId,DRLedgerId=@DRLedgerId,EntryNo=@EntryNo,EntryType=@EntryType,ReceiptID=@ReceiptID where AID=@AID";
                }
                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@AID", this.AID);
                cmd.Parameters.AddWithValue("@Date", this.Date);
                cmd.Parameters.AddWithValue("@DRAmount", this.DRAmount);
                cmd.Parameters.AddWithValue("@CRAmount", this.CRAmount);
                cmd.Parameters.AddWithValue("@Narration", this.Narration);
                cmd.Parameters.AddWithValue("@Balance", this.Balance);
                cmd.Parameters.AddWithValue("@CRGroupId", LedgerDetails.ParentGroup);
                cmd.Parameters.AddWithValue("@OrgId", LedgerDetails.OrgId);
                cmd.Parameters.AddWithValue("@DRGroupId", LedgerDetails.ParentGroup);
                cmd.Parameters.AddWithValue("@CRLedgerId", this.CRLedgerId);
                cmd.Parameters.AddWithValue("@DRLedgerId", this.DRLedgerId);
                cmd.Parameters.AddWithValue("@EntryNo", this.EntryNo);
                cmd.Parameters.AddWithValue("@EntryType", this.Entrytype);
                cmd.Parameters.AddWithValue("@ReceiptID", this.ReceiptID);


                if (this.AID == 0)
                {
                    Row = Convert.ToInt32(cmd.ExecuteScalar());
                    this.AID = Row;
                }
                else
                {
                    Row = cmd.ExecuteNonQuery();
                    //this.CategoryID = Row;
                }


            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return Row;

        }

        public int JDSave()
        {
            int JRow = 0;
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Quary = "";
                if (this.JDID == 0)
                {
                    Quary = "Insert Into ACJournalEntryDetails Values (@JornalEntryId,@GroupId,@DRLedgerId,@CRLedgerId,@Amount,@Date);SELECT SCOPE_IDENTITY();";
                }
                else
                {
                    Quary = "Update ACJournalEntryDetails Set JournalEntryId=@JornalEntryId,GroupId=@GroupId,DRLedgerId=@DRLedgerId,CRLedgerId=@CRLedgerId,Amount=@Amount,Date=@Date where ID=@ID";
                }
                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@ID", this.AID);
                cmd.Parameters.AddWithValue("@JornalEntryId", this.JournalEntryId);
                cmd.Parameters.AddWithValue("@GroupId", this.GroupId);
                cmd.Parameters.AddWithValue("@DRLedgerId", this.DRLedgerId);
                cmd.Parameters.AddWithValue("@CRLedgerId", this.CRLedgerId);
                cmd.Parameters.AddWithValue("@Amount", this.JEDAmount);
                cmd.Parameters.AddWithValue("@Date", this.Date);
                if (this.JDID == 0)
                {
                    JRow = Convert.ToInt32(cmd.ExecuteScalar());
                    this.JDID = JRow;
                }
                else
                {
                    JRow = cmd.ExecuteNonQuery();
                    //this.CategoryID = Row;
                }
                          
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return JRow;

        }


        public static List<JType> JType()
        {

            List<JType> type = new List<JType>();
            type.Add(new JType { Id = 0, Name = "DR" });
            type.Add(new JType { Id = 1, Name = "CR" });

            return type;
        }
        public static List<EntryType> EType()
        {

            List<EntryType> type = new List<EntryType>();
            type.Add(new EntryType { Name = "Payment" });
            type.Add(new EntryType { Name = "Receipt" });
            type.Add(new EntryType { Name = "Contra Entry" });
            type.Add(new EntryType { Name = "Journal Entry" });
            type.Add(new EntryType { Name = "Debit Note" });
            type.Add(new EntryType { Name = "Credit Note" });

            return type;
        }
    }
  
}
public class JType
{
    public int Id { get; set; }
    public string Name { get; set; }
}
public class EntryType
{
    public int Id { get; set; }
    public string Name { get; set; }
}