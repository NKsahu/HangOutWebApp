using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;

namespace HangOut.Models
{
    /*
     this table is used for make order from customer app admin panel and captain end, and generate single order number globally
     order number is auto increament 
     */
   
    public class HG_Orders
    {
        public Int64 OID { get; set; }
        public Int64 CID { get; set; }
        public string Status { get; set; } //Order-Placed=1,Order-running:2 Order-Completed=3,Order-Cancelled=4,
        public Int64 Create_By { get; set; } // OrderApprovlSts is yes then approved by id place here
        public DateTime Create_Date { get; set; }
        public Int64 Update_By { get; set; }
        public DateTime Update_Date { get; set; }
        public bool Deleted { get; set; }
        public int OrgId { get; set; }
        public Int64 Table_or_SheatId { get; set; }
        public int PaymentStatus { get; set; }//{'0':'unpaid',1:'PaidBycash','2':'by online','3':'ByFoodPaymeGateway'}
        public int PayReceivedBy { get; set; }
        public int TableOtp { get; set; }
        public string DisntChargeIDs { get; set; }// discntCharges CSV IDS
        public int OrderApprovlSts { get; set; }// {0:'not-approved': 1:approved by customer} customer is taken Orde  or Not
        public double DeliveryCharge { get; set; }// delivery charge amount
        public int ContactId { get; set; }// customer local Contact Id;
        public int OfferDishCBID { get; set; }
        public HG_Orders()
        {
            this.OID = 0;
            this.Status = "";
            this.Update_Date = DateTime.Now;
            this.OrgId = 0;
            this.Update_By = 0;
            this.Table_or_SheatId = 0;
            this.PayReceivedBy = 0;
            this.ContactId = 0;
            this.DisntChargeIDs="";
            this.OfferDishCBID = 0;
        }

        public Int64 Save()
        {
            SqlCommand cmd = null;
            DBCon Obj = new DBCon();
            Int64 R = 0;
            try
            {
                if (this.OID == 0)
                    cmd = new SqlCommand("INSERT INTO HG_ORDERS  VALUES (@CID,@Status,@Create_By,@Create_Date,@Update_By,@Update_Date,@Deleted,@OrgId,@Table_or_SheatId,@PaymentStatus,@PayReceivedBy,@TableOtp,@OrderByIds,@OrdAprovalSts,@DeliveryCharge,@ContactId,@OfferDishCBID);select SCOPE_IDENTITY();", Obj.Con);
                else
                {
                    cmd = new SqlCommand("UPDATE HG_ORDERS SET CID=@CID,Status=@Status,Create_By=@Create_By,Update_By=@Update_By,Update_Date=@Update_Date,Deleted=@Deleted,@OrgId=@OrgId,Table_or_SheatId=@Table_or_SheatId,PaymentStatus=@PaymentStatus,PayReceivedBy=@PayReceivedBy,TableOtp=@TableOtp,OrderByIds=@OrderByIds,OrdAprovalSts=@OrdAprovalSts,DeliveryCharge=@DeliveryCharge,ContactId=@ContactId,OfferDishCBID=@OfferDishCBID where OID=@OID", Obj.Con);
                    cmd.Parameters.AddWithValue("@OID", this.OID);
                }
                cmd.Parameters.AddWithValue("@CID", this.CID);
                cmd.Parameters.AddWithValue("@Status", this.Status);
                cmd.Parameters.AddWithValue("@Create_By", this.Create_By);
                cmd.Parameters.AddWithValue("@Create_Date", this.Create_Date);
                cmd.Parameters.AddWithValue("@Update_By", this.Update_By);
                cmd.Parameters.AddWithValue("@Update_Date", DateTime.Now);
                cmd.Parameters.AddWithValue("@Deleted", this.Deleted);
                cmd.Parameters.AddWithValue("@OrgId", this.OrgId);
                cmd.Parameters.AddWithValue("@Table_or_SheatId", this.Table_or_SheatId);
                cmd.Parameters.AddWithValue("@PaymentStatus", this.PaymentStatus);
                cmd.Parameters.AddWithValue("@PayReceivedBy", this.PayReceivedBy);
                cmd.Parameters.AddWithValue("@TableOtp", this.TableOtp);
                cmd.Parameters.AddWithValue("@OrderByIds", this.DisntChargeIDs);
                cmd.Parameters.AddWithValue("@OrdAprovalSts", this.OrderApprovlSts);
                cmd.Parameters.AddWithValue("@DeliveryCharge", this.DeliveryCharge);
                cmd.Parameters.AddWithValue("@ContactId", this.ContactId);
                cmd.Parameters.AddWithValue("@OfferDishCBID", this.OfferDishCBID);
                if (this.OID == 0)
                {
                    R = Convert.ToInt64(cmd.ExecuteScalar());
                    this.OID = R;
                }
                else
                {
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        R = this.OID;
                    }
                }
            }
            catch (Exception e) { R = 0; this.OID = 0; e.ToString(); }
            finally { cmd.Dispose(); Obj.Con.Close(); Obj.Con.Dispose(); Obj.Con = null; }
            return R;
        }

