using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models.Common
{
    public class FeedbkObj
    {
        public int id { get; set; }
        public string  Name { get; set; }
        public int ObjectiveType { get; set; }// {0 single choice, 1 multiple choice}
        public int QuestionId{ get; set; }
        public int Save()
        {
            int Row = 0;
            DBCon con = new DBCon();
            SqlCommand cmd = null;

            try
            {
                string Quary = "";

                if (this.id == 0)
                {
                    Quary = "Insert into FeedbackObjective values (@Name,@ObjectiveType,@QuestionId); SELECT SCOPE_IDENTITY();";
                }
                else
                {
                    Quary = "Update FeedbackObjective Set Name=@Name,ObjectiveType=@ObjectiveType,QuestionId=@QuestionId where id=@id";
                }
                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@id", this.id);
                cmd.Parameters.AddWithValue("@Name", this.Name);
                cmd.Parameters.AddWithValue("@ObjectiveType", this.ObjectiveType);
                cmd.Parameters.AddWithValue("@QuestionId", this.QuestionId);
                cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                if (this.id == 0)
                {
                    Row = Convert.ToInt32(cmd.ExecuteScalar());
                    this.id = Row;
                }
                else
                {
                    Row = cmd.ExecuteNonQuery();
                    //this.Categoryid = Row;
                }

            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return Row;
        }
        public static List<FeedbkObj> GetAll(int Qid)
        {
            DBCon OBJCon = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<FeedbkObj> ListTmp = new List<FeedbkObj>();

            try
            {
                string Query = "SELECT * FROM  FeedbackObjective where QuestionId="+ Qid;
                cmd = new SqlCommand(Query, OBJCon.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    FeedbkObj ObjTmp = new FeedbkObj();
                    ObjTmp.id = SDR.GetInt32(0);
                    ObjTmp.Name = SDR.GetString(1);
                    ObjTmp.ObjectiveType = SDR.GetInt32(2);
                    ObjTmp.QuestionId = SDR.GetInt32(3);
                    ListTmp.Add(ObjTmp);
                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { cmd.Dispose(); ; OBJCon.Con.Close(); }

            return (ListTmp);
        }

        public static List<Objectives> QusObjective()
        {
            List<Objectives> objectives = new List<Objectives>();
            objectives.Add(new Objectives { id = 0, Name = "Single Mode" });
            objectives.Add(new Objectives { id = 1, Name = "Multiple" });
            return objectives;
        }
    }
}
public class Objectives
{
    public int id { get; set; }
    public string Name { get; set; }
}