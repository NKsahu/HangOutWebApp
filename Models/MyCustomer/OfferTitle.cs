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
    public double FinalPrice { get; set; }
    public double Tax { get; set; }
     public int Type { get; set; }// 1: choice based, 2 fixed items


        public List<OfferMenu> OfferMenus { get; set; }
        public OfferTitle()
        {
            OfferMenus = new List<OfferMenu>();
        }
   
    public int Save(){
            int R = 0;
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Query = "";
                if (this.TitleId == 0)
                {
                    Query = "Insert into  OfferTitle  values(@CBID,@Name,@Discription,@MaxOrdQty,@FinalPrice,@Tax,@Type); SELECT SCOPE_IDENTITY();";
                    cmd = new SqlCommand(Query, dBCon.Con);
                    cmd.Parameters.AddWithValue("@CBID", this.CBID);
                    cmd.Parameters.AddWithValue("@Type", this.Type);
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


        public static OfferTitle GetOne(int CBID,int Type)
        {

            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            OfferTitle Tmp = new OfferTitle();
            string Query = "SELECT * FROM  OfferTitle where CBID=" + CBID.ToString()+" and Type="+ Type;
            try
            {
                cmd = new SqlCommand(Query, dBCon.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    int index = 0;
                    OfferTitle ObjTmp = new OfferTitle();
                    ObjTmp.TitleId = SDR.GetInt32(index++);
                    ObjTmp.CBID = SDR.GetInt32(index++);
                    ObjTmp.Name = SDR.GetString(index++);
                    ObjTmp.Discription = SDR.GetString(index++);
                    ObjTmp.MaxOrdQty = SDR.GetInt32(index++);
                    ObjTmp.FinalPrice = SDR.GetDouble(index++);
                    ObjTmp.Tax = SDR.GetDouble(index++);
                    ObjTmp.OfferMenus = OfferMenu.GetAll(ObjTmp.TitleId);
                    Tmp = ObjTmp;
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { dBCon.Close(); }

            return (Tmp);
        }
    }

    
}