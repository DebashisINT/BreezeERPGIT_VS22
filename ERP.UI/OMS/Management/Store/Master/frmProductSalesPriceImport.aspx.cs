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

namespace ERP.OMS.Management.Store.Master
{
    public partial class management_master_frmProductSalesPriceImport : System.Web.UI.Page
    {

        BusinessLogicLayer.ProductSalesPriceImportBL pBl = new ProductSalesPriceImportBL();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }

            //if (Session["ProductSalesData"] != null)
            //{
            //    DataTable salesDt = (DataTable)Session["ProductSalesData"];
            //    gridprodSalesPrice.DataSource = salesDt;
            //    gridprodSalesPrice.DataBind();
            //}

        }
        protected void grid_DataBinding(object sender, EventArgs e)
        {
            if (Session["ProductSalesData"] != null)
            {
                gridprodSalesPrice.DataSource = (DataTable)Session["ProductSalesData"];
            }

           
        }
      
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lnlDownloader_Click(object sender, EventArgs e)
        {
            string strFileName = "ProductSalesPriceTemplate.xlsx";
          //  string strPath = Server.MapPath("~/CommonFolderErpCRM/Excel/") + strFileName;
            string strPath = Server.MapPath("~/CommonFolder/") + strFileName;

            Response.ContentType = "application/CSV"; ;
            Response.AppendHeader("Content-Disposition", "attachment; filename=FileFormat.xlsx");
            Response.TransmitFile(strPath);
            Response.End();
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
                        
                        recalculateMinSalePricePlus(dt);
                        recalculateMinSalePriceMin(dt);
                        formatColummn(dt);
                        //Use of dataTable
                        gridprodSalesPrice.DataSource = dt;
                        gridprodSalesPrice.DataBind();
                        Session["ProductSalesData"] = dt;
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

        private void formatColummn(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                dr["Min Sale Price"] = string.Format("{0:0.00}", Convert.ToDecimal(dr["Min Sale Price"]));
                dr["MRP"] = string.Format("{0:0.00}", Convert.ToDecimal(dr["MRP"]));
                dr["Sale Price"] = string.Format("{0:0.00}", Convert.ToDecimal(dr["Sale Price"]));
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

        private void recalculateMinSalePriceMin(DataTable mainData)
        {
            double markupMin, mrp;

            foreach (DataRow dr in mainData.Rows)
            {
                if (Convert.ToInt32(dr["Markup(-)%"]) != 0)
                {
                    markupMin = Convert.ToDouble(dr["Markup(-)%"]);
                    mrp = Convert.ToDouble(dr["MRP"]);
                    dr["Min Sale Price"] = mrp - ((markupMin * mrp) / 100);
                }
            }

        }

        private void recalculateMinSalePricePlus(DataTable mainData)
        {
            double markupMin, mrp;

            foreach (DataRow dr in mainData.Rows)
            {
                if (Convert.ToInt32(dr["Markup(+)%"]) != 0)
                {
                    markupMin = Convert.ToDouble(dr["Markup(+)%"]);
                    mrp = Convert.ToDouble(dr["MRP"]);
                    dr["Min Sale Price"] = mrp + ((markupMin * mrp) / 100);
                }
            }

        }

        protected void btnUploadRecord_Click(object sender, EventArgs e)
        {
            if (Session["ProductSalesData"] != null)
            {
                try
                {
                    DataTable salesDt = (DataTable)Session["ProductSalesData"];

                    foreach (DataRow dr in salesDt.Rows)
                    {

                        pBl.UpdateSalesPriceimport(Convert.ToString(dr["Product Code"]), Convert.ToDecimal(dr["MRP"]), Convert.ToDecimal(dr["Markup(-)%"]), Convert.ToDecimal(dr["Markup(+)%"]), Convert.ToDecimal(dr["Sale Price"]), Convert.ToDecimal(dr["Min Sale Price"]), Convert.ToInt32(Session["userid"]));
                    }

                    ScriptManager.RegisterStartupScript(this, GetType(), UniqueID, "jAlert('Sale Price Updated Successfully.')", true);
                }
                catch (Exception Ex) { 
                
                }

            }
        }

    }
}