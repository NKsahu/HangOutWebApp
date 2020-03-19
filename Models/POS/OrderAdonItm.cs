using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace HangOut.Models.POS
{
    public class OrderAdonItm
    {
       public int OrderAddonId { get; set; }
       public Int64 OID { get; set; }
       public Int64 OIID { get; set; }
      public int AdddOnItemId { get; set; }
      public int ItemId { get; set; }
      public double Tax { get; set; }
        public double CostPrice { get; set; }
      public double Price { get; set; }// price with tax included
    
        public string ItemName { get; set; }
    public int Save()
    {
        int Row = 0;
        DBCon dBCon = new DBCon();
        SqlCommand cmd = null;
        try
        {
            string Quary = "";
            if (this.OrderAddonId == 0)
            {
                Quary = "Insert Into HG_OrderAddonItm Values(@OID,@OIID,@AdddOnItemId,@ItemId,@Tax,@CostPrice,@Price) SELECT SCOPE_IDENTITY();";
            }
            //else
            //{
            //    Quary = "Update HG_OrderAddonItm set  AddOnTitle=@AddOnTitle,Min=@Min,Max=@Max,CatOrItmId=@CatOrItmId,DeletedStatus=@DeletedStatus,IsServingAddon=@IsServingAddon where TitleId=@TitleId";
            //}
            cmd = new SqlCommand(Quary, dBCon.Con);
            cmd.Parameters.AddWithValue("@OrderAddonId", this.OrderAddonId);
            cmd.Parameters.AddWithValue("@OID", this.OID);
            cmd.Parameters.AddWithValue("@OIID", this.OIID);
            cmd.Parameters.AddWithValue("@AdddOnItemId", this.AdddOnItemId);
            cmd.Parameters.AddWithValue("@ItemId", this.ItemId);
            cmd.Parameters.AddWithValue("@Tax", this.Tax);
            cmd.Parameters.AddWithValue("@CostPrice", this.CostPrice);
            cmd.Parameters.AddWithValue("@Price", this.Price);
                if (this.OrderAddonId == 0)
            {
                Row = Convert.ToInt32(cmd.ExecuteScalar());
                this.OrderAddonId = Row;
            }
            //else
            //{
            //   cmd.ExecuteNonQuery();
            //        Row = this.OrderAddonId;
            //}

        }
        catch (Exception e) { e.ToString(); }
        finally { cmd.Dispose(); dBCon.Con.Close(); }
        return Row;
    }
        public static List<OrderAdonItm> GetAll(Int64 OIID)
        {
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<OrderAdonItm> listAddon = new List<OrderAdonItm>();
            try
            {
               // string Quary = "Select * from HG_OrderAddonItm where OIID="+OIID;
                cmd = new SqlCommand("OrderAddonItems", con.Con);
                cmd.Parameters.AddWithValue("@OIID", OIID);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    int Index = 0;
                    OrderAdonItm OBJINT = new OrderAdonItm();
                    OBJINT.OrderAddonId = SDR.GetInt32(Index++);
                    OBJINT.OID = SDR.GetInt64(Index++);
                    OBJINT.OIID = SDR.GetInt64(Index++);
                    OBJINT.AdddOnItemId = SDR.GetInt32(Index++);
                    OBJINT.ItemId = SDR.GetInt32(Index++);
                    OBJINT.Tax = SDR.GetDouble(Index++);
                    OBJINT.CostPrice = SDR.GetDouble(Index++);
                    OBJINT.Price = SDR.GetDouble(Index++);
                    OBJINT.ItemName = SDR.GetString(Index++);
                    listAddon.Add(OBJINT);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return (listAddon);
        }
}
}