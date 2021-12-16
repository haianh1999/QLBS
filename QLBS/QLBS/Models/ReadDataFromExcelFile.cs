using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace QLBS.Models
{
    public class ReadDataFromExcelFile
    {
        public DataTable ReadDataFromExcelFiles(string filepath, bool removeRow0)
        {
            string connectionString = "";
            string fileExtention = filepath.Substring(filepath.Length - 4).ToLower();
            if (fileExtention.IndexOf("xlsx") < 0)
            {
                connectionString = "Provider = Microsoft.Jet.OLEDB.4.0;Data Source=" + filepath + "; Extended Properties=Excel 8.0";
            }
            else
            {
                connectionString = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source =" + filepath + "; Extended Properties=\"Excel 12.0 Xml;HDR=NO\"";
            }
            // tao doi tuong ket noi
            OleDbConnection oledbConn = new OleDbConnection(connectionString);
            DataTable data = null;
            try
            {
                //mo ket noi
                oledbConn.Open();

                //tao doi tuong oleDBCommand 
                OleDbCommand cmd = new OleDbCommand("SELECT *FROM [Sheet1$]", oledbConn);

                //tao doi tuong OleDbDataAdapter
                OleDbDataAdapter oleda = new OleDbDataAdapter();
                oleda.SelectCommand = cmd;
                // tao doi tuong dataset de hung du dieu tuw tap tin excel
                DataSet ds = new DataSet();

                // do du lieu tu tap excel vao Dataset
                oleda.Fill(ds);
                data = ds.Tables[0];
                if (removeRow0 == true)
                {
                    data.Rows.RemoveAt(0);
                }
            }
            catch(Exception ex)
            {
                
            }
            finally
            {
                // dong chuoi ket noi
                oledbConn.Close();
            }
            return data;
        }
    }
}