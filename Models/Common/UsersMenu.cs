using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Web;

namespace HangOut.Models.Common
{
    public class UsersMenu
    {
        public int Menu_Id { get; set; }
        public string MenuDisplayName { get; set; }
        public int ParentMenuId { get; set; }
        public int MenuOrderNo { get; set; }
        public string MenuLink { get; set; }
        public string Menu_Icon { get; set; }
        public string User_Types { get; set; }
        public string Section { get; set; }
        public string[] MultipleUserType { get; set; }
        public int Save()
        {
           SqlCommand cmd = null;
            SqlConnection Con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            try
            {
                if (this.Menu_Id == 0)
                    cmd = new System.Data.SqlClient.SqlCommand("INSERT INTO Menu VALUES (@MenuDisplayName,@ParentMenuId,@MenuOrderNo,@MenuLink,@Menu_Icon,@User_Types,@Section);select SCOPE_IDENTITY();", Con);
                else
                {
                    cmd = new System.Data.SqlClient.SqlCommand("UPDATE Menu SET MenuDisplayName=@MenuDisplayName,ParentMenuId=@ParentMenuId,MenuOrderNo=@MenuOrderNo,MenuLink=@MenuLink,Menu_Icon=@Menu_Icon,User_Types=@User_Types,Section=@Section where Menu_Id=@Menu_Id", Con);
                    cmd.Parameters.AddWithValue("@Menu_Id", this.Menu_Id);
                }
                cmd.Parameters.AddWithValue("@MenuDisplayName", this.MenuDisplayName);
                cmd.Parameters.AddWithValue("@ParentMenuId", this.ParentMenuId);
                cmd.Parameters.AddWithValue("@MenuOrderNo", this.MenuOrderNo);
                if (this.MenuLink == null)
                    cmd.Parameters.AddWithValue("@MenuLink", System.DBNull.Value);
                else cmd.Parameters.AddWithValue("@MenuLink", this.MenuLink);

                if (this.Menu_Icon == null)
                    cmd.Parameters.AddWithValue("@Menu_Icon", System.DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@Menu_Icon", this.Menu_Icon);
                cmd.Parameters.AddWithValue("@User_Types", this.User_Types);
                cmd.Parameters.AddWithValue("@Section", this.Section);
                if (this.Menu_Id == 0)
                {
                    this.Menu_Id = System.Convert.ToInt32(cmd.ExecuteScalar());
                    
                      
                }
                else
                {
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                       
                    }
                }
            }
            catch (System.Exception e) { this.Menu_Id = 0; e.ToString(); }
            finally { cmd.Dispose();Con.Close(); Con.Dispose(); Con = null; }
            return this.Menu_Id;
        }
        public System.Collections.Generic.List<UsersMenu> GetAll()
        {
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlDataReader SDR = null;
            System.Collections.Generic.List<UsersMenu> ListTmp = new System.Collections.Generic.List<UsersMenu>();
            UsersMenu ObjTmp = null;
            SqlConnection Con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            try
            {
                string Query = "SELECT * FROM Menu ORDER BY Menu_Id ASC";
                cmd = new System.Data.SqlClient.SqlCommand(Query, Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    ObjTmp = new UsersMenu
                    {
                        Menu_Id = int.Parse(SDR["Menu_Id"].ToString()),
                        MenuDisplayName = SDR["MenuDisplayName"].ToString(),
                        ParentMenuId = int.Parse(SDR["ParentMenuId"].ToString()),
                        MenuOrderNo = int.Parse(SDR["MenuOrderNo"].ToString()),
                        MenuLink = SDR["MenuLink"].ToString(),
                        Menu_Icon = SDR["Menu_Icon"].ToString(),
                        User_Types = SDR["User_Types"].ToString(),
                        Section = SDR["Section"].ToString()
                    };
                    ListTmp.Add(ObjTmp);
                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { cmd.Dispose(); SDR.Close(); Con.Close(); Con.Dispose();Con = null; }
            return (ListTmp);
        }

        public UsersMenu GetOne(int ID)
        {
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            UsersMenu ObjTmp = new UsersMenu();

            try
            {
                string Query = "SELECT * FROM  Menu where  Menu_Id=@Menu_Id ";
                cmd = new SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("@Menu_Id ", Menu_Id);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    Menu_Id = int.Parse(SDR["Menu_Id"].ToString());
                    MenuDisplayName = SDR["MenuDisplayName"].ToString();
                    ParentMenuId = int.Parse(SDR["ParentMenuId"].ToString());
                    MenuOrderNo = int.Parse(SDR["MenuOrderNo"].ToString());
                    MenuLink = SDR["MenuLink"].ToString();
                    Menu_Icon = SDR["Menu_Icon"].ToString();
                    User_Types = SDR["User_Types"].ToString();
                    Section = SDR["Section"].ToString();
                }
            }
            catch (System.Exception e)
            { e.ToString(); }

            finally { Con.Close(); }

            return (ObjTmp);
        }
    }
}