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
        public int CRGroupId { get; set; }
        public int DRGroupId { get; set; }
        public int ADRLedgerId { get; set; }
        public int ACRLedgerId { get; set; }
        public int AOrgId { get; set; }
        public string ReceiptType { get; set; }
        public int ReceiptID { get; set; }

        public int EntryNo { get; set; }
        public string EntryType { get; set; }
        public string Type { get; set; }

        //Account Details
        public int ADID { get; set; }
        public int ACID { get; set; }
        public DateTime ADDate { get; set; }
        public double ADAmount { get; set; }
        public int ADGroupId { get; set; }
        public int DRLedgerId { get; set; }
        public int CRLedgerId { get; set; }
        public int ADOrgId { get; set; }




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
                    Quary = "Insert Into ACAccount Values (@Date,@DRAmount,@CRAmount,@Narration,@Balance,@GroupId,@OrgId);SELECT SCOPE_IDENTITY();";
                }
                else
                {
                    Quary = "Update ACAccount Set Date=@Date,DRAmount=@DRAmount,CRAmount=@CRAmount,Narration=@Narration,Balance=@Balance,GroupId=@GroupId,OrgId=@OrgId where AID=@AID";
                }
                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@AID", this.AID);
                cmd.Parameters.AddWithValue("@Date", this.Date);
                cmd.Parameters.AddWithValue("@DRAmount", this.DRAmount);
                cmd.Parameters.AddWithValue("@CRAmount", this.CRAmount);
                cmd.Parameters.AddWithValue("@Narration", this.Narration);
                cmd.Parameters.AddWithValue("@Balance", this.Balance);
                cmd.Parameters.AddWithValue("@CRGroupId", this.CRGroupId);
                cmd.Parameters.AddWithValue("@OrgId", this.AOrgId);
                cmd.Parameters.AddWithValue("@DRGroupId", this.DRGroupId);
                cmd.Parameters.AddWithValue("@CRLedgerId", this.ACRLedgerId);
                cmd.Parameters.AddWithValue("@DRLedgerId", this.ADRLedgerId);


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
                    adObj.ADOrgId = item.AOrgId;
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
            DBCon con1 = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Quary = "";
                if (this.ADID == 0)
                {
                    Quary = "Insert Into ACAccountDetails Values (@AID,@Date,@CRAmount,@CRGroupId,@DRLedgerId,@CRLedgerId,@OrgId,@DRGroupId,@Narration,@DRAmount);SELECT SCOPE_IDENTITY();";
                }
                else
                {
                    Quary = "Update ACAccountDetails Set AID=@AID,Date=@Date,CRAmount=@CRAmount,CRGroupId=@CRGroupId,DRLedgerId=@DRLedgerId,CRLedgerId=@CRLedgerId,OrgId=@OrgId,DRGroupId=@DRGroupId,Narration=@Narration,DRAmount=@DRAmount where ADID=@ADID";
                }
                cmd = new SqlCommand(Quary, con1.Con);
                cmd.Parameters.AddWithValue("@ADID", this.ADID);
                cmd.Parameters.AddWithValue("@AID", this.ACID);
                cmd.Parameters.AddWithValue("@Date", this.Date);
                cmd.Parameters.AddWithValue("@CRAmount", this.CRAmount);
                cmd.Parameters.AddWithValue("@CRGroupId", this.CRGroupId);
                cmd.Parameters.AddWithValue("@DRLedgerId", this.ADRLedgerId);
                cmd.Parameters.AddWithValue("@CRLedgerId", this.ACRLedgerId);
                cmd.Parameters.AddWithValue("@OrgId", this.AOrgId);
                cmd.Parameters.AddWithValue("@DRGroupId", this.DRGroupId);
                cmd.Parameters.AddWithValue("@Narration", this.Narration);
                cmd.Parameters.AddWithValue("@DRAmount", this.DRAmount);


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
            finally { cmd.Dispose(); con1.Con.Close(); }
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
                cmd.Parameters.AddWithValue("@CRGroupId", this.CRGroupId);
                cmd.Parameters.AddWithValue("@OrgId", this.AOrgId);
                cmd.Parameters.AddWithValue("@DRGroupId", this.DRGroupId);
                cmd.Parameters.AddWithValue("@CRLedgerId", this.ACRLedgerId);
                cmd.Parameters.AddWithValue("@DRLedgerId", this.ADRLedgerId);
                cmd.Parameters.AddWithValue("@EntryNo", this.EntryNo);
                cmd.Parameters.AddWithValue("@EntryType", this.EntryType);
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

        public static List<Accounts> GetAll()
        {
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<Accounts> ACList = new List<Accounts>();
            try
            {
                string Quary = "Select * from ACAccount ";
                cmd = new SqlCommand(Quary, con.Con);
            
                SDR = cmd.ExecuteReader();

                while (SDR.Read())
                {
                    Accounts OBJAC = new Accounts();
                    OBJAC.AID = SDR.GetInt32(0);
                    OBJAC.Date = SDR.GetDateTime(1);
                    OBJAC.DRAmount = SDR.GetDouble(2);
                    OBJAC.CRAmount = SDR.GetDouble(3);
                    OBJAC.Narration = SDR.GetString(4);
                    OBJAC.Balance = SDR.GetDouble(5);
                    OBJAC.CRGroupId = SDR.GetInt32(6);
                    OBJAC.AOrgId = SDR.GetInt32(7);
                    OBJAC.DRGroupId = SDR.GetInt32(8);
                    OBJAC.CRLedgerId = SDR.GetInt32(9);
                    OBJAC.DRLedgerId = SDR.GetInt32(10);


                    ACList.Add(OBJAC);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return (ACList);
        }
        public static List<Accounts> GetAllDetails()
        {
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<Accounts> ACList = new List<Accounts>();
            try
            {
                string Quary = "Select * from ACAccountDetails ";
                cmd = new SqlCommand(Quary, con.Con);

                SDR = cmd.ExecuteReader();

                while (SDR.Read())
                {
                    Accounts OBJAC = new Accounts();
                    OBJAC.ADID = SDR.GetInt32(0);
                    OBJAC.AID = SDR.GetInt32(1);
                    OBJAC.ADDate = SDR.GetDateTime(2);
                    OBJAC.ADAmount = SDR.GetDouble(3);
                    OBJAC.ADGroupId = SDR.GetInt32(4);
                    OBJAC.DRLedgerId = SDR.GetInt32(5);
                    OBJAC.CRLedgerId = SDR.GetInt32(6);
                    OBJAC.ADOrgId = SDR.GetInt32(7);
                    ACList.Add(OBJAC);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return (ACList);
        }

        public static int Dell(int ID)
        {
            int R = 0;
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            try
            {
                string Query = "Delete FROM  ACAccount where OrgId=" + ID;
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
        public static int DellAccountDetails(int ID)
        {
            int R = 0;
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            try
            {
                string Query = "Delete FROM  AcAccountDetails where OrgId=" + ID;
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

        public static List<Accounts> GetAllACDetails(int OrgId)
        {
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<Accounts> ACList = new List<Accounts>();
            try
            {           
                string Quary = "Select * from ACAccount where OrgId=" + OrgId;
                cmd = new SqlCommand(Quary, con.Con);

                SDR = cmd.ExecuteReader();

                while (SDR.Read())
                {
                    Accounts OBJAC = new Accounts();
                    OBJAC.AID = SDR.GetInt32(0);
                    OBJAC.Date = SDR.GetDateTime(1);
                    OBJAC.DRAmount = SDR.GetDouble(2);
                    OBJAC.CRAmount = SDR.GetDouble(3);
                    OBJAC.Narration = SDR.GetString(4);
                    OBJAC.Balance = SDR.GetDouble(5);
                    OBJAC.CRGroupId = SDR.GetInt32(6);
                    OBJAC.AOrgId = SDR.GetInt32(7);
                    OBJAC.DRGroupId = SDR.GetInt32(8);
                    OBJAC.CRLedgerId = SDR.GetInt32(9);
                    OBJAC.DRLedgerId = SDR.GetInt32(10);
                    OBJAC.EntryNo = SDR.GetInt32(11);
                    OBJAC.EntryType = SDR.GetString(12);
                    OBJAC.ReceiptID = SDR.GetInt32(13);
                    ACList.Add(OBJAC);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return (ACList);
        }
        public static List<Accounts> GetADetails(int OrgId,string Name)
        {
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            Accounts OBJ = new Accounts();
            List<Accounts> AccountList = new List<Accounts>();
            try
            {
                List<Accounts> ACList = Accounts.GetAllACDetails(OrgId).ToList();



                for (int i = 0; i < ACList.Count; i++)
                {
                    OBJ = new Accounts();
                    string LName = Ledger.GetAllList().Where(w => w.ID == ACList[i].DRLedgerId).Select(s => s.Name).FirstOrDefault();

                    string GName = Group.GetAll().Where(w => w.ID == ACList[i].DRGroupId).Select(s => s.Name).FirstOrDefault();

                  
                    OBJ.ReceiptType = ACList[i].EntryType;
                    OBJ.Date = ACList[i].Date;
                    if(ACList[i].EntryType=="Journal")
                    {
                        OBJ.Type = "J"+ACList[i].EntryNo.ToString();
                    }
                    if (ACList[i].EntryType == "Sale")
                    {
                        OBJ.Type = "S" + ACList[i].EntryNo.ToString();
                    }
                    if (ACList[i].EntryType == "Payment")
                    {
                        OBJ.Type = "P" + ACList[i].EntryNo.ToString();
                    }

                    OBJ.Narration = ACList[i].Narration;
                    OBJ.CRAmount = ACList[i].CRAmount;
                    OBJ.DRAmount = ACList[i].DRAmount;
                    OBJ.Balance = ACList[i].Balance;
                    OBJ.ReceiptID = ACList[i].ReceiptID;
                    AccountList.Add(OBJ);

                   
                }

            }
            catch (Exception e) { e.ToString(); }

            return (AccountList);
        }

    }
}