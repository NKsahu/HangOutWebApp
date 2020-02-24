using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace HangOut.Models.Account
{
    public class JournalEntry
    {
        public int Entrytype { get; set; }
        public int ID { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public DateTime Date { get; set; }
        public int Account { get; set; }
        public int Type { get; set; }
        public string Narration { get; set; }
        public double Amount { get; set; }
        public int GroupId { get; set; }


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
                    Quary = "Insert Into ACJournalEntry Values (@Date,@Amount,@Narration,@GroupId);SELECT SCOPE_IDENTITY();";
                }
                else
                {
                    Quary = "Update ACGroup Set Date=@Date,Amount=@Amount,Narration=@Narration,@GroupId=GroupId where ID=@ID";
                }
                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@ID", this.ID);
                cmd.Parameters.AddWithValue("@Date", this.Date);
                cmd.Parameters.AddWithValue("@Amount", this.Amount);
                cmd.Parameters.AddWithValue("@Narration", this.Narration);
                cmd.Parameters.AddWithValue("@GroupId", this.GroupId);
                if (this.ID == 0)
                {
                    Row = Convert.ToInt32(cmd.ExecuteScalar());
                    this.ID = Row;
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
            type.Add(new EntryType { Id = 0, Name = "Payment" });
            type.Add(new EntryType { Id = 1, Name = "Receipt" });
            type.Add(new EntryType { Id = 2, Name = "Contra Entry" });
            type.Add(new EntryType { Id = 3, Name = "Journal Entry" });
            type.Add(new EntryType { Id = 4, Name = "Debit Note" });
            type.Add(new EntryType { Id = 5, Name = "Credit Note" });

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