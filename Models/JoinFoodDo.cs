using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace HangOut.Models
{
    public class JoinFoodDo
    {

        public int JoinId { get; set; }
        public int JoinType { get; set; }// {1 :Business,2 : Team}
        public int ProductType { get; set; }
        public int JoinedUserd { get; set; }
        public DateTime JoinDate { get; set; }


        public JoinFoodDo()
        {
            JoinDate = DateTime.Now;
        }


        public int save()
        {
            DBCon con = new DBCon();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = new SqlCommand("insert into JoinFoodDo values(@JoinType,@ProductType,@JoinedUserd,@JoinDate)", con.Con);
                cmd.Parameters.AddWithValue("@JoinType", this.JoinType);
                cmd.Parameters.AddWithValue("@ProductType", this.ProductType);
                cmd.Parameters.AddWithValue("@JoinedUserd", this.JoinedUserd);
                cmd.Parameters.AddWithValue("@JoinDate",this.JoinDate);
                this.JoinId = System.Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception e)
            {
                e.Message.ToString();
                return 0;
            }
            finally
            {
                cmd.Dispose(); con.Con.Close();
            }
            return this.JoinId;
        }

        public List<JoinFoodDo> GetAll()
        {
            
            List<JoinFoodDo> listtemp = new List<JoinFoodDo>();
            DBCon con = new DBCon();
            SqlCommand cmd = new SqlCommand();
            string query = "select * from JoinFoodDo";
            try
            {
                cmd = new SqlCommand(query, con.Con);
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    JoinFoodDo hG_Ticket = new JoinFoodDo();
                    hG_Ticket.JoinId = sqlDataReader.GetInt32(1);
                    hG_Ticket.JoinType = sqlDataReader.GetInt32(2);
                    hG_Ticket.ProductType = sqlDataReader.GetInt32(3);
                    hG_Ticket.JoinedUserd = sqlDataReader.GetInt32(4);
                    hG_Ticket.JoinDate = sqlDataReader.GetDateTime(5);
                    listtemp.Add(hG_Ticket);
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                con.Con.Close(); con.Con.Dispose(); cmd.Dispose();
            }

            return listtemp;
        }

    }
    

    


}