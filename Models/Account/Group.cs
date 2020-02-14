using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models.Account
{
    public class Group
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public int PGID { get; set; }
        public int Type { get; set; }

        public int Save()
        {
            int Row = 0;
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            try
            {
                string Quary = "";
                if (this.ID == 0)
                {
                    Quary = "Insert Into ACGroup Values (@Name,@PGID,@Type);SELECT SCOPE_IDENTITY();";
                }
                else
                {
                    Quary = "Update ACGroup Set Name=@Name,PGID=@PGID,Type=@Type where ID=@ID";
                }
                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@ID", this.ID);
                cmd.Parameters.AddWithValue("@Name", this.Name);
                cmd.Parameters.AddWithValue("@PGID", this.PGID);  
                cmd.Parameters.AddWithValue("@Type", this.Type);
                if (this.ID == 0)
                {
                    Row = Convert.ToInt32(cmd.ExecuteScalar());
                    this.ID = Row;
                }
                else
                {
                    Row = cmd.ExecuteNonQuery();
                    //this.CategoryID = Row;
                }

            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            GetAll();
            return Row;

        }
        public static List<Group> GetAll()
        {
            DBCon con = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<Group> GroupList = new List<Group>();
            try
            {
                string Quary = "Select * from ACGroup ORDER BY ID DESC";
                cmd = new SqlCommand(Quary, con.Con);
                SDR = cmd.ExecuteReader();

                while (SDR.Read())
                {
                    Group OBJGRP = new Group();
                    OBJGRP.ID = SDR.GetInt32(0);
                    OBJGRP.Name = SDR.GetString(1);
                    OBJGRP.PGID = SDR.GetInt32(2);
                    OBJGRP.Type = SDR.GetInt32(3);
                    GroupList.Add(OBJGRP);
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return (GroupList);
        }
        public Group GetOne(int ID)
        {
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            Group ObjTmp = new Group();

            try
            {
                string Query = "SELECT * FROM  ACGroup where ID=@ID";
                cmd = new SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("@ID", ID);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    ObjTmp.ID = SDR.GetInt32(0);
                    ObjTmp.Name = SDR.GetString(1);
                    ObjTmp.PGID = SDR.GetInt32(2);
                    ObjTmp.Type = SDR.GetInt32(3);

                }
            }
            catch (System.Exception e)
            { e.ToString(); }

            finally { Con.Close(); }

            return (ObjTmp);
        }
        public static int Dell(int ID)
        {
            int R = 0;
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            try
            {
                string Query = "Delete FROM  ACGroup where ID=" + ID;
                cmd = new SqlCommand(Query, Con);
                R = cmd.ExecuteNonQuery();
            }
            catch (System.Exception e)
            { e.ToString(); }

            finally
            {
                Con.Close();
            }
            return R;
        }
        public static List<Type> GTypes()
        {

            List<Type> type = new List<Type>();
            type.Add(new Type { Id = 0, Name = "Liability" });
            type.Add(new Type { Id = 1, Name = "Asset" });
            type.Add(new Type { Id = 2, Name = "Income" });
            type.Add(new Type { Id = 3, Name = "Expense" });

            return type;
        }
    }
}
public class Type
{
    public int Id { get; set; }
    public string Name { get; set; }
}