﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models.MyCustomer
{
    public class ItemOffer
    {
       public int ItemOfferId { get; set; }
        public int ItemId { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public int CashBkId { get; set; }
        public int IsDeleted { get; set; }
        public string ItemName { get; set; }

        public ItemOffer()
        {
            IsDeleted = 0;
        }
        public int Save()
        {
            int R = 0;
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Query = "";
                if (this.ItemOfferId == 0)
                {
                    Query = "Insert into  ItemOffer  values(@ItemId,@Min,@Max,@CashBkId,@IsDeleted); SELECT SCOPE_IDENTITY();";
                    cmd = new SqlCommand(Query, dBCon.Con);
                    cmd.Parameters.AddWithValue("@ItemId", this.ItemId);
                    cmd.Parameters.AddWithValue("@CashBkId", this.CashBkId);
                }
                else
                {
                    Query = "update  ItemOffer set Min=@Min,Max=@Max,IsDeleted=@IsDeleted where ItemOfferId=@ItemOfferId";
                    cmd = new SqlCommand(Query, dBCon.Con);
                    cmd.Parameters.AddWithValue("@ItemOfferId", this.ItemOfferId);
                }
                cmd.Parameters.AddWithValue("@Min", this.Min);
                cmd.Parameters.AddWithValue("@Max", this.Max);
                cmd.Parameters.AddWithValue("@IsDeleted", this.IsDeleted);
                if (this.ItemOfferId == 0)
                {
                    R = Convert.ToInt32(cmd.ExecuteScalar());
                    this.ItemOfferId = R;

                }
                else
                {
                    R = cmd.ExecuteNonQuery();
                    if (R > 0)
                    {
                        R =this.ItemOfferId;
                    }
                       
                }

            }
            catch (Exception e) { e.ToString(); }
            finally
            {
                dBCon.Close();
                if (cmd != null) cmd.Dispose();
            }
            return R;
        }


        
    }

    public class OfferObj
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public int CBID { get; set; }
        public OfferObj()
        {
            Min = 1;
            Max = 1;
        }
        public List<ItemOffer> itemOffers { get; set; }
        public static OfferObj GetAll(int CBID)
        {
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            OfferObj offerObj = new OfferObj();
            offerObj.CBID = CBID;
            List<ItemOffer> ListTmp = new List<ItemOffer>();
            string Query = "SELECT ItemOfferId,ItemId,Min,Max,CashBkId,dbo.GetItemName(ItemId) FROM  ItemOffer where CashBkId=" + CBID.ToString() + " and IsDeleted=0";
            try
            {
                cmd = new SqlCommand(Query, dBCon.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    int index = 0;
                    ItemOffer ObjTmp = new ItemOffer();
                    ObjTmp.ItemOfferId = SDR.GetInt32(index++);
                    ObjTmp.ItemId = SDR.GetInt32(index++);
                    ObjTmp.Min = SDR.GetInt32(index++);
                    ObjTmp.Max = SDR.GetInt32(index++);
                    ObjTmp.CashBkId = SDR.GetInt32(index++);
                    ObjTmp.ItemName = SDR.GetString(index++);
                    offerObj.Min = ObjTmp.Min;
                    offerObj.Max = ObjTmp.Max;
                    ListTmp.Add(ObjTmp);
                }
                offerObj.itemOffers = ListTmp;
            }
            catch (Exception e) { e.ToString(); }
            finally { dBCon.Close(); }

            return (offerObj);
        }
    }
}
