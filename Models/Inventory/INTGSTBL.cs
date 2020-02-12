using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models.Inventory
{
    public class INTGSTBL
    {
        public int GSID { get; set; }
        public int CatID { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int Unit { get; set; }
        public double Qty { get; set; }

        public int Save()
        {
            int Row = 0;
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Quary = "";
                if (this.CatID == 0)
                {
                    Quary = "Insert Into INTGSTBL Values (@CatID,@Name,@Type,@Unit,@Qty);SELECT SCOPE_IDENTITY();";
                }
                else
                {
                    Quary = "Update INTGSTBL Set CatID=@CatID,Name=@Name,Type=@Type,Unit=@unit,Qty=@Qty where GSID=@GSID";
                }
                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@GSID", this.GSID);
                cmd.Parameters.AddWithValue("@CatID", this.CatID);
                cmd.Parameters.AddWithValue("@Name", this.Name);
                cmd.Parameters.AddWithValue("@Type", this.Type);
                cmd.Parameters.AddWithValue("@Unit", this.Unit);
                cmd.Parameters.AddWithValue("@Qty", this.Qty);
                if (this.CatID == 0)
                {
                    Row = Convert.ToInt32(cmd.ExecuteScalar());
                    this.CatID = Row;
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
        public static List<INTGSTBL> GetAll()
        {
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<INTGSTBL> listintcat = new List<INTGSTBL>();
            try
            {
                string Quary = "Select * from INTGSTBL where ORDER BY GSID DESC";
                cmd = new SqlCommand(Quary, con.Con);
                SDR = cmd.ExecuteReader();

                while (SDR.Read())
                {
                    INTGSTBL OBJINT = new INTGSTBL();
                    OBJINT.GSID = SDR.GetInt32(0);
                    OBJINT.CatID = SDR.GetInt32(1);
                    OBJINT.Name = SDR.GetString(2);
                    OBJINT.Type = SDR.GetInt32(3);
                    OBJINT.Unit = SDR.GetInt32(4);
                    OBJINT.Qty = SDR.GetDouble(5);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return (listintcat);
        }

        public INTGSTBL GetOne(int ID)
        {
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            INTGSTBL ObjTmp = new INTGSTBL();

            try
            {
                string Query = "SELECT * FROM  INTGSTBL where GSID=@GSID";
                cmd = new SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("@GSID", ID);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    ObjTmp.CatID = SDR.GetInt32(0);
                    ObjTmp.Name = SDR.GetString(1);


                }
            }
            catch (System.Exception e)
            { e.ToString(); }

            finally { Con.Close(); }

            return (ObjTmp);
        }
        public static int Dell(int ID)
        {
            int R = 0;
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            try
            {
                string Query = "Delete FROM  INTGSTBL where GSID=" + ID;
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
    }
}