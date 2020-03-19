using System;
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
      public double Price { get; set; }// price with tax included
    
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
                Quary = "Insert Into HG_OrderAddonItm Values(@OID,@OIID,@AdddOnItemId,@ItemId,@Tax,@Price) SELECT SCOPE_IDENTITY();";
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
}
}