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
        public int Type { get; set; }//1=Goods and 2 = Service
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
                if (this.GSID == 0)
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
                cmd.Parameters.AddWithValue("@Type", 1);
                cmd.Parameters.AddWithValue("@Unit", this.Unit);
                cmd.Parameters.AddWithValue("@Qty", this.Qty);
                if (this.GSID == 0)
                {
                    Row = Convert.ToInt32(cmd.ExecuteScalar());
                    this.GSID = Row;
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
        public static List<INTGSTBL> GetAll(int Type)
        {
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<INTGSTBL> listintcat = new List<INTGSTBL>();
            try
            {
                string Quary = "Select * from INTGSTBL where Type="+Type;
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
                    listintcat.Add(OBJINT);
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
                    ObjTmp.GSID = SDR.GetInt32(0);
                    ObjTmp.CatID = SDR.GetInt32(1);
                    ObjTmp.Name = SDR.GetString(2);
                    ObjTmp.Unit = SDR.GetInt32(3);
                    ObjTmp.Qty = SDR.GetInt32(4);


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
        public static List<Unit> Types()
        {

            List<Unit> unit = new List<Unit>();
            unit.Add(new Unit { Id = 0, Name = "Gram" });
            unit.Add(new Unit { Id = 1, Name = "" });
            unit.Add(new Unit { Id = 2, Name = "Subjective" });
            unit.Add(new Unit { Id = 3, Name = "Like Dislike Ok" });
            unit.Add(new Unit { Id = 4, Name = "Star-Subjective" });
            return unit;
        }

    }
   
}
public class Unit
{
   public int Id { get; set; }
   public string Name { get; set; }
}