using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models.MyCustomer
{
    public class OfferTitle
    {
      public int TitleId { get; set; }
     public int  CBID { get; set; }
     public string Name { get; set; }
    public string  Discription { get; set; }
    public int  MaxOrdQty { get; set; }
    public float FinalPrice { get; set; }
    public float Tax { get; set; }
     
    public List<OfferMenu> OfferMenus { get; set; }
    public int Save(){
            int R = 0;
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Query = "";
                if (this.TitleId == 0)
                {
                    Query = "Insert into  OfferTitle  values(@CBID,@Name,@Discription,@MaxOrdQty,@FinalPrice,@Tax); SELECT SCOPE_IDENTITY();";
                    cmd = new SqlCommand(Query, dBCon.Con);
                    cmd.Parameters.AddWithValue("@CBID", this.CBID);
                }
                else
                {
                    Query = "update  OfferTitle set Name=@Name,Discription=@Discription,MaxOrdQty=@MaxOrdQty,FinalPrice=@FinalPrice,Tax=@Tax where TitleId=@TitleId";
                    cmd = new SqlCommand(Query, dBCon.Con);
                    cmd.Parameters.AddWithValue("@TitleId", this.TitleId);
                }
                cmd.Parameters.AddWithValue("@Name", this.Name);
                cmd.Parameters.AddWithValue("@Discription", this.Discription);
                cmd.Parameters.AddWithValue("@MaxOrdQty", this.MaxOrdQty);
                cmd.Parameters.AddWithValue("@FinalPrice", this.FinalPrice);
                cmd.Parameters.AddWithValue("@Tax", this.Tax);
                if (this.TitleId == 0)
                {
                    R = Convert.ToInt32(cmd.ExecuteScalar());
                    this.TitleId = R;
                }
                else
                {
                    R = cmd.ExecuteNonQuery();
                    if (R > 0)
                    {
                        R = this.TitleId;
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

    
}