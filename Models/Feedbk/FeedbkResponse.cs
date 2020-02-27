using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
namespace HangOut.Models.Feedbk
{
    public class FeedbkResponse
    {
        public int QID { get; set; }
        public int ResponseType { get; set; }
        public int FeedbkFormId { get; set; }
        public int StarCnt { get; set; }
        public string Subject { get; set; }
        public int LikeCnt { get; set; }
        public int DislikeCnt { get; set; }
        public int NormalOkCnt { get; set; }
        public int FeedbkId { get; set; }
        public string ObjectiveOptions {get;set;}
        public DateTime  CreateDate { get; set; }
        public int CID { get; set; }
        public int OrgId { get; set; }
        //
        public Int64 OID { get; set; }
        public FeedbkResponse()
        {
            CreateDate = DateTime.Now;
            Subject = "";
        }
        public int save()
        {
            int R = 0;
            DBCon con = new DBCon();
            SqlCommand cmd = null;

            try
            {
                string Quary = "";
               
                    Quary = "Insert into FeedbkResponse values(@QID,@ResponseType,@FeedbkFormId,@StarCnt ,@Subject,@LikeCnt,@DislikeCnt,@NormalOkCnt,@FeedbkId,@ObjectiveOptions,@CreateDate,@CID,@OrgId);";
                
                //else
                //{
                //    Quary = "Update FeedbkResponse Set ResponseType=@ResponseType,FeedbkFormId=@FeedbkFormId,StarCnt =@StarCnt ,Subject=@Subject,LikeCnt=@LikeCnt,DislikeCnt=@DislikeCnt,NormalOkCnt=@NormalOkCnt,FeedbkId=@FeedbkId,ObjectiveOptions=@ObjectiveOptions,CreateDate=@CreateDate,CID=@CID where QID=@QID ";
                //}
                cmd = new SqlCommand(Quary, con.Con);
                cmd.Parameters.AddWithValue("@QID", this.QID);
                cmd.Parameters.AddWithValue("@ResponseType", this.ResponseType);
                cmd.Parameters.AddWithValue("@FeedbkFormId", this.FeedbkFormId);
                cmd.Parameters.AddWithValue("@StarCnt ", this.StarCnt);
                cmd.Parameters.AddWithValue("@Subject", this.Subject);
                cmd.Parameters.AddWithValue("@LikeCnt", this.LikeCnt);
                cmd.Parameters.AddWithValue("@DislikeCnt", this.DislikeCnt);
                cmd.Parameters.AddWithValue("@NormalOkCnt", this.NormalOkCnt);
                cmd.Parameters.AddWithValue("@FeedbkId", this.FeedbkId);
                cmd.Parameters.AddWithValue("@ObjectiveOptions", this.ObjectiveOptions);
                cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@CID", this.CID);
                cmd.Parameters.AddWithValue("@OrgId", this.OrgId);
                R = cmd.ExecuteNonQuery();
            }
            catch (Exception e) { e.ToString(); }
            finally { cmd.Dispose(); con.Con.Close(); }
            return R;
        }
        public static List<FeedbkResponse> GetAll(int Orgid,DateTime Fdate,DateTime Tdate)
        {
            DBCon dBCon = new DBCon();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<FeedbkResponse> listfeedbk = new List<FeedbkResponse>();
            try
            {
                string Quary = "Select * From FeedbkResponse where OrgId="+Orgid;
                if (Fdate != null && Tdate != null)
                {
                    Quary += "and CreateDate between '" + Fdate.ToString("MM/dd/yyyy") + "' and '" + Tdate.ToString("MM/dd/yyyy HH:mm:ss") + "";
                }
                cmd = new SqlCommand(Quary, dBCon.Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    FeedbkResponse OBJfeedbk = new FeedbkResponse();
                    OBJfeedbk.QID = SDR.GetInt32(0);
                    OBJfeedbk.ResponseType = SDR.GetInt32(1);
                    OBJfeedbk.FeedbkFormId = SDR.GetInt32(2);
                    OBJfeedbk.StarCnt = SDR.GetInt32(3);
                    OBJfeedbk.Subject = SDR.GetString(4);
                    OBJfeedbk.LikeCnt = SDR.GetInt32(5);
                    OBJfeedbk.DislikeCnt = SDR.GetInt32(6);
                    OBJfeedbk.NormalOkCnt = SDR.GetInt32(7);
                    OBJfeedbk.FeedbkId = SDR.GetInt32(8);
                    OBJfeedbk.ObjectiveOptions = SDR.GetString(9);
                    OBJfeedbk.CreateDate = SDR.GetDateTime(10);
                    OBJfeedbk.CID = SDR.GetInt32(11);
                    OBJfeedbk.OrgId= SDR.GetInt32(12);
                    listfeedbk.Add(OBJfeedbk);
                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { cmd.Dispose(); ; dBCon.Con.Close(); }

            return (listfeedbk);

        }
    }
}