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
        public int Typeid { get; set; }//1=Goods and 2 = Service
        public int UnitID { get; set; }
        public double Qty { get; set; }
        public double PurchasedPrize { get; set; }
        public double PurchasedTax { get; set; }
        public double SalePrize { get; set; }
        public double SaleTax { get; set; }
        public bool ISSaleable { get; set; }
        public bool ISdirectlyPurchased { get; set; }
        public bool ISProcessed { get; set; }
        public double ServicePrize { get; set; }
        public double ServiceTax { get; set; }
        public List<INTItems> iNTItems { get; set; }
        public INTGSTBL()
        {
            //Qty = 0;
            //PurchasedPrize = 0;
            //PurchasedTax = 0;
            //SalePrize = 0;
            //SaleTax = 0;
            //ISSaleable = false;
            //ISdirectlyPurchased = false;
            //ISProcessed = false;
            iNTItems = new List<INTItems>();
        }

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
                    Quary = "Insert Into INTGSTBL Values (@CatID,@Name,@Typeid,@UnitID,@Qty,@PurchasedPrize,@PurchasedTax,@SalePrize,@SaleTax,@ISSaleable,@ISdirectlyPurchased,@ISProcessed,@ServicePrize,@ServiceTax);SELECT SCOPE_IDENTITY();";
                }
                else
                {
                    Quary = "Update INTGSTBL Set CatID=@CatID,Name=@Name,Typeid=@Typeid,UnitID=@UnitID,Qty=@Qty,PurchasedPrize=@PurchasedPrize,PurchasedTax=@PurchasedTax,SalePrize=@SalePrize,SaleTax=@SaleTax,ISSaleable=@ISSaleable,ISdirectlyPurchased=@ISdirectlyPurchased,ISProcessed=@ISProcessed,ServicePrize=@ServicePrize,ServiceTax=@ServiceTax where GSID=@GSID";
                }
                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@GSID", this.GSID);
                cmd.Parameters.AddWithValue("@CatID", this.CatID);
                cmd.Parameters.AddWithValue("@Name", this.Name);
                cmd.Parameters.AddWithValue("@Typeid", this.Typeid);
                cmd.Parameters.AddWithValue("@UnitID", this.UnitID);
                cmd.Parameters.AddWithValue("@Qty", this.Qty);
                cmd.Parameters.AddWithValue("@PurchasedPrize", this.PurchasedPrize);
                cmd.Parameters.AddWithValue("@PurchasedTax", this.PurchasedTax);
                cmd.Parameters.AddWithValue("@SalePrize", this.SalePrize);
                cmd.Parameters.AddWithValue("@SaleTax", this.SaleTax);
                cmd.Parameters.AddWithValue("@ISSaleable", this.ISSaleable);
                cmd.Parameters.AddWithValue("@ISdirectlyPurchased", this.ISdirectlyPurchased);
                cmd.Parameters.AddWithValue("@ISProcessed", this.ISProcessed);
                cmd.Parameters.AddWithValue("@ServicePrize", this.ServicePrize);
                cmd.Parameters.AddWithValue("@ServiceTax", this.ServiceTax);
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
        public static List<INTGSTBL> GetAll()
        {
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<INTGSTBL> listintcat = new List<INTGSTBL>();
            try
            {
                string Quary = "Select * from INTGSTBL";
                cmd = new SqlCommand(Quary, con.Con);
                SDR = cmd.ExecuteReader();

                while (SDR.Read())
                {
                    INTGSTBL OBJINT = new INTGSTBL();
                    OBJINT.GSID = SDR.GetInt32(0);
                    OBJINT.CatID = SDR.GetInt32(1);
                    OBJINT.Name = SDR.GetString(2);
                    OBJINT.Typeid = SDR.GetInt32(3);
                    OBJINT.UnitID = SDR.GetInt32(4);
                    OBJINT.Qty = SDR.GetDouble(5);
                    OBJINT.PurchasedPrize = SDR.GetDouble(6);
                    OBJINT.PurchasedTax = SDR.GetDouble(7);
                    OBJINT.SalePrize = SDR.GetDouble(8);
                    OBJINT.SaleTax = SDR.GetDouble(9);
                    OBJINT.ISSaleable = SDR.GetBoolean(10);
                    OBJINT.ISdirectlyPurchased = SDR.GetBoolean(11);
                    OBJINT.ISProcessed = SDR.GetBoolean(12);
                    OBJINT.ServicePrize = SDR.GetDouble(13);
                    OBJINT.ServiceTax = SDR.GetDouble(14);
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
                    ObjTmp.Typeid = SDR.GetInt32(3);
                    ObjTmp.UnitID = SDR.GetInt32(4);
                    ObjTmp.Qty = SDR.GetDouble(5);
                    ObjTmp.PurchasedPrize = SDR.GetDouble(6);
                    ObjTmp.PurchasedTax = SDR.GetDouble(7);
                    ObjTmp.SalePrize = SDR.GetDouble(8);
                    ObjTmp.SaleTax = SDR.GetDouble(9);
                    ObjTmp.ISSaleable = SDR.GetBoolean(10);
                    ObjTmp.ISdirectlyPurchased = SDR.GetBoolean(11);
                    ObjTmp.ISProcessed = SDR.GetBoolean(12);
                    ObjTmp.ServicePrize = SDR.GetDouble(13);
                    ObjTmp.ServiceTax = SDR.GetDouble(14);

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
    public class InventoryType
    {
        public int Typeid { get; set; }
        public string Name { get; set; }
        public static List<InventoryType> List { get; set; }
        public static List<InventoryType> ListOrgTypeidList()
        {
            List<InventoryType> list = new List<InventoryType>();
            list.Add(new InventoryType { Typeid = 0, Name = "Select Type" });
            list.Add(new InventoryType { Typeid = 1, Name = "Goods" });
            list.Add(new InventoryType { Typeid = 2, Name = "Service" });
            return list;
        }
    }
}
