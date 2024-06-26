﻿#region =======================Revision History=========================================================================================================
//1.0   v2 .0.42    Debashis    26/03/2024  Customer code column is required in various reports.Refer: 0027273
#endregion=======================End Revision History====================================================================================================
using DevExpress.Web;
using DevExpress.Web.Mvc;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using System.Collections.Specialized;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using DevExpress.Data;
using System.Threading.Tasks;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Drawing;
using Reports.Model;

namespace Reports.Reports.GridReports
{
    public partial class Customer_Ledger : System.Web.UI.Page
    {
        //DataTable DTIndustry = new DataTable();
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        string data = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CommonBL cbl = new CommonBL();
        //Rev Debashis Implement LINQ
        decimal TotalDebit = 0, TotalCredit = 0, TotalDBCR=0;
        //End of Rev Debashis Implement LINQ

        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (Request.QueryString.AllKeys.Contains("From"))
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/PopUp.Master";
            }
            else
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/ERP.Master";
            }
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/Customer_Ledger.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    lookup_project.Visible = true;
                    lblProj.Visible = true;
                    //Rev 1.0 Mantis: 0027273
                    //ShowGrid.Columns[4].Visible = true;
                    ShowGrid.Columns[5].Visible = true;
                    //End of Rev 1.0 Mantis: 0027273
                    hdnProjectSelection.Value = "1";

                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    lookup_project.Visible = false;
                    lblProj.Visible = false;
                    //Rev 1.0 Mantis: 0027273
                    //ShowGrid.Columns[4].Visible = false;
                    ShowGrid.Columns[5].Visible = false;
                    //End of Rev 1.0 Mantis: 0027273
                    hdnProjectSelection.Value = "0";
                }
            }
            if (!IsPostBack)
            {
                DataTable dtBranchSelection = new DataTable();
                DataTable dtProjectSelection = new DataTable();
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Customer Ledger";
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, false, false, false, false, false);
                CompName.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, true, false, false, false, false);
                CompAdd.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, true, false, false, false);
                CompOth.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, true, false, false);
                CompPh.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, false, false, true);
                CompAccPrd.Text = GridHeader;

                //Rev Debashis Implement LINQ
                //Session["SI_ComponentData"] = null;
                Session["SI_ComponentData_Branch"] = null;
                //Session["dt_PartyLedgerRpt"] = null;
                Session["IsCustFilter"] = null;
                //End of Rev Debashis Implement LINQ
                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                //BindDropDownList();
                ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");
                //Date_finyearwise(Convert.ToString(Session["LastFinYear"]));

                Session["GROUPSELECTCL"] = null;

                ASPxFromDate.Value = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;

                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                string TODATE = dtTo.ToString("yyyy-MM-dd");

                //string TABLENAME = "Ledger Details";

                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }

                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                //Rev Subhra 18-12-2018   0017670
                Session["BranchNames"] = null;
                //End Rev
                dtBranchSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsBranchSelection'");
                hdnBranchSelection.Value = dtBranchSelection.Rows[0][0].ToString();

                dtProjectSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsProjectSelection'");
                hdnProjectSelectionInReport.Value = dtProjectSelection.Rows[0][0].ToString();

                //string CASHBANKTYPE = "";
                //string CASHBANKID = "";
                //string UserId = "";
                string CUSTVENDID = "";

                if (hdnCustomerId.Value != "")
                {
                    CUSTVENDID = hdnCustomerId.Value;
                }

                ShowGrid.Columns[5].Visible = false;

                // GetSalesRegisterdata(FROMDATE, TODATE, BRANCH_ID, CUSTVENDID);

            }
            else
            {
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
            }
            //Rev Debashis Implement LINQ
            //BindLedgerPosting();
            //End of Rev Debashis Implement LINQ
            //String callbackScript = "function CallServer(arg, context){ " + "" + ";}";
            // Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);

        }
        //protected void Page_PreInit(object sender, EventArgs e) // lead add
        //{
        //    if (!IsPostBack)
        //    {
        //        string sPath = Convert.ToString(HttpContext.Current.Request.Url);
        //        oDBEngine.Call_CheckPageaccessebility(sPath);
        //    }
        //}

        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {


                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    // BindDropDownList();
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    // BindDropDownList();
                    bindexport(Filter);
                }

                //drdExport.SelectedValue = "0";
            }

        }
        
        public void BindDropDownList()
        {
            // Declare a Dictionary to hold all the Options with Value and Text.
            Dictionary<string, string> options = new Dictionary<string, string>();
            options.Add("0", "Export to");
            options.Add("1", "PDF");
            options.Add("2", "XLS");
            options.Add("3", "RTF");
            options.Add("4", "CSV");


            // Bind the Dictionary to the DropDownList.
            drdExport.DataSource = options;
            drdExport.DataTextField = "value";
            drdExport.DataValueField = "key";
            drdExport.DataBind();
            //drdExport.SelectedValue = "0";
        }
        public void bindexport(int Filter)
        {
            string filename = "Customerledger";
            exporter.FileName = filename;
            string FileHeader = "";

            exporter.FileName = filename;

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            //Rev Debashis && Implement Party name for export in Excel
            //FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true) + Environment.NewLine + "Customer ledger Report" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true,true, true, true, true) + Environment.NewLine + "Customer ledger " + "A/C- " + Session["CUSTNM"] + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            //End of Rev Debashis && Implement Party name for export in Excel

            //Rev Subhra 18-12-2018   0017670
            FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
            //End of Rev

            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";

            //exporter.PageFooter.Center = "[Page # of Pages #]";
            //exporter.PageFooter.Right = "[Date Printed]";
            exporter.Landscape = true;
            exporter.MaxColumnWidth = 100;
            exporter.GridViewID = "ShowGrid";

            exporter.RenderBrick += exporter_RenderBrick;

            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();

                    break;
                case 2:
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }

        }
        //Rev Subhra 18-12-2018   0017670
        public string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + Environment.NewLine + replace + Environment.NewLine + text.Substring(pos + search.Length);
        }
        //End of Rev

        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }
        //Rev Debashis Implement LINQ
        //protected void BindLedgerPosting()
        //{
        //    try
        //    {
        //        if (Session["dt_PartyLedgerRpt"] != null)
        //        {
        //            ShowGrid.DataSource = (DataTable)Session["dt_PartyLedgerRpt"];
        //            ShowGrid.DataBind();
        //        }
        //    }
        //    catch { }
        //}
        //protected void Grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        //{

        //    Session.Remove("dt_PartyLedgerRpt");


        //    ShowGrid.JSProperties["cpSave"] = null;


        //    string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
        //    string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

        //    DateTime dtFrom;
        //    DateTime dtTo;
        //    dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
        //    dtTo = Convert.ToDateTime(ASPxToDate.Date);

        //    string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
        //    string TODATE = dtTo.ToString("yyyy-MM-dd");
        //    string BRANCH_ID = "";

        //    string QuoComponent2 = "";
        //    List<object> QuoList2 = lookup_branch.GridView.GetSelectedFieldValues("ID");
        //    foreach (object Quo2 in QuoList2)
        //    {
        //        QuoComponent2 += "," + Quo2;
        //    }
        //    BRANCH_ID = QuoComponent2.TrimStart(',');
        //    string CUSTVENDID = "";
        //    string QuoComponent = "";
        //    List<object> QuoList = lookup_quotation.GridView.GetSelectedFieldValues("ID");
        //    foreach (object Quo in QuoList)
        //    {
        //        QuoComponent += "," + Quo;
        //    }
        //    CUSTVENDID = QuoComponent.TrimStart(',');
        //    Task PopulateStockTrialDataTask = new Task(() => GetSalesRegisterdata(FROMDATE, TODATE, BRANCH_ID, CUSTVENDID));
        //    PopulateStockTrialDataTask.RunSynchronously();
        //}
        //End of Rev Debashis Implement LINQ

        public void GetCustomerLedgerdata(string FROMDATE, string TODATE, string BRANCH_ID, string CUSTVENDID, string PROJECT_ID)
        {
            try
            {
                //Rev Debashis Implement LINQ
                //string DriverName = string.Empty;
                //string PhoneNo = string.Empty;
                //string VehicleNo = string.Empty;
                //End of Rev Debashis Implement LINQ
                DataSet ds = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_PartyLedgerCustVend_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]).Trim());
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@BRANCHID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@P_CODE", CUSTVENDID);
                cmd.Parameters.AddWithValue("@Customer_Type", "CL");
                //cmd.Parameters.AddWithValue("@Group_Code", "");
                cmd.Parameters.AddWithValue("@Group_Code", hdnSelectedGroups.Value);
                cmd.Parameters.AddWithValue("@EXCLUDE_CASH", (chkcash.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@P_INVOICE_DATE", "0");
                cmd.Parameters.AddWithValue("@CHKALLCUSTVEND", (chkallcustomers.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@PROJECT_ID", PROJECT_ID);
                //Rev Debashis Implement LINQ
                cmd.Parameters.AddWithValue("@USERID", Convert.ToString(Session["userid"]).Trim());
                //End of Rev Debashis Implement LINQ
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();
                //Rev Debashis Implement LINQ
                //Session["dt_PartyLedgerRpt"] = ds.Tables[0];
                //ShowGrid.DataSource = ds.Tables[0];
                //ShowGrid.DataBind();
                //End of Rev Debashis Implement LINQ
            }
            catch (Exception ex)
            {
            }
        }

        //Rev Debashis Implement LINQ
        //[WebMethod]
        //public static List<string> GetBranchesList(String NoteId)
        //{
        //    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        //    StringBuilder filter = new StringBuilder();
        //    StringBuilder Supervisorfilter = new StringBuilder();
        //    BusinessLogicLayer.Others objbl = new BusinessLogicLayer.Others();
        //    DataTable dtbl = new DataTable();
        //    if (NoteId.Trim() == "")
        //    {
        //        dtbl = oDBEngine.GetDataTable("select branch_id,branch_description from tbl_master_branch where branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ")  order by branch_description asc");

        //    }

        //    List<string> obj = new List<string>();

        //    foreach (DataRow dr in dtbl.Rows)
        //    {

        //        obj.Add(Convert.ToString(dr["branch_description"]) + "|" + Convert.ToString(dr["branch_id"]));
        //    }
        //    return obj;
        //}       
        //[WebMethod]
        //public static List<string> BindLedgerType(String Ids)
        //{
        //    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);

        //    DataTable dtbl = new DataTable();
        //    if (Ids.Trim() != "")
        //    {
        //        dtbl = oDBEngine.GetDataTable("select A.MainAccount_ReferenceID AS ID,A.MainAccount_Name as 'AccountName' FROM Master_MainAccount A WHERE A.MainAccount_AccountCode IN(SELECT RTRIM(B.AccountsLedger_MainAccountID) FROM Trans_AccountsLedger B WHERE B.AccountsLedger_BranchId in(" + Ids + ")) ORDER BY A.MainAccount_Name ");

        //    }

        //    List<string> obj = new List<string>();

        //    foreach (DataRow dr in dtbl.Rows)
        //    {

        //        obj.Add(Convert.ToString(dr["AccountName"]) + "|" + Convert.ToString(dr["Id"]));
        //    }
        //    return obj;
        //}       

        //[WebMethod]
        //public static List<string> BindCustomerVendor(string type)
        //{
        //    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);

        //    DataTable dtbl = new DataTable();
        //    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //    SqlCommand cmd = new SqlCommand("prc_PartyLedgerCustVend_Report", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@type", type);
        //    cmd.CommandTimeout = 0;
        //    SqlDataAdapter da = new SqlDataAdapter();
        //    da.SelectCommand = cmd;
        //    da.Fill(dtbl);

        //    cmd.Dispose();
        //    con.Dispose();

        //    if (type == "0")
        //    {
        //        dtbl = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name',cnt_UCC as 'Contact'  FROM tbl_master_contact WHERE cnt_contactType in('CL','DV') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");

        //    }

        //    else if (type == "1")
        //    {
        //        dtbl = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name',cnt_UCC as 'Contact'  FROM tbl_master_contact WHERE cnt_contactType in('CL') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");

        //    }

        //    else if (type == "2")
        //    {
        //        dtbl = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name',cnt_UCC as 'Contact'  FROM tbl_master_contact WHERE cnt_contactType in('DV') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");

        //    }

        //    List<string> obj = new List<string>();

        //    foreach (DataRow dr in dtbl.Rows)
        //    {

        //        obj.Add(Convert.ToString(dr["Name"]) + "|" + Convert.ToString(dr["Id"]));
        //    }
        //    return obj;
        //}
        //End of Rev Debashis Implement LINQ

        public void Date_finyearwise(string Finyear)
        {
            CommonBL bll1 = new CommonBL();
            DataTable stbill = new DataTable();
            DateTime MinDate, MaxDate;

            stbill = bll1.GetDateFinancila(Finyear);
            if (stbill.Rows.Count > 0)
            {

                ASPxFromDate.MaxDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_StartDate"]));

                DateTime TodayDate = Convert.ToDateTime(DateTime.Now);
                DateTime FinYearEndDate = MaximumDate;

                if (TodayDate > FinYearEndDate)
                {
                    ASPxToDate.Date = FinYearEndDate;
                    ASPxFromDate.Date = MinimumDate;
                }
                else
                {
                    ASPxToDate.Date = TodayDate;
                    ASPxFromDate.Date = MinimumDate;
                }

            }
        }

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            if (e.Item.FieldName == "DEBIT")
            {
                TotalDebit = Convert.ToDecimal(e.Value);
            }
            else if (e.Item.FieldName == "CREDIT")
            {
                TotalCredit = Convert.ToDecimal(e.Value);
            }

            if (e.Item.FieldName == "Closing_Balance")
            {
                TotalDBCR = TotalDebit - TotalCredit;
                if (TotalDBCR > 0)
                {
                    e.Text = Convert.ToString(Math.Abs((TotalDebit - TotalCredit)));// +" Dr";
                }
                else if (TotalDBCR < 0)
                {
                    e.Text = Convert.ToString(Math.Abs((TotalDebit - TotalCredit)));// +" Cr";
                }
                else if (TotalDBCR == 0)
                {
                    e.Text = Convert.ToString(Math.Abs((TotalDebit - TotalCredit)));
                }
            }
            else
            {
                e.Text = string.Format("{0}", e.Value);
            }

            if (e.Item.FieldName == "Closebal_DBCR")
            {
                if((TotalDebit-TotalCredit) > 0)
                {
                    e.Text = "Dr.";
                }
                else if ((TotalDebit - TotalCredit) < 0)
                {
                    e.Text = "Cr.";
                }
                else if ((TotalDebit - TotalCredit) == 0)
                {
                    e.Text = "";
                }
            }
        }

        //Rev Debashis Implement LINQ
        //protected void Showgrid_Htmlprepared(object sender, EventArgs e)
        //{
        //    if (Session["dt_PartyLedgerRpt"] != null)
        //    {

        //        ShowGrid.DataSource = (DataTable)Session["dt_PartyLedgerRpt"];
        //    }
        //}

        //protected void ComponentCustomer_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{

        //    string FinYear = Convert.ToString(Session["LastFinYear"]);

        //    if (e.Parameter.Split('~')[0] == "BindComponentGrid")
        //    {
        //        string type = e.Parameter.Split('~')[1];
        //        DataTable ComponentTable = new DataTable();

        //        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //        SqlCommand cmd = new SqlCommand("GetCustomerBind", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@type", type);
        //        cmd.CommandTimeout = 0;
        //        SqlDataAdapter da = new SqlDataAdapter();
        //        da.SelectCommand = cmd;
        //        da.Fill(ComponentTable);

        //        cmd.Dispose();
        //        con.Dispose();

        //        //if (type == "0")
        //        //{
        //        //    ComponentTable = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name',cnt_UCC as 'Contact'  FROM tbl_master_contact WHERE cnt_contactType in('CL','DV') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");

        //        //}

        //        //else if (type == "1")
        //        //{
        //        //    ComponentTable = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name',cnt_UCC as 'Contact'  FROM tbl_master_contact WHERE cnt_contactType in('CL') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");

        //        //}

        //        //else if (type == "2")
        //        //{
        //        //    ComponentTable = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name',cnt_UCC as 'Contact'  FROM tbl_master_contact WHERE cnt_contactType in('DV') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");

        //        //}

        //        if (ComponentTable.Rows.Count > 0)
        //        {
        //            // grid_Products.JSProperties["cptxt_InvoiceDate"] = Convert.ToString(ComponentTable.Rows[0]["Invoice_Date"]);

        //            Session["SI_ComponentData"] = ComponentTable;

        //            lookup_customer.DataSource = ComponentTable;
        //            lookup_customer.DataBind();

        //        }
        //        else
        //        {
        //            Session["SI_ComponentData"] = null;
        //            lookup_customer.DataSource = null;
        //            lookup_customer.DataBind();

        //        }
        //    }
        //}

        //protected void lookup_customer_DataBinding(object sender, EventArgs e)
        //{

        //    if (Session["SI_ComponentData"] != null)
        //    {
        //        lookup_customer.DataSource = (DataTable)Session["SI_ComponentData"];
        //    }
        //}        

        //protected void dgvVIEW_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        //{
        //    if (e.Item == ShowGrid.TotalSummary["Closing_Balance"])
        //    {
        //        if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
        //        {
        //            Decimal gmv = Convert.ToDecimal(ShowGrid.GetTotalSummaryValue(ShowGrid.TotalSummary["DEBIT"]));
        //            Decimal equity = Convert.ToDecimal(ShowGrid.GetTotalSummaryValue(ShowGrid.TotalSummary["CREDIT"]));

        //            e.TotalValue = gmv - equity;
        //            if (Convert.ToDecimal(e.TotalValue) != 0)
        //            {
        //                if (Convert.ToDecimal(e.TotalValue) < 0)
        //                {

        //                    e.TotalValue = System.Math.Abs(Convert.ToDecimal(e.TotalValue)) + "Cr";
        //                }
        //                else
        //                {


        //                    e.TotalValue = e.TotalValue + "Dr";

        //                }
        //            }
        //            e.TotalValueReady = true;
        //        }
        //    }

        //}
        //End of Rev Debashis Implement LINQ

        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "RID";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsCustFilter"]) == "Y")
            {
                var q = from d in dc.CUSTVENDLEDGER_REPORTs
                        where Convert.ToString(d.CUSTVENDTYPE) == "CL" && Convert.ToString(d.USERID) == Userid
                        orderby d.RID ascending
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.CUSTVENDLEDGER_REPORTs
                        where Convert.ToString(d.RID) == "0"
                        orderby d.RID ascending
                        select d;
                e.QueryableSource = q;
            }

            if (Session["GROUPSELECTCL"] == null || Session["GROUPSELECTCL"] =="")
            {
                //Rev 1.0 Mantis: 0027273
                //ShowGrid.Columns[5].Visible = false;
                ShowGrid.Columns[6].Visible = false;
                //End of Rev 1.0 Mantis: 0027273
            }
            else if (Session["GROUPSELECTCL"] != null || Session["GROUPSELECTCL"] != "")
            {
                //Rev 1.0 Mantis: 0027273
                //ShowGrid.Columns[5].Visible = true;
                ShowGrid.Columns[6].Visible = true;
                //End of Rev 1.0 Mantis: 0027273
            }
        }

        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            //string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            //string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
            string IsCustFilter = Convert.ToString(hfIsCustFilter.Value);
            Session["IsCustFilter"] = IsCustFilter;
            string IsGroupSelect = Convert.ToString(hdnSelectedGroups.Value);
            Session["GROUPSELECTCL"] = IsGroupSelect;

            DateTime dtFrom;
            DateTime dtTo;
            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");
            string BRANCH_ID = "";

            string BranchComponent = "";
            List<object> BranchList = lookup_branch.GridView.GetSelectedFieldValues("ID");
            foreach (object Branch in BranchList)
            {
                BranchComponent += "," + Branch;
            }
            BRANCH_ID = BranchComponent.TrimStart(',');

            string PROJECT_ID = "";
            string Projects = "";
            List<object> ProjectList = lookup_project.GridView.GetSelectedFieldValues("ID");
            foreach (object Project in ProjectList)
            {
                Projects += "," + Project;
            }
            PROJECT_ID = Projects.TrimStart(',');

            string CUSTVENDID = "";
            //string CustComponent = "";
            //List<object> CustList = lookup_customer.GridView.GetSelectedFieldValues("ID");
            //foreach (object Cust in CustList)
            //{
            //    CustComponent += "," + Cust;
            //}
            //CUSTVENDID = CustComponent.TrimStart(',');

            CUSTVENDID = hdnCustomerId.Value;
            //Rev Debashis && Implement Party name for export in Excel

            string CUSTNM = "";
            //string CustName = "";
            //List<object> CustNmList = lookup_customer.GridView.GetSelectedFieldValues("Name");
            //foreach (object Cust in CustNmList)
            //{
            //    CustName += "," + Cust;
            //}
            //CUSTNM = CustName.TrimStart(',');
            CUSTNM = txtCustName.Text;
            Session["CUSTNM"] = CUSTNM.Trim();

            //End of Rev Debashis

            //Rev Subhra 18-12-2018   0017670

            string BRANCH_NAME = "";
            string BranchNameComponent = "";
            List<object> BranchNameList = lookup_branch.GridView.GetSelectedFieldValues("branch_description");
            foreach (object BranchName in BranchNameList)
            {
                BranchNameComponent += "," + BranchName;
            }
            if (BranchNameList.Count > 1)
            {
                BRANCH_NAME = "Multiple Branch Selected";
                Session["BranchNames"] = BRANCH_NAME;
            }
            else
            {
                BRANCH_NAME = BranchNameComponent.TrimStart(',');
                Session["BranchNames"] = "For Unit : " + BRANCH_NAME + " ";
            }
            CallbackPanel.JSProperties["cpBranchNames"] = Convert.ToString(Session["BranchNames"]);

            //End of Rev


            GetCustomerLedgerdata(FROMDATE, TODATE, BRANCH_ID, CUSTVENDID, PROJECT_ID);
        }
        #region Branch Populate

        //Rev Debashis Implement LINQ
        protected void Componentbranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];
                if (Session["userbranchHierarchy"] != null)
                {
                    ComponentTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch where branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ") order by branch_description asc");
                }
                if (ComponentTable.Rows.Count > 0)
                {
                    Session["SI_ComponentData_Branch"] = ComponentTable;
                    lookup_branch.DataSource = ComponentTable;
                    lookup_branch.DataBind();
                }
                else
                {
                    Session["SI_ComponentData_Branch"] = ComponentTable;
                    lookup_branch.DataSource = null;
                    lookup_branch.DataBind();

                }
            }
        }

        protected void lookup_branch_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData_Branch"] != null)
            {
                lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
            }
        }
        //End of Rev Debashis Implement LINQ

        //protected void BranchEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        //{
        //    e.KeyExpression = "ID";
        //    string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
        //    List<int> branchidlist;
        //    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
        //    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

        //    ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);
        //    var q = from d in dc.v_BranchLists
        //            where
        //            branchidlist.Contains(Convert.ToInt32(d.ID)) 
        //            orderby d.branch_description ascending
        //            select d;

        //    e.QueryableSource = q;
        //}

        #endregion


        //#region Customer Populate
        //protected void CustomerEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        //{
        //    e.KeyExpression = "ID";
        //    string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;       

        //    ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);
        //    //if (IsPostBack)
        //    //{
        //        var q = from d in dc.v_ReportCustomers

        //                orderby d.Name ascending
        //                select d;

        //        e.QueryableSource = q;
        //    //}
        //    //else
        //    //{
        //    //    var q = (from d in dc.v_ReportCustomers

        //    //             orderby d.Name ascending
        //    //             select d).Take(0);

        //    //    e.QueryableSource = q;
        //    //}
           
        //}
        //#endregion

        #region Project Populate
        protected void Project_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindProjectGrid")
            {
                DataTable ProjectTable = new DataTable();
                ProjectTable = GetProject();

                if (ProjectTable.Rows.Count > 0)
                {
                    Session["ProjectData"] = ProjectTable;
                    lookup_project.DataSource = ProjectTable;
                    lookup_project.DataBind();
                }
                else
                {
                    Session["ProjectData"] = ProjectTable;
                    lookup_project.DataSource = null;
                    lookup_project.DataBind();
                }
            }
        }

        public DataTable GetProject()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_FETCHPROJECTS_REPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();

            return dt;
        }

        protected void lookup_project_DataBinding(object sender, EventArgs e)
        {
            if (Session["ProjectData"] != null)
            {
                lookup_project.DataSource = (DataTable)Session["ProjectData"];
            }
        }
        #endregion
    }
}