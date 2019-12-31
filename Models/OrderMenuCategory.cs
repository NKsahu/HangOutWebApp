using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace HangOut.Models
{
    public class OrderMenuCategory
    {
        public int id { get; set; }
        public int CategoryId { get; set; }
        public string DisplayName { get; set; }
        public int OrderNo { get; set; }
        public int OrderMenuid { get; set; }
        public bool Status { get; set; }
        public List<OrdMenuCtgItems> OrdCatItems { get; set; }
        public int save()
        {
            var CurrOrgID = HttpContext.Current.Request.Cookies["UserInfo"];
            int Row = 0;
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            SqlCommand cmd = null;
            try
            {
                Con.Open();
                string Query = "";
                if (this.id == 0)
                {
                    Query = "Insert into  OrdMenuCategory  values(@CategoryId,@DispalyName,@OrderNo,@OrderMenuId,@Status);select SCOPE_IDENTITY();";
                    cmd = new SqlCommand(Query, Con);
                    
                }
                else
                {
                    Query = "update  OrdMenuCategory set CategoryId=@CategoryId,DispalyName =@DispalyName,OrderNo=@OrderNo,OrderMenuId=@OrderMenuId,Status=@Status where Id=@Id";
                    cmd = new SqlCommand(Query, Con);
                    cmd.Parameters.AddWithValue("@Id", this.id);
                }
                cmd.Parameters.AddWithValue("@CategoryId", this.CategoryId);
                cmd.Parameters.AddWithValue("@DispalyName", this.DisplayName);
                cmd.Parameters.AddWithValue("@OrderNo", this.OrderNo);
                cmd.Parameters.AddWithValue("@OrderMenuId", this.OrderMenuid);
                cmd.Parameters.AddWithValue("@Status", this.Status);
                if (this.id == 0)
                {
                    Row = Convert.ToInt32(cmd.ExecuteScalar());
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
        public static List<OrderMenuCategory> GetAll(int OderMenuId=0,int CategoryId=0)
        {
            var CurrOrgID = HttpContext.Current.Request.Cookies["UserInfo"];
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<OrderMenuCategory> ListTmp = new List<OrderMenuCategory>();
             string   Query = "SELECT * FROM  OrdMenuCategory where OrderMenuId=" + OderMenuId.ToString() + " ";
            if (CategoryId > 0)
            {
                Query = "SELECT * FROM  OrdMenuCategory where CategoryId=" + CategoryId.ToString() + " ";
            }
            try
            {
                cmd = new SqlCommand(Query, Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    int index = 0;
                    OrderMenuCategory ObjTmp = new OrderMenuCategory();
                    ObjTmp.id = SDR.GetInt32(index++);
                    ObjTmp.CategoryId = SDR.GetInt32(index++);
                    ObjTmp.DisplayName = SDR.GetString(index++);
                    ObjTmp.OrderNo = SDR.GetInt32(index++);
                    ObjTmp.OrderMenuid = SDR.GetInt32(index++);
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