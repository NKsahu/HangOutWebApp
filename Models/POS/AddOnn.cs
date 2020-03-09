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
        public string AddOnTitle { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public int AddonCatId { get; set; }// addon category id
        public List<AddOnItems> AddOnItemList { get; set; }
        //========
        public int DeletedStatus { get; set; }//  removed addonitem from form
        public AddOnn()
        {
            DeletedStatus = 0;
            AddOnItemList = new List<AddOnItems>();
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
                    Quary = "Insert Into HG_AddOn Values(@AddOnTitle,@Min,@Max,@CategoryId,@DeletedStatus) SELECT SCOPE_IDENTITY();";
                }
                else
                {
                    Quary = "Update HG_AddOn set  AddOnTitle=@AddOnTitle,Min=@Min,Max=@Max,CategoryId=@CategoryId,DeletedStatus=@DeletedStatus where TitleId=@TitleId";
                }
                cmd = new SqlCommand(Quary, dBCon.Con);
                cmd.Parameters.AddWithValue("@TitleId", this.TitleId);
                cmd.Parameters.AddWithValue("@AddOnTitle", this.AddOnTitle);
                cmd.Parameters.AddWithValue("@Min", this.Min);
                cmd.Parameters.AddWithValue("@Max", this.Max);
                cmd.Parameters.AddWithValue("@CategoryId", this.AddonCatId);
                cmd.Parameters.AddWithValue("@DeletedStatus", this.DeletedStatus);
                if (this.TitleId == 0)
                {
                    Row = Convert.ToInt32(cmd.ExecuteScalar());
                    this.TitleId = Row;
                }
                else
                {
                    Row = cmd.ExecuteNonQuery();
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
            List<AddOnn> listAddon = new List<AddOnn>();
            try
            {
                string Quary = "Select * from HG_AddOn ";
                cmd = new SqlCommand(Quary, con.Con);
                SDR = cmd.ExecuteReader();

                while (SDR.Read())
                {
                    int Index = 0;
                    AddOnn OBJINT = new AddOnn();
                    OBJINT.TitleId = SDR.GetInt32(Index++);
                    OBJINT.AddOnTitle = SDR.GetString(Index++);
                    OBJINT.Min = SDR.GetInt32(Index++);
                    OBJINT.Max = SDR.GetInt32(Index++);
                    OBJINT.AddonCatId = SDR.GetInt32(Index++);
                    listAddon.Add(OBJINT);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return (listAddon);
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
                    int Index = 0;
                    ObjTmp.TitleId = SDR.GetInt32(Index++);
                    ObjTmp.AddOnTitle = SDR.GetString(Index++);
                    ObjTmp.Min = SDR.GetInt32(Index++);
                    ObjTmp.Max = SDR.GetInt32(Index++);
                    ObjTmp.AddonCatId = SDR.GetInt32(Index++);
                }
            }
            catch (Exception e)
            { e.ToString(); }

            finally { Con.Close(); }

            return (ObjTmp);
        }
        public static int Delete(int ID)
        {
            int R = 0;
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Query = "Delete FROM  HG_AddOn where TitleId=" + ID;
                cmd = new SqlCommand(Query, dBCon.Con);
                R = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            { e.ToString(); }

            finally
            {
                cmd.Dispose(); dBCon.Con.Close();
            }
            return R;
        }
    }

public class AddOns
{
    public int AddOnCategoryId { get; set; }
    public List<AddOnn> AddonnList { get; set; }
        public AddOns()
    {
        AddonnList = new List<AddOnn>();
    }
        public static AddOns GetOne(int categoryId,int Sts)
        {
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            AddOns ObjTmp = new AddOns();
            ObjTmp.AddOnCategoryId = categoryId;
            List<AddOnn> tempAddonn = new List<AddOnn>();
            List<AddOnItems> AddonItemList = new List<AddOnItems>();
            try
            {
                //string Query = "SELECT * FROM  HG_AddOn where CategoryId="+categoryId+ " and DeletedStatus=0;SELECT * FROM  HG_AddOnItems where CategoryID=" + categoryId+ " and DelStatus=0";
                cmd = new SqlCommand("PosGetAddon", dBCon.Con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                cmd.Parameters.AddWithValue("@Status", Sts);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    AddOnn addOnn = new AddOnn();
                    int index = 0;
                    addOnn.TitleId = SDR.GetInt32(index++);
                    addOnn.AddOnTitle = SDR.GetString(index++);
                    addOnn.Min= SDR.GetInt32(index++);
                    addOnn.Max = SDR.GetInt32(index++);
                    addOnn.AddonCatId = SDR.GetInt32(index++);
                    addOnn.DeletedStatus = SDR.GetInt32(index++);
                    tempAddonn.Add(addOnn);

                }
                ObjTmp.AddonnList = tempAddonn;
                SDR.NextResult();
                if (SDR.HasRows)
                {
                    while (SDR.Read())
                    {
                        AddOnItems Addonitem = new AddOnItems();
                        int index = 0;
                        Addonitem.AddOnItemId = SDR.GetInt32(index++);
                        Addonitem.ItemId = SDR.GetInt64(index++);
                        Addonitem.CostPrice = SDR.GetDouble(index++);
                        Addonitem.Tax = SDR.GetDouble(index++);
                        Addonitem.Price = SDR.GetDouble(index++);
                        Addonitem.AddonID = SDR.GetInt32(index++);
                        Addonitem.CategoryID = SDR.GetInt32(index++);
                        Addonitem.DelStatus = SDR.GetInt32(index++);
                        Addonitem.Title= SDR.GetString(index++);
                        AddonItemList.Add(Addonitem);
                    }
                }
                
                foreach(var Addonn in ObjTmp.AddonnList)
                {
                    Addonn.AddOnItemList = AddonItemList.FindAll(x => x.AddonID == Addonn.TitleId);
                }
            }
            catch (Exception e)
            { e.ToString(); }

            finally { dBCon.Con.Close();SDR.Close(); }

            return (ObjTmp);
        }
        
    }
}
