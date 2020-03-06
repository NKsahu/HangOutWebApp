using HangOut.Models.POS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models.POS
{
    public class AddOnn
    {
        public int TitleId { get; set; }
        public string TemplateName { get; set; }
        public string AddOnTitle { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public int AddonCatId { get; set; }// addon category id
        public List<AddOnItems> AddOnItems { get; set; }
        public AddOnn()
        {
            AddOnItems = new List<AddOnItems>();
        }
        public int Save()
        {
            int Row = 0;
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Quary = "";
                if (this.TitleId == 0)
                {
                    Quary = "Insert Into HG_AddOnList Values(@TemplateName,@AddOnTitle,@Min,@Max,@AddonCatId) ";
                }
                else
                {
                    Quary = "Update set HG_AddOnList TemplateName=@TemplateName,AddOnTitle=@AddOnTitle,Min=@Min,Max=@Max,AddonCatId=@AddonCatId where TitleId=@TitleId";
                }
                cmd = new SqlCommand(Quary, dBCon.Con);
                cmd.Parameters.AddWithValue("@TitleId", this.TitleId);
                cmd.Parameters.AddWithValue("@TemplateName", this.TemplateName);
                cmd.Parameters.AddWithValue("@AddOnTitle", this.AddOnTitle);
                cmd.Parameters.AddWithValue("@Min", this.Min);
                cmd.Parameters.AddWithValue("@Max", this.Max);
                cmd.Parameters.AddWithValue("@AddonCatId", this.AddonCatId);
                if (this.TitleId == 0)
                {
                    Row = Convert.ToInt32(cmd.ExecuteScalar());
                    this.TitleId = Row;
                }
                else
                {
                    Row = cmd.ExecuteNonQuery();
                    //this.CategoryID = Row;
                }

            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); dBCon.Con.Close(); }
            return Row;

        }
        public static List<AddOnn> GetAll()
        {
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<AddOnn> listUnit = new List<AddOnn>();
            try
            {
                string Quary = "Select * from HG_AddOnList ";
                cmd = new SqlCommand(Quary, con.Con);
                SDR = cmd.ExecuteReader();

                while (SDR.Read())
                {
                    AddOnn OBJINT = new AddOnn();
                    OBJINT.TitleId = SDR.GetInt32(0);
                    OBJINT.TemplateName = SDR.GetString(1);
                    OBJINT.AddOnTitle = SDR.GetString(2);
                    OBJINT.Min = SDR.GetInt32(3);
                    OBJINT.Max = SDR.GetInt32(4);
                    OBJINT.AddonCatId = SDR.GetInt32(5);
                    listUnit.Add(OBJINT);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return (listUnit);
        }
        public AddOnn GetOne(int ID)
        {
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            AddOnn ObjTmp = new AddOnn();

            try
            {
                string Query = "SELECT * FROM  HG_AddOnList where TitleId=" + ID;
                cmd = new SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("@UnitID", ID);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    ObjTmp.TitleId = SDR.GetInt32(0);
                    ObjTmp.TemplateName = SDR.GetString(1);
                    ObjTmp.AddOnTitle = SDR.GetString(2);
                    ObjTmp.Min = SDR.GetInt32(3);
                    ObjTmp.Max = SDR.GetInt32(4);
                    ObjTmp.AddonCatId = SDR.GetInt32(5);
                }
            }
            catch (System.Exception e)
            { e.ToString(); }

            finally { Con.Close(); }

            return (ObjTmp);
        }

    }
}











public class AddOns
{
    public string TemplateName { get; set; }
    public List<AddOnn> AddonnList { get; set; }
    public AddOns()
    {
        AddonnList = new List<AddOnn>();
    }
}
 