using DevExpress.Web;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class UploadSerial : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["GRN_Serial"] = null;
            }
        }
        protected void ASPxUploadControl1_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            try
            {
                ASPxUploadControl Files = sender as ASPxUploadControl;
                if (Files.UploadedFiles[0].FileBytes.Length > 0 && Files.UploadedFiles[0].IsValid)
                {
                    string fileName = Files.UploadedFiles[0].FileName;
                    string fileExtension = System.IO.Path.GetExtension(Files.UploadedFiles[0].FileName);

                    String SavePath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/" + fileName);
                    Files.UploadedFiles[0].SaveAs(SavePath);

                    FileInfo fileInfoClone = new FileInfo(SavePath);
                    string strConn = Get_ConnectionString(fileInfoClone);

                    string strSql = "SELECT * FROM [" + fileName + "]";
                    OleDbDataAdapter adapter = new OleDbDataAdapter(strSql, strConn);
                    DataTable dt = new DataTable();
                    DataTable dtSchema = new DataTable();
                    adapter.FillSchema(dt, SchemaType.Mapped);
                    adapter.Fill(dt);

                    File.Delete(SavePath);

                    Session["GRN_Serial"] = dt;
                }
            }
            catch(Exception ex)
            {

            }
        }
        private string Get_ConnectionString(FileInfo fileInfo)
        {
            string fileExtension = fileInfo.Extension;

            switch (fileExtension)
            {
                case ".xls":
                    return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileInfo.DirectoryName + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";
                case ".xlsx":
                    return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileInfo.DirectoryName + ";Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1'";
                case ".csv":
                    return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileInfo.DirectoryName + "\\;" + "Extended Properties='text;HDR=YES;FMT=Delimited'";
                default:
                    return "";

            }
        }
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (uploadProdSalesPrice.FileName.Trim() != "")
                {
                    string fileName = Path.GetFileName(uploadProdSalesPrice.PostedFile.FileName);

                    string extention = fileName.Substring(fileName.IndexOf('.'), fileName.Length - fileName.IndexOf('.'));
                    extention = extention.TrimStart('.');
                    extention = extention.ToUpper();

                    if (extention == "XLSX")
                    {
                        fileName = fileName.Replace(fileName.Substring(0, fileName.IndexOf('.')), "ProductSalesTempExcelForUpload");

                        DataTable dt = new DataTable();
                        string filePath = Server.MapPath("~/CommonFolderErpCRM/Excel/") + fileName;
                        uploadProdSalesPrice.SaveAs(filePath);
                        using (SpreadsheetDocument doc = SpreadsheetDocument.Open(filePath, false))
                        {
                            Sheet sheet = doc.WorkbookPart.Workbook.Sheets.GetFirstChild<Sheet>();
                            Worksheet worksheet = (doc.WorkbookPart.GetPartById(sheet.Id.Value) as WorksheetPart).Worksheet;
                            IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>().DefaultIfEmpty();
                            
                            foreach (Row row in rows)
                            {
                                if (row.RowIndex.Value == 1)
                                {
                                    foreach (Cell cell in row.Descendants<Cell>())
                                    {
                                        dt.Columns.Add(GetValue(doc, cell));
                                    }
                                }
                                else
                                {
                                    dt.Rows.Add();
                                    int i = 0;
                                    foreach (Cell cell in row.Descendants<Cell>())
                                    {
                                        dt.Rows[dt.Rows.Count - 1][i] = GetValue(doc, cell);
                                        i++;
                                    }
                                }
                            }
                        }

                        if(dt!=null)
                        {
                            Session["GRN_Serial"] = dt;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), UniqueID, "jAlert('invalid File')", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), UniqueID, "jAlert('Please Select a File')", true);
                }

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "On_UploadComplete()", true);
            }
            catch (Exception ex)
            {

            }
        }
        private string GetValue(SpreadsheetDocument doc, Cell cell)
        {
            string value = cell.CellValue.InnerText;
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return doc.WorkbookPart.SharedStringTablePart.SharedStringTable.ChildElements.GetItem(int.Parse(value)).InnerText;
            }
            return value;
        }
    }
}