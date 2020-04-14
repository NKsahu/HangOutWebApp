using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models.MyCustomer
{
    public class OfferMenu
    {
      public int MenuId { get; set; }
       public string Name { get; set; }
       public bool IsComplementry { get; set; }
      public int  CBID { get; set; }
      public int OfferTitleId { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }

        // non table keys
        public double TotalItmPrice { get; set; }
        public List<ItemOffer> itemOffers { get; set; }

        public OfferMenu()
        {
            itemOffers = new List<ItemOffer>();
            Min = 1;
            Max = 1;
        }
        public int Save()
        {
            int R = 0;
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Query = "";
                if (this.MenuId == 0)
                {
                    Query = "Insert into  OfferMenu  values(@Name,@IsComplementry,@CBID,@OfferTitleId,@Min,@Max); SELECT SCOPE_IDENTITY();";
                    cmd = new SqlCommand(Query, dBCon.Con);
                    cmd.Parameters.AddWithValue("@CBID", this.CBID);

                }
                else
                {
                    Query = "update  OfferMenu set Name=@Name,IsComplementry=@IsComplementry,OfferTitleId=@OfferTitleId,Min=@Min,Max=@Max where MenuId=@MenuId";
                    cmd = new SqlCommand(Query, dBCon.Con);
                    cmd.Parameters.AddWithValue("@MenuId", this.MenuId);
                }
                cmd.Parameters.AddWithValue("@Name", this.Name);
                cmd.Parameters.AddWithValue("@IsComplementry", this.IsComplementry);
                cmd.Parameters.AddWithValue("@OfferTitleId", this.OfferTitleId);
                cmd.Parameters.AddWithValue("@Min", this.Min);
                cmd.Parameters.AddWithValue("@Max", this.Max);
                if (this.MenuId == 0)
                {
                    R = Convert.ToInt32(cmd.ExecuteScalar());
                    this.MenuId = R;

                }
                else
                {
                    R = cmd.ExecuteNonQuery();
                    if (R > 0)
                    {
                        R = this.MenuId;
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
        public static List<OfferMenu> GetAll(int OfferTitleId)
        {

            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<OfferMenu> TmpList = new List<OfferMenu>();
            string Query = "SELECT * FROM  OfferMenu where OfferTitleId="+OfferTitleId;
            try
            {
                cmd = new SqlCommand(Query, dBCon.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    int index = 0;
                    OfferMenu ObjTmp = new OfferMenu();
                    ObjTmp.MenuId = SDR.GetInt32(index++);
                    ObjTmp.Name = SDR.GetString(index++);
                    ObjTmp.IsComplementry = SDR.GetBoolean(index++);
                    ObjTmp.CBID = SDR.GetInt32(index++);
                    ObjTmp.OfferTitleId = SDR.GetInt32(index++);
                    ObjTmp.Min = SDR.GetInt32(index++);
                    ObjTmp.Max = SDR.GetInt32(index++);
                    ObjTmp.itemOffers = ItemOffer.GetAll("Select *, dbo.GetItemName(ItemId),dbo.GetItemPrice(ItemId)  FROM  ItemOffer where MenuId=" + ObjTmp.MenuId+ " and IsDeleted=0");
                    ObjTmp.TotalItmPrice = ObjTmp.itemOffers.Sum(x => x.TotalItemPrice);
                    TmpList.Add(ObjTmp);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { dBCon.Close(); }
            return (TmpList);
        }
    }
}