        public List<HG_Orders> GetAll(int OrgId=0,int CID=0,int Status=0)
        {
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<HG_Orders> ListTmp = new List<HG_Orders>();
            HG_Orders ObjTmp = null;
            DBCon Obj = new DBCon();
            try
            {
                string Query = "SELECT * FROM HG_ORDERS WHERE OrgId="+OrgId+"";
                if (OrgId > 0)
                {
                    Query = "SELECT * FROM HG_ORDERS WHERE OrgId=" + OrgId + "";
                }
                else if (CID == 0 && OrgId <= 0)
                {
                    Query = "SELECT * FROM HG_ORDERS WHERE OrgId>0";
                }
                if(CID>0)
                {
                     Query = "select * from HG_Orders where OID in (select Distinct(OID) from HG_OrderItem where OrdById="+CID+") ";
                }
                if (Status > 0)
                {
                    Query += "and Status=" + Status.ToString()+" ";
                }
                if(OrgId==0)
                {
                    Query += " ORDER BY OID DESC";
                } 
                else
                {
                    Query += " ORDER BY Create_Date ASC";
                }
                cmd = new SqlCommand(Query, Obj.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    ObjTmp = new HG_Orders
                    {
                        OID = SDR.GetInt64(0),
                        CID = SDR.GetInt64(1),
                        Status = SDR.GetString(2),
                        Create_By = SDR.GetInt64(3),
                        Create_Date = SDR.GetDateTime(4),
                        Update_By = SDR.GetInt64(5),
                        Update_Date = SDR.GetDateTime(6),
                        OrgId =SDR.GetInt32(8),
                        Table_or_SheatId=SDR.GetInt64(9),
                        PaymentStatus=SDR.GetInt32(10),
                        PayReceivedBy=SDR.GetInt32(11),
                        TableOtp=SDR.GetInt32(12),
                        DisntChargeIDs= SDR.GetString(13),
                        OrderApprovlSts=SDR.GetInt32(14),
                        DeliveryCharge=SDR.GetDouble(15),
                        ContactId=SDR.GetInt32(16),
                        OfferDishCBID = SDR.GetInt32(17)
                    };
                    ListTmp.Add(ObjTmp);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); SDR.Close(); Obj.Con.Close(); Obj.Con.Dispose(); Obj.Con = null; }
            return (ListTmp);
        }

