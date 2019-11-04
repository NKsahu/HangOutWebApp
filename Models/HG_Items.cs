using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace HangOut.Models
{
    public class HG_Items
    {
        public int ItemID { get; set; }
        public string Image { get; set; }
        public int CategoryID { get; set; }
        public int OrgID { get; set; }
        public string Items { get; set; }
        public double Price { get; set; }
        public string Qty { get; set; }
        public string ItemMode { get; set; }
        public double Discount { get; set; }
        public int EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool Status { get; set; }
        public HG_Items()
        {
            Image = "";
            EntryDate = System.DateTime.Now;
            EntryBy = 0;
        }
        public int Save()
        {
            int Row = 0;


            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            try
            {
                Con.Open();
                SqlCommand cmd = null;
                string Query = "";
                if (this.ItemID == 0)
                {
                    Query = "Insert into  HG_Items  values(@CategoryID,@OrgID,@Items,@Price,@Plates,@ItemMode,@Discount,@EntryBy,@EntryDate,@UpdateDate,@Status,@Item_Img); SELECT SCOPE_IDENTITY();";
                    cmd = new SqlCommand(Query, Con);
                    cmd.Parameters.AddWithValue("@EntryBy", this.EntryBy);
                    cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);

                }
                else
                {

                    Query = "update  HG_Items set CategoryID=@CategoryID,OrgID =@OrgID,Items=@Items,Price=@Price,Plates=@Plates,ItemMode=@ItemMode,Discount=@Discount,EntryBy=@EntryBy,UpdateDate=@UpdateDate,Status=@Status,Item_Img=@Item_Img where ItemID=@ItemID";
                    cmd = new SqlCommand(Query, Con);
                    cmd.Parameters.AddWithValue("@ItemID", this.ItemID);
                    cmd.Parameters.AddWithValue("@EntryBy", EntryBy);
                    cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                }
                cmd.Parameters.AddWithValue("@CategoryID", this.CategoryID);
                cmd.Parameters.AddWithValue("@OrgID", this.OrgID);
                cmd.Parameters.AddWithValue("@Items", this.Items);
                cmd.Parameters.AddWithValue("@Price", this.Price);
                cmd.Parameters.AddWithValue("@Plates", this.Qty);
                cmd.Parameters.AddWithValue("@ItemMode ", this.ItemMode);
                cmd.Parameters.AddWithValue("@Discount ", this.Discount);
                cmd.Parameters.AddWithValue("@Status", this.Status);
                cmd.Parameters.AddWithValue("@Item_Img", this.Image);
                if (this.ItemID == 0)
                {
                    Row = System.Convert.ToInt32(cmd.ExecuteScalar());
                    this.ItemID = Row;

                }
                else
                {
                    Row = cmd.ExecuteNonQuery();
                    if (Row > 0)
                        Row = this.ItemID;
                }
                
            }
            catch (Exception e) { e.ToString(); }
            finally { Con.Close(); }
            return Row;
        }

        public List<HG_Items> GetAll()
        {
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<HG_Items> ListTmp = new List<HG_Items>();
            
            try
            {
                string Query = "SELECT * FROM  HG_Items ORDER BY ItemID DESC";
                cmd = new SqlCommand(Query, Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    HG_Items ObjTmp = new HG_Items();
                    ObjTmp.ItemID = SDR.GetInt32(0);
                    ObjTmp.CategoryID = SDR.GetInt32(1);
                    ObjTmp.OrgID = SDR.GetInt32(2);
                    ObjTmp.Items = SDR.GetString(3);
                    ObjTmp.Price = SDR.GetDouble(4);
                    ObjTmp.Qty = SDR.GetString(5);
                    ObjTmp.ItemMode = SDR.GetString(6);
                    ObjTmp.Discount = SDR.GetDouble(7);
                    ObjTmp.Status = SDR.GetBoolean(11);
                    ObjTmp.Image = SDR.GetString(12);
                    ListTmp.Add(ObjTmp);
                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { Con.Close(); }

            return (ListTmp);
        }
        public HG_Items GetOne(int ItemID)
        {
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            HG_Items ObjTmp = new HG_Items();

            try
            {
                string Query = "SELECT * FROM  HG_Items where ItemID=@ItemID";
                cmd = new SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("@ItemID", ItemID);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    ObjTmp.ItemID = SDR.GetInt32(0);
                    ObjTmp.CategoryID = SDR.GetInt32(1);
                    ObjTmp.OrgID = SDR.GetInt32(2);
                    ObjTmp.Items = SDR.GetString(3);
                    ObjTmp.Price = SDR.GetDouble(4);
                    ObjTmp.Qty = SDR.GetString(5);
                    ObjTmp.ItemMode = SDR.GetString(6);
                    ObjTmp.Discount = SDR.GetDouble(7);
                    ObjTmp.Status = SDR.GetBoolean(11);
                    ObjTmp.Image = SDR.GetString(12);
                }
            }
            catch (System.Exception e)
            { e.ToString(); }

            finally { Con.Close(); }

            return (ObjTmp);
        }

        public int Dell(int ID)
        {
            int R = 0;
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            try
            {
                string Query = "Delete FROM  HG_Items where ItemID=" + ID;
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