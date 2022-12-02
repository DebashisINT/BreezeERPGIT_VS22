using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace ERP.OMS.Management
{
    /// <summary>
    /// Summary description for FileUploader
    /// </summary>
    public class FileUploader : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {


            context.Response.ContentType = "text/plain";
            try
            {
                string dirFullPath = HttpContext.Current.Server.MapPath("~/CommonFolder/EInvoice/");
                string[] files;
                int numFiles;
                files = System.IO.Directory.GetFiles(dirFullPath);
                numFiles = files.Length;
                numFiles = numFiles + 1;
                string str_image = "";

                foreach (string s in context.Request.Files)
                {
                    HttpPostedFile file = context.Request.Files[s];
                    string fileName = file.FileName;
                    string fileExtension = file.ContentType;

                    if (!string.IsNullOrEmpty(fileName))
                    {
                        fileExtension = Path.GetExtension(fileName);
                        str_image = DateTime.Now.ToString("dd_mm_yyyy") + "_" + numFiles.ToString() + fileExtension;
                        string pathToSave_100 = HttpContext.Current.Server.MapPath("~/CommonFolder/EInvoice/") + str_image;
                        file.SaveAs(pathToSave_100);
                        GetDataFromExcel(pathToSave_100, fileExtension, "No",context);
                    }
                }
                //  database record update logic here  ()

                context.Response.Write(str_image);
            }
            catch (Exception ac)
            {

            }
        }

        private void GetDataFromExcel(string FilePath, string Extension, string isHdr, HttpContext context)
        {
            DataTable dt = new DataTable();
            var xmlFile = FilePath;
            using (var workBook = new XLWorkbook(xmlFile))
            {
                bool FirstRow = true;
                var workSheet = workBook.Worksheet(1);
                var firstRowUsed = workSheet.FirstRowUsed();
                var firstPossibleAddress = workSheet.Row(firstRowUsed.RowNumber()).FirstCell().Address;
                var lastPossibleAddress = workSheet.LastCellUsed().Address;
                string readRange = "1:1";
                // Get a range with the remainder of the worksheet data (the range used)
                var range = workSheet.Range(firstPossibleAddress, lastPossibleAddress).AsRange(); //.RangeUsed();
                // Treat the range as a table (to be able to use the column names)
                var table = range.AsTable();

                //Specify what are all the Columns you need to get from Excel

                foreach (IXLRow row in workSheet.RowsUsed())
                {
                    //If Reading the First Row (used) then add them as column name
                    if (FirstRow)
                    {
                        //Checking the Last cellused for column generation in datatable
                        readRange = string.Format("{0}:{1}", 1, row.LastCellUsed().Address.ColumnNumber);
                        foreach (IXLCell cell in row.Cells(readRange))
                        {
                            dt.Columns.Add(cell.Value.ToString());
                        }
                        FirstRow = false;
                    }
                    else
                    {
                        //Adding a Row in datatable
                        dt.Rows.Add();
                        int cellIndex = 0;
                        //Updating the values of datatable
                        foreach (IXLCell cell in row.Cells(readRange))
                        {
                            dt.Rows[dt.Rows.Count - 1][cellIndex] = cell.Value.ToString();
                            cellIndex++;
                        }
                    }
                }




                DataTable dsInst = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(context.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_Einvoice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@aCTION", "UpdateIRN");
                cmd.Parameters.AddWithValue("@udt_IRN", dt);
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);

 


            }





        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}