using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
namespace HangOut.Models.Account
{
    public class BalanceStatement
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public string Narration { get; set; }
        public long OrderId { get; set; }
        public int OrgId { get; set; }



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
                    Quary = "Insert Into ACBalanceStatement Values (@Date,@Amount,@Narration,@OrderId,@OrgId);SELECT SCOPE_IDENTITY();";
                }
                else
                {
                    Quary = "Update ACBalanceStatement Set Date=@Date,Amount=@Amount,Narration=@Narration,OrderId=@OrderId,OrgId=@OrgId where ID=@ID";
                }
                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@ID", this.ID);
                cmd.Parameters.AddWithValue("@Date", this.Date);
                cmd.Parameters.AddWithValue("@Amount", this.Amount);
                cmd.Parameters.AddWithValue("@Narration", this.Narration);
                cmd.Parameters.AddWithValue("@OrderId", this.OrderId);
                cmd.Parameters.AddWithValue("@OrgId", this.OrgId);
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

                BalanceStatement Obj = new BalanceStatement();
                Obj.Date = DateTime.Now;
                Obj.Amount = Amount;
                Obj.OrgId =  OrgId;
                Obj.OrderId = OrderId;               
                Obj.Narration = "Commission of Order No." + OrderId;                           
                Obj.Save();

            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return Row;

        }
    
}

}  