        public List<HG_Orders> GetAllForBS(int OrgId = 0, int CID = 0, int Status = 0)
        {
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<HG_Orders> ListTmp = new List<HG_Orders>();
            HG_Orders ObjTmp = null;
            DBCon Obj = new DBCon();
            try
            {
                             
                string Query = "SELECT * FROM HG_ORDERS WHERE OrgId=" + OrgId + "ORDER BY Create_Date ASC";
                           
                cmd = new SqlCommand(Query, Obj.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    ObjTmp = new HG_Orders
                    {
                        OID = SDR.GetInt64(0),
                        CID = SDR.GetInt64(1),
                        Status = SDR.GetString(2),
                        Create_By = SDR.GetInt64(3),
                        Create_Date = SDR.GetDateTime(4),
                        Update_By = SDR.GetInt64(5),
                        Update_Date = SDR.GetDateTime(6),
                        OrgId = SDR.GetInt32(8),
                        Table_or_SheatId = SDR.GetInt64(9),
                        PaymentStatus = SDR.GetInt32(10),
                        PayReceivedBy = SDR.GetInt32(11),
                        TableOtp = SDR.GetInt32(12),
                        DisntChargeIDs = SDR.GetString(13),
                        OrderApprovlSts = SDR.GetInt32(14),
                        DeliveryCharge = SDR.GetDouble(15),
                        ContactId = SDR.GetInt32(16),
                        OfferDishCBID = SDR.GetInt32(17)
                    };
                    ListTmp.Add(ObjTmp);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); SDR.Close(); Obj.Con.Close(); Obj.Con.Dispose(); Obj.Con = null; }
            return (ListTmp);
        }

        public  HG_Orders  GetOne(Int64 OID)
        {
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR= null;
            HG_Orders ObjTemp = new HG_Orders();
            try
            {
                string Query = "SELECT * FROM HG_Orders Where OID="+OID.ToString();
                cmd = new SqlCommand(Query, dBCon.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                 ObjTemp.OID   = SDR.GetInt64(0);
                 ObjTemp.CID = SDR.GetInt64(1);
                 ObjTemp.Status = SDR.GetString(2);
                 ObjTemp.Create_By = SDR.GetInt64(3);
                 ObjTemp.Create_Date = SDR.GetDateTime(4);
                 ObjTemp.Update_By = SDR.GetInt64(5);
                 ObjTemp.Update_Date = SDR.GetDateTime(6);
                 ObjTemp.OrgId =SDR.GetInt32(8);
                ObjTemp.Table_or_SheatId = SDR.GetInt64(9);
                ObjTemp.PaymentStatus = SDR.GetInt32(10);
                ObjTemp.PayReceivedBy = SDR.GetInt32(11);
                ObjTemp.TableOtp =SDR.GetInt32(12);
                ObjTemp.DisntChargeIDs = SDR.GetString(13);
                ObjTemp.OrderApprovlSts = SDR.GetInt32(14);
                ObjTemp.DeliveryCharge= SDR.GetDouble(15);
                ObjTemp.ContactId = SDR.GetInt32(16);
                    ObjTemp.OfferDishCBID = SDR.GetInt32(17);
                }
            }
            catch (Exception e){ e.ToString(); }

            finally { dBCon.Close();cmd.Dispose(); }
            return (ObjTemp);
        }

