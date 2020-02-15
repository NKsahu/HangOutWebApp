using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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