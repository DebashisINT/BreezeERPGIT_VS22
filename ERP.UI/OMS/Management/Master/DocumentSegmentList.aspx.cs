using System;
using System.Data;
using System.Web;
using DevExpress.Web;
using BusinessLogicLayer;
using EntityLayer.CommonELS;
using ClsDropDownlistNameSpace;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using DevExpress.XtraPrinting;
using System.Net.Mime;
using ERP.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Services;
using DataAccessLayer;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using Newtonsoft.Json;
using System.Net.Http;


namespace ERP.OMS.Management.Master
{
    public partial class
        DocumentSegmentList : System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        private static String path, path1, FileName, s, time, cannotParse;
        string FilePath = "";
        public string[] InputName = new string[20];
        public string[] InputType = new string[20];
        public string[] InputValue = new string[20];
        public string[] InputName1 = new string[20];
        public string[] InputType1 = new string[20];
        public string[] InputValue1 = new string[20];

        CommonBL CBL = new CommonBL();

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/Contact_general.aspx");
            if (!IsPostBack)
            {
                string SyncUsertoWhileCreating = CBL.GetSystemSettingsResult("SyncCustomertoFSMWhileCreating");
                hdnSyncCustomertoFSMWhileCreating.Value = SyncUsertoWhileCreating;

                if (Request.QueryString["id"] != "")
                {
                    string CustId = Request.QueryString["id"];
                    hdnCustomerID.Value = CustId;
                }
            }
        }
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "ID";

            //  string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


            //string IsFilter = Convert.ToString(hfIsFilter.Value);
            //string strFromDate = Convert.ToString(hfFromDate.Value);
            //string strToDate = Convert.ToString(hfToDate.Value);
            //string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            // List<int> branchidlist;

            string CustomerID = Convert.ToString(hdnCustomerID.Value);

            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            var q = from d in dc.v_Master_Entity_Segments
                    where d.cnt_id == Convert.ToInt32(CustomerID)
                    //orderby d.Adjustment_Date descending
                    select d;
            e.QueryableSource = q;
        }

        protected void gridSegment_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            gridSegment.JSProperties["cpDelete"] = null;
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            int deletecnt = 0;
            if (Convert.ToString(e.Parameters).Contains("~"))
            {
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                {
                    WhichType = Convert.ToString(e.Parameters).Split('~')[1];

                }
                if (WhichCall == "Delete")
                {
                    deletecnt = DeleteLeadOrContact(WhichType);
                    if (deletecnt > 0)
                    {
                        gridSegment.JSProperties["cpDelete"] = "Document Deleted Successfully";

                    }
                    else if (deletecnt == -99)
                    {
                        gridSegment.JSProperties["cpDelete"] = "Used in other module can not delete.";
                    }
                    else if (deletecnt == -88)
                    {
                        gridSegment.JSProperties["cpDelete"] = "Cannot Delete as this Code is a Parent of another Code.";
                    }
                    else
                        gridSegment.JSProperties["cpDelete"] = "Fail";
                }
            }
            if (WhichCall == "MassDelete")
            {
                string ComponentDetailsIDs = string.Empty;
                for (int i = 0; i < gridSegment.GetSelectedFieldValues("ID").Count; i++)
                {
                    ComponentDetailsIDs += "," + Convert.ToString(gridSegment.GetSelectedFieldValues("ID")[i]);

                }
                ComponentDetailsIDs = ComponentDetailsIDs.TrimStart(',');

                if (ComponentDetailsIDs == "")
                {
                    gridSegment.JSProperties["cpDeleteMessage"] = "Please check atleast one row.";
                }
                else
                {
                    deletecnt = MassDelete(ComponentDetailsIDs);
                    if (deletecnt > 0)
                    {
                        gridSegment.JSProperties["cpDelete"] = "Document Deleted Successfully";

                    }
                    else if (deletecnt == -99)
                    {
                        gridSegment.JSProperties["cpDelete"] = "Used in other module can not delete.";
                    }
                    else if (deletecnt == -88)
                    {
                        gridSegment.JSProperties["cpDelete"] = "Cannot Delete as this Code is a Parent of another Code.";
                    }
                    else
                    {
                        gridSegment.JSProperties["cpDelete"] = "Fail";
                    }
                    gridSegment.Selection.UnselectAll();
                }



            }
        }
        public int DeleteLeadOrContact(string MApID)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_ENTITY_SEGMENT_MAP");
            proc.AddNVarcharPara("@action", 100, "DeleteMaster_Entity_Segment");
            proc.AddNVarcharPara("@Segment_Map_ID", 30, MApID);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }

        public int MassDelete(string IDS)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_ENTITY_SEGMENT_MAP");
            proc.AddNVarcharPara("@action", 100, "MassDeleteMaster_Entity_Segment");
            proc.AddVarcharPara("@SelectedComponentList", 2000, IDS);

            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }

        protected void lnlDownloaderexcel_Click(object sender, EventArgs e)
        {

            string strFileName = "Document Segment Import Format.xlsx";
            string strPath = (Convert.ToString(System.AppDomain.CurrentDomain.BaseDirectory) + Convert.ToString(ConfigurationManager.AppSettings["SaveFile"]) + strFileName);

            Response.ContentType = "application/xlsx";
            Response.AppendHeader("Content-Disposition", "attachment; filename=Document Segment Import Format.xlsx");
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

                if (ChkAutoUniqueCodes.Checked)
                {
                    if (Convert.ToString(txtLength.Text) == "")
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Please enter Length');</script>");
                        return;
                    }
                    if (Convert.ToString(txtPrefix.Text) == "")
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Please enter Prefix');</script>");
                        return;
                    }
                    if (Convert.ToString(txtDigits.Text) == "")
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Please enter No of Digits');</script>");
                        return;
                    }
                    if (Convert.ToString(txtStartNo.Text) == "")
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Please enter Start No');</script>");
                        return;
                    }
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

                String UploadPath = Server.MapPath((Convert.ToString(ConfigurationManager.AppSettings["SaveCSV"]) + Session["FileName"].ToString()));
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

                if (HasLog == false)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Import Process UnSuccessfully Completed!'); ShowLogData('" + HasLog + "');</script>");

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
        public Boolean Import_To_Grid(string FilePath, string Extension, HttpPostedFile file)
        {
            Employee_BL objEmploye = new Employee_BL();
            Boolean Success = false;
            Boolean HasLog = false;
            string internal_id = "";
            int loopcounter = 1;

            if (file.FileName.Trim() != "")
            {

                if (Extension.ToUpper() == ".XLS" || Extension.ToUpper() == ".XLSX")
                {
                    //DataTable dt = new DataTable();

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
                    //             {
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
                    //    }ī
                    DataTable dtExcelData = new DataTable();
                    string conString = string.Empty;
                    conString = ConfigurationManager.AppSettings["ExcelConString"];
                    conString = string.Format(conString, FilePath);
                    using (OleDbConnection excel_con = new OleDbConnection(conString))
                    {
                        excel_con.Open();
                        string sheet1 = "sheet1$"; //ī;

                        dtExcelData.Columns.Add("Segment*", typeof(string));
                        dtExcelData.Columns.Add("Unique ID*", typeof(string));
                        dtExcelData.Columns.Add("Name*", typeof(string));
                        dtExcelData.Columns.Add("Parent", typeof(string));
                        dtExcelData.Columns.Add("GSTIN", typeof(string));
                        dtExcelData.Columns.Add("Billing_Add1*", typeof(string));
                        dtExcelData.Columns.Add("Billing_Add2", typeof(string));
                        dtExcelData.Columns.Add("Billing_Country", typeof(string));
                        dtExcelData.Columns.Add("Billing_State", typeof(string));
                        dtExcelData.Columns.Add("Billing_Dist", typeof(string));
                        dtExcelData.Columns.Add("Billing_Lat", typeof(string));
                        dtExcelData.Columns.Add("Billing_Long", typeof(string));
                        dtExcelData.Columns.Add("Billing_PIN*", typeof(string));
                        dtExcelData.Columns.Add("Billing_Phone*", typeof(string));

                        dtExcelData.Columns.Add("Service_Add1*", typeof(string));
                        dtExcelData.Columns.Add("Service_Add2", typeof(string));
                        dtExcelData.Columns.Add("Service_Country", typeof(string));
                        dtExcelData.Columns.Add("Service_State", typeof(string));
                        dtExcelData.Columns.Add("Service_Dist", typeof(string));
                        dtExcelData.Columns.Add("Service_Lat", typeof(string));
                        dtExcelData.Columns.Add("Service_Long", typeof(string));
                        dtExcelData.Columns.Add("Service_PIN*", typeof(string));
                        dtExcelData.Columns.Add("Service_Phone*", typeof(string));
                        dtExcelData.Columns.Add("Treatment_Area", typeof(string));

                        dtExcelData.Columns.Add("ServiceBranch", typeof(string));

                        using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", excel_con))
                        {
                            oda.Fill(dtExcelData);
                        }
                        excel_con.Close();
                    }
                    //  }

                    //if (dt != null && dt.Rows.Count > 0)
                    if (dtExcelData != null && dtExcelData.Rows.Count > 0)
                    {

                        // foreach (DataRow row in dt.Rows)
                        foreach (DataRow row in dtExcelData.Rows)
                        {
                            loopcounter++;
                            DataSet dt2 = new DataSet();
                            try
                            {
                                string Segment = Convert.ToString(row["Segment*"]);
                                if (Segment != "")
                                {


                                    string CustomerId = hdnCustomerID.Value;
                                    // string Segment = Convert.ToString(row["Segment*"]);
                                    string UniqueID = "";
                                    if (ChkAutoUniqueCodes.Checked)
                                    {
                                        UniqueID = "Auto";
                                    }
                                    else
                                    {
                                        UniqueID = Convert.ToString(row["Unique ID*"]);
                                    }

                                    string Name = Convert.ToString(row["Name*"]);
                                    string Parent = Convert.ToString(row["Parent"]);
                                    string GSTIN = Convert.ToString(row["GSTIN"]);
                                    string PANValue = "";
                                    string contactperson = "";
                                    string Billing_Add1 = Convert.ToString(row["Billing_Add1*"]);
                                    string Billing_Add2 = Convert.ToString(row["Billing_Add2"]);
                                    string Billing_Country = Convert.ToString(row["Billing_Country"]);
                                    string Billing_State = Convert.ToString(row["Billing_State"]);
                                    string Billing_Dist = Convert.ToString(row["Billing_Dist"]);
                                    string Billing_Lat = Convert.ToString(row["Billing_Lat"]);
                                    string Billing_Long = Convert.ToString(row["Billing_Long"]);
                                    string Billing_PIN = Convert.ToString(row["Billing_PIN*"]);
                                    string Billing_Phone = Convert.ToString(row["Billing_Phone*"]);

                                    string Service_Add1 = Convert.ToString(row["Service_Add1*"]);
                                    string Service_Add2 = Convert.ToString(row["Service_Add2"]);
                                    string Service_Country = Convert.ToString(row["Service_Country"]);
                                    string Service_State = Convert.ToString(row["Service_State"]);
                                    string Service_Dist = Convert.ToString(row["Service_Dist"]);
                                    string Service_Lat = Convert.ToString(row["Service_Lat"]);
                                    string Service_Long = Convert.ToString(row["Service_Long"]);
                                    string Service_PIN = Convert.ToString(row["Service_PIN*"]);
                                    string Service_Phone = Convert.ToString(row["Service_Phone*"]);
                                    string Treatment_Area = Convert.ToString(row["Treatment_Area"]);
                                    string ServiceBranch = Convert.ToString(row["ServiceBranch"]);


                                    string user_id = Convert.ToString(HttpContext.Current.Session["userid"]);


                                    ProcedureExecute proc = new ProcedureExecute("prc_DocumentSegmentAddEdit");

                                    proc.AddVarcharPara("@Action", 200, "AutoAdd");
                                    proc.AddVarcharPara("@InternalID", 200, CustomerId);
                                    proc.AddVarcharPara("@SegmentID", 200, Segment);
                                    proc.AddVarcharPara("@ParentSegmentID", 200, Parent);
                                    proc.AddVarcharPara("@Code", 200, UniqueID);
                                    proc.AddVarcharPara("@Name", 200, Name);
                                    proc.AddVarcharPara("@Pan", 200, PANValue);
                                    proc.AddVarcharPara("@contactperson", 200, contactperson);
                                    proc.AddVarcharPara("@Address1", 200, Billing_Add1);
                                    proc.AddVarcharPara("@Address2", 200, Billing_Add2);
                                    proc.AddVarcharPara("@CountryID", 200, Billing_Country);
                                    proc.AddVarcharPara("@StateID", 200, Billing_State);
                                    proc.AddVarcharPara("@DistrictID", 200, Billing_Dist);
                                    proc.AddVarcharPara("@PincodeID", 200, Billing_PIN);
                                    proc.AddVarcharPara("@GSTIN", 200, GSTIN);
                                    proc.AddVarcharPara("@shippingAddress1", 200, Service_Add1);
                                    proc.AddVarcharPara("@shippingAddress2", 200, Service_Add2);
                                    proc.AddVarcharPara("@ServiceCountryID", 200, Service_Country);
                                    proc.AddVarcharPara("@ServiceStateID", 200, Service_State);
                                    proc.AddVarcharPara("@ServiceCityID", 200, Service_Dist);
                                    proc.AddVarcharPara("@ServicePinID", 200, Service_PIN);
                                    proc.AddVarcharPara("@BillingLatitude", 200, Billing_Lat);
                                    proc.AddVarcharPara("@BillingLongitude", 200, Billing_Long);
                                    proc.AddVarcharPara("@ServiceLatitude", 200, Service_Lat);
                                    proc.AddVarcharPara("@ServiceLongitude", 200, Service_Long);
                                    proc.AddVarcharPara("@BillPhoneNo", 200, Billing_Phone);
                                    proc.AddVarcharPara("@ServicePhoneNo", 200, Service_Phone);
                                    proc.AddVarcharPara("@userId", 200, Convert.ToString(HttpContext.Current.Session["userid"]));
                                    proc.AddVarcharPara("@FinYear", 200, HttpContext.Current.Session["LastFinYear"].ToString());
                                    proc.AddVarcharPara("@CompanyID", 200, HttpContext.Current.Session["LastCompany"].ToString());

                                    proc.AddVarcharPara("@Length", 200, txtLength.Text);
                                    proc.AddVarcharPara("@Prefix", 200, txtPrefix.Text);
                                    proc.AddVarcharPara("@Digits", 200, txtDigits.Text);
                                    proc.AddVarcharPara("@StartNo", 200, txtStartNo.Text);
                                    proc.AddVarcharPara("@TreatmentArea", 100, Treatment_Area);
                                    proc.AddVarcharPara("@ServiceBranch", 100, ServiceBranch);


                                    dt2 = proc.GetDataSet();
                                    // return ds;



                                    if (dt2 != null && dt2.Tables[0].Rows.Count > 0)
                                    {
                                        foreach (DataRow row2 in dt2.Tables[0].Rows)
                                        {

                                            Success = Convert.ToBoolean(row2["Success"]);
                                            HasLog = Convert.ToBoolean(row2["HasLog"]);
                                            internal_id = Convert.ToString(row2["internal_id"]);
                                        }
                                    }

                                    //if (!HasLog)
                                    if (HasLog == false)
                                    {
                                        string description = Convert.ToString(dt2.Tables[0].Rows[0]["MSG"]);
                                        int loginsert = InsertSegmentImportLOg(internal_id, loopcounter, Name, user_id, Session["FileName"].ToString(), description, "Failed");
                                    }

                                    else
                                    {
                                        string description = Convert.ToString(dt2.Tables[0].Rows[0]["MSG"]);
                                        int loginsert = InsertSegmentImportLOg(internal_id, loopcounter, Name, user_id, Session["FileName"].ToString(), description, "Success");
                                        //add for Segment Sync Tanmoy
                                        if (hdnSyncCustomertoFSMWhileCreating.Value == "Yes")
                                        {
                                            segmentsynctoFSM(Convert.ToInt32(dt2.Tables[0].Rows[0]["ReturnId"]), Segment, internal_id);
                                        }
                                        //add for Segment Sync Tanmoy
                                    }

                                }

                            }
                            catch (Exception ex)
                            {
                                Success = false;
                                HasLog = false;
                                string description = Convert.ToString(dt2.Tables[0].Rows[0]["MSG"]);
                                int loginsert = InsertSegmentImportLOg(UniqueID, loopcounter, "", "", Session["FileName"].ToString(), ex.Message.ToString(), "Failed");
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
        public int InsertSegmentImportLOg(string empcode, int loopnumber, string empname, string userid, string filename, string description, string status)
        {

            int i;
            //int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_DocumentSegmentAddEdit");
            proc.AddVarcharPara("@action", 150, "insertlog");
            proc.AddVarcharPara("@EmpCode", 50, empcode);
            proc.AddIntegerPara("@LoopNumber", loopnumber);
            proc.AddVarcharPara("@EmpName", 150, empname);
            proc.AddVarcharPara("@userId", 150, userid);
            proc.AddVarcharPara("@FileName", 150, filename);
            proc.AddVarcharPara("@decription", 150, description);
            proc.AddVarcharPara("@status", 150, status);
            i = proc.RunActionQuery();

            return i;
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
        private string GetValue(SpreadsheetDocument doc, Cell cell)
        {
            string value = cell.CellValue.InnerText;
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return doc.WorkbookPart.SharedStringTablePart.SharedStringTable.ChildElements.GetItem(int.Parse(value)).InnerText;
            }
            return value;
        }
        protected void GvJvSearch_DataBinding(object sender, EventArgs e)
        {
            Employee_BL objEmploye = new Employee_BL();
            string fileName = Convert.ToString(Session["FileName"]);
            string CustomerId = hdnCustomerID.Value;
            DataSet dt2 = GetSegmentLog(fileName, CustomerId);
            GvJvSearch.DataSource = dt2.Tables[0];

        }
        public DataSet GetSegmentLog(string Filename, string CustomerId)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_DocumentSegmentAddEdit");
            proc.AddVarcharPara("@action", 150, "getEmployeeLog");
            proc.AddVarcharPara("@FileName", 150, Filename);
            proc.AddVarcharPara("@InternalID", 150, CustomerId);
            ds = proc.GetDataSet();
            return ds;
        }
        public static string GetColumnName(string cellReference)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);

            return match.Value;
        }

        public static int segmentsynctoFSM(Int32 SegmentID, string SegmentType, String cnt_id)
        {
            String weburl = System.Configuration.ConfigurationSettings.AppSettings["FSMAPIBaseUrl"];
            string apiUrl = weburl + "ShopRegisterPortal/CustomerSyncinShop";
            RegisterShopOutput oview = new RegisterShopOutput();
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
            RegisterShopInputPortal empDtls = new RegisterShopInputPortal();
            int i = 0;
            DataTable dt = new DataTable();
            ProcedureExecute proc2 = new ProcedureExecute("PRC_SegmentDetailsSyncInCreation");
            proc2.AddPara("@SegmentID", SegmentID);
            proc2.AddPara("@SegmentType", SegmentType);
            dt = proc2.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    String Status = "Failed";
                    String FailedReason = "";

                    if (Convert.ToString(item["IsCustomerSync"]) == "1")
                    {
                        //if (Convert.ToString(item["IsSeg1Sync"]) == "1" && SegmentType != 1)
                        //{
                        DateTime date1 = DateTime.Parse("1970-01-01");
                        DateTime date2 = System.DateTime.Now;
                        var Difference_In_Time = Convert.ToString((int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                        var middle = (Math.Round(Convert.ToDecimal(Difference_In_Time) / 1000) * 1155) + 1;

                        empDtls.session_token = "zksjfhjsdjkskjdh";
                        empDtls.user_id = Convert.ToString(378);
                        empDtls.shop_name = Convert.ToString(item["PartyName"]);
                        empDtls.address = Convert.ToString(item["ADDRESS1"]);
                        empDtls.pin_code = Convert.ToString(item["PinCode"]);
                        empDtls.shop_lat = Convert.ToString(item["PartyLocationLat"]);
                        empDtls.shop_long = Convert.ToString(item["PartyLocationLong"]);
                        empDtls.owner_name = Convert.ToString(item["Owner"]);
                        empDtls.owner_contact_no = Convert.ToString(item["Contact"]);
                        empDtls.owner_email = Convert.ToString(item["Email"]);
                        empDtls.type = Convert.ToInt32(item["Type"]);
                        empDtls.dob = Convert.ToString(item["DOB"]);
                        empDtls.date_aniversary = Convert.ToString(item["Anniversary"]);
                        empDtls.shop_id = Convert.ToString(item["AssignToUser"]) + "_" + Convert.ToString(Difference_In_Time);
                        empDtls.added_date = Convert.ToString(System.DateTime.Now);
                        empDtls.assigned_to_pp_id = Convert.ToString(item["assigned_to_pp_id"]); ;
                        empDtls.assigned_to_dd_id = Convert.ToString(item["assigned_to_dd_id"]); ;
                        empDtls.EntityCode = Convert.ToString(item["PartyCode"]);
                        empDtls.Entity_Location = Convert.ToString(item["Location"]);
                        empDtls.Alt_MobileNo = Convert.ToString(item["AlternateContact"]);
                        empDtls.Entity_Status = Convert.ToString(item["Status"]);
                        empDtls.Entity_Type = Convert.ToString(item["EntityCategory"]);
                        empDtls.ShopOwner_PAN = Convert.ToString(item["OwnerPAN"]);
                        empDtls.ShopOwner_Aadhar = Convert.ToString(item["OwnerAadhaar"]);
                        empDtls.Remarks = Convert.ToString(item["Remarks"]);
                        empDtls.AreaId = Convert.ToString(item["Area"]);
                        empDtls.CityId = Convert.ToString(item["District"]);
                        empDtls.Entered_by = Convert.ToString(userid);
                        empDtls.retailer_id = Convert.ToString("0");
                        empDtls.dealer_id = Convert.ToString("0");
                        empDtls.entity_id = Convert.ToString("0");
                        empDtls.party_status_id = Convert.ToString(item["PartyStatus"]);
                        empDtls.beat_id = Convert.ToString(item["GroupBeat"]);
                        empDtls.IsServicePoint = Convert.ToString(dt.Rows[0]["IsServicePoint"]);

                        string data = JsonConvert.SerializeObject(empDtls);

                        HttpClient httpClient = new HttpClient();
                        MultipartFormDataContent form = new MultipartFormDataContent();
                        //byte[] fileBytes = new byte[1];
                        //var fileContent = new StreamContent(null);
                        form.Add(new StringContent(data), "data");
                        //form.Add(emp, "shop_image", null);
                        var result = httpClient.PostAsync(apiUrl, form).Result;

                        oview = JsonConvert.DeserializeObject<RegisterShopOutput>(result.Content.ReadAsStringAsync().Result);

                        //oview.status = "200";

                        if (Convert.ToString(oview.status) == "200")
                        {
                            Status = "Success";
                        }
                        else if (Convert.ToString(oview.status) == "202")
                        {
                            FailedReason = "Customer Name Not found";
                        }
                        else if (Convert.ToString(oview.status) == "203")
                        {
                            FailedReason = "Entity Code not found";
                        }
                        else if (Convert.ToString(oview.status) == "204")
                        {
                            FailedReason = "Owner Name Not found";
                        }
                        else if (Convert.ToString(oview.status) == "205")
                        {
                            FailedReason = "Customer Address not found";
                        }
                        else if (Convert.ToString(oview.status) == "206")
                        {
                            FailedReason = "Pin Code not found";
                        }
                        else if (Convert.ToString(oview.status) == "207")
                        {
                            FailedReason = "Customer Contact number not found";
                        }
                        else if (Convert.ToString(oview.status) == "208")
                        {
                            FailedReason = "User or session token not matched";
                        }
                        else if (Convert.ToString(oview.status) == "209")
                        {
                            FailedReason = "Duplicate Customer Id or contact number";
                        }
                        else if (Convert.ToString(oview.status) == "210")
                        {
                            FailedReason = "Duplicate contact number";
                        }
                        //}
                        //else
                        //{
                        //    FailedReason = "Parent segment not Sync";
                        //}
                    }
                    else
                    {
                        FailedReason = "Customer not Sync";
                    }


                    ProcedureExecute proc1 = new ProcedureExecute("PRC_SegmentDetailsForSync");
                    proc1.AddPara("@ACTION", "UpdateSegment");
                    proc1.AddPara("@ContactID", cnt_id);
                    proc1.AddPara("@SegemntName", Convert.ToString(item["PartyName"]));
                    proc1.AddPara("@SegemntAddress", Convert.ToString(item["ADDRESS1"]));
                    proc1.AddPara("@SegemntPhone", Convert.ToString(item["Contact"]));
                    proc1.AddPara("@SyncBy", userid);
                    proc1.AddPara("@Status", Status);
                    proc1.AddPara("@FailedReason", FailedReason);
                    proc1.AddPara("@Shop_Code", empDtls.shop_id);
                    proc1.AddPara("@SegmentID", Convert.ToString(item["ID"]));
                    proc1.AddPara("@SegmentCode", Convert.ToString(item["PartyCode"]));
                    proc1.AddPara("@InternalID", Convert.ToString(item["InternalID"]));
                    i = proc1.RunActionQuery();
                }
            }
            return i;
        }

        public class RegisterShopInputPortal
        {
            public string session_token { get; set; }
            //[Required]
            public string user_id { get; set; }
            //[Required]
            public string shop_name { get; set; }
            //[Required]
            public string address { get; set; }
            //[Required]
            public string pin_code { get; set; }
            //[Required]
            public string shop_lat { get; set; }
            //[Required]
            public string shop_long { get; set; }
            //[Required]
            public string owner_name { get; set; }
            //[Required]
            public string owner_contact_no { get; set; }
            //[Required]
            public string owner_email { get; set; }
            public int? type { get; set; }
            public string dob { get; set; }
            public string date_aniversary { get; set; }
            public string shop_id { get; set; }
            public string added_date { get; set; }
            public string assigned_to_pp_id { get; set; }
            public string assigned_to_dd_id { get; set; }
            public string amount { get; set; }
            public Nullable<DateTime> family_member_dob { get; set; }
            public string director_name { get; set; }
            public string key_person_name { get; set; }
            public string phone_no { get; set; }
            public Nullable<DateTime> addtional_dob { get; set; }
            public Nullable<DateTime> addtional_doa { get; set; }
            public Nullable<DateTime> doc_family_member_dob { get; set; }
            public string specialization { get; set; }
            public string average_patient_per_day { get; set; }
            public string category { get; set; }
            public string doc_address { get; set; }
            public string doc_pincode { get; set; }
            public string is_chamber_same_headquarter { get; set; }
            public string is_chamber_same_headquarter_remarks { get; set; }
            public string chemist_name { get; set; }
            public string chemist_address { get; set; }
            public string chemist_pincode { get; set; }
            public string assistant_name { get; set; }
            public string assistant_contact_no { get; set; }
            public Nullable<DateTime> assistant_dob { get; set; }
            public Nullable<DateTime> assistant_doa { get; set; }
            public Nullable<DateTime> assistant_family_dob { get; set; }
            public string EntityCode { get; set; }
            public string Entity_Location { get; set; }
            public string Alt_MobileNo { get; set; }
            public string Entity_Status { get; set; }
            public string Entity_Type { get; set; }
            public string ShopOwner_PAN { get; set; }
            public string ShopOwner_Aadhar { get; set; }
            public string Remarks { get; set; }
            public string AreaId { get; set; }
            public string CityId { get; set; }
            public string Entered_by { get; set; }
            public string entity_id { get; set; }
            public string party_status_id { get; set; }
            public string retailer_id { get; set; }
            public string dealer_id { get; set; }
            public string beat_id { get; set; }
            public string IsServicePoint { get; set; }
        }

        public class RegisterShopOutput
        {
            public string status { get; set; }
            public string message { get; set; }
            public string session_token { get; set; }
        }
    }
}