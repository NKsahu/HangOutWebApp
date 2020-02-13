using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models.Inventory
{
    public class INTUnits
    {
        public int UnitID { get; set; }
        public string Name { get; set; }
        public int Save()
        {
            int Row = 0;
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Quary = "";
                if (this.UnitID == 0)
                {
                    Quary = "Insert Into INTUnits Values (@Name);SELECT SCOPE_IDENTITY();";
                }
                else
                {
                    Quary = "Update INTUnits Set Name=@Name where UnitID=@UnitID";
                }
                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@UnitID", this.UnitID);
                cmd.Parameters.AddWithValue("@Name", this.Name);
                if (this.UnitID == 0)
                {
                    Row = Convert.ToInt32(cmd.ExecuteScalar());
                    this.UnitID = Row;
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
        public static List<INTUnits> GetAll()
        {
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<INTUnits> listUnit= new List<INTUnits>();
            try
            {
                string Quary = "Select * from INTUnits ORDER BY UnitID DESC";
                cmd = new SqlCommand(Quary, con.Con);
                SDR = cmd.ExecuteReader();

                while (SDR.Read())
                {
                    INTUnits OBJINT = new INTUnits();
                    OBJINT.UnitID = SDR.GetInt32(0);
                    OBJINT.Name = SDR.GetString(1);
                    listUnit.Add(OBJINT);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return (listUnit);
        }
        public   INTUnits GetOne(int ID)
        {
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            INTUnits ObjTmp = new INTUnits();

            try
            {
                string Query = "SELECT * FROM  INTUnits where UnitID="+ID;
                cmd = new SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("@UnitID", ID);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    ObjTmp.UnitID = SDR.GetInt32(0);
                    ObjTmp.Name = SDR.GetString(1);


                }
            }
            catch (System.Exception e)
            { e.ToString(); }

            finally { Con.Close(); }

            return (ObjTmp);
        }
    }
}