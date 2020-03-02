using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace HangOut.Models.Account
{
    public class Accounts
    {
       // Account
        public int AID { get; set; }
        public DateTime Date { get; set; }
        public double DRAmount { get; set; }
        public double CRAmount { get; set; }
        public string Narration { get; set; }
        public double Balance { get; set; }
        public int GroupId { get; set; }


        //Account Details
        public int ADID { get; set; }
        public int ACID { get; set; }
        public DateTime ADDate { get; set; }
        public double ADAmount { get; set; }
        public int ADGroupId { get; set; }
        public int DRLedgerId { get; set; }
        public int CRLedgerId { get; set; }




        public int Save(List<Accounts> adobj)
        {
            int Row = 0;
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Quary = "";
                if (this.AID == 0)
                {
                    Quary = "Insert Into ACAccount Values (@Date,@DRAmount,@CRAmount,@Narration,@Balance,@GroupId);SELECT SCOPE_IDENTITY();";
                }
                else
                {
                    Quary = "Update ACAccount Set Date=@Date,DRAmount=@DRAmount,CRAmount=@CRAmount,Narration=@Narration,Balance=@Balance,GroupId=@GroupId where AID=@AID";
                }
                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@AID", this.AID);
                cmd.Parameters.AddWithValue("@Date", this.Date);
                cmd.Parameters.AddWithValue("@DRAmount", this.DRAmount);
                cmd.Parameters.AddWithValue("@CRAmount", this.CRAmount);
                cmd.Parameters.AddWithValue("@Narration", this.Narration);
                cmd.Parameters.AddWithValue("@Balance", this.Balance);
                cmd.Parameters.AddWithValue("@GroupId", this.GroupId);
              
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

                Accounts adObj = new Accounts();

                foreach (var item in adobj)
                {
                    adObj = new Accounts();
                    adObj.ACID = Row;
                    adObj.ADDate = item.ADDate;
                    adObj.ADAmount = item.ADAmount;
                    adObj.CRLedgerId = item.CRLedgerId;
                    adObj.DRLedgerId = item.DRLedgerId;
                    adObj.ADGroupId = item.ADGroupId;             
                    adObj.ADSave();
                }

            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return Row;

        }

        public int ADSave()
        {
            int ARow = 0;
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Quary = "";
                if (this.ADID == 0)
                {
                    Quary = "Insert Into ACAccountDetails Values (@AID,@Date,@Amount,@GroupId,@DRLedgerId,@CRLedgerId);SELECT SCOPE_IDENTITY();";
                }
                else
                {
                    Quary = "Update ACAccountDetails Set AID=@AID,Date=@Date,Amount=@Amount,GroupId=@GroupId,DRLedgerId=@DRLedgerId,CRLedgerId=@CRLedgerId where ADID=@ADID";
                }
                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@ADID", this.ADID);
                cmd.Parameters.AddWithValue("@AID", this.ACID);
                cmd.Parameters.AddWithValue("@Date", this.ADDate);
                cmd.Parameters.AddWithValue("@Amount", this.ADAmount);
                cmd.Parameters.AddWithValue("@GroupId", this.ADGroupId);
                cmd.Parameters.AddWithValue("@DRLedgerId", this.DRLedgerId);
                cmd.Parameters.AddWithValue("@CRLedgerId", this.CRLedgerId);
          
                if (this.ADID == 0)
                {
                    ARow = Convert.ToInt32(cmd.ExecuteScalar());
                    this.ADID = ARow;
                }
                else
                {
                    ARow = cmd.ExecuteNonQuery();
                    //this.CategoryID = Row;
                }

            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return ARow;

        }

        public int SaveGeneral()
        {
            int Row = 0;
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Quary = "";
                if (this.AID == 0)
                {
                    Quary = "Insert Into ACAccount Values (@Date,@DRAmount,@CRAmount,@Narration,@Balance,@GroupId);SELECT SCOPE_IDENTITY();";
                }
                else
                {
                    Quary = "Update ACAccount Set Date=@Date,DRAmount=@DRAmount,CRAmount=@CRAmount,Narration=@Narration,Balance=@Balance,GroupId=@GroupId where AID=@AID";
                }
                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@AID", this.AID);
                cmd.Parameters.AddWithValue("@Date", this.Date);
                cmd.Parameters.AddWithValue("@DRAmount", this.DRAmount);
                cmd.Parameters.AddWithValue("@CRAmount", this.CRAmount);
                cmd.Parameters.AddWithValue("@Narration", this.Narration);
                cmd.Parameters.AddWithValue("@Balance", this.Balance);
                cmd.Parameters.AddWithValue("@GroupId", this.GroupId);

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

        public static List<Accounts> GetAll()
        {
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<Accounts> GroupList = new List<Accounts>();
            try
            {
                string Quary = "Select * from ACAccount ORDER BY ID DESC";
                cmd = new SqlCommand(Quary, con.Con);
                SDR = cmd.ExecuteReader();

                while (SDR.Read())
                {
                    Accounts OBJAC = new Accounts();
                    OBJAC.AID = SDR.GetInt32(0);
                    OBJAC.Narration = SDR.GetString(1);
                    OBJAC.DRAmount = SDR.GetDouble(2);
                    OBJAC.CRAmount = SDR.GetDouble(3);
                    OBJAC.Balance = SDR.GetDouble(4);
                    GroupList.Add(OBJAC);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return (GroupList);
        }
    }
}