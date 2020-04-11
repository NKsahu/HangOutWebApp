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
       public int IsComplementry { get; set; }
      public int  CBID { get; set; }
      public int OfferTitleId { get; set; }
      public List<ItemOffer> ItemOffers { get; set; }
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
                    Query = "Insert into  OfferMenu  values(@Name,@IsComplementry,@CBID,@OfferTitleId); SELECT SCOPE_IDENTITY();";
                    cmd = new SqlCommand(Query, dBCon.Con);
                    cmd.Parameters.AddWithValue("@CBID", this.CBID);

                }
                else
                {
                    Query = "update  OfferMenu set Name=@Name,IsComplementry=@IsComplementry,OfferTitleId=@OfferTitleId where MenuId=@MenuId";
                    cmd = new SqlCommand(Query, dBCon.Con);
                    cmd.Parameters.AddWithValue("@MenuId", this.MenuId);
                }
                cmd.Parameters.AddWithValue("@Name", this.Name);
                cmd.Parameters.AddWithValue("@IsComplementry", this.IsComplementry);
                cmd.Parameters.AddWithValue("@OfferTitleId", this.OfferTitleId);
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
    }
}