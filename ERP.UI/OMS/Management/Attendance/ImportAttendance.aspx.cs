using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;
using System.Web.Services;
using System.Collections.Generic;
using System.Data.OleDb;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Linq;
using DataAccessLayer;

namespace ERP.OMS.Management.Attendance
{
    public partial class ImportAttendance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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



                    if (extention == "XLS" || extention == "XLSX" || extention == "CSV")
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


                        dt = ConvertToTimeFormat(dt);

                        dt.Columns.Remove("Time");


                        ProcedureExecute proc = new ProcedureExecute("prc_importAttendance");
                        proc.AddVarcharPara("@action", 100, "ValidateInput");
                        proc.AddPara("@TimeTable",dt);
                        DataTable OutputTable = proc.GetTable();



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
            }
            catch (Exception ex)
            {

            }
        }


        public DataTable ConvertToTimeFormat(DataTable floatTime)
        {
            floatTime.Columns.Add("TimeColumn",typeof(System.DateTime));



            for (int i = 0; i < floatTime.Rows.Count;i++ )
            {

                float excelValue = float.Parse(Convert.ToString(floatTime.Rows[i]["Time"]));

                int miliseconds = (int)Math.Round(excelValue * 86400000);
                int hour = miliseconds / (60/*minutes*/* 60/*seconds*/* 1000);
                miliseconds = miliseconds - hour * 60/*minutes*/* 60/*seconds*/* 1000;
                int minutes = miliseconds / (60/*seconds*/* 1000);
                miliseconds = miliseconds - minutes * 60/*seconds*/* 1000;
                int seconds = miliseconds / 1000;


                floatTime.Rows[i]["TimeColumn"] = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, minutes, seconds, 0);
                 
            }


            return floatTime;
        
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