        public int DeleteOrderAndOrderItem(System.Int64 OID,bool DeleteOItem)
        {
            HG_Orders Order =new HG_Orders().GetOne(OID);
            if (Order != null)
            {
                Order.Deleted = true;
                Order.Save();
            }
            if (DeleteOItem)
            {
                List<HG_OrderItem> list = new HG_OrderItem().GetAll(OID);
                foreach(var obj in list)
                {
                    obj.Deleted = true;
                    obj.Save();
                }
            }
            return 0;

        }
        public List<HG_Orders> GetListByGetDate(DateTime Formdate , DateTime Todate)
        {
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<HG_Orders> ListTmp = new List<HG_Orders>();
            var theDate = new DateTime(Todate.Year, Todate.Month, Todate.Day, 23, 59, 00);
            HG_Orders ObjTmp = null;
            DBCon Obj = new DBCon();
            try
            {
              //  string Query = "SELECT * FROM HG_ORDERS WHERE Create_Date between '" + Formdate.ToString("MM/dd/yyyy")+"' and '"+ theDate.ToString("MM/dd/yyyy HH:mm:ss")+"' ORDER BY OID DESC";
                cmd = new SqlCommand("GetOrderByDates", Obj.Con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", Formdate.ToString("MM/dd/yyyy"));
                cmd.Parameters.AddWithValue("@Todate", theDate.ToString("MM/dd/yyyy HH:mm:ss"));
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    ObjTmp = new HG_Orders
                    {
                        OID = SDR.GetInt64(0),
                        CID = SDR.GetInt64(1),
                        Status = SDR.GetString(2),
                        Create_By = SDR.GetInt64(3),
                        Create_Date = SDR.GetDateTime(4),
                        Update_By = SDR.GetInt64(5),
                        Update_Date = SDR.GetDateTime(6),
                        OrgId =SDR.GetInt32(8),
                        Table_or_SheatId = SDR.GetInt64(9),
                        PaymentStatus=SDR.GetInt32(10),
                       PayReceivedBy = SDR.GetInt32(11),
                        TableOtp =SDR.GetInt32(12),
                        DisntChargeIDs = SDR.GetString(13),
                        OrderApprovlSts=SDR.GetInt32(14),
                        DeliveryCharge = SDR.GetDouble(15),
                       ContactId = SDR.GetInt32(16),
                        OfferDishCBID = SDR.GetInt32(17)
                };
                    ListTmp.Add(ObjTmp);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); SDR.Close(); Obj.Con.Close(); Obj.Con.Dispose(); }
            return (ListTmp);
        }

        public static double OrderAmt(Int64 OID,double deliverycharge)
        {
            double Amt = 0;
            SqlCommand cmd = null;
            DBCon Obj = new DBCon();
            try
            {
                cmd = new SqlCommand("select [dbo].GetOrderAmt(@OID,@DeliveryCharge) as totalAmt", Obj.Con);
                cmd.Parameters.AddWithValue("@OID", OID);
                cmd.Parameters.AddWithValue("@DeliveryCharge", deliverycharge);
                Amt = Convert.ToDouble(cmd.ExecuteScalar());
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose();  Obj.Con.Close(); Obj.Con.Dispose(); }
            return (Amt);
        }
    }
    
    public class Last3WeekOrder
    {
       public int OrgId { get; set; }
        public double OrderAmt { get; set; }
        public int NumOfOrder { get; set; }
        public List<Last3WeekOrder> GetOrderAmt()
        {
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<Last3WeekOrder> ListTmp = new List<Last3WeekOrder>();
            Last3WeekOrder ObjTmp = null;
            DBCon Obj = new DBCon();
            GetOrder.Ondate = DateTime.Now;
            try
            {
                cmd = new SqlCommand("OrderAmt", Obj.Con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", DateTime.Now.AddDays(-21).Date.ToString("MM/dd/yyyy"));
                cmd.Parameters.AddWithValue("@Todate", DateTime.Now.AddDays(-1).Date.ToString("MM/dd/yyyy"));
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    ObjTmp = new Last3WeekOrder();
                    ObjTmp.OrgId = SDR.GetInt32(0);
                    ObjTmp.OrderAmt = SDR.GetDouble(1);
                    ObjTmp.NumOfOrder= SDR.GetInt32(2);
                    ListTmp.Add(ObjTmp);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); SDR.Close(); Obj.Con.Close(); Obj.Con.Dispose(); }
            return (ListTmp);
        }
    }

    public class GetOrder
    {
        public static List<Last3WeekOrder> List { get; set; }
        public static DateTime Ondate { get; set; }

        public static double GetTotalAmt(int Orgid)
        {
            if (Ondate == null || Ondate.Date < DateTime.Now.Date ||List==null)
            {
                List =new Last3WeekOrder().GetOrderAmt();
            }
            try
            {
                var Obj = List.Find(x => x.OrgId == Orgid);
                return Obj.OrderAmt/ Obj.NumOfOrder;
            }
            catch (Exception)
            {
                return 0;
            }
            
        }
    }
}