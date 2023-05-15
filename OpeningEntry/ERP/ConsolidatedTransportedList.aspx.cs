//================================================== Revision History ==========================================================================
//1.0  12-05-2023    V2.0.38    Priti  25892 : Import module required for Consolidated Transporter Opening.
//====================================================== Revision History ======================================================================

using OpeningBusinessLogic;
using BusinessLogicLayer.Replacement;
using DevExpress.Web;
using DevExpress.Web.Data;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using OpeningBusinessLogic.Transporterconsolidate;
//Rev 1.0
//using static DevExpress.Data.Helpers.ExpressiveSortInfo;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text.RegularExpressions;
using DataAccessLayer;
using System.Web.Services.Description;
using BusinessLogicLayer;
using System.Web.Mvc;
using System.IO;
//using System.Text.RegularExpressions;
//using Color = System.Drawing.Color;
//Rev 1.0 End

namespace OpeningEntry.ERP
{
    public partial class ConsolidatedTransportedList : System.Web.UI.Page
    {
        DataTable dst = new DataTable();
        string strBranchID = "";
        TransporterConsolidate obj = new TransporterConsolidate();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        //Rev 1.0
        private static String path, path1, FileName, s, time, cannotParse;
        string FilePath = "";
        public string[] InputName = new string[20];
        public string[] InputType = new string[20];
        public string[] InputValue = new string[20];
        //Rev 1.0 End
        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/ERP/ConsolidatedTransportedList.aspx");
            if (!IsPostBack)
            {
                Session["SI_ComponentDataTagged"] = null;
                Branchpopulate();
                Grdconsolidatecustomer.DataSource = GetConsolidatedCustomerListGridData();
                Grdconsolidatecustomer.DataBind();
            }
        }



        #region ########  Branch Populate  #######
        protected void Branchpopulate()
        {
            string userbranchID = Convert.ToString(Session["userbranchID"]);
            dst = obj.GetBranch(Convert.ToInt32(HttpContext.Current.Session["userbranchID"]), Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));

