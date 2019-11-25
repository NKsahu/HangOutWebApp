﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HangOut.Models
{
    public class HG_OrganizationDetails
    {
        
        public int OrgID { get; set; }
        public string OrgTypes{ get; set; }
        public string HeadName{ get; set; }
        public string Name{ get; set; }
        public string Address{ get; set; }// address 1
        public string City{ get; set; }
        public string State{ get; set; }
        public string PinCode{ get; set; }
        public string Phone{ get; set; }
        public string Cell { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
        public string Logo { get; set; }
        public DateTime DOR{ get; set; }
        public DateTime DOE { get; set; }
        public string GSTNO { get; set; }//license1
        public string PANNO { get; set; }
        public string BankName{ get; set; }
        public string ACNO{ get; set; }
        public string AcType { get; set; }
        public int EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool Status { get; set; } 
        public int PaymentType { get; set; }// {'1':'prepaid','2':'postpaid'}
        public string IvoiceHeading { get; set; }
        public string AddressLin2 { get; set; }
        public string AddressLine3 { get; set; }
        public string Licence2 { get; set; }
        public string License3 { get; set; }


        public HG_OrganizationDetails()
        {
            EntryDate = DateTime.Now;
            UpdateDate = DateTime.Now;
            EntryBy = 0;
            PaymentType = 1;
        }
        public int Save()
        {
            int Row = 0;


            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            try
            {
                Con.Open();
                SqlCommand cmd = null;
                string Query = "";
                if (this.OrgID  == 0)
                {
                    Query = "Insert into  HG_OrganizationDetails  values(@OrgTypes,@HeadName,@Name,@Address,@City,@State,@PinCode,@Phone,@Cell,@Email,@WebSite,@Logo,@DOR,@DOE,@GSTNO,@PANNO,@BankName,@ACNO,@AcType,@EntryBy,@EntryDate,@UpdateDate,@Status,@PaymentType,@InvoiceHeading,@AddressLine2,@AddressLin3,@License2,@License3);";
                    cmd = new SqlCommand(Query, Con);
                    cmd.Parameters.AddWithValue("@EntryBy",int.Parse(HttpContext.Current.Request.Cookies["UserInfo"]["UserCode"]));
                    cmd.Parameters.AddWithValue("@EntryDate",System.DateTime.Now);
                    
                }
                else
                {

                    Query = "update  HG_OrganizationDetails set OrgTypes=@OrgTypes,HeadName =@HeadName,Name=@Name,Address=@Address,City=@City,State=@State,PinCode=@PinCode,Phone=@Phone,Cell=@Cell,Email=@Email,WebSite=@WebSite,Logo=@Logo,DOR=@DOR,DOE=@DOE,GSTNO=@GSTNO,PANNO=@PANNO,BankName=@BankName,ACNO=@ACNO,AcType=@AcType,UpdateDate=@UpdateDate,Status=@Status,PaymentType=@PaymentType,InvoiceHeading=@InvoiceHeading,AddressLine2=@AddressLine2,AddressLin3=@AddressLin3,License2=@License2,License3=@License3 where OrgID =@OrgID ";
                    cmd = new SqlCommand(Query, Con);
                    cmd.Parameters.AddWithValue("@OrgID ", this.OrgID );
                }
                cmd.Parameters.AddWithValue("@UpdateDate", System.DateTime.Now);
                cmd.Parameters.AddWithValue("@OrgTypes", this.OrgTypes);
                cmd.Parameters.AddWithValue("@HeadName", this.HeadName);
                cmd.Parameters.AddWithValue("@Name", this.Name);
                cmd.Parameters.AddWithValue("@Address", this.Address);
                cmd.Parameters.AddWithValue("@City", this.City);
                cmd.Parameters.AddWithValue("@State ", this.State);
                cmd.Parameters.AddWithValue("@PinCode ", this.PinCode);
                cmd.Parameters.AddWithValue("@Phone ", this.Phone);
                cmd.Parameters.AddWithValue("@Cell ", this.Cell);
                cmd.Parameters.AddWithValue("@Email ", this.Email);
                cmd.Parameters.AddWithValue("@WebSite ", this.WebSite);
                cmd.Parameters.AddWithValue("@Logo ", this.Logo);
                cmd.Parameters.AddWithValue("@DOR ", System.DateTime.Now);
                cmd.Parameters.AddWithValue("@DOE", System.DateTime.Now);
                cmd.Parameters.AddWithValue("@GSTNO", this.GSTNO);
                cmd.Parameters.AddWithValue("@PANNO", this.PANNO);
                cmd.Parameters.AddWithValue("@BankName", this.BankName);
                cmd.Parameters.AddWithValue("@ACNO", this.ACNO);
                cmd.Parameters.AddWithValue("@AcType", this.AcType);
                cmd.Parameters.AddWithValue("@Status", this.Status);
                cmd.Parameters.AddWithValue("@PaymentType", this.PaymentType);
                Row = cmd.ExecuteNonQuery();
                this.OrgID  = Row;
            }
            catch (Exception e) { e.ToString(); }
            finally { Con.Close(); }
            return Row;
        }

        public List<HG_OrganizationDetails> GetAll(int Orgid=0)
        {
            var CurrOrgID = HttpContext.Current.Request.Cookies["UserInfo"];
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<HG_OrganizationDetails> ListTmp = new List<HG_OrganizationDetails>();
            string Query = "SELECT * FROM  HG_OrganizationDetails ORDER BY OrgID  DESC";
            if (OrgID > 0)
            {
                Query = "SELECT * FROM  HG_OrganizationDetails where OrgID="+OrgID.ToString()+"  ORDER BY OrgID  DESC";
            }else if(CurrOrgID!=null && int.Parse(CurrOrgID["OrgId"])>0)
            {
                Query = "SELECT * FROM  HG_OrganizationDetails where OrgID=" + CurrOrgID["OrgId"] + "  ORDER BY OrgID  DESC";
            }
            try
            {
                cmd = new SqlCommand(Query, Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    HG_OrganizationDetails ObjTmp = new HG_OrganizationDetails();
                    ObjTmp.OrgID  = SDR.GetInt32(0);
                    ObjTmp.OrgTypes = SDR.GetString(1); 
                    ObjTmp.HeadName = SDR.GetString(2); 
                    ObjTmp.Name = SDR.GetString(3); 
                    ObjTmp.Address = SDR.GetString(4); 
                    ObjTmp.City = SDR.GetString(5); 
                    ObjTmp.State = SDR.GetString(6); 
                    ObjTmp.PinCode = SDR.GetString(7); 
                    ObjTmp.Phone = SDR.GetString(8); 
                    ObjTmp.Cell = SDR.GetString(9); 
                    ObjTmp.Email = SDR.GetString(10); 
                    ObjTmp.WebSite = SDR.GetString(11); 
                    ObjTmp.Logo = SDR.GetString(12); 
                    ObjTmp.GSTNO = SDR.GetString(15); 
                    ObjTmp.PANNO = SDR.GetString(16); 
                    ObjTmp.BankName = SDR.GetString(17); 
                    ObjTmp.ACNO = SDR.GetString(18); 
                    ObjTmp.AcType = SDR.GetString(19); 
                    ObjTmp.Status = SDR.GetBoolean(23);
                    ObjTmp.PaymentType =SDR.IsDBNull(24)?1: SDR.GetInt32(24);
                    ListTmp.Add(ObjTmp);
                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { Con.Close(); }

            return (ListTmp);
        }
        public HG_OrganizationDetails GetOne(int OrgID )
        {
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            HG_OrganizationDetails ObjTmp = new HG_OrganizationDetails();

            try
            {
                string Query = "SELECT * FROM  HG_OrganizationDetails where OrgID =@OrgID ";
                cmd = new SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("@OrgID ", OrgID );
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    ObjTmp.OrgID = SDR.GetInt32(0);
                    ObjTmp.OrgTypes = SDR.GetString(1);
                    ObjTmp.HeadName = SDR.GetString(2);
                    ObjTmp.Name = SDR.GetString(3);
                    ObjTmp.Address = SDR.GetString(4);
                    ObjTmp.City = SDR.GetString(5);
                    ObjTmp.State = SDR.GetString(6);
                    ObjTmp.PinCode = SDR.GetString(7);
                    ObjTmp.Phone = SDR.GetString(8);
                    ObjTmp.Cell = SDR.GetString(9);
                    ObjTmp.Email = SDR.GetString(10);
                    ObjTmp.WebSite = SDR.GetString(11);
                    ObjTmp.Logo = SDR.GetString(12);
                    ObjTmp.GSTNO = SDR.GetString(15);
                    ObjTmp.PANNO = SDR.GetString(16);
                    ObjTmp.BankName = SDR.GetString(17);
                    ObjTmp.ACNO = SDR.GetString(18);
                    ObjTmp.AcType = SDR.GetString(19);
                    ObjTmp.Status = SDR.GetBoolean(23);
                    ObjTmp.PaymentType = SDR.IsDBNull(24) ? 1 : SDR.GetInt32(24);
                }
            }
            catch (System.Exception e)
            { e.ToString(); }

            finally { Con.Close(); }

            return (ObjTmp);
        }

        public int Dell(int ID)
        {
            int R = 0;
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            try
            {
                string Query = "Delete FROM  HG_OrganizationDetails where OrgID =" + ID;
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
        public List<HG_OrganizationDetails> GetAllByType(string Type)
        {
            var CurrOrgID = HttpContext.Current.Request.Cookies[""];
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ToString());
            Con.Open();
            SqlCommand cmd = null;
            SqlDataReader SDR = null;
            List<HG_OrganizationDetails> ListTmp = new List<HG_OrganizationDetails>();
            
               string Query = "SELECT * FROM  HG_OrganizationDetails where Type=" + Type + "  ORDER BY OrgID  DESC";
           
            if (CurrOrgID != null)
            {

            }
            try
            {
                cmd = new SqlCommand(Query, Con);
                SDR = cmd.ExecuteReader();
                while (SDR.Read())
                {
                    HG_OrganizationDetails ObjTmp = new HG_OrganizationDetails();
                    ObjTmp.OrgID = SDR.GetInt32(0);
                    ObjTmp.OrgTypes = SDR.GetString(1);
                    ObjTmp.HeadName = SDR.GetString(2);
                    ObjTmp.Name = SDR.GetString(3);
                    ObjTmp.Address = SDR.GetString(4);
                    ObjTmp.City = SDR.GetString(5);
                    ObjTmp.State = SDR.GetString(6);
                    ObjTmp.PinCode = SDR.GetString(7);
                    ObjTmp.Phone = SDR.GetString(8);
                    ObjTmp.Cell = SDR.GetString(9);
                    ObjTmp.Email = SDR.GetString(10);
                    ObjTmp.WebSite = SDR.GetString(11);
                    ObjTmp.Logo = SDR.GetString(12);
                    ObjTmp.GSTNO = SDR.GetString(15);
                    ObjTmp.PANNO = SDR.GetString(16);
                    ObjTmp.BankName = SDR.GetString(17);
                    ObjTmp.ACNO = SDR.GetString(18);
                    ObjTmp.AcType = SDR.GetString(19);
                    ObjTmp.Status = SDR.GetBoolean(23);
                    ObjTmp.PaymentType = SDR.IsDBNull(24) ? 1 : SDR.GetInt32(24);
                    ListTmp.Add(ObjTmp);
                }
            }
            catch (System.Exception e) { e.ToString(); }
            finally { Con.Close(); }

            return (ListTmp);
        }
    }
}