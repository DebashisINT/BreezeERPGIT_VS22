using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using EntityLayer.CommonELS;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class stockReconcileList : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        //chinmoy started for import data

        ExcelFile ex = new ExcelFile();
        FileInfo FIICXCSV = null;
        StreamReader strReader;
        StringBuilder strbuilder = new StringBuilder();
        String readline = string.Empty;
        public string[] InputName = new string[20];
        public string[] InputType = new string[20];
        public string[] InputValue = new string[20];
        public string[] InputName1 = new string[20];
        public string[] InputType1 = new string[20];
        public string[] InputValue1 = new string[20];
        DataTable dt1 = new DataTable();
        string FilePath = "";
        DataSet dsdata = new DataSet();
        DataView dvUnmatched = new DataView();
        private static String path, path1, FileName, s, time, cannotParse;
        Int32 IsComplete = 0;
        string MSG = "";
        //end
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/stockReconcileList.aspx");
            FormDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
            FormDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);
            toDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
            toDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);
            if (!IsPostBack)
            {
                Session["MSG"] = null;
                Session["Reconid"] = null;
                FormDate.Date = DateTime.Now;
                toDate.Date = DateTime.Now;
                DataTable dt = GetWarehouseData();
                CmbWarehouse.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    CmbWarehouse.Items.Add(Convert.ToString(dt.Rows[i]["bui_WarehouseName"]), Convert.ToString(dt.Rows[i]["bui_WarehouseID"]));
                }

                CmbWarehouse.SelectedIndex = 0;
            }
        }


        protected void gridAdjustment_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            StockAdjustmentBL blLayer = new StockAdjustmentBL();

            GrdQuotation.JSProperties["cpgridAdjustmentDelete"] = "";
            Session["Reconid"] = null;

            string param = Convert.ToString(e.Parameters);
            if (param.Split('~')[0] == "Delete")
            {
                Int64 ReconId = Convert.ToInt64(param.Split('~')[1]);
                int rowsNo = blLayer.DeleteAdj(param.Split('~')[1]);
                int delcnt = ReconcileDeleteAfterAdjDelete(ReconId);
                //oDBEngine.GetDataTable("update tmr  set Is_PostedToStkAdj='N'  from tbl_master_ReconcileStock tmr inner join tbl_master_ReconcileStockDetails tmrs  on tmrs.Reconcile_Id=tmr.Reconcile_Id where tmrs.Reconcile_StkAdjId=" + ReconId);

                if (rowsNo > 0)
                {
                    grdAdjustmnentList.JSProperties["cpgridAdjustmentDelete"] = "Document Deleted Successfully";
                }
            }


        }
        public int ReconcileDeleteAfterAdjDelete(Int64 ReconId)
        {
            int i;
            int rtrnvalue = 0;

            ProcedureExecute proc = new ProcedureExecute("Prc_ReconcileStock");
            proc.AddVarcharPara("@Action", 500, "UpdateReconcileAfterAdjDelete");
            proc.AddBigIntegerPara("@Stock_Id", ReconId);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;


        }
        protected void GrdQuotation_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GrdQuotation.JSProperties["cpDelete"] = "";
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            Int64 DeleteVal = 0;
            int deletecnt = 0;
            string WhichType = null;


            if (Convert.ToString(e.Parameters).Contains("~"))
            {
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                {
                    WhichType = Convert.ToString(e.Parameters).Split('~')[1];
                    DeleteVal = Convert.ToInt64(WhichType);
                    hdnStockAdjustedLIst.Value = Convert.ToString(DeleteVal);
                    Session["Reconid"] = Convert.ToString(DeleteVal);
                }
            }


            if (WhichCall == "Delete" && WhichType != "")
            {
                deletecnt = DeleteReconcileData(DeleteVal);
                if (deletecnt == 1)
                {
                    GrdQuotation.JSProperties["cpDelete"] = "Deleted";
                    //GetQuotationListGridData(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]));
                }
                else if (deletecnt == -11)
                {
                    GrdQuotation.JSProperties["cpDelete"] = "CannotDelete";
                }
                else
                {
                    GrdQuotation.JSProperties["cpDelete"] = "Inconvenience";
                }
            }


        }




        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Reconcile_Id";

            // string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strWareHouseID = (Convert.ToString(hfwareHouseID.Value) == "") ? "0" : Convert.ToString(hfwareHouseID.Value);

            if (strFromDate != "" && strToDate != "")
            {
                if (strWareHouseID == "0")
                {
                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    var q = from d in dc.V_ReconCileLists
                            where d.ReconcileDate >= Convert.ToDateTime(strFromDate) && d.ReconcileDate <= Convert.ToDateTime(strToDate)
                            //&& strWareHouseID == Convert.ToString(d.WarehouseID)
                            orderby d.ReconcileDate descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    var q = from d in dc.V_ReconCileLists
                            where d.ReconcileDate >= Convert.ToDateTime(strFromDate) && d.ReconcileDate <= Convert.ToDateTime(strToDate)
                            && strWareHouseID == Convert.ToString(d.WarehouseID)
                            orderby d.ReconcileDate descending
                            select d;
                    e.QueryableSource = q;
                }

            }
            else
            {
                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                var q = from d in dc.V_ReconCileLists
                        where 1 == 0
                        orderby d.ReconcileDate descending
                        select d;
                e.QueryableSource = q;
            }

        }

        protected void EntityServerModeDataSourcegridAdjustment_StockAdjustedSelecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Stock_ID";



            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);




            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            var q = from d in dc.V_StockAdjReconcileDocumentWises
                    where d.Reconcile_Id == Convert.ToInt64(Session["Reconid"])
                    select d;
            //hdnStockAdjustedLIst.Value
            e.QueryableSource = q;


        }

        protected void EntityServerModeDataSource_StockAdjustedSelecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Reconcile_Id";



            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);




            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            var q = from d in dc.V_ReconCileListDocumentWises
                    where d.Reconcile_Id == Convert.ToInt64(hdnReconcilestkadjId.Value)
                    select d;
            e.QueryableSource = q;


        }


        protected void EntityServerModeDataSourceZeroStock_StockAdjustedSelecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Reconcile_Id";

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            var q = from d in dc.V_ZeroStockDEtails
                    where d.Reconcile_Id == Convert.ToInt64(ReconIdForNonStock.Value)
                    select d;
            e.QueryableSource = q;

        }

        protected void ReconcileStkAdjustmentListExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdStkAdj.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exporter"] == null)
                {
                    Session["exporter"] = Filter;

                    bindReconcileStkAdjustmentexport(Filter);
                }
                else if (Convert.ToInt32(Session["exporter"]) != Filter)
                {
                    Session["exporter"] = Filter;
                    bindReconcileStkAdjustmentexport(Filter);
                }
            }
        }


        public void bindReconcileStkAdjustmentexport(int Filter)
        {
            //GrdQuotation.Columns[6].Visible = false;
            GrdQuotation.Columns[0].Visible = false;


            string filename = "Reconcile Stock AdjustmentList";
            exporter.FileName = filename;
            exporter.FileName = "StockAdjustmentList";

            exporter.PageHeader.Left = "Reconcile Stock AdjustmentList";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";

            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }


        protected void lnlDownloaderexcel_Click(object sender, EventArgs e)
        {

            string strFileName = "Physical Stock Data.xlsx";
            string strPath = (Convert.ToString(System.AppDomain.CurrentDomain.BaseDirectory) + Convert.ToString(ConfigurationManager.AppSettings["SaveFile"]) + strFileName);

            Response.ContentType = "application/xlsx";
            Response.AppendHeader("Content-Disposition", "attachment; filename=Physical Stock Data.xlsx");
            Response.TransmitFile(strPath);
            Response.End();

        }
        protected void BtnSaveexcel_Click1(object sender, EventArgs e)
        {
            string fName = string.Empty;
            Boolean HasLog = false;
            if (OFDBankSelect.FileContent.Length != 0)
            {
                path = String.Empty;
                path1 = String.Empty;
                FileName = String.Empty;
                s = String.Empty;
                time = String.Empty;
                cannotParse = String.Empty;
                string strmodule = "InsertTradeData";


                BusinessLogicLayer.TransctionDescription td = new BusinessLogicLayer.TransctionDescription();

                FilePath = Path.GetFullPath(OFDBankSelect.PostedFile.FileName);
                FileName = Path.GetFileName(FilePath);
                string fileExtension = Path.GetExtension(FileName);

                if (fileExtension.ToUpper() != ".XLS" && fileExtension.ToUpper() != ".XLSX")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Uploaded file format not supported by the system');</script>");
                    return;
                }


                if (fileExtension.Equals(".xlsx"))
                {
                    fName = FileName.Replace(".xlsx", DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx");
                }

                else if (fileExtension.Equals(".xls"))
                {
                    fName = FileName.Replace(".xls", DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xls");
                }

                else if (fileExtension.Equals(".csv"))
                {
                    fName = FileName.Replace(".csv", DateTime.Now.ToString("ddMMyyyyhhmmss") + ".csv");
                }

                Session["FileName"] = fName;
                String UploadPath = Server.MapPath("~/Temporary/" + Session["FileName"].ToString());
                // String UploadPath = Server.MapPath((Convert.ToString(ConfigurationManager.AppSettings["SaveCSV"]) + Session["FileName"].ToString()));
                OFDBankSelect.PostedFile.SaveAs(UploadPath);
                // OFDBankSelect.SaveAs(UploadPath);
                ClearArray();


                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                try
                {
                    HttpPostedFile file = OFDBankSelect.PostedFile;
                    String extension = Path.GetExtension(FileName);
                    HasLog = Import_To_Grid(UploadPath, extension, file);
                }
                catch (Exception ex)
                {
                    HasLog = false;
                }

                if (HasLog == true && IsComplete == 1)
                {


                    Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Physical stock imported Process successfully Completed.');</script>");

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>StockImportComplete();</script>");
                }








            }


            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Selected File Cannot Be Blank');</script>");
            }

        }

        public void ClearArray()
        {
            Array.Clear(InputName, 0, InputName.Length - 1);
            Array.Clear(InputType, 0, InputType.Length - 1);
            Array.Clear(InputValue, 0, InputValue.Length - 1);
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
        public static int? GetColumnIndexFromName(string columnName)
        {
            //return columnIndex;
            string name = columnName;
            int number = 0;
            int pow = 1;
            for (int i = name.Length - 1; i >= 0; i--)
            {
                number += (name[i] - 'A' + 1) * pow;
                pow *= 26;
            }
            return number;

        }
        public static string GetColumnName(string cellReference)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);

            return match.Value;
        }
        public Boolean Import_To_Grid(string FilePath, string Extension, HttpPostedFile file)
        {
            Employee_BL objEmploye = new Employee_BL();
            Boolean Success = false;
            Boolean HasLog = false;
            int loopcounter = 1;

            if (file.FileName.Trim() != "")
            {

                if (Extension.ToUpper() == ".XLS" || Extension.ToUpper() == ".XLSX")
                {
                   // DataTable dt = new DataTable();

                    //using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(FilePath, false))
                    //{

                    //    Sheet sheet = spreadSheetDocument.WorkbookPart.Workbook.Sheets.GetFirstChild<Sheet>();
                    //    Worksheet worksheet = (spreadSheetDocument.WorkbookPart.GetPartById(sheet.Id.Value) as WorksheetPart).Worksheet;
                    //    IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();
                    //    foreach (Row row in rows)
                    //    {
                    //        if (row.RowIndex.Value == 1)
                    //        {
                    //            foreach (Cell cell in row.Descendants<Cell>())
                    //            {
                    //                if (cell.CellValue != null)
                    //                {
                    //                    dt.Columns.Add(GetValue(spreadSheetDocument, cell));
                    //                }
                    //            }
                    //        }
                    //        else
                    //        {
                    //            DataRow tempRow = dt.NewRow();
                    //            int columnIndex = 0;
                    //            foreach (Cell cell in row.Descendants<Cell>())
                    //            {
                    //                // Gets the column index of the cell with data

                    //                int cellColumnIndex = (int)GetColumnIndexFromName(GetColumnName(cell.CellReference));
                    //                cellColumnIndex--; //zero based index
                    //                if (columnIndex < cellColumnIndex)
                    //                {
                    //                    do
                    //                    {
                    //                        tempRow[columnIndex] = ""; //Insert blank data here;
                    //                        columnIndex++;
                    //                    }
                    //                    while (columnIndex < cellColumnIndex);
                    //                }
                    //                try
                    //                {
                    //                    tempRow[columnIndex] = GetValue(spreadSheetDocument, cell);

                    //                }
                    //                catch
                    //                {
                    //                    tempRow[columnIndex] = "";
                    //                }

                    //                columnIndex++;
                    //            }
                    //            dt.Rows.Add(tempRow);
                    //        }
                    //    }



                    //}
                    DataTable dtExcelData = new DataTable();
                    string conString = string.Empty;
                    conString = ConfigurationManager.AppSettings["ExcelConString"];
                    conString = string.Format(conString, FilePath);
                    using (OleDbConnection excel_con = new OleDbConnection(conString))
                    {
                        excel_con.Open();
                        string sheet1 = "sheet1$"; //ī;

                        dtExcelData.Columns.Add("Product Code", typeof(string));
                        dtExcelData.Columns.Add("Description", typeof(string));
                        dtExcelData.Columns.Add("Class", typeof(string));
                        dtExcelData.Columns.Add("Stock Unit", typeof(string));
                        dtExcelData.Columns.Add("Alt Unit", typeof(string));
                        dtExcelData.Columns.Add("Stock Unit Quantity", typeof(string));
                        dtExcelData.Columns.Add("Alt Unit Quantity", typeof(string));
                        dtExcelData.Columns.Add("Warehouse Name", typeof(string));
                        dtExcelData.Columns.Add("Date", typeof(string));
                       
                        using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", excel_con))
                        {
                            oda.Fill(dtExcelData);
                        }
                        excel_con.Close();
                    }

                   // if (dt != null && dt.Rows.Count > 0)
                  //  {
                        //string EmployeeCode = string.Empty;

                    if (dtExcelData != null && dtExcelData.Rows.Count > 0)
                    {

                        // foreach (DataRow row in dt.Rows)
                        foreach (DataRow row in dtExcelData.Rows)
                        {
                            loopcounter++;
                            try
                            {



                                string ProductName = Convert.ToString(row["Product Code"]);
                                string Description = Convert.ToString(row["Description"]);
                                string Class = Convert.ToString(row["Class"]);
                                string StockUnit = Convert.ToString(row["Stock Unit"]);
                                string AltUnit = Convert.ToString(row["Alt Unit"]);
                                string StockUnitQuantity = Convert.ToString(row["Stock Unit Quantity"]);
                                string AltUnitQuantity = Convert.ToString(row["Alt Unit Quantity"]);
                                string Warehousename = Convert.ToString(row["Warehouse Name"]);
                                string Date = Convert.ToString(row["Date"]);


                                DataSet dt2 = InsertPhysicalStockDataFromExcel(ProductName, Description, Class, StockUnit, AltUnit, StockUnitQuantity, AltUnitQuantity, Warehousename, Date);


                                if (dt2 != null && dt2.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow row2 in dt2.Tables[0].Rows)
                                    {
                                        Success = Convert.ToBoolean(row2["Success"]);
                                        HasLog = Convert.ToBoolean(row2["HasLog"]);
                                        MSG = Convert.ToString(row2["MSG"]);
                                        Session["MSG"] = MSG;
                                    }
                                }

                                //if (!HasLog)
                                //{
                                //    string description = Convert.ToString(dt2.Tables[0].Rows[0]["MSG"]);
                                //    int loginsert = objEmploye.InsertEmployeeImportLOg(EmployeeCode, loopcounter, FirstName, UserId, Session["FileName"].ToString(), description, "Failed");
                                //}

                                if (HasLog == true)
                                {
                                    string description = Convert.ToString(dt2.Tables[0].Rows[0]["MSG"]);

                                }



                            }
                            catch (Exception ex)
                            {
                                Success = false;
                                HasLog = false;
                                // string description = Convert.ToString(dt2.Tables[0].Rows[0]["MSG"]);
                                //int loginsert = objEmploye.InsertEmployeeImportLOg(EmployeeCode, loopcounter, "", "", Session["FileName"].ToString(), ex.Message.ToString(), "Failed");
                            }

                        }
                        if (HasLog == true)
                        {
                            StockCommit();
                        }
                    }

                }
                else
                {

                }
            }
            return HasLog;
        }

        protected void StockCommit()
        {
            //Int32 IsComplete = 0;
            //Int64 warehouseid = Convert.ToInt64(CmbWarehouse.Value);
            //DataTable dt = (DataTable)Session["BindProduct_CalcuCommitDetails"];
            try
            {
                DataSet dsInst = new DataSet();

                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                SqlCommand cmd = new SqlCommand("Prc_PhyStockCalculateCommitImportFromExcel", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@Action", "SaveCommitStockData");
                //cmd.Parameters.AddWithValue("@SelectedProductList", hdnCalcommitProductId.Value);
                //cmd.Parameters.AddWithValue("@WareHouseId", warehouseid);
                cmd.Parameters.AddWithValue("@CreateUser", Convert.ToString(Session["userid"]));




                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);


                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;


                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);

                IsComplete = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());



                cmd.Dispose();
                con.Dispose();
            }
            catch (Exception ex)
            {

            }

            //if (IsComplete == 1)
            //{

            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "jAlert('Please try again later.')", true);
            //}
            //else
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "jAlert('Please try again later.')", true);
            //}

        }
        public DataTable GetWarehouseData()
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_ReconcileStock");
            proc.AddVarcharPara("@Action", 500, "AllWarehouse");

            dt = proc.GetTable();
            return dt;
        }


        public DataSet InsertPhysicalStockDataFromExcel(string ProductName, string Description, string Class, string StockUnit, string AltUnit, string StockUnitQuantity, string AltUnitQuantity, string Warehousename, string Date)
        {



            DataSet ds = new DataSet();

            DateTime? Adjdate = null;
            if (Date != "")
            {
                Adjdate = DateTime.ParseExact(Date, "dd-MM-yyyy", CultureInfo.InvariantCulture); ;
            }

            ProcedureExecute proc = new ProcedureExecute("Prc_PhysicalStockImportFromExcel");
            proc.AddVarcharPara("@Action", 100, "InsertPhysicalStockDataFromExcel");
            proc.AddIntegerPara("@Userid", Convert.ToInt32(HttpContext.Current.Session["userid"]));
            proc.AddVarcharPara("@ProductName", 500, ProductName);
            proc.AddVarcharPara("@Description", 4000, Description);
            proc.AddVarcharPara("@Class", 200, Class);
            proc.AddVarcharPara("@StockUnit", 200, StockUnit);
            proc.AddVarcharPara("@AltUnit", 200, AltUnit);
            proc.AddDecimalPara("@StockUnitQuantity", 5, 18, Convert.ToDecimal(StockUnitQuantity));
            proc.AddDecimalPara("@AltUnitQuantity", 5, 18, Convert.ToDecimal(AltUnitQuantity));
            proc.AddVarcharPara("@WarehouseName", 500, Warehousename);
            proc.AddPara("@Date", Adjdate);

            ds = proc.GetDataSet();
            return ds;
        }
        public int DeleteReconcileData(Int64 Rec_id)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("Prc_ReconcileStock");
            proc.AddVarcharPara("@Action", 100, "DeleteReconcile");
            proc.AddBigIntegerPara("@Reconcile_Id", Rec_id);

            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }

        protected void GrdComitListDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "StockSheet_id";
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            var q = from d in dc.V_ReconcileComitLists
                    orderby d.StockSheet_id descending
                    select d;
            e.QueryableSource = q;
        }


        [WebMethod]
        public static string NonZerodatacheck(string id)
        {
            string AdjIdStatus = "2";
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            Int64 AdjId = Convert.ToInt64(id);
            DataTable recondt = oDBEngine.GetDataTable("Select Isnull(ReconcileStockDetails_id,0) ReconValue from tbl_master_ReconcileStockDetails where Reconcile_Id=" + Convert.ToInt64(AdjId) + "");
            if (recondt.Rows.Count > 0 && recondt != null)
            {
                if (Convert.ToString(recondt.Rows[0]["ReconValue"]) != "0" && Convert.ToString(recondt.Rows[0]["ReconValue"]) != "")
                {
                    AdjIdStatus = "1";
                }
                else
                {
                    AdjIdStatus = "2";
                }

            }
            return AdjIdStatus;
        }

    }


}