            if (dst.Rows.Count > 0)
            {

                ddl_Branch.DataSource = dst;
                ddl_Branch.DataTextField = "branch_code";
                ddl_Branch.DataValueField = "branch_id";
                ddl_Branch.DataBind();
                // ddl_Branch.SelectedValue = strBranchID;

                if (Cache["name_vendor"] != null)
                {
                    ddl_Branch.SelectedValue = Cache["name_vendor"].ToString();
                }
                else if (Session["userbranchID"] != null)
                {
                    ddl_Branch.SelectedValue = userbranchID;
                }
            }
        }

        #endregion
        public DataTable GetConsolidatedCustomerListGridData()
        {
            try
            {

                DataTable dt = obj.GetCustomesconsolidate("ListWiseCustomer", Int32.Parse(ddl_Branch.SelectedValue));
                return dt;
            }
            catch
            {
                return null;
            }

        }
        protected void GrdConsolidatedCustomer_DataBinding(object sender, EventArgs e)
        {
            Grdconsolidatecustomer.DataSource = GetConsolidatedCustomerListGridData();
        }

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
               //     Session["exportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                //    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
            }
        }
        public void bindexport(int Filter)
        {
            //GrdReplacement.Columns[6].Visible = false;
            string filename = "ConsolidatedTransporter";
            exporter.FileName = filename;
            exporter.FileName = "ConsolidatedTransporter";

            exporter.PageHeader.Left = "Consolidated Transporter";
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


        protected void OpeningGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (Cache["name_vendor"] != null)
            {
                Cache.Remove("name_vendor");
            }
            string returnPara = Convert.ToString(e.Parameters);
            string WhichCall = returnPara.Split('~')[0];
            if (WhichCall == "TemporaryData")
            {
                Grdconsolidatecustomer.DataSource = GetConsolidatedCustomerListGridData();
                Grdconsolidatecustomer.DataBind();

            }
        }


        #region Tagged documents
        protected void OpeningGrid_CustomCallbacktaggeddoc(object sourc, ASPxGridViewCustomCallbackEventArgs e)
        {

            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameters.Split('~')[0] == "BindComponentGrid")
            {

                string CustomerId = e.Parameters.Split('~')[1];



                DataTable dt = obj.GetCustomesconsolidateTagged("TaggedDocument", CustomerId);

                if (dt.Rows.Count > 0)
                {

                    Session["SI_ComponentDataTagged"] = dt;

                    grid_taggeddocuments.DataSource = dt;
                    grid_taggeddocuments.DataBind();

                }
                else
                {
                    Session["SI_ComponentDataTagged"] = null;
                    grid_taggeddocuments.DataSource = null;
                    grid_taggeddocuments.DataBind();

                }
            }
        }

        protected void GrdConsolidatedtagged_DataBinding(object sender, EventArgs e)
        {
            //   DataTable ComponentTable = new DataTable();

            if (Session["SI_ComponentDataTagged"] != null)
            {
                grid_taggeddocuments.DataSource = (DataTable)Session["SI_ComponentDataTagged"];
            }
        }
        #endregion

        //Rev 1.0
        #region Import
        protected void BtnSaveexcel_Click(object sender, EventArgs e)
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

                //var dir = @"Import";  // folder location

                //if (!Directory.Exists(dir))  // if it doesn't exist, create
                //{
                //    Directory.CreateDirectory(dir);

                //}

                string subPath = "Import"; // Your code goes here

                bool exists = System.IO.Directory.Exists(Server.MapPath(subPath));

                if (!exists)
                    System.IO.Directory.CreateDirectory(Server.MapPath(subPath));

                String UploadPath = Server.MapPath((Convert.ToString("Import/") + Session["FileName"].ToString()));
                OFDBankSelect.PostedFile.SaveAs(UploadPath);

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
                if (!HasLog)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Please flow the log file.!'); ShowLogData('" + HasLog + "');</script>");

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Import Process Successfully Completed!'); ShowLogData('" + HasLog + "');</script>");

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
            Store_MasterBL oStore_MasterBL = new Store_MasterBL();
            Boolean Success = false;
            Boolean HasLog = false;
            int loopcounter = 1;

            if (file.FileName.Trim() != "")
            {

                if (Extension.ToUpper() == ".XLS" || Extension.ToUpper() == ".XLSX")
                {
                    DataTable dt = new DataTable();

                    using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(FilePath, false))
                    {

                        Sheet sheet = spreadSheetDocument.WorkbookPart.Workbook.Sheets.GetFirstChild<Sheet>();
                        Worksheet worksheet = (spreadSheetDocument.WorkbookPart.GetPartById(sheet.Id.Value) as WorksheetPart).Worksheet;
                        IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();
                        foreach (Row row in rows)
                        {
                            if (row.RowIndex.Value == 1)
                            {
                                foreach (Cell cell in row.Descendants<Cell>())
                                {
                                    if (cell.CellValue != null)
                                    {
                                        dt.Columns.Add(GetValue(spreadSheetDocument, cell));
                                    }
                                }
                            }
                            else
                            {
                                DataRow tempRow = dt.NewRow();
                                int columnIndex = 0;
                                foreach (Cell cell in row.Descendants<Cell>())
                                {
                                    // Gets the column index of the cell with data
                                    int cellColumnIndex = (int)GetColumnIndexFromName(GetColumnName(cell.CellReference));
                                    cellColumnIndex--; //zero based index
                                    if (columnIndex < cellColumnIndex)
                                    {
                                        do
                                        {
                                            tempRow[columnIndex] = ""; //Insert blank data here;
                                            columnIndex++;
                                        }
                                        while (columnIndex < cellColumnIndex);
                                    }
                                    try
                                    {
                                        tempRow[columnIndex] = GetValue(spreadSheetDocument, cell);
                                    }
                                    catch
                                    {
                                        tempRow[columnIndex] = "";
                                    }

                                    columnIndex++;
                                }
                                dt.Rows.Add(tempRow);
                            }
                        }
                    }

                    if (dt != null && dt.Rows.Count > 0)
                    {

                        string Doc_No = string.Empty;
                        foreach (DataRow row in dt.Rows)
                        {
                            loopcounter++;
                            try
                            {
                                string Unit_Name = Convert.ToString(row["Unit*"]);
                                string Doc_Type = Convert.ToString(row["Doc. Type*"]);
                                Doc_No = Convert.ToString(row["Doc. No.*"]);
                                string Doc_Date = Convert.ToString(row["Doc. Date*"]);
                                string Customer_Name = Convert.ToString(row["Vendor/Transporter Name*"]);
                                //string PartyInvNo = Convert.ToString(row["Party Inv. No."]);
                                //string PartyInvDate = Convert.ToString(row["Party Inv. Date"]); 
                                string Doc_Amt = Convert.ToString(row["Doc. Amt.*"]);
                                string Balance_Amount = Convert.ToString(row["Balance Amount*"]);
                                string UserId = Convert.ToString(HttpContext.Current.Session["userid"]);
                                string CompanyID = Convert.ToString(Session["LastCompany"]);
                                string FinYear = Convert.ToString(Session["LastFinYear"]);


                                DataSet dt2 = InsertConsolidatedTransporterDataFromExcel(Unit_Name, Doc_Type, Doc_No, Customer_Name, Doc_Date, Doc_Amt, Balance_Amount, CompanyID, FinYear
                                    //, PartyInvNo, PartyInvDate
                                    );

                                if (dt2 != null && dt2.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow row2 in dt2.Tables[0].Rows)
                                    {
                                        Success = Convert.ToBoolean(row2["Success"]);
                                        HasLog = Convert.ToBoolean(row2["HasLog"]);
                                    }
                                }
                                if (!HasLog)
                                {
                                    string description = Convert.ToString(dt2.Tables[0].Rows[0]["MSG"]);
                                    string CustInternal_Id = Convert.ToString(dt2.Tables[0].Rows[0]["CustInternal_Id"]);

                                    int loginsert = InsertConsolidatedTransporterImportLog(Doc_No, loopcounter, UserId, Session["FileName"].ToString(), description, "Failed", CustInternal_Id);
                                }
                                else
                                {
                                    string description = Convert.ToString(dt2.Tables[0].Rows[0]["MSG"]);
                                    string CustInternal_Id = Convert.ToString(dt2.Tables[0].Rows[0]["CustInternal_Id"]);
                                    int loginsert = InsertConsolidatedTransporterImportLog(Doc_No, loopcounter, UserId, Session["FileName"].ToString(), description, "Success", CustInternal_Id);
                                }

                            }
                            catch (Exception ex)
                            {
                                Success = false;
                                HasLog = false;
                                // string description = Convert.ToString(dt2.Tables[0].Rows[0]["MSG"]);
                                int loginsert = InsertConsolidatedTransporterImportLog(Doc_No, loopcounter, "", Session["FileName"].ToString(), ex.Message.ToString(), "Failed", "");
                            }

                        }
                    }

                }
                else
                {

                }
            }
            return HasLog;
        }
        public DataSet InsertConsolidatedTransporterDataFromExcel(string Unit_Name, string Doc_Type, string Doc_No, string Customer_Name
            , string Doc_Date, string Doc_Amt, string Balance_Amount, string CompanyID, string FinYear
          //  , string PartyInvNo, string PartyInvDate
            )
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_OPENINGENTRIESIMPORTFROMEXCEL");
            proc.AddVarcharPara("@Action", 100, "InsertTransporterDataFromExcel");
            proc.AddVarcharPara("@Unit_Name", 100, Unit_Name);
            proc.AddVarcharPara("@Doc_Type", 200, Doc_Type);
            proc.AddVarcharPara("@Doc_No", 100, Doc_No);
            proc.AddVarcharPara("@Customer_Name", 200, Customer_Name);
            proc.AddVarcharPara("@Doc_Date", 20, Doc_Date);
            proc.AddIntegerPara("@Doc_Amt", Convert.ToInt32(Doc_Amt));
            proc.AddIntegerPara("@Balance_Amount", Convert.ToInt32(Balance_Amount));
            proc.AddVarcharPara("@CompanyID", 100, CompanyID);
            proc.AddVarcharPara("@FinYear", 100, FinYear);
            //proc.AddVarcharPara("@PartyInvNo", 200, PartyInvNo);
            //proc.AddVarcharPara("@PartyInvDate", 20, PartyInvDate);
            ds = proc.GetDataSet();
            return ds;
        }
        public int InsertConsolidatedTransporterImportLog(string Doc_No, int loopnumber, string userid, string filename, string description, string status, string CUSTOMERID)
        {
            int i;
            ProcedureExecute proc = new ProcedureExecute("PRC_OPENINGENTRIESIMPORTLOG");
            proc.AddVarcharPara("@action", 150, "InsertTransporterLog");
            proc.AddVarcharPara("@Doc_No", 200, Doc_No);
            proc.AddVarcharPara("@Doc_TYpe", 100, "Transporter");
            proc.AddIntegerPara("@LoopNumber", loopnumber);
            proc.AddVarcharPara("@UserId", 150, userid);
            proc.AddVarcharPara("@FileName", 150, filename);
            proc.AddVarcharPara("@decription", 150, description);
            proc.AddVarcharPara("@status", 150, status);
            proc.AddVarcharPara("@CUSTOMERID", 150, CUSTOMERID);
            i = proc.RunActionQuery();

            return i;
        }
        public DataSet GetCustomerLog(string Filename)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_OPENINGENTRIESIMPORTLOG");
            proc.AddVarcharPara("@action", 150, "GeCustomerLog");
            proc.AddVarcharPara("@FileName", 150, Filename);
            ds = proc.GetDataSet();
            return ds;
        }
        protected void lnlDownloaderexcel_Click(object sender, EventArgs e)
        {

            string strFileName = "ConsolidatedTransporterOS.xlsx";
            string strPath = (Convert.ToString(System.AppDomain.CurrentDomain.BaseDirectory) + Convert.ToString(ConfigurationManager.AppSettings["SaveFile"]) + strFileName);

            Response.ContentType = "application/xlsx";
            Response.AppendHeader("Content-Disposition", "attachment; filename=ConsolidatedTransporterOS.xlsx");
            Response.TransmitFile(strPath);
            Response.End();
        }

        protected void GvLogSearch_DataBinding(object sender, EventArgs e)
        {
            Store_MasterBL oStore_MasterBL = new Store_MasterBL();
            string fileName = Convert.ToString(Session["FileName"]);
            DataSet dt2 = GetCustomerLog(fileName);
            GvLogSearch.DataSource = dt2.Tables[0];
        }
        #endregion
        //Rev 1.0 End
    }
}