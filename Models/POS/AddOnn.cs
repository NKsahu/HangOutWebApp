using HangOut.Models.POS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace HangOut.Models.POS
{
    public class AddOnn
    {
        public int TitleId { get; set; }
        public string AddOnTitle { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public int CatOrItmId { get; set; }// addon category id or Item Id
        [JsonIgnore]
        public int DeletedStatus { get; set; }//  removed addonitem from form
        [JsonIgnore]
        public bool IsServingAddon { get; set; }// true for serving size items
        //===========
        public List<AddOnItems> AddOnItemList { get; set; }
        public AddOnn()
        {
            IsServingAddon = false;
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
                    Quary = "Insert Into HG_AddOn Values(@AddOnTitle,@Min,@Max,@CatOrItmId,@DeletedStatus,@IsServingAddon) SELECT SCOPE_IDENTITY();";
                }
                else
                {
                    Quary = "Update HG_AddOn set  AddOnTitle=@AddOnTitle,Min=@Min,Max=@Max,CatOrItmId=@CatOrItmId,DeletedStatus=@DeletedStatus,IsServingAddon=@IsServingAddon where TitleId=@TitleId";
                }
                cmd = new SqlCommand(Quary, dBCon.Con);
                cmd.Parameters.AddWithValue("@TitleId", this.TitleId);
                cmd.Parameters.AddWithValue("@AddOnTitle", this.AddOnTitle);
                cmd.Parameters.AddWithValue("@Min", this.Min);
                cmd.Parameters.AddWithValue("@Max", this.Max);
                cmd.Parameters.AddWithValue("@CatOrItmId", this.CatOrItmId);
                cmd.Parameters.AddWithValue("@DeletedStatus", this.DeletedStatus);
                cmd.Parameters.AddWithValue("@IsServingAddon", this.IsServingAddon);
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
                    OBJINT.CatOrItmId = SDR.GetInt32(Index++);
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
                    ObjTmp.CatOrItmId = SDR.GetInt32(Index++);
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
    public int AddOnCatorItmId { get; set; }
        [JsonIgnore]
        public bool IsServingAddon { get; set; }
        [JsonIgnore]
        public List<AddOnn> AddonnList { get; set; }
        //==========
        [JsonIgnore]
        public static List<AddOns> ServingAddonList { get; set; }
        [JsonIgnore]
        public int OrgID { get; set; }
        public AddOns()
    {
        AddonnList = new List<AddOnn>();
    }
        public static AddOns GetOne(int CatItmId,int Sts,bool IsSS)
        {
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            AddOns ObjTmp = new AddOns();
            ObjTmp.AddOnCatorItmId = CatItmId;
            List<AddOnn> tempAddonn = new List<AddOnn>();
            List<AddOnItems> AddonItemList = new List<AddOnItems>();
            try
            {
                if (IsSS)
                {
                    cmd = new SqlCommand("ServingSizeAddon", dBCon.Con);
                }
                else
                {
                    cmd = new SqlCommand("PosGetAddon", dBCon.Con);
                }
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CatOrItmId", CatItmId);
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
                    addOnn.CatOrItmId = SDR.GetInt32(index++);
                    addOnn.DeletedStatus = SDR.GetInt32(index++);
                    addOnn.IsServingAddon = SDR.GetBoolean(index++);
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
                        Addonitem.ItemId = SDR.GetInt32(index++);
                        Addonitem.CostPrice = SDR.GetDouble(index++);
                        Addonitem.Tax = SDR.GetDouble(index++);
                        Addonitem.Price = SDR.GetDouble(index++);
                        Addonitem.AddonID = SDR.GetInt32(index++);
                        Addonitem.CatOrItmId = SDR.GetInt32(index++);
                        Addonitem.DelStatus = SDR.GetInt32(index++);
                        Addonitem.Title= SDR.GetString(index++);
                        Addonitem.IsServingAddon = SDR.GetBoolean(index++);
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
        public static List<AddOnn> GetAddonsAndMultiSSize(HG_Items ObjItem)
        {
            AddOns addOns = new AddOns();
            if (ObjItem.MultiServing == 1)
            {
                AddOns addOnsTemp = GetOne(ObjItem.ItemID, 0, true);
                if (addOnsTemp.AddonnList.Count > 0)
                {
                    addOns.AddonnList.AddRange(addOnsTemp.AddonnList);
                }
            }
            if (ObjItem.ApplyAddOn == 2 && ObjItem.AddOnCatId > 0)
            {
                AddOns addOnsTemp = GetOne(ObjItem.AddOnCatId, 0, false);
                if (addOnsTemp.AddonnList.Count > 0)
                {
                    addOns.AddonnList.AddRange(addOnsTemp.AddonnList);
                }
            }
            return addOns.AddonnList;
        }
    }
}
