using System;
using System.Collections.Generic;


namespace HangOut.Models
{
    public class HG_Orders
    {
        public System.Int64 OID { get; set; }
        public System.Int64 CID { get; set; }
        public string Status { get; set; } //Order-Placed=1,Order-Collected=2,
        public System.Int64 Create_By { get; set; }
        public System.DateTime Create_Date { get; set; }
        public System.Int64 Update_By { get; set; }
        public System.DateTime Update_Date { get; set; }
        public bool Deleted { get; set; }
        public string MessIDs { get; set; }
        public string Type { get; set; } // Lunch,Dinner,Breakfast
        public string TifinIds { get; set; }
        public int HubId { get; set; }
        public HG_Orders()
        {
            this.Status = "";
            this.Create_Date = System.DateTime.Now;//
            this.Update_Date = System.DateTime.Now;
            this.TifinIds = "";
            this.HubId = 0;
        }

        public System.Int64 Save()
        {
            System.Data.SqlClient.SqlCommand cmd = null;
            DBCon Obj = new DBCon();
            Int64 R = 0;
            try
            {
                if (this.OID == 0)
                    cmd = new System.Data.SqlClient.SqlCommand("INSERT INTO ORDERS (CID,Status,Create_By,Create_Date,Update_By,Update_Date,Deleted,MessIds,Type,TifinIds,HubId) VALUES (@CID,@Status,@Create_By,@Create_Date,@Update_By,@Update_Date,@Deleted,@MessIds,@Type,@TifinIds,@HubId);select SCOPE_IDENTITY();", Obj.Con);
                else
                {
                    cmd = new System.Data.SqlClient.SqlCommand("UPDATE ORDERS SET CID=@CID,Status=@Status,Create_By=@Create_By,Update_By=@Update_By,Update_Date=@Update_Date,Deleted=@Deleted,MessIds=@MessIds,Type=@Type,TifinIds=@TifinIds,@HubId=@HubId where OID=@OID", Obj.Con);
                    cmd.Parameters.AddWithValue("@OID", this.OID);
                }

                cmd.Parameters.AddWithValue("@CID", this.CID);
                cmd.Parameters.AddWithValue("@Status", this.Status);
                cmd.Parameters.AddWithValue("@Create_By", this.Create_By);
                cmd.Parameters.AddWithValue("@Create_Date", this.Create_Date);
                cmd.Parameters.AddWithValue("@Update_By", this.Update_By);
                cmd.Parameters.AddWithValue("@Update_Date", System.DateTime.Now);
                cmd.Parameters.AddWithValue("@Deleted", this.Deleted);
                cmd.Parameters.AddWithValue("@MessIds", this.MessIDs);
                cmd.Parameters.AddWithValue("@Type", this.Type);
                cmd.Parameters.AddWithValue("@TifinIds", this.TifinIds);
                cmd.Parameters.AddWithValue("@HubId", this.HubId);
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

        public List<HG_Orders> GetAll()
        {
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlDataReader SDR = null;
            List<HG_Orders> ListTmp = new List<HG_Orders>();
            HG_Orders ObjTmp = null;
            DBCon Obj = new DBCon();
            try
            {
                string Query = "SELECT * FROM ORDERS WHERE Deleted=0 ORDER BY OID DESC";
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
                        MessIDs = SDR.IsDBNull(8) ? "0," : SDR.GetString(8),
                        Type = SDR.IsDBNull(9) ? "0" : SDR.GetString(9),
                        TifinIds = SDR.IsDBNull(10) ? "" : SDR.GetString(10),
                        HubId = SDR.IsDBNull(11) ? 0 : SDR.GetInt32(11)
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