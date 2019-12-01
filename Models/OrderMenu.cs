using System;
using System.Web;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace HangOut.Models
{
    public class OrderMenu
    {
        public int id { get; set; }
        public string MenuName {get;set;}
        public int CreateBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public int OrgId { get; set; }
        public bool Status { get; set; }
        public List<OrderMenuCategory> OderMenuCategry { get; set; }
        public OrderMenu()
        {
            Status = false;

        }
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
                    Query = "Insert into  OrderMenu  values(@MenuName,@CreatedBy,@UpdatedBy,@UpdatetionDate,@OrgId,@Status); SELECT SCOPE_IDENTITY();";
                    cmd = new SqlCommand(Query, Con);
                    cmd.Parameters.AddWithValue("@CreatedBy",int.Parse(CurrOrgID["UserCode"]));
                }
                else
                {
                    Query = "update  OrderMenu set MenuName=@MenuName,UpdatedBy =@UpdatedBy,UpdatetionDate=@UpdatetionDate,OrgId=@OrgId,Status=@Status where MenuID=@MenuID";
                    cmd = new SqlCommand(Query, Con);
                    cmd.Parameters.AddWithValue("@MenuID", this.id);
                }
                cmd.Parameters.AddWithValue("@MenuName", this.MenuName);
                cmd.Parameters.AddWithValue("@UpdatedBy", int.Parse(CurrOrgID["UserCode"]));
                cmd.Parameters.AddWithValue("@UpdatetionDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@OrgId", int.Parse(CurrOrgID["OrgId"]));
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
        public static List<OrderMenu>  GetAll(int OrgId=0)
        {
            var CurrOrgID = HttpContext.Current.Request.Cookies["UserInfo"];
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<OrderMenu> ListTmp = new List<OrderMenu>();
            string Query = "";
            if (OrgId > 0)
                Query = "SELECT * FROM  OrderMenu where OrgId=" + OrgId.ToString() + " ";
           else
                Query = "SELECT * FROM  OrderMenu where OrgId=" + CurrOrgID["OrgId"] +"";
            try
            {
                cmd = new SqlCommand(Query, Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    int index = 0;
                    OrderMenu ObjTmp = new OrderMenu();
                    ObjTmp.id = SDR.GetInt32(index++);
                    ObjTmp.MenuName = SDR.GetString(index++);
                    ObjTmp.CreateBy = SDR.GetInt32(index++);
                    ObjTmp.UpdatedBy = SDR.GetInt32(index++);
                    ObjTmp.UpdateDate = SDR.GetDateTime(index++);
                    ObjTmp.OrgId = SDR.GetInt32(index++);
                    ObjTmp.Status = SDR.GetBoolean(index++);
                    ListTmp.Add(ObjTmp);
                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { Con.Close();
                if (!SDR.IsClosed)
                    SDR.Close();
                    }

            return (ListTmp);
        }

    }
}