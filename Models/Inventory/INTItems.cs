using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models.Inventory
{
    public class INTItems
    {
        public int SubItemID { get; set; }
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
                if (this.SubItemID == 0)
                {
                    Quary = "Insert Into INTItems Values (@ItemID,@Qty,@UnitID,@GSID);SELECT SCOPE_IDENTITY();";
                }
                else
                {
                    Quary = "Update INTItems Set ItemID=@ItemID, Qty=@Qty,UnitID=@UnitID,GSID=@GSID where SubItemID=@SubItemID";
                }
                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@SubItemID", this.SubItemID);
                cmd.Parameters.AddWithValue("@ItemID", this.ItemID);
                cmd.Parameters.AddWithValue("@Qty", this.IQty);
                cmd.Parameters.AddWithValue("@UnitID", this.IUnitID);
                cmd.Parameters.AddWithValue("@GSID", this.IParentId);
                if (this.SubItemID == 0)
                {
                    Row = Convert.ToInt32(cmd.ExecuteScalar());
                    this.SubItemID = Row;
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
                string Quary = "Select * from INTItems ORDER BY SubItemID DESC";
                cmd = new SqlCommand(Quary, con.Con);
                SDR = cmd.ExecuteReader();

                while (SDR.Read())
                {
                    INTItems OBJINT = new INTItems();
                    OBJINT.SubItemID = SDR.GetInt32(0);
                    OBJINT.ItemID = SDR.GetInt32(1);
                    OBJINT.IQty = SDR.GetDouble(2);
                    OBJINT.IUnitID = SDR.GetInt32(3);
                    OBJINT.IParentId = SDR.GetInt32(4);
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
                string Query = "SELECT * FROM  INTItems where SubItemID=" + ID;
                cmd = new SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("@SubItemID", ID);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    ObjTmp.SubItemID = SDR.GetInt32(0);
                    ObjTmp.ItemID = SDR.GetInt32(1);
                    ObjTmp.IQty = SDR.GetDouble(2);
                    ObjTmp.IUnitID = SDR.GetInt32(3);
                    ObjTmp.IParentId = SDR.GetInt32(4);
                }
            }
            catch (System.Exception e)
            { e.ToString(); }

            finally { Con.Close(); }

            return (ObjTmp);
        }
    }
}
