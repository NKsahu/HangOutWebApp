using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;

namespace HangOut.Models.Common
{
    public class ReadExl
    {

        public static DataTable convertExcelToDatatable(string fpath)
        {
            //var fileName = string.Format("{0}\\fileNameHere", Directory.GetCurrentDirectory());
            var connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", fpath);

            var adapter = new OleDbDataAdapter("select * from [Sheet1$]", connectionString);
            var ds = new DataSet();

            adapter.Fill(ds, "anyNameHere");

            DataTable data = ds.Tables["anyNameHere"];
            return data;
        }

        public static DataTable ReadExcelFileDT(string fpath)
        {
            fpath = AppDomain.CurrentDomain.BaseDirectory + fpath.Replace("/", @"\");
            return convertExcelToDatatable(fpath);
        }

    }
}