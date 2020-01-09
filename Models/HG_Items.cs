using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace HangOut.Models
{
    public class HG_Items
    {
        public int ItemID { get; set; }
        public string Image { get; set; }
        public int CategoryID { get; set; }
        public int OrgID { get; set; }
        [Display(Name ="Item")]
        public string Items { get; set; }
        public double CostPrice { get; set; }// cost price without TAX
        public double Price { get; set; }// Final  price
        public string Qty { get; set; }
        public string ItemMode { get; set; }//{1 VEG ,2 NON-VEG
        [Display(Name = "Tax %")]
        public double Tax { get; set; }// it is tax in decimal value
        public int EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool Status { get; set; }
        [Display(Name = "Apply AddOn")]
        public int ApplyAddOn { get; set; } //{1 NO ,2 YES}
        [Display(Name ="AddOn Category")]
        public int AddOnCatId { get; set; }// addon category id
        public int Type { get; set; }// {1 : food-items  2 :AddOn items
        [Display(Name ="AddOn Type")]
        public int AddOnType { get; set; }// {0 None, 1 Base 2 Addons}
        [Display(Name = "Discription")]
        public string ItemDiscription { get; set; }


        //========
        public string Categoryname { get; set; }
        public HG_Items()
        {
            Image = "";
            EntryDate = System.DateTime.Now;
            EntryBy = 0;
            Status = true;
            ApplyAddOn = 1;
           
        }
        public int Save()
        {
            int Row = 0;


            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            SqlCommand cmd = null;
            try
            {
                Con.Open();
                
                string Query = "";
                if (this.ItemID == 0)
                {
                    Query = "Insert into  HG_Items  values(@CategoryID,@OrgID,@Items,@Price,@Plates,@ItemMode,@Discount,@EntryBy,@EntryDate,@UpdateDate,@Status,@Item_Img,@ApplyAddOn,@CostPrice,@AddOnCatId,@Type,@AddOnType,@ItmDiscriptn); SELECT SCOPE_IDENTITY();";
                    cmd = new SqlCommand(Query, Con);
                    cmd.Parameters.AddWithValue("@EntryBy", this.EntryBy);
                    cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                }
                else
                {
                    Query = "update  HG_Items set CategoryID=@CategoryID,OrgID =@OrgID,Items=@Items,Price=@Price,Plates=@Plates,ItemMode=@ItemMode,Discount=@Discount,EntryBy=@EntryBy,UpdateDate=@UpdateDate,Status=@Status,Item_Img=@Item_Img,ApplyAddOn=@ApplyAddOn,CostPrice=@CostPrice,AddOnCatId=@AddOnCatId,Type=@Type,AddOnType=@AddOnType,ItmDiscriptn=@ItmDiscriptn where ItemID=@ItemID";
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
                cmd.Parameters.AddWithValue("@Discount ", this.Tax);
                cmd.Parameters.AddWithValue("@Status", this.Status);
                cmd.Parameters.AddWithValue("@Item_Img", this.Image);
                cmd.Parameters.AddWithValue("@ApplyAddOn", this.ApplyAddOn);
                cmd.Parameters.AddWithValue("@CostPrice", this.CostPrice);
                cmd.Parameters.AddWithValue("@AddOnCatId", this.AddOnCatId);
                cmd.Parameters.AddWithValue("@Type", this.Type);
                cmd.Parameters.AddWithValue("@AddOnType", this.AddOnType);
                cmd.Parameters.AddWithValue("@ItmDiscriptn", this.ItemDiscription);
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
            finally { Con.Close();
                if (cmd != null) cmd.Dispose();
            }
            return Row;
        }

        public List<HG_Items> GetAll(int OrgId=0,int Type=1)
        {
            var CurrOrgID = HttpContext.Current.Request.Cookies["UserInfo"];
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<HG_Items> ListTmp = new List<HG_Items>();
            string Query = "SELECT * FROM  HG_Items where Type="+Type .ToString()+ " ORDER BY ItemID DESC";
            if (OrgId > 0)
            {
                Query = "SELECT * FROM  HG_Items where OrgID=" + OrgId.ToString()+ " and Type=" + Type.ToString() + " ORDER BY ItemID DESC";
            }
            else if (CurrOrgID != null && int.Parse(CurrOrgID["OrgId"]) > 0)
            {
                Query = "SELECT * FROM  HG_Items where OrgID=" + CurrOrgID["OrgId"] + "and Type=" + Type.ToString() + " ORDER BY ItemID DESC";

            }
            try
            {
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
                    ObjTmp.Tax = SDR.GetDouble(7);
                    ObjTmp.Status = SDR.GetBoolean(11);
                    ObjTmp.Image = SDR.GetString(12);
                    ObjTmp.ApplyAddOn = SDR.GetInt32(13);
                    ObjTmp.CostPrice = SDR.GetDouble(14)==0?SDR.GetDouble(4): SDR.GetDouble(14);
                    ObjTmp.AddOnCatId = SDR.GetInt32(15);
                    ObjTmp.Type = SDR.GetInt32(16);
                    ObjTmp.AddOnType = SDR.GetInt32(17);
                    ObjTmp.ItemDiscription = SDR.GetString(18);
                    ListTmp.Add(ObjTmp);
                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { Con.Close(); }

            return (ListTmp);
        }
        public HG_Items GetOne(Int64 ItemID)
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
                    ObjTmp.Tax = SDR.GetDouble(7);
                    ObjTmp.Status = SDR.GetBoolean(11);
                    ObjTmp.Image = SDR.GetString(12);
                    ObjTmp.ApplyAddOn = SDR.GetInt32(13);
                    ObjTmp.CostPrice = SDR.GetDouble(14) == 0 ? SDR.GetDouble(4) : SDR.GetDouble(14);
                    ObjTmp.AddOnCatId = SDR.GetInt32(15);
                    ObjTmp.Type = SDR.GetInt32(16);
                    ObjTmp.AddOnType = SDR.GetInt32(17);
                    ObjTmp.ItemDiscription = SDR.GetString(18);
                }
            }
            catch (System.Exception e)
            { e.ToString(); }

            finally { Con.Close(); }

            return (ObjTmp);
        }

        public static int Dell(Int64 ID)
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