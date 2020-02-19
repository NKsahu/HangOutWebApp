using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models.Inventory
{
    public class INTItems
    {

        public int ItemID { get; set; }
        public double IQty { get; set; }
        public int IUnitID { get; set; }
        public int IParentId { get; set; }
        
        public int Save()
        {
            int Row = 0;
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Quary = "";
                if (this.ItemID == 0)
                {
                    Quary = "Insert Into INTItems Values (@Qty,@UnitID,@GSID);SELECT SCOPE_IDENTITY();";
                }
                else
                {
                    Quary = "Update INTItems Set Qty=@Qty,UnitID=@UnitID,GSID=@GSID where ItemID=@ItemID";
                }
                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@ItemID", this.ItemID);
                cmd.Parameters.AddWithValue("@Qty", this.IQty);
                cmd.Parameters.AddWithValue("@UnitID", this.IUnitID);
                cmd.Parameters.AddWithValue("@GSID", this.IParentId);
                if (this.ItemID == 0)
                {
                    Row = Convert.ToInt32(cmd.ExecuteScalar());
                    this.ItemID = Row;
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
        public static List<INTItems> GetAll()
        {
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<INTItems> listUnit = new List<INTItems>();
            try
            {
                string Quary = "Select * from INTItems ORDER BY ItemID DESC";
                cmd = new SqlCommand(Quary, con.Con);
                SDR = cmd.ExecuteReader();

                while (SDR.Read())
                {
                    INTItems OBJINT = new INTItems();
                    OBJINT.ItemID = SDR.GetInt32(0);
                    OBJINT.IQty = SDR.GetDouble(1);
                    OBJINT.IUnitID = SDR.GetInt32(2);
                    OBJINT.IParentId = SDR.GetInt32(3);
                    listUnit.Add(OBJINT);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return (listUnit);
        }
        public INTItems GetOne(int ID)
        {
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            INTItems ObjTmp = new INTItems();

            try
            {
                string Query = "SELECT * FROM  INTItems where ItemID=" + ID;
                cmd = new SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("@ItemID", ID);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    ObjTmp.ItemID = SDR.GetInt32(0);
                    ObjTmp.IQty = SDR.GetDouble(1);
                    ObjTmp.IUnitID = SDR.GetInt32(2);
                    ObjTmp.IParentId = SDR.GetInt32(3);
                }
            }
            catch (System.Exception e)
            { e.ToString(); }

            finally { Con.Close(); }

            return (ObjTmp);
        }
    }
}
