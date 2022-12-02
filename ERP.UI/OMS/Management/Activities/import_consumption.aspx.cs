using BusinessLogicLayer;
using ClosedXML.Excel;
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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class import_consumption : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileImport.PostedFile != null)
                {


                    if (FileImport.HasFile)
                    {
                        string FileName = Path.GetFileName(FileImport.PostedFile.FileName);
                        string Extension = Path.GetExtension(FileImport.PostedFile.FileName);
                        string FolderPath = ConfigurationManager.AppSettings["FolderPath"];
                        string FilePath = Server.MapPath("~/CommonFolder/ProductCostMarketPrice/") + FileName;
                        FileImport.SaveAs(FilePath);
                        GetDataFromExcel(FilePath, Extension, "No");
                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>UploadComplete()</script>");
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('Please select a file to proceed.')</script>");
                    }

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('Please select a file to proceed.')</script>");
                }

            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('" + ex.Message.Replace("'", "\"") + "')</script>");
            }
        }
        private void GetDataFromExcel(string FilePath, string Extension, string isHdr)
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

                //Session["tblStucture"] = dt;
                DBEngine obj = new DBEngine();

                obj.GetDataTable("TRUNCATE TABLE STOCKTRANSFER_EXCEL_FEILD");

                foreach (DataColumn dc in dt.Columns)
                {
                    obj.GetDataTable("INSERT INTO STOCKTRANSFER_EXCEL_FEILD(VALUE_FEILD,TEXT_FEILD) VALUES ('" + dc.ColumnName + "','" + dc.ColumnName + "')");
                }


                //If no data in Excel file
                if (FirstRow)
                {
                    string output = "Empty Excel File!";
                }


            }
        }


        private void GetDataFromExcelMain(string FilePath, string Extension, string isHdr)
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



                DataTable dtCopy = dt.Copy();
                dtCopy.Columns.Add("Status");
                dtCopy.Columns.Add("Status Text");


                DataColumn Col = dtCopy.Columns.Add("SL", System.Type.GetType("System.String"));
                Col.SetOrdinal(0);
                Session["tblStucture"] = dtCopy;


                DBEngine obj = new DBEngine();
                DataTable dtMap = obj.GetDataTable("SELECT * FROM STOCKTRANSFER_EXCEL_MAP");

                string Doc_Column_Name = Convert.ToString(dtMap.Select("FEILD_NAME='StockTransfer_No'").CopyToDataTable().Rows[0]["EXCEL_MAP"]);
                string StockTransfer_No = Convert.ToString(dtMap.Select("FEILD_NAME='StockTransfer_No'").CopyToDataTable().Rows[0]["EXCEL_MAP"]);
                string BranchFrom_ID = Convert.ToString(dtMap.Select("FEILD_NAME='BranchFrom_ID'").CopyToDataTable().Rows[0]["EXCEL_MAP"]);
                string Stock_Date = Convert.ToString(dtMap.Select("FEILD_NAME='Stock_Date'").CopyToDataTable().Rows[0]["EXCEL_MAP"]);
                string Dckt_No = Convert.ToString(dtMap.Select("FEILD_NAME='Dckt_No'").CopyToDataTable().Rows[0]["EXCEL_MAP"]);
                string Dckt_Close_Date = Convert.ToString(dtMap.Select("FEILD_NAME='Dckt_Close_Date'").CopyToDataTable().Rows[0]["EXCEL_MAP"]);
                string TransportationMode = Convert.ToString(dtMap.Select("FEILD_NAME='TransportationMode'").CopyToDataTable().Rows[0]["EXCEL_MAP"]);
                string VehicleNo = Convert.ToString(dtMap.Select("FEILD_NAME='VehicleNo'").CopyToDataTable().Rows[0]["EXCEL_MAP"]);
                string Technician_Id = Convert.ToString(dtMap.Select("FEILD_NAME='Technician_Id'").CopyToDataTable().Rows[0]["EXCEL_MAP"]);
                string Entity_internalId = Convert.ToString(dtMap.Select("FEILD_NAME='Entity_internalId'").CopyToDataTable().Rows[0]["EXCEL_MAP"]);
                string Remarks_name = Convert.ToString(dtMap.Select("FEILD_NAME='Remarks'").CopyToDataTable().Rows[0]["EXCEL_MAP"]);
                string Customer_internalId = Convert.ToString(dtMap.Select("FEILD_NAME='Customer_internalId'").CopyToDataTable().Rows[0]["EXCEL_MAP"]);
                string Stock_RefNo = Convert.ToString(dtMap.Select("FEILD_NAME='Stock_RefNo'").CopyToDataTable().Rows[0]["EXCEL_MAP"]);
                string ProductID_name = Convert.ToString(dtMap.Select("FEILD_NAME='ProductID'").CopyToDataTable().Rows[0]["EXCEL_MAP"]);
                string SourceWarehouseID = Convert.ToString(dtMap.Select("FEILD_NAME='SourceWarehouseID'").CopyToDataTable().Rows[0]["EXCEL_MAP"]);
                string TransferQty = Convert.ToString(dtMap.Select("FEILD_NAME='TransferQty'").CopyToDataTable().Rows[0]["EXCEL_MAP"]);
                string Rate_name = Convert.ToString(dtMap.Select("FEILD_NAME='Rate'").CopyToDataTable().Rows[0]["EXCEL_MAP"]);
                string DetailsRemarks = Convert.ToString(dtMap.Select("FEILD_NAME='DetailsRemarks'").CopyToDataTable().Rows[0]["EXCEL_MAP"]);
                string Project_ID = Convert.ToString(dtMap.Select("FEILD_NAME='Project_ID'").CopyToDataTable().Rows[0]["EXCEL_MAP"]);
                string DetailsEntity_InternalId = Convert.ToString(dtMap.Select("FEILD_NAME='DetailsEntity_InternalId'").CopyToDataTable().Rows[0]["EXCEL_MAP"]);
                string DetailsRef_No = Convert.ToString(dtMap.Select("FEILD_NAME='DetailsRef_No'").CopyToDataTable().Rows[0]["EXCEL_MAP"]);




                DataView view = new DataView(dt);
                DataTable distinctValues = view.ToTable(true, Doc_Column_Name);

                foreach (DataRow dr in distinctValues.Rows)
                {
                    DataTable AdjustmentTable = new DataTable();
                    AdjustmentTable.Columns.Add("SrlNo", typeof(string));
                    AdjustmentTable.Columns.Add("ProductName", typeof(string));
                    AdjustmentTable.Columns.Add("Discription", typeof(string));
                    AdjustmentTable.Columns.Add("TransferQuantity", typeof(decimal));
                    AdjustmentTable.Columns.Add("AvlStkSourceWH", typeof(decimal));
                    AdjustmentTable.Columns.Add("Rate", typeof(decimal));
                    AdjustmentTable.Columns.Add("Value", typeof(decimal));
                    AdjustmentTable.Columns.Add("ProductID", typeof(string));
                    AdjustmentTable.Columns.Add("ActualSL", typeof(string));
                    AdjustmentTable.Columns.Add("SourceWarehouse", typeof(string));
                    AdjustmentTable.Columns.Add("SourceWarehouseID", typeof(string));
                    AdjustmentTable.Columns.Add("SaleUOM", typeof(string));
                    AdjustmentTable.Columns.Add("Remarks", typeof(string));
                    AdjustmentTable.Columns.Add("PackingQty", typeof(decimal));
                    AdjustmentTable.Columns.Add("EntityID", typeof(string));
                    AdjustmentTable.Columns.Add("gridRefNo", typeof(string));
                    AdjustmentTable.Columns.Add("EntityCode", typeof(string));
                    AdjustmentTable.Columns.Add("EntityName", typeof(string));



                    DataTable AdjustmentTableDest = new DataTable();
                    AdjustmentTableDest.Columns.Add("SrlNo", typeof(string));
                    AdjustmentTableDest.Columns.Add("ProductNameDest", typeof(string));
                    AdjustmentTableDest.Columns.Add("DestDiscription", typeof(string));
                    AdjustmentTableDest.Columns.Add("DestQuantity", typeof(decimal));
                    AdjustmentTableDest.Columns.Add("AvlStkDestWH", typeof(decimal));
                    AdjustmentTableDest.Columns.Add("DestRate", typeof(decimal));
                    AdjustmentTableDest.Columns.Add("DestValue", typeof(decimal));
                    AdjustmentTableDest.Columns.Add("DestProductID", typeof(string));
                    AdjustmentTableDest.Columns.Add("ActualDestSL", typeof(string));
                    AdjustmentTableDest.Columns.Add("DestinationWarehouse", typeof(string));
                    AdjustmentTableDest.Columns.Add("DestinationWarehouseID", typeof(string));
                    AdjustmentTableDest.Columns.Add("DestUOM", typeof(string));
                    AdjustmentTableDest.Columns.Add("DestRemarks", typeof(string));
                    AdjustmentTableDest.Columns.Add("DestPackingQty", typeof(decimal));
                    AdjustmentTableDest.Columns.Add("EntityID", typeof(string));
                    AdjustmentTableDest.Columns.Add("gridRefNo", typeof(string));
                    AdjustmentTableDest.Columns.Add("EntityCode", typeof(string));
                    AdjustmentTableDest.Columns.Add("EntityName", typeof(string));

                    DataTable Final_Table = dt.Select("[" + Doc_Column_Name + "]='" + Convert.ToString(dr[Doc_Column_Name]) + "'").CopyToDataTable();

                    string Adjustment_No = "";
                    string Adjustment_Date = "";
                    string Branch = "";
                    string TransportationModes = "";
                    string VehicleNos = "";
                    string Remarkss = "";
                    string Proj_id = "";
                    string Technician = "";
                    string Entity = "";
                    string Customer = "";
                    string RefNo = "";
                    string ddlType = "1";
                    string docket_cl_date = "";
                    string docket_nos = "";




                    int i = 0;
                    foreach (DataRow drr in Final_Table.Rows)
                    {
                        i = i + 1;
                        if (!string.IsNullOrEmpty(Doc_Column_Name))
                            Adjustment_No = Convert.ToString(drr[Doc_Column_Name]);

                        if (!string.IsNullOrEmpty(Stock_Date))
                        {
                            try
                            {
                                Adjustment_Date = DateTime.ParseExact(string.Join("", Convert.ToString(drr[Stock_Date]).Take(10)), "yyyy-MM-dd", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd");
                            }
                            catch
                            {
                                Adjustment_Date = "";
                            }
                        }
                        if (!string.IsNullOrEmpty(BranchFrom_ID))
                            Branch = GetBranchByShortCode(Convert.ToString(drr[BranchFrom_ID]));
                        else
                            Branch = GetWarehouseByShortCode(Convert.ToString(drr[SourceWarehouseID])).Split('~')[1];



                        if (!string.IsNullOrEmpty(TransportationMode))
                            TransportationModes = Convert.ToString(drr[TransportationMode]);

                        if (!string.IsNullOrEmpty(VehicleNo))
                            VehicleNos = Convert.ToString(drr[VehicleNo]);

                        if (!string.IsNullOrEmpty(Dckt_No))
                            docket_nos = Convert.ToString(drr[Dckt_No]);

                        if (!string.IsNullOrEmpty(Dckt_Close_Date))
                        {
                            try
                            {
                                docket_cl_date = DateTime.ParseExact(string.Join("", Convert.ToString(drr[Dckt_Close_Date]).Take(10)), "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                            }
                            catch
                            {
                                docket_cl_date = null;
                            }
                        }


                        if (!string.IsNullOrEmpty(Remarks_name))
                            Remarkss = Convert.ToString(drr[Remarks_name]);

                        if (!string.IsNullOrEmpty(Proj_id))
                            Proj_id = GetProjectByShortCode(Convert.ToString(drr[Proj_id]));

                        if (!string.IsNullOrEmpty(Technician_Id))
                            Technician = GetContactByShortCodeTechnicianSearch(Convert.ToString(drr[Technician_Id]), "TM");

                        if (!string.IsNullOrEmpty(Entity_internalId))
                            Entity = GetContactByShortCode(Convert.ToString(drr[Entity_internalId]), "EN");

                        if (!string.IsNullOrEmpty(Customer_internalId))
                            Customer = GetContactByShortCode(Convert.ToString(drr[Customer_internalId]), "CL");

                        if (!string.IsNullOrEmpty(Stock_RefNo))
                            RefNo = Convert.ToString(drr[Stock_RefNo]);

                        ddlType = "1";


                        string ProductName = "0~~~0";
                        string TransferQuantity = "";
                        string AvlStkSourceWH = "0~0";
                        string Rate = "";
                        string Value = "";
                        string ActualSL = "";
                        string SourceWarehouses = "";
                        string Remarks = "";
                        string PackingQty = "";
                        string EntityID = "~";
                        string gridRefNo = "";
                        string EntityCode = "";





                        string SrlNo = Convert.ToString(i);
                        if (!string.IsNullOrEmpty(ProductID_name))
                            ProductName = GetProductByShortCode(Convert.ToString(drr[ProductID_name]));

                        if (!string.IsNullOrEmpty(TransferQty))
                            TransferQuantity = Convert.ToString(drr[TransferQty]);

                        AvlStkSourceWH = "0";
                        if (!string.IsNullOrEmpty(Rate_name))
                            Rate = Convert.ToString(drr[Rate_name]);
                        else
                            Rate = "0";

                        if (Rate != "0" && !string.IsNullOrEmpty(TransferQuantity))
                            Value = Convert.ToString(Convert.ToDecimal(Rate) * Convert.ToDecimal(TransferQuantity));
                        else
                            Value = "0";

                        ActualSL = Convert.ToString(i);
                        if (!string.IsNullOrEmpty(SourceWarehouseID))
                            SourceWarehouses = GetWarehouseByShortCode(Convert.ToString(drr[SourceWarehouseID]));
                        if (!string.IsNullOrEmpty(DetailsRemarks))
                            Remarks = Convert.ToString(drr[DetailsRemarks]);
                        PackingQty = "0";
                        if (!string.IsNullOrEmpty(DetailsEntity_InternalId))
                            EntityID = GetEntityByShortCode(Convert.ToString(drr[DetailsEntity_InternalId]), "EN");
                        if (!string.IsNullOrEmpty(DetailsRef_No))
                            gridRefNo = Convert.ToString(drr[DetailsRef_No]);
                        if (!string.IsNullOrEmpty(DetailsEntity_InternalId))
                            EntityCode = Convert.ToString(drr[DetailsEntity_InternalId]);


                        AdjustmentTable.Rows.Add(SrlNo, ProductName.Split('~')[1], ProductName.Split('~')[2], TransferQuantity, AvlStkSourceWH, Rate, Value,
                            ProductName.Split('~')[0], ActualSL, SourceWarehouses.Split('~')[1], SourceWarehouses.Split('~')[0], ProductName.Split('~')[3],
                            Remarks, PackingQty, EntityID.Split('~')[0], gridRefNo, EntityCode, EntityID.Split('~')[1]
                            );


                    }

                    string validate = "";

                    //DataTable cheDt = AdjustmentTable.Select("SourceWarehouseID=''").CopyToDataTable();
                    if (AdjustmentTable.Select("SourceWarehouseID='' OR SourceWarehouseID='0'").Count() > 0)
                    {
                        validate = "Warehouse not found.";
                    }

                    if (AdjustmentTable.Select("ProductID='' OR ProductID='0'").Count() > 0)
                    {
                        validate = "Warehouse not found.";
                    }
                    // cheDt = AdjustmentTable.Select("TransferQuantity='' OR TransferQuantity='0'").CopyToDataTable();
                    if (AdjustmentTable.Select("TransferQuantity<=0").Count() > 0)
                    {
                        validate = "Quantity can not be zero or blank.";
                    }

                    if (string.IsNullOrEmpty(Adjustment_Date))
                    {
                        validate = "Date is not in proper format or Date is blank.";
                    }

                    if (string.IsNullOrEmpty(Adjustment_No))
                    {
                        validate = "Doc number can not be generated.";
                    }

                    if (string.IsNullOrEmpty(Branch) || Branch == "0")
                    {
                        validate = "Branch Not Found.";
                    }

                    if (!string.IsNullOrEmpty(Branch) && Branch != "0" && !string.IsNullOrEmpty(Technician) && Branch != "0" && string.IsNullOrEmpty(validate))
                    {
                        validate = GetContactByShortCodeTechnician(Technician, Branch);
                    }

                    if (validate == "")
                    {
                        string StockCheck = "0";
                        string StockCheckMsg = "";
                        foreach (DataRow item in AdjustmentTable.Rows)
                        {
                            DBEngine oDBEngine = new DBEngine();
                            DataTable dtAvailableStockCheck = oDBEngine.GetDataTable("Select dbo.fn_GetWarehousewiseStock(" + Branch + ",'" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "','" + Convert.ToString(HttpContext.Current.Session["LastFinYear"]) + "','" + Convert.ToString(item["ProductID"]) + "','" + Convert.ToString(item["SourceWarehouseID"]) + "','" + Adjustment_Date + "') as branchopenstock");

                            if (dtAvailableStockCheck.Rows.Count > 0)
                            {
                                StockCheck = Convert.ToString(Math.Round(Convert.ToDecimal(dtAvailableStockCheck.Rows[0]["branchopenstock"]), 4));

                                if (Convert.ToDecimal(item["TransferQuantity"]) > Convert.ToDecimal(StockCheck))
                                {
                                    validate = "Stock not available. Can not proceed.";
                                    break;
                                }
                                if (StockCheck == "0.00")
                                {
                                    validate = "Zero Stock . Can not proceed.";
                                    break;
                                }

                            }
                        }










                        AdjustmentTable.Columns.Remove("ActualSL");
                        AdjustmentTable.Columns.Remove("SourceWarehouse");
                        AdjustmentTable.Columns.Remove("SaleUOM");
                        AdjustmentTable.Columns.Remove("EntityCode");
                        AdjustmentTable.Columns.Remove("EntityName");
                        AdjustmentTable.Columns.Remove("PackingQty");
                        AdjustmentTable.AcceptChanges();


                        AdjustmentTableDest.Columns.Remove("ActualDestSL");
                        AdjustmentTableDest.Columns.Remove("DestinationWarehouse");
                        AdjustmentTableDest.Columns.Remove("DestUOM");
                        AdjustmentTableDest.Columns.Remove("EntityCode");
                        AdjustmentTableDest.Columns.Remove("EntityName");
                        AdjustmentTableDest.Columns.Remove("DestPackingQty");
                        AdjustmentTableDest.AcceptChanges();

                        if (validate == "")
                        {
                            DataTable dsInst = new DataTable();
                            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                            SqlCommand cmd = new SqlCommand("prc_WarehouseStockIN_OUT_Import", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Mode", "Add");
                            cmd.Parameters.AddWithValue("@Adjustment_No", Adjustment_No);
                            cmd.Parameters.AddWithValue("@Adjustment_Date", Adjustment_Date);
                            cmd.Parameters.AddWithValue("@Branch", Branch);
                            cmd.Parameters.AddWithValue("@BranchTo", Branch);
                            cmd.Parameters.AddWithValue("@userId", Session["userid"]);
                            cmd.Parameters.AddWithValue("@DetailTable", AdjustmentTable);
                            cmd.Parameters.AddWithValue("@DetailTableDWH", AdjustmentTableDest);
                            //cmd.Parameters.AddWithValue("@WarehouseDetail", Warehousedt);
                            //cmd.Parameters.AddWithValue("@WarehouseDetailSource", tempWarehousedt);
                            cmd.Parameters.AddWithValue("@FinYear", HttpContext.Current.Session["LastFinYear"].ToString());
                            cmd.Parameters.AddWithValue("@CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                            cmd.Parameters.AddWithValue("@TransportationMode", TransportationMode);
                            cmd.Parameters.AddWithValue("@VehicleNo", VehicleNo);
                            cmd.Parameters.AddWithValue("@Remarks", Remarkss);
                            cmd.Parameters.AddWithValue("@Project_Id", Proj_id);
                            cmd.Parameters.AddWithValue("@Docket_Number", docket_nos);
                            cmd.Parameters.AddWithValue("@Docket_Close_Date", docket_cl_date);
                            cmd.Parameters.AddWithValue("@Technician_ID", Technician);
                            cmd.Parameters.AddWithValue("@Entitycnt_internalId", Entity);
                            cmd.Parameters.AddWithValue("@Customer_internalId", Customer);
                            cmd.Parameters.AddWithValue("@RefNo", RefNo);
                            cmd.Parameters.AddWithValue("@ReplaceableType", ddlType);
                            cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, -1);
                            cmd.Parameters.Add("@ReturnId", SqlDbType.VarChar, 10);
                            cmd.Parameters.Add("@ErrorCode", SqlDbType.Int);

                            cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                            cmd.Parameters["@ReturnId"].Direction = ParameterDirection.Output;
                            cmd.Parameters["@ErrorCode"].Direction = ParameterDirection.Output;

                            cmd.CommandTimeout = 0;
                            SqlDataAdapter Adap = new SqlDataAdapter();
                            Adap.SelectCommand = cmd;
                            Adap.Fill(dsInst);

                            int AdjustedId = Convert.ToInt32(cmd.Parameters["@ReturnId"].Value);
                            string ReturnNumber = Convert.ToString(cmd.Parameters["@ReturnValue"].Value);
                            int ErrorCode = Convert.ToInt32(cmd.Parameters["@ErrorCode"].Value);


                            if (AdjustedId > 0)
                            {
                                UpdateDataTable("Success", ReturnNumber, Adjustment_No, Doc_Column_Name);
                            }
                            else
                            {
                                UpdateDataTable("Failure", ReturnNumber, Adjustment_No, Doc_Column_Name);
                            }

                        }
                        else
                        {
                            UpdateDataTable("Failure", validate, Adjustment_No, Doc_Column_Name);
                        }


                    }
                    else
                    {
                        UpdateDataTable("Failure", validate, Adjustment_No, Doc_Column_Name);
                    }

                }




                //If no data in Excel file
                if (FirstRow)
                {
                    string output = "Empty Excel File!";
                }

                gridFinancer.DataBind();

            }
        }

        private void UpdateDataTable(string sucsess, string ReturnNumber, string doc_no, string doc_column_name)
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["tblStucture"];

            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToString(dr[doc_column_name]) == doc_no)
                {
                    dr["Status"] = sucsess;
                    dr["Status Text"] = ReturnNumber;
                }
            }

        }
        private static DataTable ConvertListToDataTable(IReadOnlyList<string[]> list)
        {
            var table = new DataTable("CustomTable");
            var rows = list.Select(array => array.Length).Concat(new[] { 0 }).Max();

            table.Columns.Add("Solution Number");
            table.Columns.Add("Name");
            table.Columns.Add("Date");

            for (var j = 0; j < rows; j++)
            {
                var row = table.NewRow();
                row["Solution Number"] = list[0][j];
                row["Name"] = list[1][j];
                row["Date"] = list[2][j];
                table.Rows.Add(row);
            }
            return table;
        }


        [System.Web.Services.WebMethod]
        public static object GetList()
        {

            DataSet dsInst = new DataSet();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("Proc_StockTr_Out_import", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", "GetDynamicData");
            cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
            cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);

            DataTable dt1 = dsInst.Tables[0];
            DataTable dt2 = dsInst.Tables[1];


            List<Excel_Map_Table> oList = new List<Excel_Map_Table>();

            oList = (from DataRow dr in dt2.Rows
                     select new Excel_Map_Table()
                     {
                         ValueFeild = Convert.ToString(dr["VALUE_FEILD"]),
                         TextFeild = Convert.ToString(dr["TEXT_FEILD"])

                     }).ToList();


            List<DynamicMappingDetails> oviewList = new List<DynamicMappingDetails>();
            foreach (DataRow dr in dt1.Rows)
            {
                DynamicMappingDetails ob = new DynamicMappingDetails();
                ob.Caption = Convert.ToString(dr["CAPTION"]);
                ob.Desc = Convert.ToString(dr["FEILD_DESC"]);
                ob.Excel_Map = Convert.ToString(dr["EXCEL_MAP"]);
                ob.Mandatory = Convert.ToString(dr["IS_MANDATORY"]);
                ob.Excel_Table = oList;
                ob.FeildName = Convert.ToString(dr["FEILD_NAME"]);
                ob.ID = Convert.ToString(dr["ID"]);

                if (string.IsNullOrEmpty(ob.Excel_Map))
                {
                    var query = (from tb in oList
                                 where RemoveSpecialCharacters(tb.TextFeild.ToUpper()) == RemoveSpecialCharacters(ob.Caption.ToUpper())
                                 select tb).FirstOrDefault();

                    if (query != null && !string.IsNullOrEmpty(query.TextFeild))
                    {
                        ob.Excel_Map = query.TextFeild;
                    }

                }

                oviewList.Add(ob);


            }


            return oviewList;

        }


        [System.Web.Services.WebMethod]
        public static object SaveList(List<Map> model)
        {
            Output_Map omodel = new Output_Map();

            if (model != null)
            {

                DataTable dt = new DataTable();
                dt.Columns.Add("ID");
                dt.Columns.Add("VALUE");

                foreach (var item in model)
                {
                    dt.Rows.Add(item.ID, item.Map_ID);
                }


                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("Proc_StockTr_Out_import", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "SaveDynamicData");
                cmd.Parameters.AddWithValue("@MAP_TABLE", dt);
                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@ReturnText", SqlDbType.VarChar, 500);
                cmd.Parameters["@ReturnText"].Direction = ParameterDirection.Output;
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                string ReturnValue = Convert.ToString(cmd.Parameters["@ReturnValue"].Value.ToString());
                string ReturnText = Convert.ToString(cmd.Parameters["@ReturnText"].Value.ToString());
                omodel.text = ReturnText;
                omodel.val = ReturnValue;
                return omodel;
            }
            else
            {
                omodel.text = "Nothing to save.";
                omodel.val = "0";
                return omodel;
            }

        }


        public string GetBranchByShortCode(string branchName)
        {
            DBEngine obj = new DBEngine();
            DataTable dt = obj.GetDataTable("select branch_id from tbl_master_branch where branch_code='" + branchName + "'");
            if (dt != null && dt.Rows.Count > 0)
                return Convert.ToString(dt.Rows[0]["branch_id"]);
            else
                return "0";
        }

        public string GetProjectByShortCode(string projectcode)
        {
            DBEngine obj = new DBEngine();
            DataTable dt = obj.GetDataTable("select Proj_Id from Master_ProjectManagement where Proj_Code='" + projectcode + "'");
            if (dt != null && dt.Rows.Count > 0)
                return Convert.ToString(dt.Rows[0]["Proj_Id"]);
            else
                return "0";
        }
        public string GetContactByShortCode(string code, string Type)
        {
            DBEngine obj = new DBEngine();
            DataTable dt = obj.GetDataTable("select cnt_internalId from tbl_master_contact where cnt_contactType='" + Type + "' and cnt_UCC='" + code + "'");
            if (dt != null && dt.Rows.Count > 0)
                return Convert.ToString(dt.Rows[0]["cnt_internalId"]);
            else
                return "";
        }

        public string GetContactByShortCodeTechnicianSearch(string code, string Type)
        {
            DBEngine obj = new DBEngine();
            DataTable dt = obj.GetDataTable("select CNT.cnt_internalId from tbl_master_contact CNT INNER JOIN tbl_master_contact CNT1 ON CNT.cnt_mainAccount=CNT1.cnt_internalId where CNT.cnt_contactType='TM' and CNT1.cnt_UCC='" + code + "'");
            if (dt != null && dt.Rows.Count > 0)
                return Convert.ToString(dt.Rows[0]["cnt_internalId"]);
            else
                return "";
        }


        public string GetContactByShortCodeTechnician(string technitian, string branch)
        {
            DBEngine obj = new DBEngine();
            DataTable dt = obj.GetDataTable("select * from Srv_master_TechnicianBranch_map WHERE Tech_InternalId='" + technitian + "' and branch_id='" + branch + "'");
            if (dt != null && dt.Rows.Count > 0)
                return "";
            else
                return "Technician and branch does not match.";
        }
        public string GetEntityByShortCode(string code, string Type)
        {
            DBEngine obj = new DBEngine();
            DataTable dt = obj.GetDataTable("select cnt_internalId,cnt_firstName from tbl_master_contact where cnt_contactType='" + Type + "' and cnt_UCC='" + code + "'");
            if (dt != null && dt.Rows.Count > 0)
                return Convert.ToString(dt.Rows[0]["cnt_internalId"]) + "~" + Convert.ToString(dt.Rows[0]["cnt_firstName"]);
            else
                return "~";
        }

        public string GetProductByShortCode(string product_code)
        {
            DBEngine obj = new DBEngine();
            DataTable dt = obj.GetDataTable("select sProducts_ID,sProducts_Name,sProducts_Description sProducts_Descriptions,sProducts_DeliveryLotUnit Product_StockUOM from Master_sProducts where sProducts_Code='" + product_code + "'");
            if (dt != null && dt.Rows.Count > 0)
                return Convert.ToString(dt.Rows[0]["sProducts_ID"]) + "~" + Convert.ToString(dt.Rows[0]["sProducts_Name"]) + "~" + Convert.ToString(dt.Rows[0]["sProducts_Descriptions"]) + "~" + Convert.ToString(dt.Rows[0]["Product_StockUOM"]);
            else
                return "0~~~0";
        }

        public string GetWarehouseByShortCode(string warehouse_code)
        {
            DBEngine obj = new DBEngine();
            DataTable dt = obj.GetDataTable("select top 1 bui.bui_id bui_WarehouseID,Branch_id from  tbl_master_building bui inner join Master_Warehouse_Branchmap map on bui.bui_id=map.Bui_id WHERE bui_code='" + warehouse_code + "'");
            if (dt != null && dt.Rows.Count > 0)
                return Convert.ToString(dt.Rows[0]["bui_WarehouseID"]) + "~" + Convert.ToString(dt.Rows[0]["Branch_id"]);
            else
                return "0~0";
        }

        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileUploadMain.PostedFile != null)
                {


                    if (FileUploadMain.HasFile)
                    {
                        string FileName = Path.GetFileName(FileUploadMain.PostedFile.FileName);
                        string Extension = Path.GetExtension(FileUploadMain.PostedFile.FileName);
                        string FolderPath = ConfigurationManager.AppSettings["FolderPath"];
                        string FilePath = Server.MapPath("~/CommonFolder/ProductCostMarketPrice/") + FileName;
                        FileUploadMain.SaveAs(FilePath);
                        GetDataFromExcelMain(FilePath, Extension, "No");

                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('Please select a file to proceed.')</script>");
                    }

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('Please select a file to proceed.')</script>");
                }

                //Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>UploadComplete()</script>");
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('" + ex.Message.Replace("'", "\"") + "')</script>");
            }
        }

        protected void gridFinancer_DataBinding(object sender, EventArgs e)
        {
            if (Session["tblStucture"] != null)
            {
                DataTable dt = (DataTable)Session["tblStucture"];
                int i = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    dr["SL"] = i;
                    i = i + 1;
                }

                dt.AcceptChanges();
                gridFinancer.DataSource = dt;


            }
        }

    }

    public class DynamicMappingDetails
    {
        public string ID { get; set; }
        public string Caption { get; set; }
        public string FeildName { get; set; }
        public string Desc { get; set; }
        public string Excel_Map { get; set; }
        public string Mandatory { get; set; }
        public List<Excel_Map_Table> Excel_Table { get; set; }

    }

    public class Excel_Map_Table
    {
        public string ValueFeild { get; set; }
        public string TextFeild { get; set; }

    }

    public class Output_Map
    {
        public string val { get; set; }
        public string text { get; set; }

    }

    public class Map
    {
        public string ID { get; set; }
        public string Map_ID { get; set; }

    }
}