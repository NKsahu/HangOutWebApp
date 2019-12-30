using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Web;
using System.Linq;
using System.IO;

namespace HangOut.Models.Common
{
    public class ReadExl
    {

        public static DataTable convertExcelToDatatable(string fpath)
        {
            //var fileName = string.Format("{0}\\fileNameHere", Directory.GetCurrentDirectory());
            //var connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", fpath);

            //var adapter = new OleDbDataAdapter("select * from [Sheet1$]", connectionString);
            //var ds = new DataSet();

            //adapter.Fill(ds, "anyNameHere");

            //DataTable data = ds.Tables["anyNameHere"];

            FileStream stream = System.IO.File.Open(fpath, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            DataSet result = excelReader.AsDataSet();
            //excelReader.IsFirstRowAsColumnNames = true;
            DataSet ds = excelReader.AsDataSet();
            while (excelReader.Read())
            {
            }
            excelReader.Close();
            return ds.Tables[0];


          //  return data;
        }

        public static DataTable ReadExcelFileDT(string fpath)
        {
            fpath = System.IO.Path.Combine(HttpContext.Current.Server.MapPath(fpath));
            return convertExcelToDatatable(fpath);
        }

    }
}