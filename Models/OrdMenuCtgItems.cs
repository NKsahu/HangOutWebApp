using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace HangOut.Models
{
    public class OrdMenuCtgItems
    {
        public Int64 id { get; set; }
        public Int64 ItemId { get; set; }
        public string DisplayName { get; set; }
        public int OrderNo {get;set;}
        public int OrdMenuCatId { get; set; }
        public bool Status { get; set; }
        public Int64 save()
        {
            Int64 Row = 0;
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            SqlCommand cmd = null;
            try
            {
                Con.Open();
                string Query = "";
                if (this.id == 0)
                {
                    Query = "Insert into  OrderMenuCatItems values(@ItemId,@DisplayName,@OrderNo,@OrdMenuCatId,@Status,);";
                    cmd = new SqlCommand(Query, Con);
                }
                else
                {
                    Query = "update  OrderMenuCatItems set ItemId=@ItemId,DisplayName =@DisplayName,OrderNo=@OrderNo,OrdMenuCatId=@OrdMenuCatId,Status=@Status where Id=@Id";
                    cmd = new SqlCommand(Query, Con);
                    cmd.Parameters.AddWithValue("@Id", this.id);
                }
                cmd.Parameters.AddWithValue("@ItemId", this.ItemId);
                cmd.Parameters.AddWithValue("@DisplayName", this.DisplayName);
                cmd.Parameters.AddWithValue("@OrderNo", this.OrderNo);
                cmd.Parameters.AddWithValue("@OrdMenuCatId", this.OrdMenuCatId);
                cmd.Parameters.AddWithValue("@Status", this.Status);
                if (this.id == 0)
                {
                    Row = Convert.ToInt64(cmd.ExecuteScalar());
                    this.id = Row;
                }
                else
                {
                    Row = cmd.ExecuteNonQuery();
                    if (Row > 0)
                        Row = this.id;
                }
            }
            catch (Exception e) { e.ToString(); }
            finally
            {
                Con.Close();
                if (cmd != null) cmd.Dispose();
            }
            return Row;


        }
        public static List<OrdMenuCtgItems> GetAll(int OderMenuCtgId)
        {
            var CurrOrgID = HttpContext.Current.Request.Cookies["UserInfo"];
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<OrdMenuCtgItems> ListTmp = new List<OrdMenuCtgItems>();
            string Query = "SELECT * FROM  OrderMenuCatItems where OrdMenuCatId=" + OderMenuCtgId.ToString() + " ";

            try
            {
                cmd = new SqlCommand(Query, Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    int index = 0;
                    OrdMenuCtgItems ObjTmp = new OrdMenuCtgItems();
                    ObjTmp.id = SDR.GetInt64(index++);
                    ObjTmp.ItemId = SDR.GetInt64(index++);
                    ObjTmp.DisplayName = SDR.GetString(index++);
                    ObjTmp.OrderNo = SDR.GetInt32(index++);
                    ObjTmp.OrdMenuCatId = SDR.GetInt32(index++);
                    ObjTmp.Status = SDR.GetBoolean(index++);
                    ListTmp.Add(ObjTmp);
                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally
            {
                Con.Close();
                if (!SDR.IsClosed)
                    SDR.Close();
            }

            return (ListTmp);
        }
    }
}