using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models.Feedbk
{
    public class FeedbkItem
    {
       public Int64  ItemID { get; set; }
       public int Rating { get; set; }
       public string Comment { get; set; }
       public int FeedbkFormID { get; set; }
       public int FeedBkID { get; set; }
       public int  ResponseType { get; set; }
       public DateTime CreateOn { get; set; }
       public int CID { get; set; }
        public int save()
        {
            int R = 0;
            DBCon con = new DBCon();
            SqlCommand cmd = null;

            try
            {
                string Quary = "";
                if (ItemID == 0)
                {
                    Quary = "Insert into FeedbkItem values(@Rating,@Comment,@FeedbkFormID ,@FeedBkID,@ResponseType,@CreateOn,@CID);select SCOPE_IDENTITY();";
                }
                else
                {
                    Quary = "Update FeedbkItem Set Rating=@Rating,Comment=@Comment,FeedbkFormID =@FeedbkFormID ,FeedBkID=@FeedBkID,ResponseType=@ResponseType,CreateOn=@CreateOn,CID=@CID where ItemID=@ItemID ";
                }
                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@ItemID", this.ItemID);
                cmd.Parameters.AddWithValue("@Rating", this.Rating);
                cmd.Parameters.AddWithValue("@Comment", this.Comment);
                cmd.Parameters.AddWithValue("@FeedbkFormID ", this.FeedbkFormID );
                cmd.Parameters.AddWithValue("@FeedBkID", this.FeedBkID);
                cmd.Parameters.AddWithValue("@CreateOn", DateTime.Now);
                cmd.Parameters.AddWithValue("@CID", this.CID);
                if (this.ItemID == 0)
                {
                    R = Convert.ToInt32(cmd.ExecuteScalar());
                    this.ItemID = R;
                }
                else
                {
                    R = cmd.ExecuteNonQuery();
                    //this.CategoryID = Row;
                }

            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return R;
        }
        public List<FeedbkItem> GetAll()
        {
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<FeedbkItem> listfeedbk = new List<FeedbkItem>();
            try
            {
                string Quary = "Select * From FeedbkItem ORDER BY ItemID  DESC";
                cmd = new SqlCommand(Quary, dBCon.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {


                    FeedbkItem OBJfeedbk = new FeedbkItem();
                    OBJfeedbk.ItemID = SDR.GetInt32(0);
                    OBJfeedbk.Rating = SDR.GetInt32(1);
                    OBJfeedbk.Comment = SDR.GetString(2);
                    OBJfeedbk.FeedbkFormID  = SDR.GetInt32(3);
                    OBJfeedbk.FeedBkID= SDR.GetInt32(4);
                    OBJfeedbk.CreateOn= SDR.GetDateTime(5);
                    OBJfeedbk.CID= SDR.GetInt32(6);
                    listfeedbk.Add(OBJfeedbk);
                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { cmd.Dispose(); ; dBCon.Con.Close(); }

            return (listfeedbk);

        }

    }
}