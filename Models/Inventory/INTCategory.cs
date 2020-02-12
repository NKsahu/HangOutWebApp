using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models.Inventory
{
    public class INTCategory
    {
        public int CatID { get; set; }
        public string Name { get; set; }

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
                    Quary = "Insert Into INTCategory Values (@Name);SELECT SCOPE_IDENTITY();";
                }
                else
                {
                    Quary = "Update INTCategory Set Name=@Name where CatID=@CatID";
                }
                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@CatID", this.CatID);
                cmd.Parameters.AddWithValue("@Name", this.Name);
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
        public static List<INTCategory> GetAll()
        {
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<INTCategory> listintcat = new List<INTCategory>();
            try
            {
                string Quary = "Select * from INTCategory where ORDER BY CatID DESC";
                cmd = new SqlCommand(Quary, con.Con);
                SDR = cmd.ExecuteReader();

                while (SDR.Read())
                {
                    INTCategory OBJINT = new INTCategory();
                    OBJINT.CatID = SDR.GetInt32(0);
                    OBJINT.Name = SDR.GetString(1);
                    listintcat.Add(OBJINT);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return (listintcat);
        }
    }
}