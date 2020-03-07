using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models.POS
{
    public class AddOnItems
    {
        public int AddOnItemId { get; set; }
        public Int64 ItemId { get; set; }
        public double CostPrice {get;set;}
        public double Tax { get; set; }
        public double Price { get; set; }
        public int AddonID { get; set; }
        //===
        public string Title { get; set; }
        public int Save()
        {
            int Row = 0;
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Quary = "";
                if (this.AddOnItemId == 0)
                {
                    Quary = "Insert Into HG_AddOnItems Values(@ItemId,@CostPrice,@Tax,@Price,@AddonID) ";
                }
                else
                {
                    Quary = "Update HG_AddOnItems set  ItemId=@ItemId,CostPrice=@CostPrice,Tax=@Tax,Price=@Price,AddonID=@AddonID where AddOnItemId=@AddOnItemId";
                }
                cmd = new SqlCommand(Quary, dBCon.Con);
                cmd.Parameters.AddWithValue("@AddOnItemId", this.AddOnItemId);
                cmd.Parameters.AddWithValue("@ItemId", this.ItemId);
                cmd.Parameters.AddWithValue("@CostPrice", this.CostPrice);
                cmd.Parameters.AddWithValue("@Tax", this.Tax);
                cmd.Parameters.AddWithValue("@Price", this.Price);
                cmd.Parameters.AddWithValue("@AddonID", this.AddonID);
                if (this.AddOnItemId == 0)
                {
                    Row = Convert.ToInt32(cmd.ExecuteScalar());
                    this.AddOnItemId = Row;
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
        public static List<AddOnItems> GetAll()
        {
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<AddOnItems> listAddOnItems = new List<AddOnItems>();
            try
            {
                string Quary = "Select * from HG_AddOnItems ";
                cmd = new SqlCommand(Quary, con.Con);
                SDR = cmd.ExecuteReader();

                while (SDR.Read())
                {
                    AddOnItems OBJINT = new AddOnItems();
                    OBJINT.AddOnItemId = SDR.GetInt32(0);
                    OBJINT.ItemId = SDR.GetInt64(1);
                    OBJINT.CostPrice = SDR.GetDouble(2);
                    OBJINT.Tax = SDR.GetDouble(3);
                    OBJINT.Price = SDR.GetDouble(4);
                    OBJINT.AddonID = SDR.GetInt32(5);
                    listAddOnItems.Add(OBJINT);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return (listAddOnItems);
        }
        public AddOnItems GetOne(int ID)
        {
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            AddOnItems ObjTmp = new AddOnItems();

            try
            {
                string Query = "SELECT * FROM  HG_AddOnItems where AddOnItemId=" + ID;
                cmd = new SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("@AddOnItemId", ID);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    int index = 0;
                    ObjTmp.AddOnItemId = SDR.GetInt32(index++);
                    ObjTmp.ItemId = SDR.GetInt64(index++);
                    ObjTmp.CostPrice = SDR.GetDouble(index++);
                    ObjTmp.Tax = SDR.GetDouble(index++);
                    ObjTmp.Price = SDR.GetDouble(index++);
                    ObjTmp.AddonID = SDR.GetInt32(index++);
                }
            }
            catch (Exception e)
            { e.ToString(); }

            finally { Con.Close(); }

            return (ObjTmp);
        }
    }
}