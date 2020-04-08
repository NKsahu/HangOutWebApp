using HangOut.Models.DynamicList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models.MyCustomer
{
    public class Cashback
    {
        public int CashBkId { get; set; }
        public Int64 CBUniqId { get; set; }// unique dateitme id
        public int OrgID { get; set; }
        public DateTime StartDate { get; set; }
        public int ValidTill { get; set; }// 1 unspecify , 2 specify ValidTillDate 
        public DateTime ValidTillDate { get; set; }

        public int CashBkType { get; set; }  // 1 :Percentage

        public double Percentage { get; set; }
        public double MaxAmt { get; set; }
        public double BilAmt { get; set; }
        public bool RaiseDynamic { get; set; }
        //========
        public int CashBkStatus { get; set; } // 1 :running ,2: pause 
        public string SeatingIds { get; set; }// comma seprated applied seating ids
        public int TerminateSts { get; set; }// 1 activate , 2 terminated;
        public int CampeignType { get; set; }// 1 : CashBack, 2 : Complementry dish

        public string StrStartDate { get; set; }
        public string ValidTillStr { get; set; }
        public Cashback()
        {
            StartDate = DateTime.Now;
            ValidTillDate= DateTime.Now;
            RaiseDynamic = true;
            CashBkStatus = 1;
            SeatingIds = "";
            TerminateSts = 1;
            StrStartDate = StartDate.ToString("dd-MM-yyyy");
            ValidTillStr= ValidTillDate.ToString("dd-MM-yyyy");
        }

        public int Save()
        {
            int R = 0;
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Query = "";
                if (this.CashBkId == 0)
                {
                    Query = "Insert into  CashBack  values(@OrgID,@StartDate,@ValidTill,@ValidTillDate,@CashBkType,@Percentage,@MaxAmt,@BilAmt,@RaiseDynamic,@CashBkStatus,@SeatingIds,@TerminateSts,@CBUniqId,@CampeignType); SELECT SCOPE_IDENTITY();";
                    cmd = new SqlCommand(Query, dBCon.Con);
                    cmd.Parameters.AddWithValue("@OrgID", this.OrgID);
                    cmd.Parameters.AddWithValue("@CBUniqId", this.CBUniqId);
                    cmd.Parameters.AddWithValue("@CampeignType", this.CampeignType);
                }
                else
                {
                    Query = "update  CashBack set StartDate=@StartDate,ValidTill=@ValidTill,ValidTillDate=@ValidTillDate,Percentage=@Percentage,CashBkType=@CashBkType,MaxAmt=@MaxAmt,BilAmt=@BilAmt,RaiseDynamic=@RaiseDynamic,CashBkStatus=@CashBkStatus,SeatingIds=@SeatingIds,TerminateSts=@TerminateSts where CashBkId=@CashBkId";
                    cmd = new SqlCommand(Query, dBCon.Con);
                    cmd.Parameters.AddWithValue("@CashBkId", this.CashBkId);
                }
                
                cmd.Parameters.AddWithValue("@StartDate", this.StartDate);
                cmd.Parameters.AddWithValue("@ValidTill", this.ValidTill);
                cmd.Parameters.AddWithValue("@ValidTillDate", this.ValidTillDate);
                cmd.Parameters.AddWithValue("@CashBkType", this.CashBkType);
                cmd.Parameters.AddWithValue("@Percentage", this.Percentage);
                cmd.Parameters.AddWithValue("@MaxAmt ", this.MaxAmt);
                cmd.Parameters.AddWithValue("@BilAmt", this.BilAmt);
                cmd.Parameters.AddWithValue("@RaiseDynamic", this.RaiseDynamic);
                cmd.Parameters.AddWithValue("@CashBkStatus", this.CashBkStatus);
                cmd.Parameters.AddWithValue("@SeatingIds", this.SeatingIds);
                cmd.Parameters.AddWithValue("@TerminateSts", this.TerminateSts);
                
                if (this.CashBkId == 0)
                {
                    R = Convert.ToInt32(cmd.ExecuteScalar());
                    this.CashBkId = R;

                }
                else
                {
                    R = cmd.ExecuteNonQuery();
                    if (R > 0)
                        R = this.CashBkId;
                }

            }
            catch (Exception e) { e.ToString(); }
            finally
            {
                dBCon.Close();
                if (cmd != null) cmd.Dispose();
            }
            return R;
        }

        public static List<Cashback> GetAll(int OrgId,int TerminateSts=0)
        {
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<Cashback> ListTmp = new List<Cashback>();
            string Query = "SELECT * FROM  CashBack where OrgID=" + OrgId.ToString();
            if (TerminateSts > 0)
            {
                Query+= "and TerminateSts = "+TerminateSts;
            }
            try
            {
                cmd = new SqlCommand(Query, dBCon.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    int index = 0;
                    Cashback ObjTmp = new Cashback();
                    ObjTmp.CashBkId = SDR.GetInt32(index++);
                    ObjTmp.OrgID = SDR.GetInt32(index++);
                    ObjTmp.StartDate = SDR.GetDateTime(index++);
                    ObjTmp.ValidTill = SDR.GetInt32(index++);
                    ObjTmp.ValidTillDate = SDR.GetDateTime(index++);
                    ObjTmp.CashBkType = SDR.GetInt32(index++);
                    ObjTmp.Percentage = SDR.GetDouble(index++);
                    ObjTmp.MaxAmt = SDR.GetDouble(index++);
                    ObjTmp.BilAmt = SDR.GetDouble(index++);
                    ObjTmp.RaiseDynamic = SDR.GetBoolean(index++);
                    ObjTmp.CashBkStatus = SDR.GetInt32(index++);
                    ObjTmp.SeatingIds = SDR.GetString(index++);
                    ObjTmp.TerminateSts = SDR.GetInt32(index++);
                    ObjTmp.CBUniqId = SDR.GetInt64(index++);
                    ObjTmp.CampeignType= SDR.GetInt32(index++);
                    ListTmp.Add(ObjTmp);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { dBCon.Close(); }

            return (ListTmp);
        }
        public static Cashback Getone(int CashBkId)
        {
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            Cashback Tmp = new Cashback();
            string Query = "SELECT * FROM  CashBack where CashBkId=" + CashBkId.ToString();
            try
            {
                cmd = new SqlCommand(Query, dBCon.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    int index = 0;
                    Cashback ObjTmp = new Cashback();
                    ObjTmp.CashBkId = SDR.GetInt32(index++);
                    ObjTmp.OrgID = SDR.GetInt32(index++);
                    ObjTmp.StartDate = SDR.GetDateTime(index++);
                    ObjTmp.ValidTill = SDR.GetInt32(index++);
                    ObjTmp.ValidTillDate = SDR.GetDateTime(index++);
                    ObjTmp.CashBkType = SDR.GetInt32(index++);
                    ObjTmp.Percentage = SDR.GetDouble(index++);
                    ObjTmp.MaxAmt = SDR.GetDouble(index++);
                    ObjTmp.BilAmt = SDR.GetDouble(index++);
                    ObjTmp.RaiseDynamic = SDR.GetBoolean(index++);
                    ObjTmp.CashBkStatus = SDR.GetInt32(index++);
                    ObjTmp.SeatingIds = SDR.GetString(index++);
                    ObjTmp.TerminateSts = SDR.GetInt32(index++);
                    ObjTmp.CBUniqId = SDR.GetInt64(index++);
                    ObjTmp.CampeignType = SDR.GetInt32(index++);
                    ObjTmp.StrStartDate = ObjTmp.StartDate.ToString("dd-MM-yyyy");
                    ObjTmp.ValidTillStr = ObjTmp.ValidTillDate.ToString("dd-MM-yyyy");
                    Tmp = ObjTmp;
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { dBCon.Close(); }

            return (Tmp);
        }

        public static List<int> GetRedSeatings(Cashback Current)
        {
            List<Cashback> Cashbakcs = Cashback.GetAll(OrderType.CurrOrgId(), 1);// all active cashbk of current outlet
            List<int> RedSeatings = new List<int>();
            Cashbakcs = Cashbakcs.FindAll(x => x.SeatingIds != "");
            Cashbakcs = Cashbakcs.FindAll(x => x.CashBkId != Current.CashBkId && x.CashBkStatus == 1);// all running cashback not Current cashbk
            if (Current.StartDate.Date > DateTime.Now.Date)
            {
                Cashbakcs = Cashbakcs.FindAll(x => x.StartDate.Date >= Current.StartDate.Date && x.ValidTillDate <= Current.ValidTillDate.Date).ToList();
            }
            else
            {
                Cashbakcs = Cashbakcs.FindAll(x => x.StartDate.Date <= Current.StartDate.Date && x.ValidTillDate >= Current.StartDate.Date).ToList();
            }
            foreach (var cashbak in Cashbakcs)
            {
               
                RedSeatings.AddRange(cashbak.SeatingIds.Split(',').Select(int.Parse).ToList());
            }

            return RedSeatings;
        }
        
        public static Cashback GetAppliedCashBk(int OrgId,Int64 SeatinId)
        {

            List<Cashback> Cashbacks = Cashback.GetAll(OrgId, 1);// only actives
            Cashbacks = Cashbacks.FindAll(x => x.CashBkStatus == 1);// only running
            Cashbacks = Cashbacks.FindAll(x => x.SeatingIds != "");
            Cashbacks = Cashbacks.FindAll(x => x.StartDate.Date <= DateTime.Now.Date && x.ValidTillDate.Date >= DateTime.Now.Date).ToList();
            for (int i = 0; i < Cashbacks.Count; i++)
            {
                List<int> seats = Cashbacks[i].SeatingIds.Split(',').Select(int.Parse).ToList();
                int seat = seats.Find(x => x == SeatinId);
                if (seat > 0)
                {
                    return Cashbacks[i];
                    
                }
            }
            return null;
        }
    }
    
    public class CashBkSeating
    {
        public int CashBkId { get; set; }

        public int[] Seatings { get; set; }

    }
}

