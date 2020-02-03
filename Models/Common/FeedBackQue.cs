using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models.Common
{
    public class FeedBackQue
    {
       public int ID { get; set; }
      public string Title { get; set; }
      public bool Status { get; set; }
      public int QuestionType { get; set; }// 0:Star,1: Objective,2:Subjective,3:Like Dislike Ok,4:star-subjective
        public int FeedBkFormID { get; set; }
        public int OrderNo { get; set; }
        public List<FeedbkObj> Objectives { get; set; }
        public int Save()
        {
            int Row = 0;
            DBCon con = new DBCon();
            SqlCommand cmd = null;
           
            try
            {
                string Quary = "";

                if(this.ID==0)
                {
                    Quary = "Insert into FeedBackQuestion values (@Title,@Status,@QuestionType,@FeedBKFormID,@OrderNo); SELECT SCOPE_IDENTITY();";
                }else
                {
                    Quary = "Update FeedBackQuestion Set Title=@Title,Status=@Status,QuestionType=@QuestionType,FeedBKFormID=@FeedBKFormID,OrderNo=@OrderNo where ID=@ID";
                }
                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@ID",this.ID);
                cmd.Parameters.AddWithValue("@Title", this.Title);
                cmd.Parameters.AddWithValue("@Status",this.Status);
                cmd.Parameters.AddWithValue("@QuestionType", this.QuestionType);
                cmd.Parameters.AddWithValue("@FeedBkFormID", this.FeedBkFormID);
                cmd.Parameters.AddWithValue("@OrderNo", this.OrderNo);
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
            return Row;
        }
        public static List<FeedBackQue> GetAll(int FeedBkId)
        {
            DBCon OBJCon = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<FeedBackQue> ListTmp = new List<FeedBackQue>();

            try
            {
                string Query = "SELECT * FROM  FeedBackQuestion where FeedBkFormID="+FeedBkId;
                cmd = new SqlCommand(Query, OBJCon.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    FeedBackQue ObjTmp = new FeedBackQue();
                    ObjTmp.ID = SDR.GetInt32(0);
                    ObjTmp.Title = SDR.GetString(1);
                    ObjTmp.Status = SDR.GetBoolean(2);
                    ObjTmp.QuestionType = SDR.GetInt32(3);
                    ObjTmp.FeedBkFormID = SDR.GetInt32(4);
                    ObjTmp.OrderNo = SDR.GetInt32(5);
                    ListTmp.Add(ObjTmp);
                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { cmd.Dispose(); ; OBJCon.Con.Close(); }

            return (ListTmp);
        }
        public static List<Questions>  QuestioType()
        {

            List<Questions> questions = new List<Questions>();
            questions.Add(new Questions { Id = 0, Name = "Star" });
            questions.Add(new Questions { Id = 1, Name = "Objective" });
            questions.Add(new Questions { Id = 2, Name = "Subjective" });
            questions.Add(new Questions { Id = 3, Name = "Like Dislike Ok" });
            questions.Add(new Questions { Id = 4, Name = "Star-Subjective" });
            return questions;
        }
    }
}
public class Questions
{
    public int Id { get; set; }
    public string Name { get; set; }
}