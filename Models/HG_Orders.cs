using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace HangOut.Models
{
    public class HG_Orders
    {
        public System.Int64 OID { get; set; }
        public System.Int64 CID { get; set; }
        public string Status { get; set; } //Order-Placed=1,Order-running:2 Order-Completed=3,Order-Cancelled=4,
        public System.Int64 Create_By { get; set; }
        public System.DateTime Create_Date { get; set; }
        public System.Int64 Update_By { get; set; }
        public System.DateTime Update_Date { get; set; }
        public bool Deleted { get; set; }
        public int OrgId { get; set; }
        public Int64 Table_or_SheatId { get; set; }
        public int PaymentStatus { get; set; }//{'0':'unpaid',1:'PaidBycash','2':'by online','3':'ByFoodPaymeGateway'}
        public HG_Orders()
        {
            this.OID = 0;
            this.Status = "";
            this.Create_Date = System.DateTime.Now;//
            this.Update_Date = System.DateTime.Now;
            this.OrgId = 0;
            this.Update_By = 0;
            this.Table_or_SheatId = 0;
        }

        public System.Int64 Save()
        {
            System.Data.SqlClient.SqlCommand cmd = null;
            DBCon Obj = new DBCon();
            Int64 R = 0;
            try
            {
                if (this.OID == 0)
                    cmd = new System.Data.SqlClient.SqlCommand("INSERT INTO HG_ORDERS (CID,Status,Create_By,Create_Date,Update_By,Update_Date,Deleted,OrgId,Table_or_SheatId,PaymentStatus) VALUES (@CID,@Status,@Create_By,@Create_Date,@Update_By,@Update_Date,@Deleted,@OrgId,@Table_or_SheatId,@PaymentStatus);select SCOPE_IDENTITY();", Obj.Con);
                else
                {
                    cmd = new System.Data.SqlClient.SqlCommand("UPDATE HG_ORDERS SET CID=@CID,Status=@Status,Create_By=@Create_By,Update_By=@Update_By,Update_Date=@Update_Date,Deleted=@Deleted,@OrgId=@OrgId,Table_or_SheatId=@Table_or_SheatId,PaymentStatus=@PaymentStatus where OID=@OID", Obj.Con);
                    cmd.Parameters.AddWithValue("@OID", this.OID);
                }

                cmd.Parameters.AddWithValue("@CID", this.CID);
                cmd.Parameters.AddWithValue("@Status", this.Status);
                cmd.Parameters.AddWithValue("@Create_By", this.Create_By);
                cmd.Parameters.AddWithValue("@Create_Date", this.Create_Date);
                cmd.Parameters.AddWithValue("@Update_By", this.Update_By);
                cmd.Parameters.AddWithValue("@Update_Date", System.DateTime.Now);
                cmd.Parameters.AddWithValue("@Deleted", this.Deleted);
                cmd.Parameters.AddWithValue("@OrgId", this.OrgId);
                cmd.Parameters.AddWithValue("@Table_or_SheatId", this.Table_or_SheatId);
                cmd.Parameters.AddWithValue("@PaymentStatus", this.PaymentStatus);
                if (this.OID == 0)
                {
                    R = System.Convert.ToInt64(cmd.ExecuteScalar());
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
            catch (System.Exception e) { R = 0; this.OID = 0; e.ToString(); }
            finally { cmd.Dispose(); Obj.Con.Close(); Obj.Con.Dispose(); Obj.Con = null; }
            return R;
        }

        public List<HG_Orders> GetAll(int OrgId=0,int CID=0)
        {
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlDataReader SDR = null;
            List<HG_Orders> ListTmp = new List<HG_Orders>();
            HG_Orders ObjTmp = null;
            DBCon Obj = new DBCon();
            try
            {

                string Query = "SELECT * FROM HG_ORDERS WHERE OrgId="+OrgId+" and Deleted=0 ORDER BY OID DESC";
                if(CID>0)
                {
                     Query = "SELECT * FROM HG_ORDERS WHERE CID=" + CID + " and Deleted=0 ORDER BY OID DESC";

                }
                cmd = new System.Data.SqlClient.SqlCommand(Query, Obj.Con);
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
                        OrgId = SDR.IsDBNull(8) ? 0 : SDR.GetInt32(8),
                        Table_or_SheatId=SDR.IsDBNull(9)?0:SDR.GetInt64(9),
                        PaymentStatus=SDR.IsDBNull(10)?0:SDR.GetInt32(10)
                    };
                    ListTmp.Add(ObjTmp);
                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { cmd.Dispose(); SDR.Close(); Obj.Con.Close(); Obj.Con.Dispose(); Obj.Con = null; }
            return (ListTmp);
        }

        public  HG_Orders  GetOne(Int64 OID)
        {
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR= null;
            HG_Orders ObjTemp = new HG_Orders();
            try
            {
                string Query = "SELECT * FROM HG_Orders Where OID="+OID.ToString();
                cmd = new SqlCommand(Query, Con);
                //cmd.Parameters.AddWithValue("@OID",this.OID);
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
                 ObjTemp.OrgId = SDR.IsDBNull(8) ? 0 : SDR.GetInt32(8);
                ObjTemp.Table_or_SheatId = SDR.IsDBNull(9) ? 0 : SDR.GetInt64(9);
                    ObjTemp.PaymentStatus = SDR.IsDBNull(10) ? 0 : SDR.GetInt32(10);
                }
            }
            catch (System.Exception e){ e.ToString(); }

            finally { Con.Close(); }
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
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlDataReader SDR = null;
            List<HG_Orders> ListTmp = new List<HG_Orders>();
            HG_Orders ObjTmp = null;
            DBCon Obj = new DBCon();
            try
            {
                string Query = "SELECT * FROM HG_ORDERS WHERE Create_Date between '" + Formdate.ToString("MM/dd/yyyy")+"' and '"+Todate.ToString("MM/dd/yyyy HH:mm:ss")+"' ORDER BY OID DESC";
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
                        OrgId = SDR.IsDBNull(8) ? 0 : SDR.GetInt32(8),
                        Table_or_SheatId = SDR.IsDBNull(9) ? 0 : SDR.GetInt64(9),
                        PaymentStatus=SDR.IsDBNull(10)?0:SDR.GetInt32(10)
                    };
                    ListTmp.Add(ObjTmp);
                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { cmd.Dispose(); SDR.Close(); Obj.Con.Close(); Obj.Con.Dispose(); Obj.Con = null; }
            return (ListTmp);
        }
    }
}