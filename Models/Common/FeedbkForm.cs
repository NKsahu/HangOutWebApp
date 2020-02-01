using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models.Common
{
    public class FeedbkForm
    {
       public int Id { get; set; }
       public string Name { get; set; }
       public int  OrgId { get; set; }
       public bool Status { get; set; }
       public DateTime CreateDate { get; set; }
        public List<FeedBackQue> Questions { get; set; }
        public FeedbkForm()
        {
            CreateDate = DateTime.Now;
            Status = true;
        }
        public int Save()
        {
            int Row = 0;
            DBCon con = new DBCon();
            SqlCommand cmd = null;

            try
            {
                string Quary = "";

                if (this.Id == 0)
                {
                    Quary = "Insert into FeedbackForm values (@Name,@OrgId,@Status,@CreateDate); SELECT SCOPE_IDENTITY();";
                }
                else
                {
                    Quary = "Update FeedbackForm Set Name=@Name,OrgId=@OrgId,Status=@Status,CreateDate=@CreateDate where Id=@Id";
                }
                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@Id", this.Id);
                cmd.Parameters.AddWithValue("@Name", this.Name);
                cmd.Parameters.AddWithValue("@OrgId", this.OrgId);
                cmd.Parameters.AddWithValue("@Status", this.Status);
                cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                if (this.Id == 0)
                {
                    Row = Convert.ToInt32(cmd.ExecuteScalar());
                    this.Id = Row;
                }
                else
                {
                    Row = cmd.ExecuteNonQuery();
                    //this.CategoryId = Row;
                }

            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return Row;
        }
        public static List<FeedbkForm> GetAll(int Orgid)
        {
            DBCon OBJCon = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<FeedbkForm> ListTmp = new List<FeedbkForm>();

            try
            {
                string Query = "SELECT * FROM  FeedbackForm where OrgId="+Orgid;
                cmd = new SqlCommand(Query, OBJCon.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    FeedbkForm ObjTmp = new FeedbkForm();
                    ObjTmp.Id = SDR.GetInt32(0);
                    ObjTmp.Name = SDR.GetString(1);
                    ObjTmp.OrgId = SDR.GetInt32(2);
                    ObjTmp.Status = SDR.GetBoolean(3);
                    ObjTmp.CreateDate = SDR.GetDateTime(4);

                    ListTmp.Add(ObjTmp);
                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { cmd.Dispose(); ; OBJCon.Con.Close(); }

            return (ListTmp);
        }
    }
}